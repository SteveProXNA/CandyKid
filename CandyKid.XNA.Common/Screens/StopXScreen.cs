using System;
using Microsoft.Xna.Framework;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class StopXScreen : KillScreen, IScreen
	{
		public override void Initialize()
		{
			base.Initialize();
			LoadTextData(typeof(StopScreen).Name);
		}

		public override void LoadContent()
		{
			base.LoadContent();

			MyGame.Manager.InputManager.ResetMotors();
			MyGame.Manager.SoundManager.PauseMusic();
		}

		public ScreenType Update(GameTime gameTime)
		{
			Boolean popupNo = MyGame.Manager.InputManager.PopupNo();
			if (ImmediateExit() || popupNo)
			{
				MyGame.Manager.SoundManager.ResumeMusic();
				return ScreenType.DeadX;
			}
			Boolean popupOk = MyGame.Manager.InputManager.PopupOk();
			if (popupOk)
			{
				return ScreenType.Menu;
			}

			return ScreenType.StopX;
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.BoardManager.Draw();

			// Draw death skull n' crossbones.
			base.Draw();
			MyGame.Manager.EntityManager.DrawEnemies();
			MyGame.Manager.BorderManager.DrawOver();

			MyGame.Manager.DeviceManager.DrawStripes();
			MyGame.Manager.DeviceManager.DrawGameArrows(Direction.None);

			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.TextManager.Draw(TextDataList);
			MyGame.Manager.ScoreManager.Draw();
		}

	}
}