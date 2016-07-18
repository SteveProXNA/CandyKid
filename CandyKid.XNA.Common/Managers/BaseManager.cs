using System;
using WindowsGame.Data;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public abstract class BaseManager
	{
		protected static String GetGlobalBaseContentRoot()
		{
			return String.Format("{0}{1}", BaseData.BaseRoot, Constants.CONTENT_DIRECTORY);
		}
		protected static String GetPlatformBaseContentRoot(Platform thePlatform)
		{
			return String.Format("{0}{1}{2}/", BaseData.BaseRoot, Constants.CONTENT_DIRECTORY, thePlatform);
		}

	}
}