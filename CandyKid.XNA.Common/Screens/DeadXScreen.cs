using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class DeadXScreen : KillScreen, IScreen
	{
		public ScreenType Update(GameTime gameTime)
		{
			UpdateTimer(gameTime);
			if (Timer > DELAY1)
			{
				MyGame.Manager.InputManager.ResetMotors();
			}
			if (Timer > BaseData.DeadDelay)
			{
				return FinishDeathSequence();
			}

			Boolean pause = MyGame.Manager.InputManager.Pause();
			if (pause)
			{
				return ScreenType.StopX;
			}

			Boolean board = MyGame.Manager.InputManager.Board();
			if (board)
			{
				return FinishDeathSequence();
			}

			MyGame.Manager.EventManager.ClearEvents();
			MyGame.Manager.EntityManager.MoveEnemies(gameTime);
			MyGame.Manager.EventManager.ProcessEvents(gameTime);

			return ScreenType.DeadX;
		}

		private static ScreenType FinishDeathSequence()
		{
			MyGame.Manager.InputManager.ResetMotors();
			EnemyType deadEnemyType = BaseData.DeadEnemyType;
			if (EnemyType.None != deadEnemyType)
			{
				MyGame.Manager.EntityManager.ResetEnemy(deadEnemyType);
				BaseData.SetDeadEnemy(EnemyType.None);
			}

			if (BaseData.ResetEnemies)
			{
				MyGame.Manager.EntityManager.ResetEnemies();
			}

			MyGame.Manager.SoundManager.PlayReadySoundEffect();
			MyGame.Manager.EntityManager.ResetPlayer();
			return ScreenType.Play;
		}

		public override void Draw()
		{
			// MUST draw everything in this order!
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.BoardManager.Draw();

			// Draw death skull n' crossbones.
			base.Draw();
			MyGame.Manager.EntityManager.DrawEnemies();

			MyGame.Manager.DeviceManager.DrawStripes();
			MyGame.Manager.DeviceManager.DrawGameArrows(Direction.None);

			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.ScoreManager.Draw();
		}
	}
}