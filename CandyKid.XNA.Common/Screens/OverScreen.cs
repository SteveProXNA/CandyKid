using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class OverScreen : KillScreen, IScreen
	{
		private Single delay1, delay2;
		private OverType overType;

		public override void Initialize()
		{
			delay2 = 3000;
			base.Initialize();
			LoadTextData();
		}

		public override void LoadContent()
		{
			base.LoadContent();

			MyGame.Manager.SoundManager.StopMusic();
			delay1 = BaseData.OverStopage * 1000;
			overType = OverType.Explosion;
		}

		public ScreenType Update(GameTime gameTime)
		{
			UpdateTimer(gameTime);
			if (Timer > DELAY1)
			{
				MyGame.Manager.InputManager.ResetMotors();
			}
			if (OverType.Explosion == overType && Timer >= delay1)
			{
				overType = OverType.GameOver;
				MyGame.Manager.SoundManager.PlayOverXSoundEffect();
			}

			if (OverType.GameOver == overType && Timer >= delay2)
			{
				overType = OverType.Advance;
			}

			Boolean next = false;
			if (OverType.Advance == overType)
			{
				if (Timer > BaseData.OverDelay)
				{
					return ScreenType.Menu;
				}
				if (ImmediateExit())
				{
					return ScreenType.Menu;
				}

				next = MyGame.Manager.InputManager.Next();
			}

			return next ? ScreenType.Menu : ScreenType.Over;
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.BoardManager.Draw();
			//MyGame.Manager.EntityManager.DrawPlayer();

			// Draw death skull n' crossbones.
			base.Draw();
			MyGame.Manager.EntityManager.DrawEnemies();
			MyGame.Manager.BorderManager.DrawOver();

			MyGame.Manager.TextManager.Draw(TextDataList);
			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.ScoreManager.Draw();
		}

	}
}