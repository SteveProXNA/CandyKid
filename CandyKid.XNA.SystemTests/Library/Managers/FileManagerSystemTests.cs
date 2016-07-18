using System;
using WindowsGame.Data;
using WindowsGame.Static;
using NUnit.Framework;

namespace WindowsGame.SystemTests.Library.Managers
{
	[TestFixture]
	public class FileManagerSystemTests : BaseSystemTests
	{
		[SetUp]
		public void SetUp()
		{
			FileManager = MyGame.Manager.FileManager;
		}

		[Test]
		public void LoadTxtTest()
		{
			const String world = "01";
			const String round = "01";
			String file = String.Format("{0}{1}/{2}/{3}/{4}{5}/{6}{7}.txt", CONTENT_ROOT, Constants.CONTENT_DIRECTORY,
				Constants.DATA_DIRECTORY, Constants.LEVELS_DIRECTORY, Constants.WORLD_FILENAME, world, Constants.ROUND_FILENAME,
				round);

			var levelData = FileManager.LoadTxt(file);
			Console.WriteLine("LevelData:" + levelData.Count);
		}

		[Test]
		public void LoadXmlTest()
		{
			String file = String.Format("{0}{1}/{2}/{3}/{4}", CONTENT_ROOT, Constants.CONTENT_DIRECTORY, Constants.DATA_DIRECTORY,
				Constants.CONFIG_DIRECTORY, Constants.GLOBAL_CONFIG_FILENAME);

			var configData = FileManager.LoadXml<GlobalConfigData>(file);
			Console.WriteLine("ScreenType:" + configData.ScreenType);
		}

		[TearDown]
		public void TearDown()
		{
			FileManager = null;
		}
	}
}