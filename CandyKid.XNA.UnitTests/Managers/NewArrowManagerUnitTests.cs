using WindowsGame.Managers;
using NUnit.Framework;

namespace WindowsGame.UnitTests.Managers
{
	[TestFixture]
	public class NewArrowManagerUnitTests : BaseUnitTests
	{
		[SetUp]
		public new void SetUp()
		{
			// System under test.
			NewArrowManager = new NewArrowManager();
			base.SetUp();
		}

		[Test]
		public void LoadContentTest()
		{
			NewArrowManager.LoadContent();
			Assert.That(NewArrowManager.QuadArrowDictionary, Is.Not.Null);
		}

		[TearDown]
		public void TearDown()
		{
			NewArrowManager = null;
		}

	}
}