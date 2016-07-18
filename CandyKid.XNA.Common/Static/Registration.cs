using WindowsGame.Devices;
using WindowsGame.Implementation;
using WindowsGame.Inputs;
using WindowsGame.Inputs.Types;
using WindowsGame.Interfaces;
using WindowsGame.Library.Interfaces;
using WindowsGame.Library.IoC;
using WindowsGame.Library.Managers;
using WindowsGame.Managers;
using WindowsGame.TheGame;

namespace WindowsGame.Static
{
	public static class Registration
	{
		public static void Initialize()
		{
			IoCContainer.Initialize<IGameManager, GameManager>();

			IoCContainer.Initialize<IBoardManager, BoardManager>();
			IoCContainer.Initialize<IBorderManager, BorderManager>();
			IoCContainer.Initialize<IBotAIManager, BotAIManager>();
			IoCContainer.Initialize<ICommandManager, CommandManager>();
			IoCContainer.Initialize<ICollisionManager, CollisionManager>();
			IoCContainer.Initialize<IConfigManager, ConfigManager>();
			IoCContainer.Initialize<IContentManager, ContentManager>();
			IoCContainer.Initialize<IDeviceManager, DeviceManager>();
			IoCContainer.Initialize<IEntityManager, EntityManager>();
			IoCContainer.Initialize<IEventManager, EventManager>();
			IoCContainer.Initialize<IImageManager, ImageManager>();
			IoCContainer.Initialize<IInputManager, InputManager>();
			IoCContainer.Initialize<IMoveManager, MoveManager>();
			IoCContainer.Initialize<INewArrowManager, NewArrowManager>();
			IoCContainer.Initialize<INumberManager, NumberManager>();
			IoCContainer.Initialize<IResolutionManager, ResolutionManager>();
			IoCContainer.Initialize<IScoreManager, ScoreManager>();
			IoCContainer.Initialize<IScreenManager, ScreenManager>();
			IoCContainer.Initialize<ISoundManager, SoundManager>();
			IoCContainer.Initialize<IStorageManager, StorageManager>();
			IoCContainer.Initialize<ITextManager, TextManager>();
			IoCContainer.Initialize<IThreadManager, ThreadManager>();

			IoCContainer.Initialize<IJoystickInput, JoystickInput>();
			IoCContainer.Initialize<IKeyboardInput, KeyboardInput>();
			IoCContainer.Initialize<IMouseScreenInput, MouseScreenInput>();
			IoCContainer.Initialize<ITouchScreenInput, TouchScreenInput>();

			IoCContainer.Initialize<IFileProxy, RealFileProxy>();
			IoCContainer.Initialize<IFileManager, FileManager>();

#if WINDOWS
			IoCContainer.Initialize<IDeviceFactory, WorkDeviceFactory>();
			IoCContainer.Initialize<IInputFactory, WorkInputFactory>();
			IoCContainer.Initialize<ILogger, Logger.Implementation.RealLogger>();
#endif
#if !WINDOWS
			IoCContainer.Initialize<IDeviceFactory, FoneDeviceFactory>();
			IoCContainer.Initialize<IInputFactory, FoneInputFactory>();
			IoCContainer.Initialize<ILogger, Library.Implementation.TestLogger>();
#endif
		}
	}
}