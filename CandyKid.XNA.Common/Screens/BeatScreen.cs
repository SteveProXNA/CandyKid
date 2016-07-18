using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class BeatScreen : BaseScreen, IScreen
	{
		private IList<Direction> directionList;
		private CandyKid player;
		private Byte index, count;
		private UInt16 timer1;
		private Boolean flag;

		public override void Initialize()
		{
			directionList = GetDirectionList();
			count = (Byte) directionList.Count;
		}

		public override void LoadContent()
		{
			base.LoadContent();
			LoadTextData();

			MyGame.Manager.SoundManager.StopMusic();
			MyGame.Manager.SoundManager.StartWinner();
			player = MyGame.Manager.EntityManager.Player;
			player.Reset();
			index = 0;
			timer1 = 0;
			flag = true;
		}

		public ScreenType Update(GameTime gameTime)
		{
			UpdateTimer(gameTime);
			if (Timer > BaseData.BeatDelay)
			{
				ClockLevel();
				return ScreenType.Title;
			}

			if (ImmediateExit())
			{
				ClockLevel();
				return ScreenType.Title;
			}

			Boolean next = MyGame.Manager.InputManager.Next();
			if (next)
			{
				ClockLevel();
				return ScreenType.Title;
			}

			timer1 += (UInt16)(gameTime.ElapsedGameTime.Milliseconds);
			if (timer1 > DELAY1)
			{
				timer1 -= DELAY1;
				flag = !flag;
			}

			MyGame.Manager.EventManager.ClearEvents();

			// Move player.
			MovePlayer(gameTime);

			MyGame.Manager.EventManager.ProcessEvents(gameTime);
			return ScreenType.Beat;
		}

		private void MovePlayer(GameTime gameTime)
		{
			if (Direction.None == player.Direction && Lifecycle.Idle == player.Lifecycle)
			{
				Direction playerDirection = directionList[index];
				index++;
				if (index >= count)
				{
					index = 0;
				}
				MyGame.Manager.EventManager.AddPlayerMoveEvent(playerDirection);
			}
			else if (Direction.None != player.Direction && Lifecycle.Move == player.Lifecycle)
			{
				player.Update(gameTime);
			}

			if (Direction.None != player.Direction && Lifecycle.Idle == player.Lifecycle)
			{
				player.Stop();
			}
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.EntityManager.DrawPlayer();
			MyGame.Manager.BorderManager.DrawOver();

			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.ScoreManager.Draw(); 
			if (flag)
			{
				MyGame.Manager.TextManager.Draw(TextDataList);
			}

		}

		private static IList<Direction> GetDirectionList()
		{
			return new List<Direction>
			{
				Direction.Right, Direction.Right, Direction.Right, Direction.Right, Direction.Right, Direction.Right, Direction.Right,
				Direction.Down, Direction.Down, Direction.Down, Direction.Down, Direction.Down, Direction.Down, Direction.Down,
				Direction.Left, Direction.Left, Direction.Left, Direction.Left, Direction.Left, Direction.Left, Direction.Left,
				Direction.Up, Direction.Up, Direction.Up, Direction.Up, Direction.Up, Direction.Up, Direction.Up,
			};
		}

		private static void ClockLevel()
		{
			MyGame.Manager.SoundManager.StopWinner();

			UInt16 clockLevel = BaseData.TrialedGame ? BaseData.TrialLevel : BaseData.ClockLevel;
			if (BaseData.ScoreLevel > clockLevel)
			{
				BaseData.ResetScoreLevel();
			}
		}
	}
}
