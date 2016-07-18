using System;
using WindowsGame.Data;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface ICollisionManager
	{
		Boolean CheckEnemyCollisionExit(SByte playerX, SByte playerY);
		Boolean CheckEnemyCollisionExit(Byte minTile, Byte maxTile, SByte playerX, SByte playerY);
		Boolean CheckEnemyCollisionFast(Byte playerX, Byte playerY, Byte enemyX, Byte enemyY);
		Boolean CheckEnemyCollisionSlow(Byte theGamerCrash, Single playerPosX, Single playerPosY, Single enemyPosX, Single enemyPosY);

		EventType CheckTilesCollision(TileType[,] boardData, SByte playerX, SByte playerY, Direction direction);
		EventType CheckTilesCollision(TileType[,] boardData, SByte playerX, SByte playerY, Direction direction, Boolean useOpenExits, Byte minTile, Byte maxTile, Byte exitLower, Byte exitUpper);

		Byte GetGamerCrash();
		void SetGamerCrash(Byte crashMin, Byte crashMax, Byte scoreWorld);
	}

	public class CollisionManager : ICollisionManager
	{
		private Byte gamerCrash;

		public Boolean CheckEnemyCollisionExit(SByte playerX, SByte playerY)
		{
			return CheckEnemyCollisionExit(BaseData.MinTile, BaseData.MaxTile, playerX, playerY);
		}
		public Boolean CheckEnemyCollisionExit(Byte minTile, Byte maxTile, SByte playerX, SByte playerY)
		{
			return playerX < minTile || playerX > maxTile || playerY < minTile || playerY > maxTile;
		}

		public Boolean CheckEnemyCollisionFast(Byte playerX, Byte playerY, Byte enemyX, Byte enemyY)
		{
			Byte deltaX = (Byte)Math.Abs(playerX - enemyX);
			Byte deltaY = (Byte)Math.Abs(playerY - enemyY);

			return deltaX <= 1 && deltaY <= 1;
		}

		public Boolean CheckEnemyCollisionSlow(Byte theGamerCrash, Single playerPosX, Single playerPosY, Single enemyPosX, Single enemyPosY)
		{
			Single deltaX = Math.Abs(playerPosX - enemyPosX);
			Single deltaY = Math.Abs(playerPosY - enemyPosY);

			return deltaX < theGamerCrash && deltaY < theGamerCrash;
		}

		public EventType CheckTilesCollision(TileType[,] boardData, SByte playerX, SByte playerY, Direction direction)
		{
			return CheckTilesCollision(boardData, playerX, playerY, direction, BaseData.UseOpenExits, BaseData.MinTile, BaseData.MaxTile, BaseData.ExitLower, BaseData.ExitUpper);
		}

		public EventType CheckTilesCollision(TileType[,] boardData, SByte playerX, SByte playerY, Direction direction, Boolean useOpenExits, Byte minTile, Byte maxTile, Byte exitLower, Byte exitUpper)
		{
			switch (direction)
			{
				case Direction.Left:
					{
						if (useOpenExits)
						{
							if (minTile - 2 == playerX || maxTile + 1 == playerX || maxTile + 2 == playerX)
							{
								return EventType.None;
							}
							if (minTile - 1 == playerX && ((Byte)(exitLower - 1) == playerY || (Byte)(exitUpper - 1) == playerY))
							{
								return EventType.None;
							}
						}
						if (minTile - 1 == playerX)
						{
							return EventType.DeathTree;
						}
						break;
					}
				case Direction.Right:
					{
						if (useOpenExits)
						{
							if (maxTile + 2 == playerX || minTile - 1 == playerX || minTile - 2 == playerX)
							{
								return EventType.None;
							}
							if (maxTile + 1 == playerX && ((Byte)(exitLower - 1) == playerY || (Byte)(exitUpper - 1) == playerY))
							{
								return EventType.None;
							}
						}
						if (maxTile + 1 == playerX)
						{
							return EventType.DeathTree;
						}
						break;
					}
				case Direction.Up:
					{
						if (useOpenExits)
						{
							if (minTile - 2 == playerY || maxTile + 1 == playerY || maxTile + 2 == playerY)
							{
								return EventType.None;
							}
							if (minTile - 1 == playerY && ((Byte)(exitLower - 1) == playerX || (Byte)(exitUpper - 1) == playerX))
							{
								return EventType.None;
							}
						}
						if (minTile - 1 == playerY)
						{
							return EventType.DeathTree;
						}
						break;
					}
				case Direction.Down:
					{
						if (useOpenExits)
						{
							if (maxTile + 2 == playerY || minTile - 1 == playerY || minTile - 2 == playerY)
							{
								return EventType.None;
							}
							if (maxTile + 1 == playerY && ((Byte)(exitLower - 1) == playerX || (Byte)(exitUpper - 1) == playerX))
							{
								return EventType.None;
							}
						}
						if (maxTile + 1 == playerY)
						{
							return EventType.DeathTree;
						}
						break;
					}
				default:
					{
						return EventType.None;
					}
			}

			TileType tileType = boardData[(Byte)(playerY), (Byte)(playerX)];
			if (TileType.Trees == tileType)
			{
				return EventType.DeathTree;
			}
			if (TileType.Candy == tileType)
			{
				return EventType.EatCandy;
			}
			if (TileType.Bonus1 == tileType)
			{
				return EventType.EatBonus1;
			}
			if (TileType.Bonus2 == tileType)
			{
				return EventType.EatBonus2;
			}
			if (TileType.Bonus3 == tileType)
			{
				return EventType.EatBonus3;
			}
			if (TileType.Bonus4 == tileType)
			{
				return EventType.EatBonus4;
			}

			return EventType.None;
		}

		public Byte GetGamerCrash()
		{
			return gamerCrash;
		}
		public void SetGamerCrash(Byte crashMin, Byte crashMax, Byte scoreWorld)
		{
			Byte theWorld = scoreWorld;
			if (theWorld > crashMin)
			{
				theWorld = crashMin;
			}

			gamerCrash = (Byte)(crashMin + theWorld - 1);
			if (gamerCrash > crashMax)
			{
				gamerCrash = crashMax;
			}
		}

	}
}