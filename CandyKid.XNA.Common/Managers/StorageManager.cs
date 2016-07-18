using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using WindowsGame.Data;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IStorageManager
	{
		void Init();
		void Init(Boolean trial);

		void Load();
		void Save();
	}

	public class StorageManager : IStorageManager
	{
		private IsolatedStorageFile storage;
		private StoragePersistData persist;
		private String fileName;

		public void Init()
		{
			Init(BaseData.TrialedGame);
		}
		public void Init(Boolean trial)
		{
			String suffix = trial ? Constants.FREE_TEXT : String.Empty;
			fileName = String.Format("CandyKid{0}.xml", suffix);
		}

		public void Load()
		{
			persist = null;
			try
			{
				using (storage = GetUserStoreAsAppropriateForCurrentPlatform())
				{
					if (storage.FileExists(fileName))
					{
						using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, storage))
						{
							XmlSerializer serializer = new XmlSerializer(typeof(StoragePersistData));
							persist = (StoragePersistData)serializer.Deserialize(fileStream);
						}
					}
				}
			}
			catch
			{
			}

			if (null == persist)
			{
				return;
			}

			// High score.
			MyGame.Manager.ScoreManager.SetHighScore(persist.HighScore);

			// Level select.
			BaseData.SetWorldRound(persist.ScoreWorld, persist.ScoreRound);

			// General game.
			BaseData.SetUseKillTrees(persist.UseKillTrees);
			BaseData.SetUseOpenExits(persist.UseOpenExits);
			BaseData.SetUsePlayMusic(persist.UsePlayMusic);
			BaseData.SetUsePlaySound(persist.UsePlaySound);

			// Gamer option.
			BaseData.SetGamerVelIndex(persist.GamerVelIndex);
			BaseData.SetNewArrowIndex(persist.NewArrowIndex);
			BaseData.SetIsFullScreen(persist.IsFullScreen);

			// Enemy option.
			BaseData.SetResetEnemies(persist.ResetEnemies);
			BaseData.SetEnemyVelIndex(persist.EnemyVelIndex);

			// Sprite index.
			if (!BaseData.TrialedGame)
			{
				BaseData.SetCanContinue(persist.CanContinue);
				BaseData.SetScoreLives(persist.ScoreLives);

				BaseData.SetIsInGodMode(persist.IsInGodMode);
				BaseData.SetMoveAdriana(persist.MoveAdriana);
				BaseData.SetMoveSuzanne(persist.MoveSuzanne);
				BaseData.SetMoveStevePro(persist.MoveStevePro);

				BaseData.SetGamerSpriteIndex(persist.GamerSpriteIndex);
				BaseData.SetEnemyOneSpriteIndex(persist.EnemyOneSpriteIndex);
				BaseData.SetEnemyTwoSpriteIndex(persist.EnemyTwoSpriteIndex);
				BaseData.SetEnemyXyzSpriteIndex(persist.EnemyXyzSpriteIndex);
			}
		}

		public void Save()
		{
			if (null == persist)
			{
				persist = new StoragePersistData
				{
					HighScore = Constants.DEF_HIGH_SCORE,
					ScoreWorld = 1,
					ScoreRound = 1
				};
			}

			// High score.
			if (null != MyGame.Manager.ScoreManager.ScoreList)
			{
				var highXObj = MyGame.Manager.ScoreManager.ScoreList[ScoreType.HighX];
				persist.HighScore = highXObj.Value;
			}

			// Level select.
			UInt16 clockLevel = BaseData.TrialedGame ? BaseData.TrialLevel : BaseData.ClockLevel;
			if (BaseData.ScoreLevel > clockLevel)
			{
				BaseData.ResetScoreLevel();
			}

			persist.ScoreWorld = BaseData.ScoreWorld;
			persist.ScoreRound = BaseData.ScoreRound;

			// General game.
			persist.UseKillTrees = BaseData.UseKillTrees;
			persist.UseOpenExits = BaseData.UseOpenExits;
			persist.UsePlayMusic = BaseData.UsePlayMusic;
			persist.UsePlaySound = BaseData.UsePlaySound;

			// Gamer option.
			persist.GamerVelIndex = BaseData.GamerVelIndex;
			persist.NewArrowIndex = BaseData.NewArrowIndex;
			persist.IsFullScreen = BaseData.IsFullScreen;

			// Enemy option.
			persist.ResetEnemies = BaseData.ResetEnemies;
			persist.EnemyVelIndex = BaseData.EnemyVelIndex;

			// Sprite index.
			if (!BaseData.TrialedGame)
			{
				persist.CanContinue = BaseData.CanContinue;
				persist.ScoreLives = BaseData.ScoreLives;

				persist.IsInGodMode = BaseData.IsInGodMode;
				persist.MoveAdriana = BaseData.MoveAdriana;
				persist.MoveSuzanne = BaseData.MoveSuzanne;
				persist.MoveStevePro = BaseData.MoveStevePro;

				persist.GamerSpriteIndex = BaseData.GamerSpriteIndex;
				persist.EnemyOneSpriteIndex = BaseData.EnemyOneSpriteIndex;
				persist.EnemyTwoSpriteIndex = BaseData.EnemyTwoSpriteIndex;
				persist.EnemyXyzSpriteIndex = BaseData.EnemyXyzSpriteIndex;
			}

			try
			{
				using (storage = GetUserStoreAsAppropriateForCurrentPlatform())
				{
					using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, storage))
					{
						XmlSerializer serializer = new XmlSerializer(typeof(StoragePersistData));
						serializer.Serialize(fileStream, persist);
					}
				}
			}
			catch
			{
			}
		}

		// http://blogs.msdn.com/b/shawnhar/archive/2010/12/16/isolated-storage-windows-and-clickonce.aspx
		private static IsolatedStorageFile GetUserStoreAsAppropriateForCurrentPlatform()
		{
#if WINDOWS
			return IsolatedStorageFile.GetUserStoreForDomain();
#else
			return IsolatedStorageFile.GetUserStoreForApplication();
#endif
		}

	}
}