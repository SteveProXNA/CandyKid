using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame.Data;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public abstract class BaseScreen
	{
		protected Vector2 BannerPosition { get; set; }
		protected Texture2D BannerTexture { get; set; }
		protected IList<TextData> TextDataList { get; private set; }
		protected UInt16 Timer { get; set; }
		protected const UInt16 DELAY1 = (UInt16)(Constants.TITLE_DELAY/2.0f);

		public virtual void Initialize()
		{
		}

		public virtual void LoadContent()
		{
			Timer = 0;
		}

		protected void UpdateTimer(GameTime gameTime)
		{
			Timer += (UInt16)gameTime.ElapsedGameTime.Milliseconds;
		}

		public virtual void Draw()
		{
		}

		protected void LoadTextData()
		{
			LoadTextData(GetType().Name);
		}
		protected void LoadTextData(Byte suffix)
		{
			String name = String.Format("{0}{1}", GetType().Name, suffix);
			LoadTextData(name);
		}
		protected void LoadTextData(String screen)
		{
			TextDataList = MyGame.Manager.TextManager.LoadTextData(screen);
		}

		protected static Vector2 GetBannerPosition(Texture2D texture)
		{
			return new Vector2((BaseData.ScreenWide - texture.Width) / 2.0f, BaseData.TilesSize);
		}
		protected static Vector2 GetVector2(Byte x, Byte y, Byte size)
		{
			Byte offset = (Byte)((BaseData.TilesSize - size) / 2.0f);
			return new Vector2(BaseData.GameOffsetX + x * BaseData.TilesSize + offset, y * BaseData.TextsSize + offset);
		}
		protected static Vector2 GetNewVector2(Byte x, Byte y, Byte size)
		{
			Byte offset = (Byte)((BaseData.TilesSize - size) / 2.0f);
			return new Vector2(BaseData.GameOffsetX + x * BaseData.TilesSize + offset, y * BaseData.TextsSize - offset);
		}

		protected static Boolean ImmediateExit()
		{
			return MyGame.Manager.InputManager.Escape() || MyGame.Manager.InputManager.Sides();
		}
		protected static Boolean ImmediateExit2()
		{
			return MyGame.Manager.InputManager.Escape();
		}
		protected static Boolean ReturnToMenu()
		{
			Boolean escape = MyGame.Manager.InputManager.Escape();
			if (escape)
			{
				return true;
			}
			Boolean space = MyGame.Manager.InputManager.Next();
			if (space)
			{
				return true;
			}

			return false;
		}
	}

}