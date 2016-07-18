using System.Collections.Generic;
using WindowsGame.Managers;
using WindowsGame.Static;
using NUnit.Framework;
using Rhino.Mocks;

namespace WindowsGame.UnitTests.Managers
{
	[TestFixture]
	public class BotAIManagerUnitTests : BaseUnitTests
	{
		[SetUp]
		public new void SetUp()
		{
			// System under test.
			BotAIManager = new BotAIManager();
			BotAIManager.Initialize();
			base.SetUp();
		}

		[Test]
		public void GotoBaseTest()
		{
			// Arrange.
			IList<Direction> directionList = GetDirectionList();
			NumberManager.Stub(nm => nm.Generate(4)).Return(2).Repeat.Once();

			// Act.
			Direction direction = BotAIManager.GotoBase(directionList, 0, 0, Direction.None, BehaveType.Vertical, 1, 8, 1, 8);

			// Assert.
			Assert.That(Direction.Up, Is.EqualTo(direction));
		}

		[Test]
		public void GotoTileTest()
		{
			// Arrange.
			IList<Direction> directionList = GetDirectionList();
			NumberManager.Stub(nm => nm.Generate(2)).Return(1).Repeat.Once();

			// Act.
			Direction direction = BotAIManager.GotoTile(directionList, 0, 7, Direction.None, BehaveType.Vertical, 1, 8, 1, 1, 15);

			// Assert.
			Assert.That(Direction.Down, Is.EqualTo(direction));
		}

		[Test]
		public void GetOppositeDirectionTest()
		{
			Assert.That(Direction.Right, Is.EqualTo(BotAIManager.GetOppositeDirection(Direction.Left)));
			Assert.That(Direction.Left, Is.EqualTo(BotAIManager.GetOppositeDirection(Direction.Right)));
			Assert.That(Direction.Down, Is.EqualTo(BotAIManager.GetOppositeDirection(Direction.Up)));
			Assert.That(Direction.Up, Is.EqualTo(BotAIManager.GetOppositeDirection(Direction.Down)));
			Assert.That(Direction.None, Is.EqualTo(BotAIManager.GetOppositeDirection(Direction.None)));
		}
		[Test]
		public void GetPrefHorizontalTest()
		{
			Assert.That(Direction.Left, Is.EqualTo(BotAIManager.GetPrefHorizontal(3, 2)));
			Assert.That(Direction.Right, Is.EqualTo(BotAIManager.GetPrefHorizontal(2, 3)));
			Assert.That(Direction.None, Is.EqualTo(BotAIManager.GetPrefHorizontal(3, 3)));
		}
		[Test]
		public void GetPrefVerticalTest()
		{
			Assert.That(Direction.Up, Is.EqualTo(BotAIManager.GetPrefVertical(3, 2)));
			Assert.That(Direction.Down, Is.EqualTo(BotAIManager.GetPrefVertical(2, 3)));
			Assert.That(Direction.None, Is.EqualTo(BotAIManager.GetPrefVertical(3, 3)));
		}

		private static IList<Direction> GetDirectionList()
		{
			return new List<Direction> { Direction.Left, Direction.Right, Direction.Up, Direction.Down };
		}

		[TearDown]
		public void TearDown()
		{
			BotAIManager = null;
		}
	}

}