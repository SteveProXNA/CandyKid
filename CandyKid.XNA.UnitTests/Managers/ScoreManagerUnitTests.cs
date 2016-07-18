using System;
using WindowsGame.Managers;
using WindowsGame.Static;
using NUnit.Framework;

namespace WindowsGame.UnitTests.Managers
{
	[TestFixture]
	public class ScoreManagerUnitTests : BaseUnitTests
	{
		[SetUp]
		public new void SetUp()
		{
			// System under test.
			ScoreManager = new ScoreManager();
			ScoreManager.Initialize();
			base.SetUp();
		}

		[Test]
		public void ResetHighTest()
		{
			ScoreManager.ResetHigh();
			Assert.That(Constants.DEF_HIGH_SCORE, Is.EqualTo(ScoreManager.ScoreList[ScoreType.HighX].Value));
		}
		[Test]
		public void ResetLevelTest()
		{
			const Int32 reset = 1;
			ScoreManager.ResetLevel();
			Assert.That(reset, Is.EqualTo(ScoreManager.ScoreList[ScoreType.Level].Value));
			Assert.That(reset, Is.EqualTo(ScoreManager.ScoreList[ScoreType.World].Value));
			Assert.That(reset, Is.EqualTo(ScoreManager.ScoreList[ScoreType.Round].Value));
		}
		[Test]
		public void ResetScoreTest()
		{
			ScoreManager.ResetScore();
			Assert.That(0, Is.EqualTo(ScoreManager.ScoreList[ScoreType.Score].Value));
		}

		[Test]
		public void InsertLevel01Test()
		{
			const Int32 level = 27;
			ScoreManager.InsertLevel(level);
			Assert.That(level, Is.EqualTo(ScoreManager.ScoreList[ScoreType.Level].Value));
		}
		[Test]
		public void InsertLevel02Test()
		{
			const Int32 world = 1;
			const Int32 round = 1;
			ScoreManager.InsertLevel(world, round);
			Assert.That(world, Is.EqualTo(ScoreManager.ScoreList[ScoreType.World].Value));
			Assert.That(round, Is.EqualTo(ScoreManager.ScoreList[ScoreType.Round].Value));
		}
		[Test]
		public void InsertTest()
		{
			const Int32 lives = 10;
			ScoreManager.Insert(ScoreType.Lives, lives);
			Assert.That(lives, Is.EqualTo(ScoreManager.ScoreList[ScoreType.Lives].Value));
		}

		[Test]
		public void UpdateScoreTest()
		{
			ScoreManager.Insert(ScoreType.Score, 100);
			ScoreManager.UpdateScore(10);
			Assert.That(110, Is.EqualTo(ScoreManager.ScoreList[ScoreType.Score].Value));
		}
		[Test]
		public void UpdateLivesTest()
		{
			ScoreManager.Insert(ScoreType.Lives, 3);
			ScoreManager.UpdateLives(1);
			Assert.That(4, Is.EqualTo(ScoreManager.ScoreList[ScoreType.Lives].Value));
		}
		[Test]
		public void SetHighScoreTest()
		{
			const UInt32 highX = 100010;
			ScoreManager.SetHighScore(highX);
			Assert.That(highX, Is.EqualTo(ScoreManager.ScoreList[ScoreType.HighX].Value));
		}

		[Test]
		public void ConvertEventToBonusTest()
		{
			UInt16 bonus = ScoreManager.ConvertEventToBonus(EventType.EatBonus1);
			Assert.That(bonus, Is.EqualTo(Constants.BONUS1));
		}

		[TearDown]
		public void TearDown()
		{
			ScoreManager = null;
		}

	}
}