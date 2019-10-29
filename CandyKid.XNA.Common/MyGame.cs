using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Library;
using WindowsGame.Static;
using WindowsGame.TheGame;

namespace WindowsGame
{
	public static class MyGame
	{
		public static void Construct(IGameManager manager)
		{
			Manager = manager;
		}

		public static void Initialize()
		{
			Manager.Logger.Initialize();
			Manager.ConfigManager.Initialize();
#if WINDOWS
			Manager.ConfigManager.LoadContent();
#endif

			Manager.ContentManager.Initialize();
			Manager.ContentManager.LoadContentSplash();

			Manager.InputManager.Initialize();
			Manager.ScoreManager.Initialize();
#if WINDOWS
			Manager.StorageManager.Init();
			Manager.StorageManager.Load();
#endif
			Manager.ResolutionManager.Initialize();
			Manager.ScreenManager.Initialize();
			Manager.ThreadManager.Initialize();
		}

		public static void LoadContent()
		{
			Engine.Game.IsFixedTimeStep = Constants.IsFixedTimeStep;
			Engine.Game.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / Constants.FramesPerSecond);
			Engine.Game.IsMouseVisible = Constants.IsMouseVisible;

			Boolean isFullScreen = Manager.DeviceManager.ChooseIsFullScreen();
			Manager.ResolutionManager.LoadContent(isFullScreen, Constants.ScreenWide, Constants.ScreenHigh, Constants.UseExposed, Constants.ExposeWide, Constants.ExposeHigh);
		}

		public static void LoadContentAsync()
		{
#if !WINDOWS
			Manager.ConfigManager.LoadContent();
#endif
			Manager.DeviceManager.Initialize();
			Manager.InputManager.Initialize2();
			Manager.ContentManager.LoadContent();
			Manager.ImageManager.LoadContent();
			Manager.BotAIManager.Initialize();
			Manager.BoardManager.Initialize();
			Manager.CommandManager.Initialize();
			Manager.EntityManager.Initialize();
			Manager.EventManager.Initialize();
			Manager.MoveManager.Initialize();
			Manager.NumberManager.Initialize();
			Manager.TextManager.Initialize();
			Manager.TextManager.InitializeBuild();


			//LoadContent() methods.
#if !WINDOWS
			Manager.StorageManager.Init();
			Manager.StorageManager.Load();
#endif
			Manager.CommandManager.LoadContent();
			Manager.BorderManager.LoadContent();
			Manager.DeviceManager.LoadContent();
			Manager.NewArrowManager.LoadContent();
			Manager.ScoreManager.LoadContent();
			Manager.TextManager.LoadContent();
			Manager.EntityManager.LoadContent();
			Manager.ScreenManager.LoadContent();

			GC.Collect();
		}

		public static void UnloadContent()
		{
			Engine.Game.Content.Unload();
		}

		public static void Update(GameTime gameTime)
		{
			Manager.InputManager.Update(gameTime);

#if WINDOWS
			if (BaseData.QuitsToExit)
			{
				Boolean escape = Manager.InputManager.Escape();
				if (escape)
				{
					Engine.Game.Exit();
					return;
				}
			}

			Boolean space = Manager.InputManager.Space();
			if (space)
			{
				LoadContent();
				return;
			}
#endif

			Manager.ScreenManager.Update(gameTime);
		}

		public static void Draw()
		{
			Manager.ScreenManager.Draw();
		}

		public static void OnActivated()
		{
			Manager.StorageManager.Load();
		}
		public static void OnDeactivated()
		{
			Manager.StorageManager.Save();

#if ANDROID
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
			System.Environment.Exit(0);
#endif
		}

		public static IGameManager Manager { get; private set; }

	}
}