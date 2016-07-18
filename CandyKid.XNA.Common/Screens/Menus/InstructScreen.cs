using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Screens.Menus
{
	public class InstructScreen : MainScreen, IScreen
	{
		private UInt16 timer1;
		private Vector2 candyEat, bonusEat;
		private Vector2[] safePositions, killPositions, treePositions;
		private Rectangle source;
		private Byte bonusIndex, candyIndex;
		private Boolean trialedGame;

		public override void Initialize()
		{
			candyEat = MyGame.Manager.BoardManager.GetCandyVector2(31);
			bonusEat = MyGame.Manager.BoardManager.GetBonusVector2(51);

			safePositions = GetSafePositions();
			killPositions = GetKillPositions();
			treePositions = GetTreePositions();

			trialedGame = BaseData.TrialedGame;
			LoadTextData();
		}

		public override void LoadContent()
		{
			base.LoadContent();
			NextScreen = ScreenType.SubMenuOne;

			MyGame.Manager.EntityManager.ResetPlayer();
			MyGame.Manager.EntityManager.ResetEnemies();

			candyIndex = GetCandyIndex();
			timer1 = 0;

			source = BaseData.UseKillTrees ? MyGame.Manager.ImageManager.TreesElectricRectangle : MyGame.Manager.ImageManager.TreesStandardRectangle;
		}

		public ScreenType Update(GameTime gameTime)
		{
			timer1 += (UInt16)(gameTime.ElapsedGameTime.Milliseconds);
			if (timer1 > Constants.TITLE_DELAY)
			{
				timer1 -= Constants.TITLE_DELAY;
				bonusIndex++;
				if (bonusIndex >= Constants.BONUS_EXISTS)
				{
					bonusIndex = 0;
				}
			}

			if (!trialedGame)
			{
				Boolean escape = MyGame.Manager.InputManager.Pause() || MyGame.Manager.InputManager.Escape();
				if (escape)
				{
					return NextScreen;
				}

				ProcessSwappedSprites();

#if !WINDOWS
				Direction direction = MyGame.Manager.InputManager.HoldDirection();
				return Direction.None == direction ? ScreenType.Instruct : NextScreen;
#endif
			}

			return Update(gameTime, ScreenType.Instruct);
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.EntityManager.Draw();

			Engine.SpriteBatch.Draw(Assets.TilemapsTexture, candyEat, MyGame.Manager.ImageManager.AllCandyRectangles[candyIndex], Color.White);
			Engine.SpriteBatch.Draw(Assets.TilemapsTexture, bonusEat, MyGame.Manager.ImageManager.AllBonusRectangles[bonusIndex], Color.White);

			for (Byte index = 0; index < 2; ++index)
			{
				Engine.SpriteBatch.Draw(Assets.TilemapsTexture, killPositions[index], MyGame.Manager.ImageManager.TreesElectricRectangle, Color.White);
				Engine.SpriteBatch.Draw(Assets.TilemapsTexture, safePositions[index], MyGame.Manager.ImageManager.TreesStandardRectangle, Color.White);
			}
			for (Byte index = 0; index < 4; ++index)
			{
				Engine.SpriteBatch.Draw(Assets.TilemapsTexture, treePositions[index], source, Color.White);
			}

			MyGame.Manager.TextManager.Draw(TextDataList);
		}

		// Code smell but will do for now!!
		private static void ProcessSwappedSprites()
		{
			Quadrant quadrant = MyGame.Manager.InputManager.HoldQuadrant();
			if (Quadrant.None == quadrant)
			{
				return;
			}

			if (Quadrant.TopLeft == quadrant)
			{
				Byte index = BaseData.GamerSpriteIndex;
				index = (Byte)(1 - index);
				BaseData.SetGamerSpriteIndex(index);
				Rectangle rectangle = MyGame.Manager.ImageManager.GamerOneRectangle[index];
				MyGame.Manager.EntityManager.Player.SetSource(rectangle);
			}
			else if (Quadrant.BotLeft == quadrant)
			{
				Byte index = BaseData.EnemyOneSpriteIndex;
				index = (Byte)(1 - index);
				BaseData.SetEnemyOneSpriteIndex(index);
				Rectangle rectangle = MyGame.Manager.ImageManager.EnemyOneRectangle[index];
				MyGame.Manager.EntityManager.Enemies[EnemyType.Adriana].SetSource(rectangle);
			}
			else if (Quadrant.TopRight == quadrant)
			{
				Byte index = BaseData.EnemyTwoSpriteIndex;
				index = (Byte)(1 - index);
				BaseData.SetEnemyTwoSpriteIndex(index);
				Rectangle rectangle = MyGame.Manager.ImageManager.EnemyTwoRectangle[index];
				MyGame.Manager.EntityManager.Enemies[EnemyType.Suzanne].SetSource(rectangle);
			}
			else if (Quadrant.BotRight == quadrant)
			{
				Byte index = BaseData.EnemyXyzSpriteIndex;
				index = (Byte)(1 - index);
				BaseData.SetEnemyXyzSpriteIndex(index);
				Rectangle rectangle = MyGame.Manager.ImageManager.EnemyXyzRectangle[index];
				MyGame.Manager.EntityManager.Enemies[EnemyType.StevePro].SetSource(rectangle);
			}

			MyGame.Manager.SoundManager.PlayBonusSoundEffect();
		}
		private static Vector2[] GetSafePositions()
		{
			Vector2[] theSafePositions = new Vector2[2];
			theSafePositions[0] = GetVector2(13, 06, BaseData.TreesSize);
			theSafePositions[1] = GetVector2(14, 06, BaseData.TreesSize);
			return theSafePositions;
		}
		private static Vector2[] GetKillPositions()
		{
			Vector2[] theKillPositions = new Vector2[2];
			theKillPositions[0] = GetVector2(13, 16, BaseData.TreesSize);
			theKillPositions[1] = GetVector2(14, 16, BaseData.TreesSize);
			return theKillPositions;
		}
		private static Vector2[] GetTreePositions()
		{
			Vector2[] theTreePositions = new Vector2[4];
			theTreePositions[0] = GetVector2(05, 04, BaseData.TreesSize);
			theTreePositions[1] = GetVector2(06, 04, BaseData.TreesSize);
			theTreePositions[2] = GetVector2(05, 18, BaseData.TreesSize);
			theTreePositions[3] = GetVector2(06, 18, BaseData.TreesSize);
			return theTreePositions;
		}

		private static Byte GetCandyIndex()
		{
			return (Byte)(MyGame.Manager.NumberManager.Generate(Constants.CANDY_NUMBER - 1));
		}

	}
}