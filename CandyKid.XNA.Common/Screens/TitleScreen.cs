using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class TitleScreen : BaseScreen, IScreen
	{
		private const Byte MaxStrip = 14;
		private Vector2[] positions;
		private Vector2 candyKid, candyMama, candyEat, bonusEat, safeTree, killTree, bonusPosition, freePosition;
		private UInt16 timer0, timer1, timer2;
		private Rectangle gamerBase, gamerRect, enemyBase, enemyRect, enemyOneRect, enemyTwoRect, enemyXyzRect;
		private Byte bonusIndex, candyIndex;
		private Boolean flag, trialedGame;
		private Byte frame;
		private String[] bonusTexts;

		public override void Initialize()
		{
			BannerTexture = Assets.TitleTexture;
			BannerPosition = new Vector2((BaseData.ScreenWide - BannerTexture.Width) / 2.0f, BaseData.TilesSize);
			LoadTextData();

			positions = GetPositions(9, 20);
			const Byte left = 3;
			const Byte rght = 9;
			const Byte lower = 8;
			const Byte middl = 12;
			const Byte upper = 16;
			candyKid = GetNewVector2(left, lower, BaseData.GamerSize);
			safeTree = GetVector2(left, middl, BaseData.TreesSize);
			candyEat = GetVector2(left, upper, BaseData.CandySize);

			candyMama = GetNewVector2(rght, lower, BaseData.GamerSize);
			killTree = GetVector2(rght, middl, BaseData.TreesSize);
			bonusEat = GetVector2(rght, upper, BaseData.BonusSize);

			bonusTexts = GetBonusText();
			bonusPosition = MyGame.Manager.TextManager.GetTextPosition(20, 17, BaseData.TextsSize, BaseData.GameOffsetX, BaseData.FontOffsetX, BaseData.FontOffsetY);

			freePosition = MyGame.Manager.TextManager.GetTextPosition(14, 5, BaseData.TextsSize, BaseData.GameOffsetX, BaseData.FontOffsetX, BaseData.FontOffsetY - 1);
			trialedGame = BaseData.TrialedGame;
		}

		public override void LoadContent()
		{
			gamerBase = gamerRect = MyGame.Manager.ImageManager.GamerOneRectangle[BaseData.GamerSpriteIndex];
			enemyOneRect = MyGame.Manager.ImageManager.EnemyOneRectangle[BaseData.EnemyOneSpriteIndex];
			enemyTwoRect = MyGame.Manager.ImageManager.EnemyTwoRectangle[BaseData.EnemyTwoSpriteIndex];
			enemyXyzRect = MyGame.Manager.ImageManager.EnemyXyzRectangle[BaseData.EnemyXyzSpriteIndex];
			enemyBase = enemyRect = enemyXyzRect;

			candyIndex = GetCandyIndex();
			bonusIndex = 0;
			timer0 = timer1 = timer2 = 0;
			flag = false;

			SetFrameRect(0);
		}

		public ScreenType Update(GameTime gameTime)
		{
			if (ImmediateExit2())
			{
				return BaseData.TrialedGame ? ScreenType.Unlock : ScreenType.Exit;
			}

			timer0 += (UInt16)(gameTime.ElapsedGameTime.Milliseconds);
			if (timer0 > BaseData.TitleDelay)
			{
				BaseData.PrevScreen = ScreenType.Title;
				return ScreenType.Demo;
			}
			timer1 += (UInt16)(gameTime.ElapsedGameTime.Milliseconds);
			if (timer1 > DELAY1)
			{
				timer1 -= DELAY1;
				candyIndex = GetCandyIndex();
				flag = !flag;
			}
			timer2 += (UInt16)(gameTime.ElapsedGameTime.Milliseconds);
			if (timer2 > Constants.TITLE_DELAY)
			{
				timer2 -= Constants.TITLE_DELAY;
				SetFrameRect((Byte)(1 - frame));

				bonusIndex++;
				if (bonusIndex % 2 == 0)
				{
					SwapCandyMama();
				}
				if (bonusIndex >= Constants.BONUS_EXISTS)
				{
					bonusIndex = 0;
				}
			}

			Boolean next = MyGame.Manager.InputManager.Next();
			if (next)
			{
				MyGame.Manager.SoundManager.PlayBonusSoundEffect();
				return ScreenType.Menu;
			}

			return ScreenType.Title;
		}

		public override void Draw()
		{
			Engine.SpriteBatch.Draw(BannerTexture, BannerPosition, Color.White);
			MyGame.Manager.BorderManager.DrawMenu();

			Engine.SpriteBatch.Draw(Assets.SpritemapsTexture, candyKid, gamerRect, Color.White);
			Engine.SpriteBatch.Draw(Assets.SpritemapsTexture, candyMama, enemyRect, Color.White);
			Engine.SpriteBatch.Draw(Assets.TilemapsTexture, safeTree, MyGame.Manager.ImageManager.TreesStandardRectangle, Color.White);
			Engine.SpriteBatch.Draw(Assets.TilemapsTexture, killTree, MyGame.Manager.ImageManager.TreesElectricRectangle, Color.White);
			Engine.SpriteBatch.Draw(Assets.TilemapsTexture, candyEat, MyGame.Manager.ImageManager.AllCandyRectangles[candyIndex], Color.White);

			Engine.SpriteBatch.Draw(Assets.TilemapsTexture, bonusEat, MyGame.Manager.ImageManager.AllBonusRectangles[bonusIndex], Color.White);
			MyGame.Manager.TextManager.Draw(TextDataList);
			if (flag)
			{
				for (Byte index = 0; index < MaxStrip; ++index)
				{
					Engine.SpriteBatch.Draw(Assets.NewArrowTexture, positions[index], MyGame.Manager.ImageManager.BlackStripRectangle, Color.White);
				}
			}

			// Draw all text.
			Engine.SpriteBatch.DrawString(Assets.EmulogicFont, bonusTexts[bonusIndex], bonusPosition, Color.White);
			if (trialedGame)
			{
				Engine.SpriteBatch.DrawString(Assets.EmulogicFont, Constants.FREE_TEXT, freePosition, Color.White);
			}
		}

		private static Vector2[] GetPositions(Byte x, Byte y)
		{
			Vector2[] positions = new Vector2[MaxStrip];
			for (Byte index = 0; index < MaxStrip; ++index)
			{
				Vector2 position = new Vector2(BaseData.GameOffsetX + BaseData.TextsSize * (x + index), BaseData.TextsSize * y);
				positions[index] = position;
			}

			return positions;
		}
		private static String[] GetBonusText()
		{
			String[] texts = new String[Constants.BONUS_EXISTS];
			for (Byte index = 0; index < Constants.BONUS_EXISTS; ++index)
			{
				UInt16 score = Constants.BONUS_SCORE[index];
				texts[index] = String.Format("{0}PTS", score);
			}

			return texts;
		}
		private static Byte GetCandyIndex()
		{
			return (Byte)(MyGame.Manager.NumberManager.Generate(Constants.CANDY_NUMBER - 1));
		}

		private void SetFrameRect(Byte theFrame)
		{
			frame = theFrame;

			Rectangle gamerRectSource = gamerRect;
			gamerRectSource.X = gamerBase.X + BaseData.GamerSize * frame;
			gamerRect = gamerRectSource;

			Rectangle enemyRectSource = enemyRect;
			enemyRectSource.X = enemyBase.X + BaseData.GamerSize * frame;
			enemyRect = enemyRectSource;
		}

		private void SwapCandyMama()
		{
			if (enemyXyzRect == enemyRect)
			{
				enemyBase = enemyRect = enemyOneRect;
			}
			else if (enemyOneRect == enemyRect)
			{
				enemyBase = enemyRect = enemyTwoRect;
			}
			else if (enemyTwoRect == enemyRect)
			{
				enemyBase = enemyRect = enemyXyzRect;
			}
		}

	}
}