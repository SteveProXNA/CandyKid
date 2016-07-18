using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class SelectScreen : BaseScreen, IScreen
	{
		private Direction saveDirection;
		private Byte world, round;
		private UInt16 level;
		private Vector2[] worldPosition, roundPosition, levelPosition1, levelPosition2;
		private Vector2 levelPosition, textPostion1, textPostion2, lockPosition, xtraPosition, twicePosition1, twicePosition2;
		private String worldName, roundName, levelName;
		private String selectText1, selectText2;
		private Boolean locked, twiceBonus;

		public override void Initialize()
		{
			GetTextPositions();

			saveDirection = Direction.None;
			world = round = BaseData.StartLevel;
			level = BaseData.StartLevel;
			UpdateNames(world, round, level);
		}

		public override void LoadContent()
		{
			base.LoadContent();
			LoadTextData((Byte)BaseData.GameLayout);

			world = BaseData.ScoreWorld;
			round = BaseData.ScoreRound;
			level = BaseData.ScoreLevel;
			locked = BaseData.TrialedGame && level > BaseData.TrialLevel;
			twiceBonus = level > Constants.DIV_BONUS_VALUE;

			UpdateNames(world, round, level);
			MyGame.Manager.BoardManager.LoadContent(world, round);

			// Set velocity for the (new) level?
			MyGame.Manager.EntityManager.ResetPlayer();
			MyGame.Manager.EntityManager.ResetEnemies();
		}

		public ScreenType Update(GameTime gameTime)
		{
			if (ImmediateExit2())
			{
				return ScreenType.Menu;
			}

			Boolean board = MyGame.Manager.InputManager.Board();
			if (board)
			{
				if (locked && level > BaseData.TrialLevel)
				{
					BaseData.SetMaxTrialLevel();
				}

				BaseData.SetWorldRound(world, round, level);
				return ScreenType.Menu;
			}

			Boolean released = MyGame.Manager.InputManager.Released();
			if (released)
			{
				saveDirection = Direction.None;
			}
			Direction currDirection = MyGame.Manager.InputManager.HoldDirection();
			if (Direction.None == currDirection)
			{
				return ScreenType.Select;
			}

			UInt16 prevLevel = level;
			saveDirection = currDirection;
			if (Direction.Up == saveDirection)
			{
				world += 1;
				if (world > BaseData.TotalWorld)
				{
					world = BaseData.StartLevel;
				}
			}
			if (Direction.Down == saveDirection)
			{
				world -= 1;
				if (world < BaseData.StartLevel)
				{
					world = BaseData.TotalWorld;
				}
			}

			if (Direction.Left == saveDirection)
			{
				round -= 1;
				if (round < BaseData.StartLevel)
				{
					round = BaseData.TotalRound;
				}
			}
			if (Direction.Right == saveDirection)
			{
				round += 1;
				if (round > BaseData.TotalRound)
				{
					round = BaseData.StartLevel;
				}
			}

			level = BaseData.GetLevelDataKey(world, round);
			locked = BaseData.TrialedGame && level > BaseData.TrialLevel;
			twiceBonus = level > Constants.DIV_BONUS_VALUE;

			if (prevLevel != level)
			{
				UpdateNames(world, round, level);
				MyGame.Manager.BoardManager.LoadContent(world, round);
			}

			return ScreenType.Select;
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawLock(locked);
			MyGame.Manager.BoardManager.Draw(locked);
			MyGame.Manager.EntityManager.Draw();
			MyGame.Manager.DeviceManager.DrawStripes();
			MyGame.Manager.DeviceManager.DrawGameArrows(saveDirection);

			if (LayoutType.Custom == BaseData.GameLayout)
			{
				if (locked)
				{
					Engine.SpriteBatch.Draw(Assets.TilemapsTexture, lockPosition, MyGame.Manager.ImageManager.TreesLockGameRectangle, Color.White);
					Engine.SpriteBatch.DrawString(Assets.EmulogicFont, Constants.LOCKED_TEXT, xtraPosition, Color.White);
				}
				else if (twiceBonus)
				{
					Engine.SpriteBatch.DrawString(Assets.EmulogicFont, Constants.DOUBLE1_TEXT, twicePosition1, Color.White);
					Engine.SpriteBatch.DrawString(Assets.EmulogicFont, Constants.DOUBLE2_TEXT, twicePosition2, Color.White);
				}

				Engine.SpriteBatch.DrawString(Assets.EmulogicFont, selectText1, textPostion1, Color.White);
				Engine.SpriteBatch.DrawString(Assets.EmulogicFont, selectText2, textPostion2, Color.White);
			}

			MyGame.Manager.TextManager.Draw(TextDataList);
			Engine.SpriteBatch.DrawString(Assets.EmulogicFont, worldName, worldPosition[(Byte)BaseData.GameLayout], Color.White);
			Engine.SpriteBatch.DrawString(Assets.EmulogicFont, roundName, roundPosition[(Byte)BaseData.GameLayout], Color.White);
			Engine.SpriteBatch.DrawString(Assets.EmulogicFont, levelName, levelPosition, Color.White);
		}

		private void GetTextPositions()
		{
			worldPosition = new Vector2[Constants.MAX_LAYOUT];
			roundPosition = new Vector2[Constants.MAX_LAYOUT];
			levelPosition1 = new Vector2[Constants.MAX_LAYOUT];
			levelPosition2 = new Vector2[Constants.MAX_LAYOUT];

			const SByte right = 29;
			worldPosition[(Byte)LayoutType.Custom] = MyGame.Manager.TextManager.GetTextPosition(right, 5);
			roundPosition[(Byte)LayoutType.Custom] = MyGame.Manager.TextManager.GetTextPosition(right, 9);
			levelPosition1[(Byte)LayoutType.Custom] = MyGame.Manager.TextManager.GetTextPosition(right, 17);
			levelPosition2[(Byte)LayoutType.Custom] = MyGame.Manager.TextManager.GetTextPosition((SByte)(right - 1), 17);

			worldPosition[(Byte)LayoutType.BotRight] = MyGame.Manager.TextManager.GetTextPosition(right, 4);
			roundPosition[(Byte)LayoutType.BotRight] = MyGame.Manager.TextManager.GetTextPosition(right, 7);
			levelPosition1[(Byte)LayoutType.BotRight] = MyGame.Manager.TextManager.GetTextPosition(-3, 23);
			levelPosition2[(Byte)LayoutType.BotRight] = MyGame.Manager.TextManager.GetTextPosition(-4, 23);

			textPostion1 = MyGame.Manager.TextManager.GetTextPosition(25, 12);
			textPostion2 = MyGame.Manager.TextManager.GetTextPosition(25, 13);

			lockPosition = GetVector2(13, 18, BaseData.TreesSize);
			xtraPosition = MyGame.Manager.TextManager.GetTextPosition(25, 20);

			twicePosition1 = MyGame.Manager.TextManager.GetTextPosition(25, 19);
			twicePosition2 = MyGame.Manager.TextManager.GetTextPosition(25, 20);

			selectText1 = MyGame.Manager.DeviceManager.LocalizationDict[LocalizeType.SelectText1];
			selectText2 = MyGame.Manager.DeviceManager.LocalizationDict[LocalizeType.SelectText2];
		}

		private void UpdateNames(Byte theWorld, Byte theRound, UInt16 theLevel)
		{
			worldName = GetWorldName(theWorld);
			roundName = GetRoundName(theRound);

			levelName = GetLevelName(theLevel);
			levelPosition = 2 == levelName.Length ? levelPosition1[(Byte)BaseData.GameLayout] : levelPosition2[(Byte)BaseData.GameLayout];
		}
		private static String GetWorldName(Byte theWorld)
		{
			return theWorld.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
		}
		private static String GetRoundName(Byte theRound)
		{
			return theRound.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
		}
		private static String GetLevelName(UInt16 theLevel)
		{
			return theLevel.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
		}

	}
}