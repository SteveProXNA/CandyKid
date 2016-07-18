using System;
using System.Collections.Generic;
using WindowsGame.Objects;
using NUnit.Framework;

namespace WindowsGame.SystemTests.Managers
{
	[TestFixture]
	public class TextManagerSystemTests : BaseSystemTests
	{
		[SetUp]
		public void SetUp()
		{
			// System under test.
			TextManager = MyGame.Manager.TextManager;
			TextManager.Initialize(CONTENT_ROOT);
		}

		[Test]
		public void LoadTextDataTest()
		{
			IList<TextData> textDataList = TextManager.LoadTextData("PlayScreen0", 20, 0, -1.0f, -4.0f);
			Assert.That(textDataList, Is.Not.Null);
			ShowTextDataList(textDataList);
		}
		[Test]
		public void LoadMenuDataTest()
		{
			IList<TextData> textDataList = TextManager.LoadMenuData("SubMenuOneScreen", 20, 0, -1.0f, -4.0f);
			Assert.That(textDataList, Is.Not.Null);
			ShowTextDataList(textDataList);
		}
		private static void ShowTextDataList(IEnumerable<TextData> textDataList)
		{
			foreach (TextData textData in textDataList)
			{
				Console.WriteLine(textData.Text);
			}
		}

		[TearDown]
		public void TearDown()
		{
			TextManager = null;
		}

	}
}