using System;
using System.Collections.Generic;
using WindowsGame.Data;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IBotAIManager
	{
		void Initialize();
		Direction GotoDirX(IList<Direction> directionList, CandyKid player, EnemyType enemyType, CandyMama enemyObject);

		Direction GotoBase(IList<Direction> directionList, Byte deltaX, Byte deltaY, Direction prevDirection, BehaveType behaveType, Byte srceX, Byte srceY, Byte destX, Byte destY);
		Direction GotoTile(IList<Direction> directionList, Byte deltaX, Byte deltaY, Direction prevDirection, BehaveType behaveType, Byte srceX, Byte srceY, Byte destX, Byte destY, Byte attacker);

		Direction GetOppositeDirection(Direction direction);
		Direction GetPrefHorizontal(Byte srceX, Byte destX);
		Direction GetPrefVertical(Byte srceY, Byte destY);
	}

	public class BotAIManager : IBotAIManager
	{
		private IList<Direction> directionPoss;

		public void Initialize()
		{
			directionPoss = new List<Direction>();
		}

		public Direction GotoDirX(IList<Direction> directionList, CandyKid player, EnemyType enemyType, CandyMama enemyObject)
		{
			Direction prevDirection = enemyObject.PrevDirection;
			BehaveType behaveType = enemyObject.BehaveType;

			// Quit out quick as necc.
			if (1 == directionList.Count)
			{
				return directionList[0];
			}

			// Remove opposite to previous direction just like Pacman.
			Direction oppPrevDirection = GetOppositeDirection(prevDirection);
			if (directionList.Contains(oppPrevDirection))
			{
				directionList.Remove(oppPrevDirection);
			}
			if (1 == directionList.Count)
			{
				return directionList[0];
			}

			// Remove invalid directions for border.
			Byte srceX = (Byte)enemyObject.CurrX;
			Byte srceY = (Byte)enemyObject.CurrY;

			// Check again before continuing.
			directionList = ValidateDirectionList(directionList, srceX, srceY);
			if (1 == directionList.Count)
			{
				return directionList[0];
			}

			// Determine destination tile.
			Byte destX = 0;
			Byte destY = 0;
			if (Lifecycle.Dead == player.Lifecycle)
			{
				destX = enemyObject.BaseX;
				destY = enemyObject.BaseY;
			}
			else
			{
				SByte playerCurrX = player.CurrX;
				SByte playerCurrY = player.CurrY;
				GetEnemyTile(playerCurrX, playerCurrY, enemyType, ref destX, ref destY);
			}

			// Begin with common AI decisions.
			Byte deltaX = (Byte)Math.Abs(srceX - destX);
			Byte deltaY = (Byte)Math.Abs(srceY - destY);

			// Enemy reaches dest. tile...
			if (0 == deltaX && 0 == deltaY)
			{
				// And if player still alive then kill!
				if (Lifecycle.Dead != player.Lifecycle)
				{
					if (player.CurrX > BaseData.MinTile && player.CurrX < BaseData.MaxTile &&
						player.CurrY > BaseData.MinTile && player.CurrY < BaseData.MaxTile)
					{
						// Chase player in kid direction to kill.
						if (Direction.None != player.Direction)
						{
							return player.Direction;
						}
					}
				}
				else
				{
					return Direction.None;
				}
			}

			// NOTE: enemy could reach dest tile and if player alive then keep looking...

			// If player dead then enemy goto base.
			if (Lifecycle.Dead == player.Lifecycle && Lifecycle.Dead != enemyObject.Lifecycle)
			{
				return GotoBase(directionList, deltaX, deltaY, prevDirection, behaveType, srceX, srceY, destX, destY);
			}

			// Otherwise check if enemy is within player attack range!
			Byte distance = (Byte)(deltaX * deltaX + deltaY * deltaY);
			Byte attacker = enemyObject.Attacker;

			if (distance < attacker)
			{
				Direction gotoDirection = GotoBase(directionList, deltaX, deltaY, prevDirection, behaveType, srceX, srceY, destX, destY);
				if (Direction.None != gotoDirection)
				{
					return gotoDirection;
				}
			}

			// If not then enemy simply "wanders" around randomly until within attack range; similar to scatter in Pacman.
			return GotoTile(directionList, deltaX, deltaY, prevDirection, behaveType, srceX, srceY, destX, destY, attacker);
		}

		public Direction GotoBase(IList<Direction> directionList, Byte deltaX, Byte deltaY, Direction prevDirection, BehaveType behaveType, Byte srceX, Byte srceY, Byte destX, Byte destY)
		{
			// Get preferred directions.
			Direction prefHorizontal = GetPrefHorizontal(srceX, destX);
			Direction prefVertical = GetPrefVertical(srceY, destY);

			// Will this make enemy more attacking?  Always go in prev direction.
			if (prevDirection == prefHorizontal && directionList.Contains(prefHorizontal))
			{
				return prefHorizontal;
			}
			if (prevDirection == prefVertical && directionList.Contains(prefVertical))
			{
				return prefVertical;
			}

			if (BehaveType.Horizontal == behaveType)
			{
				if (directionList.Contains(prefHorizontal))
				{
					return prefHorizontal;
				}
				if (directionList.Contains(prefVertical))
				{
					return prefVertical;
				}
			}
			if (BehaveType.Vertical == behaveType)
			{
				if (directionList.Contains(prefVertical))
				{
					return prefVertical;
				}
				if (directionList.Contains(prefHorizontal))
				{
					return prefHorizontal;
				}
			}
			if (BehaveType.Random == behaveType)
			{
				if (directionList.Contains(prefHorizontal) && directionList.Contains(prefVertical))
				{
					Int32 index = MyGame.Manager.NumberManager.Generate(2);
					return index == 0 ? prefHorizontal : prefVertical;
				}
				if (directionList.Contains(prefHorizontal) && !directionList.Contains(prefVertical))
				{
					return prefHorizontal;
				}
				if (!directionList.Contains(prefHorizontal) && directionList.Contains(prefVertical))
				{
					return prefVertical;
				}
			}

			// Wants to go horizontal but can't so choose random vertical.
			if (Direction.None != prefHorizontal && Direction.None == prefVertical)
			{
				Direction oppositeDirection = GetOppositeDirection(prefHorizontal);
				if (directionList.Contains(oppositeDirection))
				{
					directionList.Remove(oppositeDirection);
				}
				if (1 == directionList.Count)
				{
					return directionList[0];
				}

				Int32 index = MyGame.Manager.NumberManager.Generate(directionList.Count);
				return directionList[index];
			}

			// Wants to go vertical but can't so choose random horizontal.
			if (Direction.None == prefHorizontal && Direction.None != prefVertical)
			{
				Direction oppositeDirection = GetOppositeDirection(prefVertical);
				if (directionList.Contains(oppositeDirection))
				{
					directionList.Remove(oppositeDirection);
				}
				if (1 == directionList.Count)
				{
					return directionList[0];
				}

				Int32 index = MyGame.Manager.NumberManager.Generate(directionList.Count);
				return directionList[index];
			}

			if (1 == directionList.Count)
			{
				return directionList[0];
			}
			if (deltaX < deltaY)
			{
				if (directionList.Contains(Direction.Left))
				{
					return Direction.Left;
				}
				if (directionList.Contains(Direction.Right))
				{
					return Direction.Right;
				}
			}
			if (deltaX > deltaY)
			{
				if (directionList.Contains(Direction.Up))
				{
					return Direction.Up;
				}
				if (directionList.Contains(Direction.Down))
				{
					return Direction.Down;
				}
			}
			if (deltaX == deltaY)
			{
				Int32 index = MyGame.Manager.NumberManager.Generate(directionList.Count);
				return directionList[index];
			}

			return Direction.None;
		}

		public Direction GotoTile(IList<Direction> directionList, Byte deltaX, Byte deltaY, Direction prevDirection, BehaveType behaveType, Byte srceX, Byte srceY, Byte destX, Byte destY, Byte attacker)
		{
			directionPoss.Clear();
			if (deltaX > deltaY)
			{
				behaveType = BehaveType.Horizontal;
			}
			if (deltaX < deltaY)
			{
				behaveType = BehaveType.Vertical;
			}

			if (BehaveType.Horizontal == behaveType || BehaveType.Random == behaveType)
			{
				if (directionList.Contains(Direction.Left))
				{
					directionPoss.Add(Direction.Left);
				}
				if (directionList.Contains(Direction.Right))
				{
					directionPoss.Add(Direction.Right);
				}
			}
			if (BehaveType.Vertical == behaveType || BehaveType.Random == behaveType)
			{
				if (directionList.Contains(Direction.Up))
				{
					directionPoss.Add(Direction.Up);
				}
				if (directionList.Contains(Direction.Down))
				{
					directionPoss.Add(Direction.Down);
				}
			}

			Int32 index;
			Int32 count = directionPoss.Count;
			if (0 != count)
			{
				index = MyGame.Manager.NumberManager.Generate(count);
				return directionPoss[index];
			}

			count = directionList.Count;
			index = MyGame.Manager.NumberManager.Generate(count);
			return directionList[index];
		}

		public Direction GetOppositeDirection(Direction direction)
		{
			if (Direction.Up == direction)
			{
				return Direction.Down;
			}
			if (Direction.Down == direction)
			{
				return Direction.Up;
			}
			if (Direction.Left == direction)
			{
				return Direction.Right;
			}
			if (Direction.Right == direction)
			{
				return Direction.Left;
			}
			return Direction.None;
		}

		public Direction GetPrefHorizontal(Byte srceX, Byte destX)
		{
			if (srceX < destX)
			{
				return Direction.Right;
			}
			if (srceX > destX)
			{
				return Direction.Left;
			}

			return Direction.None;
		}
		public Direction GetPrefVertical(Byte srceY, Byte destY)
		{
			if (srceY < destY)
			{
				return Direction.Down;
			}
			if (srceY > destY)
			{
				return Direction.Up;
			}

			return Direction.None;
		}

		private static void GetEnemyTile(SByte playerCurrX, SByte playerCurrY, EnemyType enemyType, ref Byte destX, ref Byte destY)
		{
			if (EnemyType.Adriana == enemyType)
			{
				Byte index = (Byte)MyGame.Manager.NumberManager.Generate(10);
				if (index < 4)
				{
					destX = (Byte)(BaseData.MaxTile - playerCurrX);
					destY = (Byte)playerCurrY;
				}
				else if (index < 8)
				{
					destX = (Byte)playerCurrX;
					destY = (Byte)(BaseData.MaxTile - playerCurrY);
				}
				else
				{
					destX = (Byte)playerCurrX;
					destY = (Byte)playerCurrY;
				}
			}
			else if (EnemyType.Suzanne == enemyType)
			{
				destX = (Byte)(BaseData.MaxTile - playerCurrX);
				destY = (Byte)(BaseData.MaxTile - playerCurrY);
			}
			else
			{
				destX = (Byte)playerCurrX;
				destY = (Byte)playerCurrY;
			}

			// Boundary check.
			if (destX < BaseData.MinTile)
			{
				destX = BaseData.MinTile;
			}
			if (destX > BaseData.MaxTile)
			{
				destX = BaseData.MaxTile;
			}
			if (destY < BaseData.MinTile)
			{
				destY = BaseData.MinTile;
			}
			if (destY > BaseData.MaxTile)
			{
				destY = BaseData.MaxTile;
			}
		}

		private static IList<Direction> ValidateDirectionList(IList<Direction> directionList, Byte srceX, Byte srceY)
		{
			// Validation here:
			if (BaseData.MinTile == srceX && directionList.Contains(Direction.Left))
			{
				directionList.Remove(Direction.Left);
			}
			if (BaseData.MaxTile == srceX && directionList.Contains(Direction.Right))
			{
				directionList.Remove(Direction.Right);
			}
			if (BaseData.MinTile == srceY && directionList.Contains(Direction.Up))
			{
				directionList.Remove(Direction.Up);
			}
			if (BaseData.MaxTile == srceY && directionList.Contains(Direction.Down))
			{
				directionList.Remove(Direction.Down);
			}

			return directionList;
		}

	}
}