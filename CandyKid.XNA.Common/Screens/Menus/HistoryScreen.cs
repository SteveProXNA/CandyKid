using Microsoft.Xna.Framework;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Screens.Menus
{
	public class HistoryScreen : MainScreen, IScreen
	{
		public override void Initialize()
		{
			BannerTexture = Assets.HistoryTexture;
			BannerPosition = GetBannerPosition(BannerTexture);
			LoadTextData();

			base.Initialize();
		}

		public override void LoadContent()
		{
			base.LoadContent();
			NextScreen = ScreenType.SubMenuOne;
		}

		public ScreenType Update(GameTime gameTime)
		{
			return Update(gameTime, ScreenType.History);
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawMenu();
			Engine.SpriteBatch.Draw(BannerTexture, BannerPosition, Color.White);
			//CandyKid.Draw();
			MyGame.Manager.TextManager.Draw(TextDataList);
			DrawBuild();
		}

	}
}