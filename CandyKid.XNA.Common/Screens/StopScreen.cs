using System;
using Microsoft.Xna.Framework;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class StopScreen : BaseScreen, IScreen
	{
		public override void Initialize()
		{
			LoadTextData();
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
				return ScreenType.Play;
			}
			Boolean popupOk = MyGame.Manager.InputManager.PopupOk();
			if (popupOk)
			{
				return ScreenType.Menu;
			}

			return ScreenType.Stop;
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.BoardManager.Draw();
			MyGame.Manager.EntityManager.Draw();
			MyGame.Manager.BorderManager.DrawOver();

			MyGame.Manager.DeviceManager.DrawStripes();
			MyGame.Manager.DeviceManager.DrawGameArrows(Direction.None);

			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.ScoreManager.Draw();
			MyGame.Manager.TextManager.Draw(TextDataList);
		}

	}
}