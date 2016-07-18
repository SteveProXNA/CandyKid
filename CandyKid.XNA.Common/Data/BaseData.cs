using System;
using WindowsGame.Static;

namespace WindowsGame.Data
{
	public abstract class BaseData
	{
		public static void Initialize()
		{
			BaseRoot = String.Empty;
		}
		public static void Initialize(String root)
		{
			BaseRoot = root;
		}

		public static void LoadContent()
		{
			// Global generic data.
			var globalData = MyGame.Manager.ConfigManager.GlobalConfigData;

			Platform = MyGame.Manager.DeviceManager.GetPlatform();
			BorderMenu = Constants.BORDER_MENU;
			BorderGame = Constants.BORDER_GAME;
			BorderHigh = Constants.BORDER_HIGH;
			PopupsWide = Constants.POPUPS_WIDE;
			PopupsHigh = Constants.POPUPS_HIGH;

			MinTile = Constants.MIN_TILE;
			MaxTile = Constants.MAX_TILE;
			ValidWide = (Byte)(MaxTile + 1);
			ValidHigh = (Byte)(MaxTile + 1);

			ExitLower = Constants.EXIT_LOWER;
			ExitUpper = Constants.EXIT_UPPER;
			UseKillTrees = globalData.UseKillTrees;
			UseOpenExits = globalData.UseOpenExits;
			UsePlayMusic = globalData.UsePlayMusic;
			UsePlaySound = globalData.UsePlaySound;

			ScoreWorld = globalData.ScoreWorld;
			ScoreRound = globalData.ScoreRound;
			StartLevel = 1;
			TotalWorld = globalData.TotalWorld;
			TotalRound = globalData.TotalRound;

			ClockLevel = globalData.ClockLevel;
			AdjustClockLevel(TotalWorld, TotalRound);

			if (ScoreWorld > TotalWorld)
			{
				ScoreWorld = TotalWorld;
			}
			if (ScoreRound > TotalRound)
			{
				ScoreRound = TotalRound;
			}

			TrialLevel = Constants.TrialLevel;
			ScoreLevel = GetLevelDataKey(ScoreWorld, ScoreRound);
			ExtraLives = globalData.ExtraLives;

			PlayerPosition = Constants.PlayerPosition;
			AdrianaPosition = Constants.AdrianaPosition;
			SuzannePosition = Constants.SuzannePosition;
			SteveProPosition = Constants.SteveProPosition;
			MoveAdriana = globalData.MoveAdriana;
			MoveSuzanne = globalData.MoveSuzanne;
			MoveStevePro = globalData.MoveStevePro;

			GamerVelIndex = globalData.GamerVelIndex;
			if (GamerVelIndex > Constants.GAMERVEL_INDEX)
			{
				GamerVelIndex = Constants.GAMERVEL_INDEX;
			}
			EnemyVelIndex = globalData.EnemyVelIndex;
			if (EnemyVelIndex > Constants.ENEMYVEL_INDEX)
			{
				EnemyVelIndex = Constants.ENEMYVEL_INDEX;
			}
			Byte index = MyGame.Manager.DeviceManager.GetNewArrowIndex(globalData.NewArrowIndex);
			SetNewArrowIndex(index);

			SplashDelay = globalData.SplashDelay;
			TitleDelay = globalData.TitleDelay;
			DemoDelay = globalData.DemoDelay;
			LoadDelay = globalData.LoadDelay;
			CompDelay = globalData.CompDelay;
			DeadDelay = globalData.DeadDelay;
			OverDelay = globalData.OverDelay;
			BeatDelay = globalData.BeatDelay;
			UnlockDelay = globalData.UnlockDelay;

			LoadZoomOut = globalData.LoadZoomOut;
			QuitsToExit = globalData.QuitsToExit;
			ResetEnemies = globalData.ResetEnemies;
			RefreshGame = globalData.RefreshGame;
			TrialedGame = Constants.TrialedGame;

			// Default values for trial game...
			IsInGodMode = false;
			CanContinue = !TrialedGame;
			MoveAdriana = MoveSuzanne = MoveStevePro = true;

			ScoreLives = Constants.SCORE_LIVES_MIN;
			GamerSpriteIndex = EnemyOneSpriteIndex = EnemyTwoSpriteIndex = EnemyXyzSpriteIndex = 0;
			if (!TrialedGame)
			{
				IsInGodMode = globalData.IsInGodMode;
				CanContinue = globalData.CanContinue;

				MoveAdriana = globalData.MoveAdriana;
				MoveSuzanne = globalData.MoveSuzanne;
				MoveStevePro = globalData.MoveStevePro;

				ScoreLives = globalData.ScoreLives;
				if (ScoreLives > Constants.SCORE_LIVES_MAX)
				{
					ScoreLives = Constants.SCORE_LIVES_MAX;
				}
			}

			OverStopage = CanContinue ? (Byte)0 : (Byte)1;

			// Platform specific data.
			var platformData = MyGame.Manager.ConfigManager.PlatformConfigData;
			IsFullScreen = MyGame.Manager.DeviceManager.GetIsFullScreen(platformData.IsFullScreen);

			ScreenWide = Constants.ScreenWide;
			ScreenHigh = Constants.ScreenHigh;
			TileRatio = Constants.TileRatio;
			TilesSize = Constants.TilesSize;
			TextsSize = (Byte)(TilesSize / 2.0f);

			ArrowSize = GetSizeType(SizeType.Arrow);
			TreesSize = GetSizeType(SizeType.Trees);
			GamerSize = GetSizeType(SizeType.Gamer);
			BonusSize = GetSizeType(SizeType.Bonus);
			CandySize = GetSizeType(SizeType.Candy);

			EntityOffset = Constants.EntityOffset;
			GameOffsetX = Constants.GameOffsetX;
			FontOffsetX = Constants.FontOffsetX;
			FontOffsetY = Constants.FontOffsetY;
			MenuVelocity = Constants.MenuVelocity;
			GamerCrashMin = platformData.GamerCrashMin;
			GamerCrashMax = platformData.GamerCrashMax;

			PlayerVelocityArr = GetArrayFromString(platformData.PlayerVelocityStr);
			AdrianaVelocityArr = GetArrayFromString(platformData.AdrianaVelocityStr);
			AdrianaAttackerArr = GetArrayFromString(platformData.AdrianaAttackerStr);
			SuzanneVelocityArr = GetArrayFromString(platformData.SuzanneVelocityStr);
			SuzanneAttackerArr = GetArrayFromString(platformData.SuzanneAttackerStr);
			SteveProVelocityArr = GetArrayFromString(platformData.SteveProVelocityStr);
			SteveProAttackerArr = GetArrayFromString(platformData.SteveProAttackerStr);

			DeadEnemyType = EnemyType.None;
		}

		private static Byte GetSizeType(SizeType type)
		{
			return (Byte)((Int32)type * TileRatio);
		}

		private static void AdjustClockLevel(Byte theTotalWorld, Byte theTotalRound)
		{
			UInt16 maxClockLevel = GetLevelDataKey(theTotalWorld, theTotalRound);
			if (ClockLevel > maxClockLevel)
			{
				ClockLevel = maxClockLevel;
				return;
			}

			// Round down to the nearest world.
			UInt16 tmpClockLevel = ClockLevel;
			Byte tmpWorld = (Byte)(tmpClockLevel / theTotalRound);

			ClockLevel = GetLevelDataKey(tmpWorld, theTotalRound);
			TotalWorld = tmpWorld;
		}

		public static void SetWorldRound(Byte world, Byte round)
		{
			ScoreWorld = world;
			ScoreRound = round;
			ScoreLevel = GetLevelDataKey(ScoreWorld, ScoreRound);
		}
		public static void SetWorldRound(Byte world, Byte round, UInt16 level)
		{
			ScoreWorld = world;
			ScoreRound = round;
			ScoreLevel = level;
		}
		public static void SetUseKillTrees()
		{
			UseKillTrees = !UseKillTrees;
			UpdateContentData();
		}
		public static void SetUseKillTrees(Boolean useKillTrees)
		{
			UseKillTrees = useKillTrees;
			UpdateContentData();
		}
		public static void SetUseOpenExits()
		{
			UseOpenExits = !UseOpenExits;
			UpdateContentData();
		}
		public static void SetUseOpenExits(Boolean useOpenExits)
		{
			UseOpenExits = useOpenExits;
			UpdateContentData();
		}
		public static void SetUsePlayMusic()
		{
			UsePlayMusic = !UsePlayMusic;
		}
		public static void SetUsePlayMusic(Boolean usePlayMusic)
		{
			UsePlayMusic = usePlayMusic;
		}
		public static void SetUsePlaySound()
		{
			UsePlaySound = !UsePlaySound;
		}
		public static void SetUsePlaySound(Boolean usePlaySound)
		{
			UsePlaySound = usePlaySound;
		}

		public static void SetCanContinue()
		{
			CanContinue = !CanContinue;
			OverStopage = (Byte)(1 - OverStopage);
		}
		public static void SetCanContinue(Boolean canContinue)
		{
			CanContinue = canContinue;
			OverStopage = CanContinue ? (Byte)0 : (Byte)1;
		}
		public static void SetScoreLives(Byte lives)
		{
			ScoreLives = lives;
		}
		public static void SetIsInGodMode()
		{
			IsInGodMode = !IsInGodMode;
		}
		public static void SetIsInGodMode(Boolean isInGodMode)
		{
			IsInGodMode = isInGodMode;
		}
		public static void SetResetEnemies()
		{
			ResetEnemies = !ResetEnemies;
		}
		public static void SetResetEnemies(Boolean resetEnemies)
		{
			ResetEnemies = resetEnemies;
		}

		public static void SetMoveAdriana(Boolean move)
		{
			MoveAdriana = move;
		}
		public static void SetMoveSuzanne(Boolean move)
		{
			MoveSuzanne = move;
		}
		public static void SetMoveStevePro(Boolean move)
		{
			MoveStevePro = move;
		}

		public static void SetGamerSpriteIndex(Byte index)
		{
			GamerSpriteIndex = index;
		}
		public static void SetEnemyOneSpriteIndex(Byte index)
		{
			EnemyOneSpriteIndex = index;
		}
		public static void SetEnemyTwoSpriteIndex(Byte index)
		{
			EnemyTwoSpriteIndex = index;
		}
		public static void SetEnemyXyzSpriteIndex(Byte index)
		{
			EnemyXyzSpriteIndex = index;
		}

		public static void SetDeadEnemy(EnemyType enemyType)
		{
			DeadEnemyType = enemyType;
		}

		public static void SetGamerVelIndex(Byte index)
		{
			GamerVelIndex = index;
		}
		public static void SetEnemyVelIndex(Byte index)
		{
			EnemyVelIndex = index;
		}
		public static void SetNewArrowIndex(Byte index)
		{
			NewArrowIndex = index;
			GameLayout = 0 == index ? LayoutType.BotRight : LayoutType.Custom; 
		}
		public static void SetIsFullScreen()
		{
			IsFullScreen = !IsFullScreen;
		}
		public static void SetIsFullScreen(Boolean isFullScreen)
		{
			IsFullScreen = isFullScreen;
		}

		public static void IncrementScoreLevel()
		{
			ScoreRound += 1;
			if (ScoreRound > TotalRound)
			{
				ScoreWorld += 1;
				ScoreRound = StartLevel;
			}
			ScoreLevel = GetLevelDataKey(ScoreWorld, ScoreRound);
		}
		public static void ResetScoreLevel()
		{
			ScoreLevel = 1;
			ScoreWorld = 1;
			ScoreRound = 1;
		}
		
		private static void UpdateContentData()
		{
			MyGame.Manager.BorderManager.UpdateContentData(UseKillTrees, UseOpenExits);
		}

		public static UInt16 GetLevelDataKey(Byte world, Byte round)
		{
			return (UInt16)((world - 1) * TotalRound + round);
		}
		public static void SetMaxTrialLevel()
		{
			UInt16 level = BaseData.TrialLevel;
			Byte round = BaseData.TotalRound;
			Byte world = (Byte)(level / round);

			BaseData.SetWorldRound(world, round, level);
		}

		private static Byte[] GetArrayFromString(String str)
		{
			Char[] delim = new[] {','};
			String[] array = str.Split(delim);

			Byte[] data = new Byte[4];
			for (Byte index = 0; index < 4; ++index)
			{
				String item = array[index];
				data[index] = Convert.ToByte(item);
			}

			return data;
		}

		public static void SetBonusFreePlayer(Boolean value)
		{
			BonusFreePlayer = value;
		}
	
		public static ScreenType PrevScreen { get; set; }
		public static String BaseRoot { get; private set; }
		public static Boolean BonusFreePlayer { get; private set; }
		public static LayoutType GameLayout { get; private set; }

		// Global generic data.
		public static Platform Platform { get; private set; }

		public static Byte BorderGame { get; private set; }
		public static Byte BorderMenu { get; private set; }
		public static Byte BorderHigh { get; private set; }
		public static Byte PopupsWide { get; private set; }
		public static Byte PopupsHigh { get; private set; }

		public static Byte MinTile { get; private set; }
		public static Byte MaxTile { get; private set; }

		public static Byte GamerSpriteIndex { get; private set; }
		public static Byte EnemyOneSpriteIndex { get; private set; }	// Adriana.
		public static Byte EnemyTwoSpriteIndex { get; private set; }	// Suzanne.
		public static Byte EnemyXyzSpriteIndex { get; private set; }	// StevePro.

		public static EnemyType DeadEnemyType { get; private set; }
		public static Byte ValidWide { get; private set; }
		public static Byte ValidHigh { get; private set; }
		public static Byte ExitLower { get; private set; }
		public static Byte ExitUpper { get; private set; }
		public static Boolean UseKillTrees { get; private set; }
		public static Boolean UseOpenExits { get; private set; }
		public static Boolean UsePlayMusic { get; private set; }
		public static Boolean UsePlaySound { get; private set; }

		public static Byte ScoreLives { get; private set; }
		public static UInt16 ClockLevel { get; private set; }
		public static UInt16 TrialLevel { get; private set; }
		public static UInt16 ScoreLevel { get; private set; }
		public static Byte ScoreWorld { get; private set; }
		public static Byte ScoreRound { get; private set; }
		public static Byte StartLevel { get; private set; }
		public static Byte TotalWorld { get; private set; }
		public static Byte TotalRound { get; private set; }
		public static UInt16 ExtraLives { get; private set; }

		public static Byte PlayerPosition { get; private set; }
		public static Byte PlayerVelocity { get; private set; }
		public static Byte AdrianaPosition { get; private set; }
		public static Byte AdrianaVelocity { get; private set; }
		public static Byte AdrianaAttacker { get; private set; }
		public static Byte SuzannePosition { get; private set; }
		public static Byte SuzanneVelocity { get; private set; }
		public static Byte SuzanneAttacker { get; private set; }
		public static Byte SteveProPosition { get; private set; }
		public static Byte SteveProVelocity { get; private set; }
		public static Byte SteveProAttacker { get; private set; }
		public static Boolean MoveAdriana { get; private set; }
		public static Boolean MoveSuzanne { get; private set; }
		public static Boolean MoveStevePro { get; private set; }
		public static Byte GamerVelIndex { get; private set; }
		public static Byte EnemyVelIndex { get; private set; }
		public static Byte NewArrowIndex { get; private set; }

		public static UInt16 SplashDelay { get; private set; }
		public static UInt16 TitleDelay { get; private set; }
		public static UInt16 DemoDelay { get; private set; }
		public static UInt16 LoadDelay { get; private set; }
		public static UInt16 CompDelay { get; private set; }
		public static UInt16 DeadDelay { get; private set; }
		public static UInt16 OverDelay { get; private set; }
		public static UInt16 BeatDelay { get; private set; }
		public static UInt16 UnlockDelay { get; private set; }

		public static Boolean LoadZoomOut { get; private set; }
		public static Boolean IsInGodMode { get; private set; }
		public static Boolean CanContinue { get; private set; }
		public static Byte OverStopage { get; private set; }
		public static Boolean QuitsToExit { get; private set; }
		public static Boolean ResetEnemies { get; private set; }
		public static Boolean RefreshGame { get; private set; }
		public static Boolean TrialedGame { get; private set; }

		// Platform specific data.
		public static Boolean IsFullScreen { get; private set; }
		public static UInt16 ScreenWide { get; private set; }
		public static UInt16 ScreenHigh { get; private set; }
		public static Byte TileRatio { get; private set; }
		public static Byte TilesSize { get; private set; }
		public static Byte TextsSize { get; private set; }

		public static Byte ArrowSize { get; private set; }
		public static Byte TreesSize { get; private set; }
		public static Byte GamerSize { get; private set; }
		public static Byte BonusSize { get; private set; }
		public static Byte CandySize { get; private set; }

		public static Byte EntityOffset { get; private set; }
		public static Byte GameOffsetX { get; private set; }
		public static SByte FontOffsetX { get; private set; }
		public static SByte FontOffsetY { get; private set; }
		public static Byte MenuVelocity { get; private set; }
		public static Byte GamerCrashMin { get; private set; }
		public static Byte GamerCrashMax { get; private set; }

		public static Byte[] PlayerVelocityArr { get; private set; }
		public static Byte[] AdrianaVelocityArr { get; private set; }
		public static Byte[] AdrianaAttackerArr { get; private set; }
		public static Byte[] SuzanneVelocityArr { get; private set; }
		public static Byte[] SuzanneAttackerArr { get; private set; }
		public static Byte[] SteveProVelocityArr { get; private set; }
		public static Byte[] SteveProAttackerArr { get; private set; }
	}
}