using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Managers;
using NUnit.Framework;

namespace WindowsGame.UnitTests.Managers
{
	[TestFixture]
	public class BorderManagerUnitTests : BaseUnitTests
	{
		[SetUp]
		public new void SetUp()
		{
			// System under test.
			BorderManager = new BorderManager();
			base.SetUp();
		}

		[Test]
		public void LoadAllTilesTest()
		{
			IDictionary<Int16, Vector2> allBorderTiles = BorderManager.LoadAllTiles(12, 16, 12, 40, 0);
			Assert.That(allBorderTiles, Is.Not.Null);
		}
		[Test]
		public void LoadAllPopupTest()
		{
			IList<Vector2> allBorderPopup = BorderManager.LoadAllPopup(12, 12, 6, 4, 40, 0);
			Assert.That(allBorderPopup, Is.Not.Null);
		}
		[Test]
		public void LoadAllStripTest()
		{
			IList<Vector2> allBorderStrip = BorderManager.LoadAllStrip(12, 12, 6, 4, 40, 20, 0);
			Assert.That(allBorderStrip, Is.Not.Null);
		}

		[TearDown]
		public void TearDown()
		{
			BorderManager = null;
		}
	}

}