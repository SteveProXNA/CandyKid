using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Screens.Menus
{
	public class UnlockScreen : MainScreen, IScreen
	{
		private const Byte MaxStrip = 14;
		private Vector2[] positions;
		private UInt16 timer;
		private Boolean flag;

		public override void Initialize()
		{
			BannerTexture = Assets.UnlockTexture;
			BannerPosition = GetBannerPosition(BannerTexture);
			LoadTextData();

			positions = GetPositions(9, 20);
			base.Initialize();
		}

		public override void LoadContent()
		{
			base.LoadContent();
			flag = false;
		}

		public ScreenType Update(GameTime gameTime)
		{
			timer += (UInt16)(gameTime.ElapsedGameTime.Milliseconds);
			if (timer > DELAY1)
			{
				timer -= DELAY1;
				flag = !flag;
			}

			Quadrant quadrant = MyGame.Manager.InputManager.HoldQuadrant();
			if (Quadrant.BotLeft == quadrant || Quadrant.BotRight == quadrant)
			{
#if ANDROID
				// https://www.packtpub.com/books/content/code-sharing-between-ios-and-android
				// http://stackoverflow.com/questions/31320905/run-android-activity-from-c-sharp-codei-use-xamarin-with-monogame
				var intent = new Android.Content.Intent(Android.Content.Intent.ActionView, Android.Net.Uri.Parse(Constants.UnlockUrl));
				intent.SetFlags(Android.Content.ActivityFlags.NewTask);
				Android.App.Application.Context.StartActivity(intent);
#endif
#if IOS
				UIKit.UIApplication.SharedApplication.OpenUrl(Foundation.NSUrl.FromString(Constants.UnlockUrl));
#endif
				return ScreenType.Exit;
			}

			if (MyGame.Manager.InputManager.Escape() || MyGame.Manager.InputManager.Next())
			{
				return ScreenType.Exit;
			}

			UpdateTimer(gameTime);
			if (Timer > BaseData.UnlockDelay)
			{
				return ScreenType.Exit;
			}

			return ScreenType.Unlock;
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawMenu();
			Engine.SpriteBatch.Draw(BannerTexture, BannerPosition, Color.White);

			MyGame.Manager.TextManager.Draw(TextDataList);
			if (flag)
			{
				for (Byte index = 0; index < MaxStrip; ++index)
				{
					Engine.SpriteBatch.Draw(Assets.NewArrowTexture, positions[index], MyGame.Manager.ImageManager.BlackStripRectangle, Color.White);
				}
			}
		}

		private static Vector2[] GetPositions(Byte x, Byte y)
		{
			Vector2[] positions = new Vector2[MaxStrip];
			for (Byte index = 0; index < MaxStrip; ++index)
			{
				Vector2 position = new Vector2(BaseData.GameOffsetX + BaseData.TextsSize * (x + index), BaseData.TextsSize * y);
				positions[index] = position;
			}

			return positions;
		}

	}
}