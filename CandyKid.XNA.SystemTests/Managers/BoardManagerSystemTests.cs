using System;
using System.Text;
using WindowsGame.Static;
using NUnit.Framework;

namespace WindowsGame.SystemTests.Managers
{
	[TestFixture]
	public class BoardManagerSystemTests : BaseSystemTests
	{
		private const Byte World = 1;
		private const Byte Round = 1;

		[SetUp]
		public void SetUp()
		{
			// System under test.
			BoardManager = MyGame.Manager.BoardManager;
			BoardManager.Initialize(CONTENT_ROOT, 0, 9, WIDE, HIGH, 40, 0);
		}

		[Test]
		public void LoadLevelDataTest()
		{
			TileType[,] boardData = BoardManager.LoadLevelData(World, Round, WIDE, HIGH);
			Assert.That(boardData, Is.Not.Null);
			ShowBoardData(boardData);
		}

		private static void ShowBoardData(TileType[,] boardData)
		{
			Console.WriteLine("World #" + World);
			Console.WriteLine("Round #" + Round);
			StringBuilder sb = new StringBuilder();
			for (Byte row = 0; row < HIGH; ++row)
			{
				for (Byte col = 0; col < WIDE; ++col)
				{
					TileType tileType = boardData[row, col];
					sb.Append(tileType.ToString().PadRight(7, ' '));
				}

				Console.WriteLine(sb.ToString());
				sb.Length = 0;
			}
		}

		[TearDown]
		public void TearDown()
		{
			BoardManager = null;
		}

	}
}