using System;

namespace WindowsGame.Data
{
	public class StoragePersistData
	{
		// High score.
		public UInt32 HighScore;

		// Level select.
		public Byte ScoreWorld;
		public Byte ScoreRound;

		// General game.
		public Boolean UseKillTrees;
		public Boolean UseOpenExits;
		public Boolean UsePlayMusic;
		public Boolean UsePlaySound;

		// Gamer option.
		public Boolean CanContinue;
		public Byte ScoreLives;
		public Byte GamerVelIndex;
		public Byte NewArrowIndex;
		public Boolean IsFullScreen;

		// Enemy option.
		public Boolean IsInGodMode;
		public Boolean ResetEnemies;
		public Byte EnemyVelIndex;

		public Boolean MoveAdriana;
		public Boolean MoveSuzanne;
		public Boolean MoveStevePro;

		// Sprite index.
		public Byte GamerSpriteIndex;
		public Byte EnemyOneSpriteIndex;
		public Byte EnemyTwoSpriteIndex;
		public Byte EnemyXyzSpriteIndex;
	}
}