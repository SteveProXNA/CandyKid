using System;
using WindowsGame.Static;

namespace WindowsGame.Data
{
	public struct GlobalConfigData
	{
		public ScreenType ScreenType;
		public Boolean UseKillTrees;
		public Boolean UseOpenExits;
		public Boolean UsePlayMusic;
		public Boolean UsePlaySound;
		public Byte ScoreLives;
		public UInt16 ClockLevel;
		public Byte ScoreWorld;
		public Byte ScoreRound;
		public Byte TotalWorld;
		public Byte TotalRound;
		public UInt16 ExtraLives;
		public Boolean MoveAdriana;
		public Boolean MoveSuzanne;
		public Boolean MoveStevePro;
		public Byte GamerVelIndex;
		public Byte EnemyVelIndex;
		public Byte NewArrowIndex;
		public Byte GamerSpriteIndex;
		public Byte EnemyOneSpriteIndex;
		public Byte EnemyTwoSpriteIndex;
		public Byte EnemyXyzSpriteIndex;
		public UInt16 SplashDelay;
		public UInt16 TitleDelay;
		public UInt16 DemoDelay;
		public UInt16 LoadDelay;
		public UInt16 CompDelay;
		public UInt16 DeadDelay;
		public UInt16 OverDelay;
		public UInt16 BeatDelay;
		public UInt16 UnlockDelay;
		public Boolean LoadZoomOut;
		public Boolean IsInGodMode;
		public Boolean CanContinue;
		public Boolean QuitsToExit;
		public Boolean ResetEnemies;
		public Boolean RefreshGame;
	}
}