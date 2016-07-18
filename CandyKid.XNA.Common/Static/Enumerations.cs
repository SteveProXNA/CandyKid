namespace WindowsGame.Static
{
	public enum Platform
	{
		Desk = 0,
		Port = 1
	}

	public enum TileType
	{
		Empty,
		Candy,
		Trees,
		Bonus1,
		Bonus2,
		Bonus3,
		Bonus4,
	}

	public enum ScreenType
	{
		Splash,
		Init,
		Title,
		Menu,
		SubMenuOne,
		SubMenuTwo,
		SubMenuWee,
		SubMenuXyz,
		Credits,
		Demo,
		History,
		Load,
		Options,
		Instruct,
		Select,
		Play,
		Stop,
		StopX,
		Comp,
		Beat,
		Dead,
		DeadX,
		Cont,
		Over,
		Unlock,
		Exit,
	}

	public enum EventType
	{
		None,
		EntityFree,
		EntityStay,
		PlayerMove,
		BadOneMove,
		BadTwoMove,
		BadThrMove,
		DeathTree,
		BonusTile,
		CandyTile,
		EatCandy,
		EatBonus,
		EatBonus1,
		EatBonus2,
		EatBonus3,
		EatBonus4,
	}

	public enum LayoutType
	{
		Custom = 0,
		BotRight = 1,
	}

	public enum Direction
	{
		None,
		Left,
		Right,
		Up,
		Down,
	}

	public enum Lifecycle
	{
		Idle,
		Move,
		Swap,
		Dead
	}

	public enum BehaveType
	{
		None,
		Horizontal,
		Vertical,
		Random,
	}

	public enum SizeType
	{
		//Pixel = 1,
		Candy = 2,
		Bonus = 3,
		Gamer = 4,
		Trees = 5,
		Arrow = 8
	}

	public enum ArrowType
	{
		WhiteTop,
		WhiteLeft,
		GrayTop,
		GrayLeft
	}

	public enum ScoreType
	{
		HighX,
		Score,
		Lives,
		Level,
		World,
		Round
	}

	public enum EnemyType
	{
		None,
		Adriana,
		Suzanne,
		StevePro
	}

	public enum Quadrant
	{
		None,
		TopLeft,
		BotLeft,
		TopRight,
		BotRight
	}

	public enum SoundEffectType
	{
		Celebrate,
		EatBonus,
		EatCandy,
		GameOver,
		GamerDie1,
		GamerKill,
		GetExtra,
		GetReady,
		MetalSolo,
		TrialBuzz,
	}

	public enum LocalizeType
	{
		SelectText1,
		SelectText2
	}

	public enum OverType
	{
		Explosion,
		GameOver,
		Advance
	}
}