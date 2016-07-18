using System;
using WindowsGame.Library.Interfaces;
using WindowsGame.Library.IoC;
using WindowsGame.Managers;
using WindowsGame.Static;
using WindowsGame.SystemTests.Implementation;
using WindowsGame.TheGame;
using NUnit.Framework;

namespace WindowsGame.SystemTests
{
	public abstract class BaseSystemTests
	{
		protected IBoardManager BoardManager;
		protected IBorderManager BorderManager;
		protected IBotAIManager BotAIManager;
		protected ICollisionManager CollisionManager;
		protected ICommandManager CommandManager;
		protected IConfigManager ConfigManager;
		protected IContentManager ContentManager;
		protected IDeviceManager DeviceManager;
		protected IEntityManager EntityManager;
		protected IEventManager EventManager;
		protected IFileManager FileManager;
		protected IImageManager ImageManager;
		protected IInputManager InputManager;
		protected IMoveManager MoveManager;
		protected INewArrowManager NewArrowManager;
		protected INumberManager NumberManager;
		protected IResolutionManager ResolutionManager;
		protected IScoreManager ScoreManager;
		protected IScreenManager ScreenManager;
		protected ISoundManager SoundManager;
		protected IStorageManager StorageManager;
		protected ITextManager TextManager;
		protected IThreadManager ThreadManager;
		protected ILogger Logger;

		// mklink /D C:\CandyKid.XNA.Content  C:\CandyKid.XNA\bin\x86\Debug\
		protected const String CONTENT_ROOT = @"C:\CandyKid.XNA.Content\";

		protected const Byte WIDE = 10;
		protected const Byte HIGH = 10;

#pragma warning disable 618
		[TestFixtureSetUp]
#pragma warning restore 618
		public void TestFixtureSetUp()
		{
			Registration.Initialize();
			IoCContainer.Initialize<IFileProxy, TestFileProxy>();

			IGameManager manager = GameFactory.Resolve();
			MyGame.Construct(manager);
		}

#pragma warning disable 618
		[TestFixtureTearDown]
#pragma warning restore 618
		public void TestFixtureTearDown()
		{
			GameFactory.Release();
		}

	}
}