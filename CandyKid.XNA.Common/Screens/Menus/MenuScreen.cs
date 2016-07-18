using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Screens.Menus
{
	public class MenuScreen : MainScreen, IScreen
	{
		private Vector2 basePosition, position2, freePosition;
		private UInt16 wrapAmount;
		private Byte distance;
		private Boolean trialedGame;

		public override void Initialize()
		{
			BannerTexture = Assets.TitleTexture;
			BannerPosition = new Vector2((BaseData.ScreenWide - BannerTexture.Width) / 2.0f, BaseData.TilesSize);
			LoadTextData();

			basePosition = GetNewVector2(Constants.MENUS_LEFCOL, Constants.MENUS_TOPEND, BaseData.GamerSize);
			position2 = GetNewVector2(Constants.MENUS_RGTCOL, Constants.MENUS_TOPEND, BaseData.GamerSize);
			wrapAmount = (UInt16)(Constants.MENUS_SPACES * BaseData.TextsSize * Constants.MENUS_NUMBER);

			Rectangle source = MyGame.Manager.ImageManager.GamerOneRectangle[BaseData.GamerSpriteIndex];
			distance = (Byte)(Constants.MENUS_SPACES * BaseData.TextsSize);
			Byte velocity = BaseData.MenuVelocity;

			ConstructCandyKid(Constants.MENUS_LEFCOL, Constants.MENUS_TOPEND, basePosition, source, BaseData.EntityOffset, velocity, distance, BaseData.GamerSize);

			freePosition = MyGame.Manager.TextManager.GetTextPosition(14, 5, BaseData.TextsSize, BaseData.GameOffsetX, BaseData.FontOffsetX, BaseData.FontOffsetY - 1);
			trialedGame = BaseData.TrialedGame;
		}

		public override void LoadContent()
		{
			NextScreen = ScreenType.Menu;
			base.LoadContent();
		}

		public ScreenType Update(GameTime gameTime)
		{
			if (ImmediateExit())
			{
				return ScreenType.Title;
			}

			if (MenuSelect)
			{
				if (Lifecycle.Idle == CandyKid.Lifecycle)
				{
					return NextScreen;
				}

				CandyKid.Update(gameTime);
			}
			else
			{
				// Move Candy Kid up or down.
				if (Lifecycle.Move == CandyKid.Lifecycle)
				{
					CandyKid.Update(gameTime);
				}
				else
				{
					if (Direction.None != CandyKid.Direction)
					{
						CandyKid.None();
					}
					else
					{
						// Candy Kid idle so detect move.
						Direction menuDirection = MyGame.Manager.InputManager.MenuDirection();
						Boolean menuUp = Direction.Up == menuDirection;
						Boolean menuDown = Direction.Down == menuDirection;
						if (menuUp)
						{
							if (Constants.MENUS_TOPEND == CandyKid.CurrY)
							{
								CandyKid.MenuWrap(CandyKid.CurrX, (Constants.MENUS_BOTTOM + 1), 0, (Int16)(1 * wrapAmount));
							}

							CandyKid.Move(Direction.Up);
						}
						if (menuDown)
						{
							if (Constants.MENUS_BOTTOM == CandyKid.CurrY)
							{
								CandyKid.MenuWrap(CandyKid.CurrX, (Constants.MENUS_TOPEND - 1), 0, (Int16)(-1 * wrapAmount));
							}

							CandyKid.Move(Direction.Down);
						}

						// Or test if select menu item.
						Byte nextChoose = MyGame.Manager.InputManager.MenuChoose();
						if (0 != nextChoose)
						{
							MenuSelect = true;
							CandyKid.SetCurrY(nextChoose);

							UInt16 delta = (UInt16)(nextChoose - Constants.MENUS_TOPEND);
							Vector2 position = new Vector2(basePosition.X, basePosition.Y + delta * distance);

							if (position != CandyKid.Position)
							{
								CandyKid.SetPosition(position);
							}

							CandyKid.Swap();
							NextScreen = GetNextScreen((Byte)CandyKid.CurrY);
						}
						Boolean next = MyGame.Manager.InputManager.MenuSelect();
						if (next)
						{
							MenuSelect = true;
							CandyKid.Swap();
							NextScreen = GetNextScreen((Byte)CandyKid.CurrY);
						}
					}
				}
			}

			return ScreenType.Menu;
		}

		public override void Draw()
		{
			Engine.SpriteBatch.Draw(BannerTexture, BannerPosition, Color.White);
			MyGame.Manager.BorderManager.DrawMenu();
			CandyKid.Draw();

			position2.Y = CandyKid.Position.Y;
			Engine.SpriteBatch.Draw(Assets.SpritemapsTexture, position2, CandyKid.Source, Color.White);
			MyGame.Manager.TextManager.Draw(TextDataList);

			if (trialedGame)
			{
				Engine.SpriteBatch.DrawString(Assets.EmulogicFont, Constants.FREE_TEXT, freePosition, Color.White);
			}
		}

		private ScreenType GetNextScreen(Byte y)
		{
			if (Constants.MENUS_BOTTOM == y)
			{
				return BaseData.TrialedGame ? ScreenType.Unlock : ScreenType.Exit;
			}

			MyGame.Manager.SoundManager.PlayBonusSoundEffect();
			if (Constants.MENUS_TOPEND == y)
			{
				MyGame.Manager.ScoreManager.ResetHigh();
				MyGame.Manager.ScoreManager.ResetLives();
				MyGame.Manager.ScoreManager.ResetScore();
				return ScreenType.Load;
			}
			if (Constants.MENUS_TOPEND + 1 == y)
			{
				return ScreenType.Select;
			}
			if (Constants.MENUS_TOPEND + 2 == y)
			{
				return ScreenType.Options;
			}
			if (Constants.MENUS_TOPEND + 3 == y)
			{
				return ScreenType.Title;
			}

			return ScreenType.Menu;
		}

	}
}