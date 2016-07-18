using WindowsGame.Managers;
using NUnit.Framework;

namespace WindowsGame.UnitTests.Managers
{
	[TestFixture]
	public class ImageManagerUnitTests : BaseUnitTests
	{
		[SetUp]
		public new void SetUp()
		{
			// System under test.
			ImageManager = new ImageManager();
			base.SetUp();
		}

		[Test]
		public void LoadContentDataTest()
		{
			ImageManager.LoadContentData(8, 64, 40, 32, 24, 16);
			Assert.That(ImageManager.AllArrowRectangles, Is.Not.Null);
			Assert.That(ImageManager.AllBonusRectangles, Is.Not.Null);
			Assert.That(ImageManager.AllCandyRectangles, Is.Not.Null);
		}

		[TearDown]
		public void TearDown()
		{
			ImageManager = null;
		}

	}
}