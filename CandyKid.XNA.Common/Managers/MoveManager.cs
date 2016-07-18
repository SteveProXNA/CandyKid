using System;
using System.Collections.Generic;
using WindowsGame.Data;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IMoveManager
	{
		void Initialize();

		EventType CheckDirection(TileType[,] boardData, SByte playerX, SByte playerY, Direction direction);
		EventType CheckDirection(TileType[,] boardData, SByte playerX, SByte playerY, Direction direction, Boolean useOpenExits, Byte minTile, Byte maxTile, Byte exitLower, Byte exitUpper);

		IList<Direction> CheckFreeMoves(TileType[,] boardData, Byte enemyX, Byte enemyY);
		IList<Direction> CheckFreeMoves(TileType[,] boardData, Byte enemyX, Byte enemyY, Byte minTile, Byte maxTile, Byte exitLower, Byte exitUpper);
	}

	public class MoveManager : IMoveManager
	{
		private IList<Direction> enemyFreeMoves;

		public void Initialize()
		{
			enemyFreeMoves = new List<Direction>();
		}

		public EventType CheckDirection(TileType[,] boardData, SByte playerX, SByte playerY, Direction direction)
		{
			return CheckDirection(boardData, playerX, playerY, direction, BaseData.UseOpenExits, BaseData.MinTile, BaseData.MaxTile, BaseData.ExitLower, BaseData.ExitUpper);
		}
		public EventType CheckDirection(TileType[,] boardData, SByte playerX, SByte playerY, Direction direction, Boolean useOpenExits)
		{
			return CheckDirection(boardData, playerX, playerY, direction, useOpenExits, BaseData.MinTile, BaseData.MaxTile, BaseData.ExitLower, BaseData.ExitUpper);
		}
		public EventType CheckDirection(TileType[,] boardData, SByte playerX, SByte playerY, Direction direction, Boolean useOpenExits, Byte minTile, Byte maxTile, Byte exitLower, Byte exitUpper)
		{
			TileType tileType;
			switch (direction)
			{
				case Direction.Left:
					{
						if (playerX <= minTile)
						{
							if (useOpenExits)
							{
								return ((Byte)(exitLower - 1) == playerY || (Byte)(exitUpper - 1) == playerY)
									? EventType.EntityFree
									: EventType.EntityStay;
							}

							return EventType.EntityStay;
						}

						tileType = boardData[(Byte)(playerY), (Byte)(playerX - 1)];
						break;
					}
				case Direction.Right:
					{
						if (playerX >= maxTile)
						{
							if (useOpenExits)
							{
								return ((Byte)(exitLower - 1) == playerY || (Byte)(exitUpper - 1) == playerY)
									? EventType.EntityFree
									: EventType.EntityStay;
							}

							return EventType.EntityStay;
						}

						tileType = boardData[(Byte)(playerY), (Byte)(playerX + 1)];
						break;
					}
				case Direction.Up:
					{
						if (playerY <= minTile)
						{
							if (useOpenExits)
							{
								return ((Byte)(exitLower - 1) == playerX || (Byte)(exitUpper - 1) == playerX)
									? EventType.EntityFree
									: EventType.EntityStay;
							}

							return EventType.EntityStay;
						}

						tileType = boardData[(Byte)(playerY - 1), (Byte)(playerX)];
						break;
					}
				case Direction.Down:
					{
						if (playerY >= maxTile)
						{
							if (useOpenExits)
							{
								return ((Byte)(exitLower - 1) == playerX || (Byte)(exitUpper - 1) == playerX)
									? EventType.EntityFree
									: EventType.EntityStay;
							}

							return EventType.EntityStay;
						}

						tileType = boardData[(Byte)(playerY + 1), (Byte)(playerX)];
						break;
					}
				default:
					{
						return EventType.None;
					}
			}

			return TileType.Trees == tileType ? EventType.EntityStay : EventType.EntityFree;
		}


		public IList<Direction> CheckFreeMoves(TileType[,] boardData, Byte enemyX, Byte enemyY)
		{
			return CheckFreeMoves(boardData, enemyX, enemyY, BaseData.MinTile, BaseData.MaxTile, BaseData.ExitLower, BaseData.ExitUpper);
		}
		public IList<Direction> CheckFreeMoves(TileType[,] boardData, Byte enemyX, Byte enemyY, Byte minTile, Byte maxTile, Byte exitLower, Byte exitUpper)
		{
			// Enemies cannot use open exits!
			const Boolean useOpenExits = false;

			enemyFreeMoves.Clear();
			for (Direction direction = Direction.Left; direction <= Direction.Down; ++direction)
			{
				if (Direction.None != CheckThisDirection(boardData, enemyX, enemyY, direction, useOpenExits, minTile, maxTile, exitLower, exitUpper))
				{
					enemyFreeMoves.Add(direction);
				}
			}

			return enemyFreeMoves;
		}

		private Direction CheckThisDirection(TileType[,] boardData, Byte x, Byte y, Direction direction, Boolean useOpenExits, Byte minTile, Byte maxTile, Byte exitLower, Byte exitUpper)
		{
			EventType eventType = CheckDirection(boardData, (SByte)x, (SByte)y, direction, useOpenExits, minTile, maxTile, exitLower, exitUpper);
			return EventType.EntityStay == eventType ? Direction.None : direction;
		}

	}

}