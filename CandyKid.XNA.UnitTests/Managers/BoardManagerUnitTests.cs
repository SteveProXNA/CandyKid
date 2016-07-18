using System;
using WindowsGame.Managers;
using WindowsGame.Static;
using NUnit.Framework;

namespace WindowsGame.UnitTests.Managers
{
	[TestFixture]
	public class BoardManagerUnitTests : BaseUnitTests
	{
		[SetUp]
		public new void SetUp()
		{
			// System under test.
			BoardManager = new BoardManager();
			BoardManager.Initialize(String.Empty, 0, 9, WIDE, HIGH, 40, 0);

			base.SetUp();
		}

		[Test]
		public void HardCodeExitsTest()
		{
			TileType tileType = BoardManager.HardCodeExits(TileType.Trees, WIDE, HIGH, 3, 2, true, 3, 8, 0, 9);
			Assert.That(TileType.Trees, Is.EqualTo(tileType));
		}

		[Test]
		public void ConvertCharToTileTest()
		{
			Assert.That(TileType.Empty, Is.EqualTo(BoardManager.ConvertCharToTile('0')));
			Assert.That(TileType.Candy, Is.EqualTo(BoardManager.ConvertCharToTile('1')));
			Assert.That(TileType.Trees, Is.EqualTo(BoardManager.ConvertCharToTile('2')));
			Assert.That(TileType.Bonus1, Is.EqualTo(BoardManager.ConvertCharToTile('3')));
		}

		[Test]
		public void ConvertTileToCharTest()
		{
			Assert.That('0', Is.EqualTo(BoardManager.ConvertTileToChar(TileType.Empty)));
			Assert.That('1', Is.EqualTo(BoardManager.ConvertTileToChar(TileType.Candy)));
			Assert.That('2', Is.EqualTo(BoardManager.ConvertTileToChar(TileType.Trees)));
			Assert.That('3', Is.EqualTo(BoardManager.ConvertTileToChar(TileType.Bonus1)));
		}

		[TearDown]
		public void TearDown()
		{
			BoardManager = null;
		}

	}
}