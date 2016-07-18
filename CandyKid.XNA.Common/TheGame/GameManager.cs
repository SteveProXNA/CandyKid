using WindowsGame.Library.Interfaces;
using WindowsGame.Managers;

namespace WindowsGame.TheGame
{
	public interface IGameManager
	{
		IBoardManager BoardManager { get; }
		IBorderManager BorderManager { get; }
		IBotAIManager BotAIManager { get; }
		ICollisionManager CollisionManager { get; }
		ICommandManager CommandManager { get; }
		IConfigManager ConfigManager { get; }
		IContentManager ContentManager { get; }
		IDeviceManager DeviceManager { get; }
		IEntityManager EntityManager { get; }
		IEventManager EventManager { get; }
		IFileManager FileManager { get; }
		IImageManager ImageManager { get; }
		IInputManager InputManager { get; }
		IMoveManager MoveManager { get; }
		INewArrowManager NewArrowManager { get; }
		INumberManager NumberManager { get; }
		IResolutionManager ResolutionManager { get; }
		IScoreManager ScoreManager { get; }
		IScreenManager ScreenManager { get; }
		ISoundManager SoundManager { get; }
		IStorageManager StorageManager { get; }
		ITextManager TextManager { get; }
		IThreadManager ThreadManager { get; }
		ILogger Logger { get; }
	}

	public class GameManager : IGameManager
	{
		public GameManager(
			IBoardManager boardManager,
			IBorderManager borderManager,
			IBotAIManager botAIManager,
			ICollisionManager collisionManager,
			ICommandManager commandManager,
			IConfigManager configManager,
			IContentManager contentManager,
			IDeviceManager deviceManager,
			IEntityManager entityManager,
			IEventManager eventManager,
			IFileManager fileManager,
			IImageManager imageManager,
			IInputManager inputManager,
			IMoveManager moveManager,
			INewArrowManager newArrowManager,
			INumberManager numberManager,
			IResolutionManager resolutionManager,
			IScoreManager scoreManager,
			IScreenManager screenManager,
			ISoundManager soundManager,
			IStorageManager storageManager,
			ITextManager textManager,
			IThreadManager threadManager,
			ILogger logger
			)
		{
			BoardManager = boardManager;
			BorderManager = borderManager;
			BotAIManager = botAIManager;
			CollisionManager = collisionManager;
			CommandManager = commandManager;
			ConfigManager = configManager;
			ContentManager = contentManager;
			DeviceManager = deviceManager;
			EntityManager = entityManager;
			EventManager = eventManager;
			FileManager = fileManager;
			ImageManager = imageManager;
			InputManager = inputManager;
			MoveManager = moveManager;
			NewArrowManager = newArrowManager;
			NumberManager = numberManager;
			ResolutionManager = resolutionManager;
			ScoreManager = scoreManager;
			ScreenManager = screenManager;
			SoundManager = soundManager;
			StorageManager = storageManager;
			TextManager = textManager;
			ThreadManager = threadManager;
			Logger = logger;
		}

		public IBoardManager BoardManager { get; private set; }
		public IBorderManager BorderManager { get; private set; }
		public IBotAIManager BotAIManager { get; private set; }
		public ICollisionManager CollisionManager { get; private set; }
		public ICommandManager CommandManager { get; private set; }
		public IConfigManager ConfigManager { get; private set; }
		public IContentManager ContentManager { get; private set; }
		public IDeviceManager DeviceManager { get; private set; }
		public IEntityManager EntityManager { get; private set; }
		public IEventManager EventManager { get; private set; }
		public IFileManager FileManager { get; private set; }
		public IImageManager ImageManager { get; private set; }
		public IInputManager InputManager { get; private set; }
		public IMoveManager MoveManager { get; private set; }
		public INewArrowManager NewArrowManager { get; private set; }
		public INumberManager NumberManager { get; private set; }
		public IResolutionManager ResolutionManager { get; private set; }
		public IScoreManager ScoreManager { get; private set; }
		public IScreenManager ScreenManager { get; private set; }
		public ISoundManager SoundManager { get; private set; }
		public IStorageManager StorageManager { get; private set; }
		public ITextManager TextManager { get; private set; }
		public IThreadManager ThreadManager { get; private set; }
		public ILogger Logger { get; private set; }
	}
}