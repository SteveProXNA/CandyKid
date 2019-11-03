using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IBoardManager
	{
		void Initialize();
		void Initialize(String root, Byte minTile, Byte maxTile, Byte validWide, Byte validHigh, Byte tilesSize, Byte gameOffsetX);
		void LoadContent(Byte world, Byte round);
		void XtraContent();

		TileType[,] LoadLevelData(Byte world, Byte round, Byte width, Byte height);
		TileType[,] ParseBoardData(IList<String> lineBoardData, Byte width, Byte height);
		TileType[,] CleanBoardData(TileType[,] tempBoardData,  Byte width, Byte height, Byte playerPosition, Boolean moveAdriana, Boolean moveSuzanne, Boolean moveStevepro, Byte adrianaPosition, Byte suzannePosition, Byte steveproPosition);

		TileType HardCodeExits(TileType tileType, Byte width, Byte height, Byte row, Byte col, Boolean useOpenExits, Byte exitLower, Byte exitUpper, Byte minTile, Byte maxTile);
		Byte CalcLocation(Byte x, Byte y);
		void CalcPosition(Byte location, out Byte row, out Byte col);
		void CalcPosition(Byte location, Byte wide, Byte high, out Byte row, out Byte col);
		void ClearTreesTile(Byte location);
		void ClearBonusTile(Byte location);
		void ClearCandyTile(Byte location);

		TileType ConvertCharToTile(Char data);
		Char ConvertTileToChar(TileType tileType);
		void Draw();
		void Draw(Boolean locked);
		//void Draw(TileType[,] levelData, IDictionary<Int16, Vector2> tilesData, IDictionary<Int16, Rectangle> rectsData);

		Vector2 GetCandyVector2(Byte key);
		Vector2 GetBonusVector2(Byte key);

		Byte CandyCount { get; }
		Byte BonusCount { get; }
		TileType[,] BoardData { get; }
		
		IDictionary<Int16, Vector2> BoardBasePositions { get; }
		IDictionary<Int16, Vector2> BoardTilePositions { get; }
		IDictionary<Int16, Rectangle> BoardRectangles { get; }
	}

	public class BoardManager : IBoardManager
	{
		private String boardRoot;
		private Byte validWide, validHigh;
		private IList<Byte> bonusSquares;

		public void Initialize()
		{
			Initialize(String.Empty, BaseData.MinTile, BaseData.MaxTile, BaseData.ValidWide, BaseData.ValidHigh, BaseData.TilesSize, BaseData.GameOffsetX);
		}
		public void Initialize(String root, Byte minTile, Byte maxTile, Byte theValidWide, Byte theValidHigh, Byte tilesSize, Byte gameOffsetX)
		{
			boardRoot = root;

			BoardBasePositions = new Dictionary<Int16, Vector2>();
			BoardTilePositions = new Dictionary<Int16, Vector2>();
			BoardRectangles = new Dictionary<Int16, Rectangle>();

			validWide = theValidWide;
			validHigh = theValidHigh;

			for (Byte high = minTile; high < validHigh; ++high)
			{
				for (Byte wide = minTile; wide < validWide; ++wide)
				{
					Byte key = (Byte)(high * validWide + wide);
					BoardBasePositions[key] = new Vector2(wide * tilesSize + gameOffsetX + tilesSize, high * tilesSize + tilesSize);
				}
			}

			bonusSquares = new List<Byte>();
		}

		public void LoadContent(Byte world, Byte round)
		{
			BoardData = LoadLevelData(world, round);
			UInt16 level = BaseData.GetLevelDataKey(world, round);
			LoadContentInfo(level);
		}

		private void LoadContentInfo(UInt16 level)
		{
			// Clone original dictionary.
			BoardTilePositions = (from x in BoardBasePositions select x).ToDictionary(x => x.Key, x => x.Value);
			for (Byte row = 0; row < validHigh; ++row)
			{
				for (Byte col = 0; col < validWide; ++col)
				{
					Byte key = (Byte)(row * validHigh + col);
					TileType tileType = BoardData[row, col];

					if (TileType.Trees == tileType)
					{
						Rectangle rect = BaseData.UseKillTrees
							? MyGame.Manager.ImageManager.TreesElectricRectangle
							: MyGame.Manager.ImageManager.TreesStandardRectangle;
						BoardRectangles[key] = rect;
					}
					else if (TileType.Candy == tileType)
					{
						Byte candy = (Byte)(MyGame.Manager.NumberManager.Generate(Constants.CANDY_NUMBER - 1));
						BoardRectangles[key] = MyGame.Manager.ImageManager.AllCandyRectangles[candy];
						BoardTilePositions[key] = GetCandyVector2(key);
					}
					else if (tileType >= TileType.Bonus1 && tileType <= TileType.Bonus4)
					{
						BoardRectangles[key] = GetBonusRectangle(tileType, level);
						BoardTilePositions[key] = GetBonusVector2(key);
					}
				}
			}
		}

		public void XtraContent()
		{
			// Set one of the four available corners to bonus white candy!
			bonusSquares.Clear();
			if (!BaseData.MoveAdriana && !BaseData.MoveSuzanne && !BaseData.MoveStevePro)
			{
				bonusSquares.Add(BaseData.PlayerPosition);
			}

			if (BaseData.MoveAdriana) { bonusSquares.Add(BaseData.AdrianaPosition); }
			if (BaseData.MoveSuzanne) { bonusSquares.Add(BaseData.SuzannePosition); }
			if (BaseData.MoveStevePro) { bonusSquares.Add(BaseData.SteveProPosition); }

			Byte index = (Byte)MyGame.Manager.NumberManager.Generate(bonusSquares.Count);
			Byte bonusSquare = bonusSquares[index];
			BoardRectangles[bonusSquare] = MyGame.Manager.ImageManager.AllCandyRectangles[Constants.CANDY_NUMBER - 1];
		}

		public TileType[,] ParseBoardData(IList<String> lineBoardData, Byte width, Byte height)
		{
			TileType[,] tempBoardData = new TileType[width, height];
			for (Byte row = 0; row < height; ++row)
			{
				Char[] line = lineBoardData[row].ToCharArray();
				for (Byte col = 0; col < width; ++col)
				{
					Char data = line[col];

					TileType tileType = ConvertCharToTile(data);
					if (TileType.Trees == tileType)
					{
						tileType = HardCodeExits(tileType, width, height, col, row);
					}
					if (TileType.Candy == tileType)
					{
						CandyCount += 1;
					}
					if (TileType.Bonus1 == tileType || TileType.Bonus2 == tileType || TileType.Bonus3 == tileType || TileType.Bonus4 == tileType)
					{
						BonusCount += 1;
					}
					tempBoardData[row, col] = tileType;
				}
			}

			return tempBoardData;
		}

		public TileType[,] CleanBoardData(TileType[,] tempBoardData, Byte width, Byte height)
		{
			return CleanBoardData(tempBoardData, width, height, BaseData.PlayerPosition, BaseData.MoveAdriana, BaseData.MoveSuzanne, BaseData.MoveStevePro, BaseData.AdrianaPosition, BaseData.SuzannePosition, BaseData.SteveProPosition);
		}
		public TileType[,] CleanBoardData(TileType[,] tempBoardData, Byte width, Byte height, Byte playerPosition, Boolean moveAdriana, Boolean moveSuzanne, Boolean moveStevepro, Byte adrianaPosition, Byte suzannePosition, Byte steveproPosition)
		{
			CleanPlayerTile(tempBoardData, width, height, playerPosition);
			CleanEnemyTile(tempBoardData, width, height, moveAdriana, adrianaPosition);
			CleanEnemyTile(tempBoardData, width, height, moveSuzanne, suzannePosition);
			CleanEnemyTile(tempBoardData, width, height, moveStevepro, steveproPosition);

			return tempBoardData;
		}

		public TileType[,] LoadLevelData(Byte world, Byte round)
		{
			return LoadLevelData(world, round, validWide, validHigh);
		}
		public TileType[,] LoadLevelData(Byte world, Byte round, Byte wide, Byte high)
		{
			CandyCount = 0;
			BonusCount = 0;
			String file = GetLevelFile(world, round);
			var lines = MyGame.Manager.FileManager.LoadTxt(file);

			var tempBoardData = ParseBoardData(lines, wide, high);
			return CleanBoardData(tempBoardData, wide, high);
		}
		private void CleanPlayerTile(TileType[,] tempBoardData, Byte width, Byte height, Byte location)
		{
			Byte row;
			Byte col;

			CalcPosition(location, width, height, out row, out col);
			TileType tileType = tempBoardData[row, col];

			CleanTile(tempBoardData, row, col, tileType);
		}
		private void CleanEnemyTile(TileType[,] tempBoardData, Byte width, Byte height, Boolean moveEnemy, Byte location)
		{
			Byte row;
			Byte col;

			CalcPosition(location, width, height, out row, out col);
			TileType tileType = tempBoardData[row, col];

			if (!moveEnemy)
			{
				if (TileType.Bonus1 == tileType || TileType.Bonus2 == tileType || TileType.Bonus3 == tileType || TileType.Bonus4 == tileType)
				{
					BonusCount -= 1;
				}
				if (TileType.Candy == tileType)
				{
					CandyCount -= 1;
				}

				tempBoardData[row, col] = TileType.Empty;
			}
			else
			{
				CleanTile(tempBoardData, row, col, tileType);
			}
		}
		private void CleanTile(TileType[,] tempBoardData, Byte row, Byte col, TileType tileType)
		{
			if (TileType.Empty == tileType || TileType.Trees == tileType)
			{
				CandyCount += 1;
				tempBoardData[row, col] = TileType.Candy;
			}
			if (TileType.Bonus1 == tileType || TileType.Bonus2 == tileType || TileType.Bonus3 == tileType || TileType.Bonus4 == tileType)
			{
				BonusCount -= 1;
				CandyCount += 1;
				tempBoardData[row, col] = TileType.Candy;
			}
		}

		private TileType HardCodeExits(TileType tileType, Byte width, Byte height, Byte row, Byte col)
		{
			return HardCodeExits(tileType, width, height, col, row, BaseData.UseOpenExits, BaseData.ExitLower, BaseData.ExitUpper, BaseData.MinTile, BaseData.MaxTile);
		}

		public TileType HardCodeExits(TileType tileType, Byte width, Byte height, Byte row, Byte col, Boolean useOpenExits, Byte exitLower, Byte exitUpper, Byte minTile, Byte maxTile)
		{
			if (CalcHardCodeExits(row, col, exitLower, exitUpper, minTile, maxTile))
			{
				return useOpenExits ? TileType.Candy : TileType.Trees;
			}

			return tileType;
		}

		private static Boolean CalcHardCodeExits(Byte row, Byte col, Byte exitLower, Byte exitUpper, Byte minTile, Byte maxTile)
		{
			return	(row == (Byte)(exitLower - 1)) && (col == minTile || col == maxTile) ||
					(row == (Byte)(exitUpper - 1)) && (col == minTile || col == maxTile) ||
					(col == (Byte)(exitLower - 1)) && (row == minTile || row == maxTile) ||
					(col == (Byte)(exitUpper - 1)) && (row == minTile || row == maxTile);
		}
		public Byte CalcLocation(Byte x, Byte y)
		{
			return (Byte)(y * validHigh + x);
		}
		public void CalcPosition(Byte location, out Byte row, out Byte col)
		{
			CalcPosition(location, validWide, validHigh, out row, out col);
		}
		public void CalcPosition(Byte location, Byte wide, Byte high, out Byte row, out Byte col)
		{
			row = (Byte)(location / high);
			col = (Byte)(location % wide);
		}

		public void ClearTreesTile(Byte location)
		{
			ClearTile(location);
		}
		public void ClearBonusTile(Byte location)
		{
			BonusCount -= 1;
			ClearTile(location);
		}
		public void ClearCandyTile(Byte location)
		{
			CandyCount -= 1;
			ClearTile(location);
		}
		private void ClearTile(Byte location)
		{
			Byte row, col;
			CalcPosition(location, out row, out col);

			BoardData[row, col] = TileType.Empty;
		}

		public TileType ConvertCharToTile(Char data)
		{
			String text = data.ToString();
			Byte conv = Convert.ToByte(text);
			return (TileType)conv;
		}
		public Char ConvertTileToChar(TileType tileType)
		{
			Byte conv = Convert.ToByte(tileType);
			String text = conv.ToString();

			Char result;
			Char.TryParse(text, out result);
			return result;
		}

		public void Draw()
		{
			Draw(false);
		}

		public void Draw(Boolean locked)
		{
			for (Byte row = 0; row < validHigh; ++row)
			{
				for (Byte col = 0; col < validWide; ++col)
				{
					if (TileType.Empty == BoardData[row, col])
					{
						continue;
					}

					Byte key = (Byte)(row * validHigh + col);
					Vector2 position = BoardTilePositions[key];

					Rectangle source = BoardRectangles[key];
					if (TileType.Trees == BoardData[row, col] && locked)
					{
						source = MyGame.Manager.ImageManager.TreesLockGameRectangle;
					}

					Engine.SpriteBatch.Draw(Assets.TilemapsTexture, position, source, Color.White);
				}
			}
		}

		private static Rectangle GetBonusRectangle(TileType tileType, UInt16 level)
		{
			Byte index = 0;
			if (TileType.Bonus2 == tileType)
			{
				index = 1;
			}
			else if (TileType.Bonus3 == tileType)
			{
				index = 2;
			}
			else if (TileType.Bonus4 == tileType)
			{
				index = 3;
			}

			// Double up bonus from here on in...
			if (level > Constants.DIV_BONUS_VALUE)
			{
				index += Constants.BONUS_EXISTS;
			}

			return MyGame.Manager.ImageManager.AllBonusRectangles[index];
		}
		public Vector2 GetCandyVector2(Byte key)
		{
			return GetVector2(key, BaseData.CandySize);
		}
		public Vector2 GetBonusVector2(Byte key)
		{
			return GetVector2(key, BaseData.BonusSize);
		}
		private Vector2 GetVector2(Byte key, Byte size)
		{
			Vector2 position = BoardBasePositions[key];
			position.X += (BaseData.TilesSize - size) / 2.0f;
			position.Y += (BaseData.TilesSize - size) / 2.0f;
			return position;
		}

		private String GetLevelFile(UInt16 world, UInt16 round)
		{
			String worldName = world.ToString().PadLeft(2, '0');
			String roundName = round.ToString().PadLeft(2, '0');

			return String.Format("{0}{1}/{2}/{3}/{4}{5}/{6}{7}.txt", boardRoot, Constants.CONTENT_DIRECTORY, Constants.DATA_DIRECTORY, Constants.LEVELS_DIRECTORY, Constants.WORLD_FILENAME, worldName, Constants.ROUND_FILENAME, roundName);
		}

		public Byte BonusCount { get; private set; }
		public Byte CandyCount { get; private set; }
		public TileType[,] BoardData { get; private set; }
		public IDictionary<Int16, Vector2> BoardBasePositions { get; private set; }
		public IDictionary<Int16, Vector2> BoardTilePositions { get; private set; }
		public IDictionary<Int16, Rectangle> BoardRectangles { get; private set; }
	}
}