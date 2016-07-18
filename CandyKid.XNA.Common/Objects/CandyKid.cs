using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Static;

namespace WindowsGame.Objects
{
	public class CandyKid : BaseObject
	{
		private SByte minTileMinusOne, minTileMinusTwo;
		private Byte maxTilePlusOne, maxTilePlusTwo;

		public CandyKid(Byte baseX, Byte baseY, Vector2 basePosition, Rectangle baseSource, Byte increase, Byte velocity, Byte distance, Byte gamerSize) :
			base(baseX, baseY, basePosition, baseSource, increase, velocity, distance, gamerSize)
		{
		}

		public void Initialize(Byte minTile, Byte maxTile)
		{
			minTileMinusOne = (SByte)(minTile - 1);
			minTileMinusTwo = (SByte)(minTile - 2);
			maxTilePlusOne = (Byte)(maxTile + 1);
			maxTilePlusTwo = (Byte)(maxTile + 2);
		}
		public override void Move(Direction direction)
		{
			base.Move(direction);
			SwapFrame();
		}

		public override void Update(GameTime gameTime)
		{
			UpdatePosition(gameTime);

			// Swap frame half way.
			if (1 == Frame && Delta >= Distance / 2.0f)
			{
				SwapFrame();
			}

			UpdateLifeCycle(gameTime);
		}

		public override void Stop()
		{
			// Cannot wrap if dead.
			if (Lifecycle.Dead == Lifecycle)
			{
				return;
			}

			if (minTileMinusOne == CurrX || minTileMinusOne == CurrY)
			{
				Move(Direction);
			}
			else if (minTileMinusTwo == CurrX || maxTilePlusTwo == CurrX || minTileMinusTwo == CurrY || maxTilePlusTwo == CurrY)
			{
				GameWrap();
				Move(Direction);
			}
			else if (maxTilePlusOne == CurrX || maxTilePlusOne == CurrY)
			{
				Move(Direction);
			}
			else
			{
				None();
			}
		}

		public void Swap()
		{
			Direction = Direction.None;
			Lifecycle = Lifecycle.Swap;
			SwapFrame();
		}

		private void GameWrap()
		{
			Vector2 position = Vector2.Zero;
			position.X = Position.X;
			position.Y = Position.Y;

			Byte tilesSize = BaseData.TilesSize;
			Byte gameOffsetX = BaseData.GameOffsetX;
			Byte entityOffset = BaseData.EntityOffset;

			if (Direction.Left == Direction)
			{
				CurrX = (SByte)maxTilePlusTwo;
				position.X = CurrX * tilesSize + tilesSize + gameOffsetX + entityOffset;
			}
			else if (Direction.Right == Direction)
			{
				CurrX = minTileMinusTwo;
				position.X = CurrX * tilesSize + tilesSize + gameOffsetX + entityOffset;
			}
			if (Direction.Up == Direction)
			{
				CurrY = (SByte)maxTilePlusTwo;
				position.Y = CurrY * tilesSize + tilesSize + entityOffset;
			}
			else if (Direction.Down == Direction)
			{
				CurrY = minTileMinusTwo;
				position.Y = CurrY * tilesSize + tilesSize + entityOffset;
			}

			Position = position;
		}

		public void MenuWrap(SByte x, SByte y, Int16 posX, Int16 posY)
		{
			CurrX = x;
			CurrY = y;
			Vector2 position = Position;
			position.X += posX;
			position.Y += posY;
			Position = position;
		}

		public void SetCurrY(Byte currY)
		{
			CurrY = (SByte)currY;
		}
		public void SetPosition(Vector2 position)
		{
			Position = position;
		}

	}
}