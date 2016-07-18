using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IBorderManager
	{
		void LoadContent();
		void LoadContentData(Byte borderGame, Byte borderMenu, Byte borderHigh, Byte popupsWide, Byte popupsHigh, Byte tilesSize, Byte textsSize, Byte gameOffsetX);
		IDictionary<Byte, Vector2> LoadAllTiles(Byte borderGame, Byte borderMenu, Byte borderHigh, Byte tilesSize, Byte gameOffsetX);
		IList<Vector2> LoadAllPopup(Byte borderGame, Byte borderHigh, Byte popupsWide, Byte popupsHigh, Byte tilesSize, Byte gameOffsetX);
		IList<Vector2> LoadAllStrip(Byte borderGame, Byte borderHigh, Byte popupsWide, Byte popupsHigh, Byte tilesSize, Byte textsSize, Byte gameOffsetX);
		void UpdateContentData(Boolean useKillTrees, Boolean useOpenExits);
		void DrawGame();
		void DrawMenu();
		void DrawLock(Boolean locked);
		void DrawOver();
	}

	public class BorderManager : IBorderManager
	{
		private IList<Byte> menuOpen, menuKill, gameOpen, gameKill;
		private IList<Byte> gameList, menuList;
		private Rectangle rectThis, rectKill, rectNorm, rectLock;

		public void LoadContent()
		{
			LoadContentData(BaseData.BorderGame, BaseData.BorderMenu, BaseData.BorderHigh, BaseData.PopupsWide,
							BaseData.PopupsHigh, BaseData.TilesSize, BaseData.TextsSize, BaseData.GameOffsetX);
		}

		public void LoadContentData(Byte borderGame, Byte borderMenu, Byte borderHigh, Byte popupsWide, Byte popupsHigh, Byte tilesSize, Byte textsSize, Byte gameOffsetX)
		{
			AllBorderTiles = LoadAllTiles(borderGame, borderMenu, borderHigh, tilesSize, gameOffsetX);
			AllPopupsTiles = LoadAllPopup(borderGame, borderHigh, popupsWide, popupsHigh, tilesSize, gameOffsetX);
			AllPopupsStrip = LoadAllStrip(borderGame, borderHigh, popupsWide, popupsHigh, tilesSize, textsSize, gameOffsetX);

			menuKill = LoadMenuKill();
			menuOpen = LoadMenuOpen();
			gameKill = LoadGameKill();
			gameOpen = LoadGameOpen();

			rectKill = MyGame.Manager.ImageManager.TreesElectricRectangle;
			rectNorm = MyGame.Manager.ImageManager.TreesStandardRectangle;
			rectLock = MyGame.Manager.ImageManager.TreesLockGameRectangle;

			UpdateContentData(BaseData.UseKillTrees, BaseData.UseOpenExits);
		}

		public void UpdateContentData(Boolean useKillTrees, Boolean useOpenExits)
		{
			rectThis = useKillTrees ? rectKill : rectNorm;
			gameList = useOpenExits ? gameOpen : gameKill;
			menuList = useOpenExits ? menuOpen : menuKill;
		}

		public void DrawMenu()
		{
			DrawTrees(rectThis, menuList);
		}
		public void DrawGame()
		{
			DrawTrees(rectThis, gameList);
		}
		public void DrawLock(Boolean locked)
		{
			Rectangle rectCurr = locked ? rectLock : rectThis;
			DrawTrees(rectCurr, gameList);
		}
		public void DrawOver()
		{
			for (Byte index = 0; index < AllPopupsTiles.Count; ++index)
			{
				Vector2 position = AllPopupsTiles[index];
				Engine.SpriteBatch.Draw(Assets.TilemapsTexture, position, rectThis, Color.White);
			}
			for (Byte index = 0; index < AllPopupsStrip.Count; ++index)
			{
				Vector2 position = AllPopupsStrip[index];
				Engine.SpriteBatch.Draw(Assets.NewArrowTexture, position, MyGame.Manager.ImageManager.BlackStripRectangle, Color.White);
			}
		}

		public IDictionary<Byte, Vector2> LoadAllTiles(Byte borderGame, Byte borderMenu, Byte borderHigh, Byte tilesSize, Byte gameOffsetX)
		{
			var allBorderTiles = new Dictionary<Byte, Vector2>();

			Byte key = 0;
			Byte height = 0;
			Byte width;
			Byte widthMinusOne = (Byte)(borderMenu - 1);
			Byte heightMinusOne = (Byte)(borderHigh - 1);

			// Top: left to right.
			for (width = 0; width < borderMenu; ++width)
			{
				allBorderTiles.Add(key++, GetPosition(width, height, tilesSize, gameOffsetX));
			}

			// Right: top to bottom (#1).
			width = (Byte)(borderMenu - 1);
			for (height = 1; height < heightMinusOne; ++height)
			{
				allBorderTiles.Add(key++, GetPosition(width, height, tilesSize, gameOffsetX));
			}

			// Bottom: right to left;.
			height = (Byte)(borderHigh - 1);
			for (width = widthMinusOne; width > 0; --width)
			{
				allBorderTiles.Add(key++, GetPosition(width, height, tilesSize, gameOffsetX));
			}

			// Left: bottom to top.
			width = 0;
			for (height = heightMinusOne; height > 0; --height)
			{
				allBorderTiles.Add(key++, GetPosition(width, height, tilesSize, gameOffsetX));
			}

			// Right: top to bottom (#2).
			width = (Byte)(borderGame - 1);
			for (height = 1; height < heightMinusOne; ++height)
			{
				allBorderTiles.Add(key++, GetPosition(width, height, tilesSize, gameOffsetX));
			}

			return allBorderTiles;
		}

		public IList<Vector2> LoadAllPopup(Byte borderGame, Byte borderHigh, Byte popupsWide, Byte popupsHigh, Byte tilesSize, Byte gameOffsetX)
		{
			var allBorderPopup = new List<Vector2>();

			Byte startX = (Byte)((borderGame - popupsWide) / 2);
			Byte startY = (Byte)((borderHigh - popupsHigh) / 2);

			// Top: left to right.
			for (Byte wide = startX; wide < (startX + popupsWide); ++wide)
			{
				allBorderPopup.Add(GetPosition(wide, startY, tilesSize, gameOffsetX));
				allBorderPopup.Add(GetPosition(wide, (Byte)(startY + popupsHigh - 1), tilesSize, gameOffsetX));
			}

			// Right: top to bottom.
			for (Byte high = (Byte)(startY + 1); high < (Byte)(startY + 1 + popupsHigh - 2); ++high)
			{
				allBorderPopup.Add(GetPosition(startX, high, tilesSize, gameOffsetX));
				allBorderPopup.Add(GetPosition((Byte)(startX + popupsWide - 1), high, tilesSize, gameOffsetX));
			}

			return allBorderPopup;
		}

		public IList<Vector2> LoadAllStrip(Byte borderGame, Byte borderHigh, Byte popupsWide, Byte popupsHigh, Byte tilesSize, Byte textsSize, Byte gameOffsetX)
		{
			var allBorderStrip = new List<Vector2>();

			Byte startX = (Byte)(((borderGame - popupsWide) / 2) + 1);
			Byte startY = (Byte)(((borderHigh - popupsHigh) / 2) + 1);

			// Top: left to right.
			for (Byte index = 0; index < 2; ++index)
			{
				for (Byte wide = startX; wide < (startX + popupsWide - 2); ++wide)
				{
					Vector2 position1 = GetPosition((Byte)(wide * 2 + 0), (Byte)(startY * 2 + 0), textsSize, gameOffsetX);
					Vector2 position2 = GetPosition((Byte)(wide * 2 + 1), (Byte)(startY * 2 + 0), textsSize, gameOffsetX);
					Vector2 position3 = GetPosition((Byte)(wide * 2 + 0), (Byte)(startY * 2 + 1), textsSize, gameOffsetX);
					Vector2 position4 = GetPosition((Byte)(wide * 2 + 1), (Byte)(startY * 2 + 1), textsSize, gameOffsetX);
					allBorderStrip.Add(position1); allBorderStrip.Add(position2);
					allBorderStrip.Add(position3); allBorderStrip.Add(position4);
				}

				startY += 1;
			}

			return allBorderStrip;
		}

		private static Vector2 GetPosition(Byte width, Byte height, Byte tilesSize, Byte gameOffsetX)
		{
			return new Vector2(width * tilesSize + gameOffsetX, height * tilesSize);
		}
		private static Vector2 GetPosition2(Byte width, Byte height, Byte textsSize, Byte gameOffsetX)
		{
			return new Vector2(width * textsSize + gameOffsetX, height * textsSize);
		}
		private static List<Byte> LoadMenuKill()
		{
			return new List<Byte> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51 };
		}
		private static List<Byte> LoadMenuOpen()
		{
			return new List<Byte> { 0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 17, 19, 20, 21, 22, 24, 25, 26, 27, 28, 29, 31, 32, 33, 34, 35, 36, 38, 39, 40, 41, 42, 43, 45, 46, 47, 48, 50, 51 };
		}
		private static List<Byte> LoadGameKill()
		{
			return new List<Byte> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61 };
		}
		private static List<Byte> LoadGameOpen()
		{
			return new List<Byte> { 0, 1, 2, 4, 5, 6, 7, 9, 10, 11, 30, 31, 32, 34, 35, 36, 37, 39, 40, 41, 42, 43, 45, 46, 47, 48, 50, 51, 52, 53, 55, 56, 57, 58, 60, 61 };
		}
		private void DrawTrees(Rectangle theRectangle, IList<Byte> treeList)
		{
			for (Byte index = 0; index < treeList.Count; ++index)
			{
				Byte key = treeList[index];
				Vector2 position = AllBorderTiles[key];

				Engine.SpriteBatch.Draw(Assets.TilemapsTexture, position, theRectangle, Color.White);
			}
		}

		private IDictionary<Byte, Vector2> AllBorderTiles { get; set; }
		private IList<Vector2> AllPopupsTiles { get; set; }
		private IList<Vector2> AllPopupsStrip { get; set; }
	}
}