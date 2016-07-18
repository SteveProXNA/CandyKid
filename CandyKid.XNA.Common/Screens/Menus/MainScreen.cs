using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Library;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Screens.Menus
{
	public abstract class MainScreen : BaseScreen
	{
		protected IList<TextData> TextMenuList { get; private set; }
		protected Boolean MenuSelect { get; set; }
		protected ScreenType NextScreen { get; set; }
		protected CandyKid CandyKid { get; private set; }

		private String BuildVersion { get; set; }
		private Vector2 BuildVector2 { get; set; }

		public override void Initialize()
		{
			BuildVersion = "V" + MyGame.Manager.TextManager.BuildVersion;
			BuildVector2 = MyGame.Manager.TextManager.GetTextPosition((SByte)(30 - BuildVersion.Length), 21);

			base.Initialize();
		}

		public override void LoadContent()
		{
			if (BaseData.RefreshGame)
			{
				LoadTextData();
			}
			if (null != CandyKid)
			{
				CandyKid.SetSource(MyGame.Manager.ImageManager.GamerOneRectangle[BaseData.GamerSpriteIndex]);
			}

			NextScreen = ScreenType.Menu;
			MenuSelect = false;
		}

		protected void ConstructCandyKid(Byte x, Byte y, Vector2 position, Rectangle source, Byte increase, Byte velocity, Byte distance, Byte gamerSize)
		{
			CandyKid = new CandyKid(x, y, position, source, increase, velocity, distance, gamerSize);
		}

		protected ScreenType Update(GameTime gameTime, ScreenType screenType)
		{
			if (MenuSelect)
			{
				if (null == CandyKid || Lifecycle.Idle == CandyKid.Lifecycle)
				{
					return NextScreen;
				}

				CandyKid.Update(gameTime);
			}
			else
			{
				if (ReturnToMenu())
				{
					MenuSelect = true;
				}
			}

			return screenType;
		}

		protected void DrawBuild()
		{
			Engine.SpriteBatch.DrawString(Assets.EmulogicFont, BuildVersion, BuildVector2, Color.White);
		}
		protected void LoadMenuData()
		{
			LoadMenuData(GetType().Name);
		}
		protected void LoadMenuData(String screen)
		{
			TextMenuList = MyGame.Manager.TextManager.LoadMenuData(screen);
		}

	}
}