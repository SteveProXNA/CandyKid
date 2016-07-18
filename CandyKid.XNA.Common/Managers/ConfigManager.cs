using System;
using WindowsGame.Data;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IConfigManager
	{
		void Initialize();
		void Initialize(String root);
		void LoadContent();
		void LoadGlobalConfigData();
		void LoadPlaformConfigData(Platform platform);
		GlobalConfigData GlobalConfigData { get; }
		PlatformConfigData PlatformConfigData { get; }
	}

	public class ConfigManager : BaseManager, IConfigManager
	{
		public void Initialize()
		{
			BaseData.Initialize();
		}
		public void Initialize(String root)
		{
			BaseData.Initialize(root);
		}

		public void LoadContent()
		{
			LoadGlobalConfigData();
			LoadPlaformConfigData(BaseData.Platform);

			BaseData.LoadContent();
		}

		public void LoadGlobalConfigData()
		{
			String file = GetGlobalConfigFile(Constants.GLOBAL_CONFIG_FILENAME);
			GlobalConfigData = MyGame.Manager.FileManager.LoadXml<GlobalConfigData>(file);
		}

		public void LoadPlaformConfigData(Platform thePlatform)
		{
			String file = Constants.PLATFORM_CONFIG_FILENAME.Replace("{0}", thePlatform.ToString());

			file = GetPlatformConfigFile(file);
			PlatformConfigData = MyGame.Manager.FileManager.LoadXml<PlatformConfigData>(file);
		}

		public GlobalConfigData GlobalConfigData { get; private set; }
		public PlatformConfigData PlatformConfigData { get; private set; }

		private static String GetGlobalConfigFile(String configFile)
		{
			return GetConfigFile(GetGlobalBaseContentRoot(), configFile);
		}
		private static String GetPlatformConfigFile(String configFile)
		{
			String root = GetGlobalBaseContentRoot();
			return GetConfigFile(root, configFile);
		}
		private static String GetConfigFile(String root, String file)
		{
			return String.Format("{0}/{1}/{2}/{3}", root, Constants.DATA_DIRECTORY, Constants.CONFIG_DIRECTORY, file);
		}
	}
}