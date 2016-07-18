using System;
using Microsoft.Xna.Framework;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class SplashScreen : BaseScreen, IScreen
	{
		private Boolean flag;

		public override void Initialize()
		{
			Single wide = (Constants.ScreenWide - Assets.SteveProTexture.Width) / 2.0f;
			Single high = (Constants.ScreenHigh - Assets.SteveProTexture.Height) / 2.0f;

			BannerPosition = new Vector2(wide, high);
			flag = false;
		}

		public ScreenType Update(GameTime gameTime)
		{
			return flag ? ScreenType.Init : ScreenType.Splash;
		}

		public override void Draw()
		{
			Engine.SpriteBatch.Draw(Assets.SteveProTexture, BannerPosition, Color.White);
			flag = true;
		}

	}
}