using NUnit.Framework;

namespace WindowsGame.SystemTests.Managers
{
	[TestFixture]
	public class StorageManagerSystemTests : BaseSystemTests
	{
		[SetUp]
		public void SetUp()
		{
			// System under test.
			StorageManager = MyGame.Manager.StorageManager;
			StorageManager.Init(true);
		}

		[Test]
		public void LoadTest()
		{
			StorageManager.Load();
		}
		[Test]
		public void SaveTest()
		{
			//StorageManager.Save();
			Assert.True(true);
		}

		[TearDown]
		public void TearDown()
		{
			StorageManager = null;
		}

	}
}