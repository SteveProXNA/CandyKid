using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class DemoScreen : BaseScreen, IScreen
	{
		private CandyKid player;
		private IDictionary<EnemyType, CandyMama> enemies;

		private IList<Single> eventTimeList;
		private IList<String> eventTypeList;
		private IList<String> eventArgsList;

		private IList<EventType> eventTypeData;
		private IList<ValueType> eventArgsData;
		private UInt16 index;
		private Single delta;

		public override void Initialize()
		{
			player = MyGame.Manager.EntityManager.Player;
			enemies = MyGame.Manager.EntityManager.Enemies;

			LoadTextData();
		}

		public override void LoadContent()
		{
			// Special demo screen.
			MyGame.Manager.ScoreManager.ResetLevel();
			MyGame.Manager.BoardManager.LoadContent(1, 1);

			MyGame.Manager.ScoreManager.ResetHigh();
			MyGame.Manager.ScoreManager.ResetLives();
			MyGame.Manager.ScoreManager.ResetScore();
			MyGame.Manager.EntityManager.ResetPlayer();
			MyGame.Manager.EntityManager.ResetEnemies();

			player = MyGame.Manager.EntityManager.Player;
			player.SetVelocity(25);
			enemies = MyGame.Manager.EntityManager.Enemies;
			enemies[EnemyType.Adriana].SetVelocity(15);
			enemies[EnemyType.Suzanne].SetVelocity(12);
			enemies[EnemyType.StevePro].SetVelocity(20);

			Byte commandId = 0;
			if (!BaseData.UseOpenExits)
			{
				commandId = 1;
			}

			eventTimeList = MyGame.Manager.CommandManager.CommandTimeList[commandId];
			eventTypeList = MyGame.Manager.CommandManager.CommandTypeList[commandId];
			eventArgsList = MyGame.Manager.CommandManager.CommandArgsList[commandId];

			index = 0;
			delta = 0.0f;

			base.LoadContent();
		}

		public ScreenType Update(GameTime gameTime)
		{
			if (ImmediateExit())
			{
				return BaseData.PrevScreen;
			}
			Boolean next = MyGame.Manager.InputManager.Next();
			if (next)
			{
				return BaseData.PrevScreen;
			}

			UpdateTimer(gameTime);
			if (Timer > BaseData.DemoDelay)
			{
				return BaseData.PrevScreen;
			}

			UpdateEntity(gameTime, player);
			foreach (var key in enemies.Keys)
			{
				CandyMama enemy = enemies[key];
				UpdateEnemy(gameTime, enemy);
			}

			Single eventTime = eventTimeList[index];
			Single time = (Single)Math.Round(gameTime.ElapsedGameTime.TotalSeconds, 2);
			delta += time;

			if (delta < eventTime)
			{
				return ScreenType.Demo;
			}

			// Process current event.
			delta -= eventTime;
			LoadNowEvents(index);

			Byte count = (Byte)(eventTypeData.Count);
			for (Byte delim = 0; delim < count; ++delim)
			{
				EventType eventType = eventTypeData[delim];
				ValueType eventArgs = eventArgsData[delim];

				MyGame.Manager.EventManager.ProcessEvent(eventType, eventArgs);
			}

			return ++index >= eventTimeList.Count ? BaseData.PrevScreen : ScreenType.Demo;
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.BoardManager.Draw();
			MyGame.Manager.EntityManager.Draw();

			MyGame.Manager.BorderManager.DrawOver();
			MyGame.Manager.DeviceManager.DrawStripes();
			MyGame.Manager.DeviceManager.DrawGameArrows(player.Direction);

			MyGame.Manager.TextManager.Draw(TextDataList);
			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.ScoreManager.Draw();
		}

		private static void UpdateEnemy(GameTime gameTime, CandyMama enemy)
		{
			enemy.UpdateFrame(gameTime);
			UpdateEntity(gameTime, enemy);
		}

		private static void UpdateEntity(GameTime gameTime, BaseObject entity)
		{
			if (Direction.None != entity.Direction)
			{
				switch (entity.Lifecycle)
				{
					case Lifecycle.Move:
						entity.Update(gameTime);
						break;
					case Lifecycle.Idle:
						entity.Stop();
						break;
				}
			}
		}

		private void LoadNowEvents(UInt16 theIndex)
		{
			String eventTypeText = eventTypeList[theIndex];
			String eventArgsText = eventArgsList[theIndex];

			eventTypeData = MyGame.Manager.EventManager.DeserializeTypeText(eventTypeText);
			eventArgsData = MyGame.Manager.EventManager.DeserializeArgsText(eventArgsText);
		}

	}
}