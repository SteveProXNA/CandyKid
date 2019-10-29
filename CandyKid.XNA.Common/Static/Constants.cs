using System;

namespace WindowsGame.Static
{
	public static class Constants
	{
		public const String CONTENT_DIRECTORY = "Content";

		public const String DATA_DIRECTORY = "Data";
		public const String COMMAND_DIRECTORY = "Events";
		public const String CONFIG_DIRECTORY = "Config";
		public const String LEVELS_DIRECTORY = "Levels";
		public const String WORLD_FILENAME = "World";
		public const String ROUND_FILENAME = "Round";
		public const String TEXTS_DIRECTORY = "Texts";

		public const String FONTS_DIRECTORY = "Fonts";

		public const String SOUND_DIRECTORY = "Sound";
		public const String TEXTURES_DIRECTORY = "Textures";
		public const String BANNERS_DIRECTORY = "Banners";

		public const String GLOBAL_CONFIG_FILENAME = "GlobalConfig.xml";
		public const String PLATFORM_CONFIG_FILENAME = "PlatformConfig{0}.xml";

		public const Byte ARROW_NUMBER = 4;
		public const Byte BONUS_NUMBER = 8;
		public const Byte BONUS_EXISTS = 4;
		public const Byte CANDY_NUMBER = 14;
		public const Byte DEATH_NUMBER = 4;
		public const Byte ENEMY_NUMBER = 3;
		public const Byte SPRITE_NUMBER = 2;

		public const Byte STRIP_NUMBER = 16;
		public const Byte MENUS_SPACES = 3;
		public const Byte MENUS_NUMBER = 5;
		public const Byte MENUS_TOPEND = 8;
		public const Byte MENUS_BOTTOM = 12;
		public const Byte MENUS_LEFCOL = 3;
		public const Byte MENUS_RGTCOL = 12;
		public const Byte POPUP_COLUMN = 6;

		public const Byte CANDY_SCORE = 10;

		public const Byte SCORE_LIVES_MIN = 3;
		public const Byte SCORE_LIVES_MAX = 255;
		public const UInt16 TITLE_DELAY = 1600;
		public const Byte GAMERVEL_INDEX = 3;
		public const Byte ENEMYVEL_INDEX = 3;
		public const Byte NEWARROW_INDEX = 4;

		public static readonly UInt16[] BONUS_SCORE = new UInt16[] { 100, 200, 400, 800 };
		public const UInt32 MAX_HIGH_SCORE = 9999999;
		public const UInt32 DEF_HIGH_SCORE = 100000;
		public const UInt32 COMPLETE_ROUND = 5000;

		// Global data.
		public const Boolean IsFixedTimeStep = true;
		public const UInt32 FramesPerSecond = 100;

		public const Byte BORDER_MENU = 16;
		public const Byte BORDER_GAME = 12;
		public static readonly Byte BORDER_HIGH = 12;

		public const Byte POPUPS_WIDE = 6;
		public const Byte POPUPS_HIGH = 4;
		public const Byte MIN_TILE = 0;
		public const Byte MAX_TILE = 9;
		public const Byte EXIT_LOWER = 3;
		public const Byte EXIT_UPPER = 8;
		public const Byte DIV_BONUS_VALUE = 70;
		public const Byte MAX_BONUS_VALUE = 15;
		public const Byte MAX_LAYOUT = 2;
		public const Single JOYSTICK_TOLERANCE = 0.4f;

		// Hard coded text strings. (Not in files).
		public const String SELECT_TEXT = "SELECT";
		public const String SELECT_FONE = "TAP TO";
		public const String SELECT_WORK = "ENTER:";
		public const String SELECT_XBOX = "\"A\" TO";
		public const String PERFECT_TEXT = "PERFECT!";
		public const String LOCKED_TEXT = "LOCKED!";
		public const String DOUBLE1_TEXT = "DOUBLE";
		public const String DOUBLE2_TEXT = "BONUS!";
		public const String FREE_TEXT = "FREE";

		public const Byte PlayerPosition = 11;
		public const Byte AdrianaPosition = 81;
		public const Byte SuzannePosition = 18;
		public const Byte SteveProPosition = 88;

#if WINDOWS && DEBUG
		public const Boolean IsFullScreen = false;
		public const Boolean IsMouseVisible = true;
#endif
#if WINDOWS && !DEBUG
		public const Boolean IsFullScreen = true;
		public const Boolean IsMouseVisible = true;
#endif
#if WINDOWS
		public const UInt16 ScreenWide = 640;
		public const UInt16 ScreenHigh = 480;

		public const Boolean UseExposed = true;
		public const UInt16 ExposeWide = 640;
		public const UInt16 ExposeHigh = 480;

		public const Byte MenuVelocity = 60;
		public const Byte TileRatio = 8;
		public const Byte TilesSize = 40;
		public const Byte EntityOffset = 4;
		public const Byte GameOffsetX = 0;
		public const SByte FontOffsetX = -1;
		public const SByte FontOffsetY = -4;
#endif
#if !WINDOWS
		public const Boolean IsFullScreen = true;
		public const Boolean IsMouseVisible = false;
		public const UInt16 ScreenWide = 800;
		public const UInt16 ScreenHigh = 480;

		public const Boolean UseExposed = false;
		public const UInt16 ExposeWide = 800;
		public const UInt16 ExposeHigh = 480;

		public const Byte MenuVelocity = 60;
		public const Byte TileRatio = 8;
		public const Byte TilesSize = 40;
		public const Byte EntityOffset = 4;
		public const Byte GameOffsetX = 80;
		public const SByte FontOffsetX = -1;
		public const SByte FontOffsetY = -4;
#endif

#if ANDROID
		public const String UnlockUrl = @"https://play.google.com/store/apps/details?id=com.steveproxna.candykid";
#endif
#if IOS
		public const String UnlockUrl = @"https://itunes.apple.com/app/retro-candy-kid/id1055478836";
#endif

		public const UInt16 TrialLevel = 30;
#if PAID_GAME
		public const Boolean TrialedGame = false;
#elif FREE_GAME
		public const Boolean TrialedGame = true;
#else
		public const Boolean TrialedGame = true;
#endif
	}

}