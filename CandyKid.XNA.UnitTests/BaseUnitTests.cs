using System;
using WindowsGame.Library.Interfaces;
using WindowsGame.Managers;
using WindowsGame.Static;
using WindowsGame.TheGame;
using NUnit.Framework;
using Rhino.Mocks;

namespace WindowsGame.UnitTests
{
	public abstract class BaseUnitTests
	{
		protected IBoardManager	BoardManager;
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

		protected const Byte WIDE = 10;
		protected const Byte HIGH = 10;

#pragma warning disable 618
		[TestFixtureSetUp]
#pragma warning restore 618
		public void TestFixtureSetUp()
		{
			BoardManager = MockRepository.GenerateStub<IBoardManager>();
			BorderManager = MockRepository.GenerateStub<IBorderManager>();
			BotAIManager = MockRepository.GenerateStub<IBotAIManager>();
			CollisionManager = MockRepository.GenerateStub<ICollisionManager>();
			CommandManager = MockRepository.GenerateStub<ICommandManager>();
			ConfigManager = MockRepository.GenerateStub<IConfigManager>();
			ContentManager = MockRepository.GenerateStub<IContentManager>();
			DeviceManager = MockRepository.GenerateStub<IDeviceManager>();
			EntityManager = MockRepository.GenerateStub<IEntityManager>();
			EventManager = MockRepository.GenerateStub<IEventManager>();
			FileManager = MockRepository.GenerateStub<IFileManager>();
			ImageManager = MockRepository.GenerateStub<IImageManager>();
			InputManager = MockRepository.GenerateStub<IInputManager>();
			MoveManager = MockRepository.GenerateStub<IMoveManager>();
			NewArrowManager = MockRepository.GenerateStub<INewArrowManager>();
			NumberManager = MockRepository.GenerateStub<INumberManager>();
			ResolutionManager = MockRepository.GenerateStub<IResolutionManager>();
			ScoreManager = MockRepository.GenerateStub<IScoreManager>();
			ScreenManager = MockRepository.GenerateStub<IScreenManager>();
			SoundManager = MockRepository.GenerateStub<ISoundManager>();
			StorageManager = MockRepository.GenerateStub<IStorageManager>();
			TextManager = MockRepository.GenerateStub<ITextManager>();
			ThreadManager = MockRepository.GenerateStub<IThreadManager>();
			Logger = MockRepository.GenerateStub<ILogger>();
		}

		protected void SetUp()
		{
			IGameManager manager = new GameManager
			(
				BoardManager,
				BorderManager,
				BotAIManager,
				CollisionManager,
				CommandManager,
				ConfigManager,
				ContentManager,
				DeviceManager,
				EntityManager,
				EventManager,
				FileManager,
				ImageManager,
				InputManager,
				MoveManager,
				NewArrowManager,
				NumberManager,
				ResolutionManager,
				ScoreManager,
				ScreenManager,
				SoundManager,
				StorageManager,
				TextManager,
				ThreadManager,
				Logger
			);

			MyGame.Construct(manager);
		}

		protected TileType[,] GetBoadData()
		{
			const Byte size = 10;
			TileType[,] boardData = new TileType[size, size];
			for (Byte row = 0; row < size; ++row)
			{
				for (Byte col = 0; col < size; ++col)
				{
					boardData[row, col] = TileType.Empty;
				}
			}

			#region Update Tiles
			boardData[0, 0] = TileType.Bonus1;
			boardData[0, 9] = TileType.Bonus2;
			boardData[1, 1] = TileType.Candy;
			boardData[1, 3] = TileType.Candy;
			boardData[1, 4] = TileType.Candy;
			boardData[1, 5] = TileType.Candy;
			boardData[1, 6] = TileType.Candy;
			boardData[1, 8] = TileType.Candy;
			boardData[3, 1] = TileType.Candy;
			boardData[3, 2] = TileType.Trees;
			boardData[3, 3] = TileType.Trees;
			boardData[3, 4] = TileType.Trees;
			boardData[3, 5] = TileType.Trees;
			boardData[3, 6] = TileType.Trees;
			boardData[3, 7] = TileType.Trees;
			boardData[3, 8] = TileType.Candy;
			boardData[4, 1] = TileType.Candy;
			boardData[4, 2] = TileType.Trees;
			boardData[4, 4] = TileType.Trees;
			boardData[4, 5] = TileType.Trees;
			boardData[4, 7] = TileType.Trees;
			boardData[4, 8] = TileType.Candy;
			boardData[5, 1] = TileType.Candy;
			boardData[5, 2] = TileType.Trees;
			boardData[5, 4] = TileType.Trees;
			boardData[5, 5] = TileType.Trees;
			boardData[5, 7] = TileType.Trees;
			boardData[5, 8] = TileType.Candy;
			boardData[6, 1] = TileType.Candy;
			boardData[6, 2] = TileType.Trees;
			boardData[6, 3] = TileType.Trees;
			boardData[6, 4] = TileType.Trees;
			boardData[6, 5] = TileType.Trees;
			boardData[6, 6] = TileType.Trees;
			boardData[6, 7] = TileType.Trees;
			boardData[6, 8] = TileType.Candy;
			boardData[8, 1] = TileType.Candy;
			boardData[8, 3] = TileType.Candy;
			boardData[8, 4] = TileType.Candy;
			boardData[8, 5] = TileType.Candy;
			boardData[8, 6] = TileType.Candy;
			boardData[8, 8] = TileType.Candy;
			boardData[9, 0] = TileType.Bonus3;
			boardData[9, 9] = TileType.Bonus4;
			#endregion
			return boardData;
		}

#pragma warning disable 618
		[TestFixtureTearDown]
#pragma warning restore 618
		public void TestFixtureTearDown()
		{
			BoardManager = null;
			BorderManager = null;
			BotAIManager = null;
			CollisionManager = null;
			CommandManager = null;
			ConfigManager = null;
			ContentManager = null;
			DeviceManager = null;
			EntityManager = null;
			EventManager = null;
			FileManager = null;
			ImageManager = null;
			InputManager = null;
			MoveManager = null;
			NewArrowManager = null;
			NumberManager = null;
			ResolutionManager = null;
			ScoreManager = null;
			ScreenManager = null;
			SoundManager = null;
			StorageManager = null;
			TextManager = null;
			ThreadManager = null;
			Logger = null;
		}

	}
}