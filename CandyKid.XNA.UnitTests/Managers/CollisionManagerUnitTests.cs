using WindowsGame.Managers;
using WindowsGame.Static;
using NUnit.Framework;

namespace WindowsGame.UnitTests.Managers
{
	[TestFixture]
	public class CollisionManagerUnitTests : BaseUnitTests
	{
		[SetUp]
		public new void SetUp()
		{
			// System under test.
			CollisionManager = new CollisionManager();
			base.SetUp();
		}

		[Test]
		public void CheckEnemyCollisionExitTest()
		{
			Assert.That(CollisionManager.CheckEnemyCollisionExit(0, 9, 10, 1), Is.True);
			Assert.That(CollisionManager.CheckEnemyCollisionExit(0, 9, 1, 1), Is.False);
		}
		[Test]
		public void CheckEnemyCollisionFastTest()
		{
			Assert.That(CollisionManager.CheckEnemyCollisionFast(1, 1, 2, 2), Is.True);
			Assert.That(CollisionManager.CheckEnemyCollisionFast(1, 1, 2, 3), Is.False);
		}
		[Test]
		public void CheckEnemyCollisionSlowTest()
		{
			Assert.That(CollisionManager.CheckEnemyCollisionSlow(16, 84.0f, 84.0f, 90.0f, 90.0f), Is.True);
			Assert.That(CollisionManager.CheckEnemyCollisionSlow(16, 84.0f, 84.0f, 364.0f, 90.0f), Is.False);
		}

		[Test]
		public void CheckTilesCollisionTest()
		{
			// Arrange.
			TileType[,] boardData = GetBoadData();

			// Act.
			EventType eventType = CollisionManager.CheckTilesCollision(boardData, 3, 1, Direction.Right, true, 0, 9, 3, 8);

			// Assert.
			Assert.That(EventType.EatCandy, Is.EqualTo(eventType));
		}

		[TearDown]
		public void TearDown()
		{
			CollisionManager = null;
		}
	}

}