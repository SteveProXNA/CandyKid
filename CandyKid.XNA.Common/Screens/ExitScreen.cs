using Microsoft.Xna.Framework;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class ExitScreen : BaseScreen, IScreen
	{
		public ScreenType Update(GameTime gameTime)
		{
			MyGame.Manager.StorageManager.Save();

#if ANDROID
			// Android
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
			System.Environment.Exit(0);
			return ScreenType.Exit;
#endif
#if IOS
			// iOS
			throw new System.DivideByZeroException();
#endif
#if !IOS && !ANDROID
			// Default.
			Engine.Game.Exit();
			return ScreenType.Exit;
#endif
		}

	}
}