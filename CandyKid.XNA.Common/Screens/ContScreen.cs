using System;
using Microsoft.Xna.Framework;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class ContScreen : KillScreen, IScreen
	{
		private Single delay1;
		private Boolean flag;

		public override void Initialize()
		{
			base.Initialize();
			LoadTextData();
			delay1 = 500;
		}

		public override void LoadContent()
		{
			base.LoadContent();
			//LoadTextData();

			MyGame.Manager.SoundManager.StopMusic();
			flag = false;
		}

		public ScreenType Update(GameTime gameTime)
		{
			UpdateTimer(gameTime);
			if (Timer > DELAY1)
			{
				MyGame.Manager.InputManager.ResetMotors();
			}
			if (flag)
			{
				if (Timer > delay1)
				{
					MyGame.Manager.InputManager.ResetMotors();
					MyGame.Manager.SoundManager.StartMusic();
					return ScreenType.Play;
				}
			}

			Boolean popupNo = MyGame.Manager.InputManager.PopupNo();
			if (popupNo)
			{
				MyGame.Manager.InputManager.ResetMotors();
				return ScreenType.Over;
			}
			Boolean popupOk = MyGame.Manager.InputManager.PopupOk();
			if (popupOk)
			{
				MyGame.Manager.ScoreManager.ResetLives();
				MyGame.Manager.EntityManager.ResetPlayer();
				MyGame.Manager.EntityManager.ResetEnemies();

				MyGame.Manager.SoundManager.PlayReadySoundEffect();
				Timer = 0;
				flag = true;
			}

			return ScreenType.Cont;
		}

		public ScreenType Updatex(GameTime gameTime)
		{
			if (flag)
			{
				UpdateTimer(gameTime);
				if (Timer > delay1)
				{
					MyGame.Manager.SoundManager.StartMusic();
					return ScreenType.Play;
				}
			}

			Boolean popupNo = MyGame.Manager.InputManager.PopupNo();
			if (popupNo)
			{
				return ScreenType.Over;
			}
			Boolean popupOk = MyGame.Manager.InputManager.PopupOk();
			if (popupOk)
			{
				MyGame.Manager.ScoreManager.ResetLives();
				MyGame.Manager.EntityManager.ResetPlayer();
				MyGame.Manager.EntityManager.ResetEnemies();
				MyGame.Manager.SoundManager.PlayReadySoundEffect();
				flag = true;
			}

			return ScreenType.Cont;
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.BoardManager.Draw();
			

			// Draw death skull n' crossbones.
			if (flag)
			{
				MyGame.Manager.EntityManager.DrawPlayer();
			}
			else
			{
				base.Draw();
			}

			MyGame.Manager.EntityManager.DrawEnemies();
			MyGame.Manager.DeviceManager.DrawStripes();
			MyGame.Manager.DeviceManager.DrawGameArrows(Direction.None);

			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.ScoreManager.Draw();

			if (!flag)
			{
				MyGame.Manager.BorderManager.DrawOver();
				MyGame.Manager.TextManager.Draw(TextDataList);
			}
		}

	}
}