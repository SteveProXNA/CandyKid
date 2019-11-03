using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IImageManager
	{
		void LoadContent();
		void LoadContentData(Byte tileRatio, Byte arrowSize, Byte treesSize, Byte gamerSize, Byte bonusSize, Byte candySize);

		Rectangle TreesStandardRectangle { get; }
		Rectangle TreesElectricRectangle { get; }
		Rectangle TreesLockGameRectangle { get; }

		Rectangle DeathStandardRectangle { get; }
		Rectangle DeathElectricRectangle { get; }
		Rectangle BlackStripRectangle { get; }

		Rectangle[] GamerOneRectangle { get; }
		Rectangle[] EnemyOneRectangle { get; }
		Rectangle[] EnemyTwoRectangle { get; }
		Rectangle[] EnemyXyzRectangle { get; }

		IDictionary<ArrowType, Rectangle> AllArrowRectangles { get; }
		IDictionary<Int16, Rectangle> AllBonusRectangles { get; }
		IDictionary<Int16, Rectangle> AllCandyRectangles { get; }
	}

	public class ImageManager : IImageManager
	{
		private Byte tileRatio;

		public void LoadContent()
		{
			LoadContentData(BaseData.TileRatio, BaseData.ArrowSize, BaseData.TreesSize, BaseData.GamerSize, BaseData.BonusSize, BaseData.CandySize);
		}

		public void LoadContentData(Byte theTileRatio, Byte arrowSize, Byte treesSize, Byte gamerSize, Byte bonusSize, Byte candySize)
		{
			tileRatio = theTileRatio;

			// Arrow.
			AllArrowRectangles = new Dictionary<ArrowType, Rectangle>(Constants.ARROW_NUMBER);
			AllArrowRectangles[ArrowType.WhiteTop] = GetRectangle(0, 0, arrowSize);
			AllArrowRectangles[ArrowType.WhiteLeft] = GetRectangle(8, 0, arrowSize);
			AllArrowRectangles[ArrowType.GrayTop] = GetRectangle(0, 8, arrowSize);
			AllArrowRectangles[ArrowType.GrayLeft] = GetRectangle(8, 8, arrowSize);

			// Bonus.
			const Byte bonusNumber = Constants.BONUS_NUMBER;
			AllBonusRectangles = new Dictionary<Int16, Rectangle>(bonusNumber);

			Byte incBonus = (Byte)(bonusSize / tileRatio);
			for (Byte key = 0; key < bonusNumber / 2; ++key)
			{
				AllBonusRectangles[(Byte)(key + 0)] = GetRectangle(key * incBonus, 09, bonusSize);
			}
			for (Byte key = 0; key < bonusNumber / 2; ++key)
			{
				AllBonusRectangles[(Byte)(key + Constants.BONUS_EXISTS)] = GetRectangle(key * incBonus, 12, bonusSize);
			}

			// Candy.
			const Byte candyNumber = Constants.CANDY_NUMBER;
			AllCandyRectangles = new Dictionary<Int16, Rectangle>(candyNumber);

			Byte incCandy = (Byte)(candySize / tileRatio);
			for (Byte key = 0; key < theTileRatio; ++key)
			{
				AllCandyRectangles[key] = GetRectangle(key * incCandy, 5, candySize);
			}
			for (Byte key = 0; key < (candyNumber - theTileRatio); ++key)
			{
				AllCandyRectangles[(Byte)(key + tileRatio)] = GetRectangle(key * incCandy, 7, candySize);
			}

			// Death.
			DeathStandardRectangle = GetRectangle(12, 07, gamerSize);
			DeathElectricRectangle = GetRectangle(12, 11, gamerSize);

			// Gamer.
			GamerOneRectangle = new Rectangle[Constants.SPRITE_NUMBER];
			GamerOneRectangle[0] = GetRectangle(0, 0, gamerSize);
			GamerOneRectangle[1] = GetRectangle(8, 0, gamerSize);

			// Adriana.
			EnemyOneRectangle = new Rectangle[Constants.SPRITE_NUMBER];
			EnemyOneRectangle[0] = GetRectangle(0, 8, gamerSize);
			EnemyOneRectangle[1] = GetRectangle(8, 8, gamerSize);

			// Suzanne.
			EnemyTwoRectangle = new Rectangle[Constants.SPRITE_NUMBER];
			EnemyTwoRectangle[0] = GetRectangle(0, 12, gamerSize);
			EnemyTwoRectangle[1] = GetRectangle(8, 12, gamerSize);

			// StevePro.
			EnemyXyzRectangle = new Rectangle[Constants.SPRITE_NUMBER];
			EnemyXyzRectangle[0] = GetRectangle(0, 4, gamerSize);
			EnemyXyzRectangle[1] = GetRectangle(8, 4, gamerSize);

			// Strip.
			BlackStripRectangle = GetRectangle(0, 6, (Byte)(treesSize / 2));

			// Trees.
			TreesStandardRectangle = GetRectangle(00, 0, treesSize);
			TreesElectricRectangle = GetRectangle(05, 0, treesSize);
			TreesLockGameRectangle = GetRectangle(10, 0, treesSize);
		}

		private Rectangle GetRectangle(Int32 x, Int32 y, Byte size)
		{
			return GetRectangle(x, y, size, size);
		}
		private Rectangle GetRectangle(Int32 x, Int32 y, Byte wide, Byte high)
		{
			return new Rectangle(x * tileRatio, y * tileRatio, wide, high);
		}

		public Rectangle TreesStandardRectangle { get; private set; }
		public Rectangle TreesElectricRectangle { get; private set; }
		public Rectangle TreesLockGameRectangle { get; private set; }

		public Rectangle DeathStandardRectangle { get; private set; }
		public Rectangle DeathElectricRectangle { get; private set; }
		public Rectangle BlackStripRectangle { get; private set; }

		public Rectangle[] GamerOneRectangle { get; private set; }
		public Rectangle[] EnemyOneRectangle { get; private set; }
		public Rectangle[] EnemyTwoRectangle { get; private set; }
		public Rectangle[] EnemyXyzRectangle { get; private set; }

		public IDictionary<ArrowType, Rectangle> AllArrowRectangles { get; private set; }
		public IDictionary<Int16, Rectangle> AllBonusRectangles { get; private set; }
		public IDictionary<Int16, Rectangle> AllCandyRectangles { get; private set; }
	}
}