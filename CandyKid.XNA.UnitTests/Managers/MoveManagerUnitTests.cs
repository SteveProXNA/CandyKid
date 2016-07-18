using System.Collections.Generic;
using WindowsGame.Managers;
using WindowsGame.Static;
using NUnit.Framework;

namespace WindowsGame.UnitTests.Managers
{
	[TestFixture]
	public class MoveManagerUnitTests : BaseUnitTests
	{
		[SetUp]
		public new void SetUp()
		{
			// System under test.
			MoveManager = new MoveManager();
			MoveManager.Initialize();
			base.SetUp();
		}

		[Test]
		public void CheckDirectionTest()
		{
			// Arrange.
			TileType[,] boardData = GetBoadData();

			// Act.
			EventType eventType = MoveManager.CheckDirection(boardData, 1, 8, Direction.Left, false, 0, 9, 3, 8);

			// Assert.
			Assert.That(EventType.EntityFree, Is.EqualTo(eventType));
		}

		[Test]
		public void CheckFreeMovesTest()
		{
			// Arrange.
			TileType[,] boardData = GetBoadData();

			// Act.
			IList<Direction> directionList = MoveManager.CheckFreeMoves(boardData, 1, 8, 0, 9, 3, 8);

			// Assert.
			Assert.That(directionList, Is.Not.Null);
			Assert.That(4, Is.EqualTo(directionList.Count));
		}

		[TearDown]
		public void TearDown()
		{
			MoveManager = null;
		}
	}

}