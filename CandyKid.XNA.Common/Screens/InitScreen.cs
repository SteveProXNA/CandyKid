using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class InitScreen : BaseScreen, IScreen
	{
		private Vector2 messagePosition;
		private String messageText;
		private ScreenType nextScreen;
		private Boolean join;

		public override void Initialize()
		{
			Single wide = (Constants.ScreenWide - Assets.SteveProTexture.Width) / 2.0f;
			Single high = (Constants.ScreenHigh - Assets.SteveProTexture.Height) / 2.0f;

			BannerPosition = new Vector2(wide, high);
			messageText = "ROCK ON!";

			SByte x = (SByte)(32 - messageText.Length);
			const SByte y = 22;
			const Byte textsSize = Constants.TilesSize / 2;
			const UInt32 offsetX = Constants.GameOffsetX;
			const Single fontX = Constants.FontOffsetX;
			const Single fontY = Constants.FontOffsetY;

			messagePosition = MyGame.Manager.TextManager.GetTextPosition(x, y, textsSize, offsetX, fontX, fontY);
			join = false;
		}

		public override void LoadContent()
		{
			base.LoadContent();
			MyGame.Manager.ThreadManager.LoadContentAsync();
		}

		public ScreenType Update(GameTime gameTime)
		{
			UpdateTimer(gameTime);

			// Do not attempt to progress until join.
			join = MyGame.Manager.ThreadManager.Join(1);
			if (!join)
			{
				return ScreenType.Init;
			}

			nextScreen = GetNextScreen();
			if (Timer > BaseData.SplashDelay)
			{
				return nextScreen;
			}

			Boolean next = MyGame.Manager.InputManager.Next();
			return next ? nextScreen : ScreenType.Init;
		}

		public override void Draw()
		{
			Engine.SpriteBatch.Draw(Assets.SteveProTexture, BannerPosition, Color.White);
			if (join)
			{
				Engine.SpriteBatch.DrawString(Assets.EmulogicFont, messageText, messagePosition, Color.White);
			}
		}

		private static ScreenType GetNextScreen()
		{
			ScreenType screenType = MyGame.Manager.ConfigManager.GlobalConfigData.ScreenType;
			if (ScreenType.Splash == screenType || ScreenType.Init == screenType)
			{
				screenType = ScreenType.Title;
			}

			return screenType;
		}

	}
}