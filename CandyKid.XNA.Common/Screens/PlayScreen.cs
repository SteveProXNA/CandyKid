using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class PlayScreen : BaseScreen, IScreen
	{
		private CandyKid player;
		private Direction detectDirection;

		private IDictionary<EnemyType, CandyMama> enemies;
		private Boolean isInGodMode, bonusFreePlayer;
		private Byte gamerCrash, bonusMultipler;

		public override void LoadContent()
		{
			base.LoadContent();
			player = MyGame.Manager.EntityManager.Player;
			enemies = MyGame.Manager.EntityManager.Enemies;
			isInGodMode = BaseData.IsInGodMode;
			bonusFreePlayer = BaseData.BonusFreePlayer;
			gamerCrash = MyGame.Manager.CollisionManager.GetGamerCrash();
			bonusMultipler = GetBonusMultiper();
		}

		public ScreenType Update(GameTime gameTime)
		{
			Boolean pause = MyGame.Manager.InputManager.Pause();
			if (pause)
			{
				return ScreenType.Stop;
			}

			if (bonusFreePlayer)
			{
				CheckBonusFreePlayer();
			}

			MyGame.Manager.EventManager.ClearEvents();

			// Move player.
			ScreenType nextScreen = MovePlayer(gameTime);

			// Move enemies.
			MyGame.Manager.EntityManager.MoveEnemies(gameTime);

			// Main collision detection.
			if (ScreenType.Play == nextScreen)
			{
				if (!isInGodMode)
				{
					nextScreen = CheckCollision();
				}
			}

			MyGame.Manager.EventManager.ProcessEvents(gameTime);
			if (MyGame.Manager.BoardManager.CandyCount <= 0)
			{
				nextScreen = ScreenType.Comp;
			}

			return nextScreen;
		}

		private void CheckBonusFreePlayer()
		{
			Boolean board = MyGame.Manager.InputManager.Board();
			if (!board)
			{
				return;
			}

			// Hit the secret and get the bonus life!
			MyGame.Manager.ScoreManager.UpdateExtra();
			bonusFreePlayer = false;
			BaseData.SetBonusFreePlayer(bonusFreePlayer);
		}
		private ScreenType MovePlayer(GameTime gameTime)
		{
			detectDirection = MyGame.Manager.InputManager.MoveDirection();

			ScreenType nextScreen = ScreenType.Play;
			EventType eventType;
			if (Direction.None == player.Direction && Lifecycle.Idle == player.Lifecycle)
			{
				if (Direction.None != detectDirection)
				{
					// Anticipate next direction.
					Direction thisDirection = detectDirection;

					eventType = EventType.EntityFree;
					if (!BaseData.UseKillTrees)
					{
						TileType[,] boardData = MyGame.Manager.BoardManager.BoardData;
						eventType = MyGame.Manager.MoveManager.CheckDirection(boardData, player.CurrX, player.CurrY, thisDirection);
					}
					if (EventType.EntityFree == eventType)
					{
						MyGame.Manager.EventManager.AddPlayerMoveEvent(thisDirection);
						MyGame.Manager.InputManager.ResetMotors();
					}
					else
					{
						MyGame.Manager.InputManager.SetMotors(0, 1);
					}
				}
				else
				{
					MyGame.Manager.InputManager.ResetMotors();
				}
			}
			else if (Direction.None != player.Direction && Lifecycle.Move == player.Lifecycle)
			{
				player.Update(gameTime);
			}

			// Finished crossing tile: check for collision on electric tree OR eat candy/bonus.
			if (Direction.None != player.Direction && Lifecycle.Idle == player.Lifecycle)
			{
				TileType[,] boardData = MyGame.Manager.BoardManager.BoardData;
				eventType = MyGame.Manager.CollisionManager.CheckTilesCollision(boardData, player.CurrX, player.CurrY, player.Direction);
				if (EventType.None != eventType)
				{
					// Process collision.
					if (EventType.DeathTree == eventType)
					{
						player.Dead();
						nextScreen = ScreenType.Dead;
					}

					Byte location = MyGame.Manager.BoardManager.CalcLocation((Byte)player.CurrX, (Byte)player.CurrY);
					if (EventType.EatCandy == eventType)
					{
						MyGame.Manager.EventManager.AddPlayerCandyEvent(location, Constants.CANDY_SCORE);
						MyGame.Manager.SoundManager.PlayCandySoundEffect();
					}
					if (eventType >= EventType.EatBonus1 && eventType <= EventType.EatBonus4)
					{
						UInt16 bonus = MyGame.Manager.ScoreManager.ConvertEventToBonus(eventType);
						bonus *= bonusMultipler;
						MyGame.Manager.EventManager.AddPlayerBonusEvent(location, bonus);
						MyGame.Manager.SoundManager.PlayBonusSoundEffect();
					}
				}

				player.Stop();
			}

			return nextScreen;
		}
		private ScreenType CheckCollision()
		{
			const ScreenType nextScreen = ScreenType.Play;
			Boolean playerInExit = MyGame.Manager.CollisionManager.CheckEnemyCollisionExit(player.CurrX, player.CurrY);
			if (playerInExit)
			{
				return nextScreen;
			}

			EnemyType enemyType = CheckPlayerCollision();
			if (EnemyType.None == enemyType)
			{
				return nextScreen;
			}

			BaseData.SetDeadEnemy(enemyType);
			CandyMama enemyObject = enemies[enemyType];
			enemyObject.Dead();

			player.Dead();
			return ScreenType.Dead;
		}
		private EnemyType CheckPlayerCollision()
		{
			for (EnemyType enemyType = EnemyType.Adriana; enemyType <= EnemyType.StevePro; enemyType++)
			{
				CandyMama enemyObject = enemies[enemyType];
				Boolean collision = CheckPlayerCollideEnemy(enemyObject);
				if (collision)
				{
					return enemyType;
				}
			}

			return EnemyType.None;
		}
		private Boolean CheckPlayerCollideEnemy(BaseObject enemyObject)
		{
			return MyGame.Manager.CollisionManager.CheckEnemyCollisionSlow(gamerCrash, player.Position.X, player.Position.Y, enemyObject.Position.X, enemyObject.Position.Y);
		}
		private static Byte GetBonusMultiper()
		{
			Byte multiplier = 1;
			if (BaseData.ScoreLevel > Constants.DIV_BONUS_VALUE)
			{
				multiplier *= 2;
			}

			return multiplier;
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.BoardManager.Draw();
			MyGame.Manager.EntityManager.Draw();

			MyGame.Manager.DeviceManager.DrawStripes();
			MyGame.Manager.DeviceManager.DrawGameArrows(player.Direction);

			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.ScoreManager.Draw();
		}

	}
}