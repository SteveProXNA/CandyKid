using System;
using Microsoft.Xna.Framework;
using WindowsGame.Managers;
using NUnit.Framework;

namespace WindowsGame.UnitTests.Managers
{
	[TestFixture]
	public class TextManagerSystemTests : BaseUnitTests
	{
		[SetUp]
		public new void SetUp()
		{
			// System under test.
			TextManager = new TextManager();
			TextManager.Initialize(String.Empty);
		}

		[Test]
		public void InitializeBuildTest()
		{
			// Arrange.
			const String assemblyName = "WindowsGame, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null";

			// Act.
			TextManager.InitializeBuild(assemblyName);

			// Assert.
			Assert.That("1.1.0", Is.EqualTo(TextManager.BuildVersion));
		}
		[Test]
		public void GetTextPositionTest()
		{
			Vector2 position = TextManager.GetTextPosition(25, 5, 20, 0, -1.0f, -4.0f);
			Assert.That(position, Is.EqualTo(new Vector2(499, 96)));
		}

		[TearDown]
		public void TearDown()
		{
			TextManager = null;
		}

	}
}