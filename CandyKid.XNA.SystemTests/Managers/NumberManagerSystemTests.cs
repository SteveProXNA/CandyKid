using System;
using NUnit.Framework;

namespace WindowsGame.SystemTests.Managers
{
	[TestFixture]
	public class NumberManagerSystemTests : BaseSystemTests
	{
		[SetUp]
		public void SetUp()
		{
			// System under test.
			NumberManager = MyGame.Manager.NumberManager;
			NumberManager.Initialize();
		}

		[Test]
		public void Generate01Test()
		{
			const Int32 max = 10;
			Int32 result = NumberManager.Generate(max);

			Assert.That(result, Is.LessThan(max));
			Console.WriteLine(result);
		}

		[Test]
		public void Generate02Test()
		{
			const Int32 min = 1;
			const Int32 max = 10;
			Int32 result = NumberManager.Generate(min, max);

			Assert.That(result, Is.GreaterThanOrEqualTo(min));
			Assert.That(result, Is.LessThan(max));
			Console.WriteLine(result);
		}

		[TearDown]
		public void TearDown()
		{
			NumberManager = null;
		}

	}
}