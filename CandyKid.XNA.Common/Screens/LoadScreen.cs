using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class LoadScreen : BaseScreen, IScreen
	{
		private Vector2 worldPosition, roundPosition, basePosition;
		private String worldText, roundText;
		private Single delay1, delay2, delta, scale;
		private Boolean flag1, flag2;
		private CandyKid candyKid;

		public override void Initialize()
		{
			worldPosition = MyGame.Manager.TextManager.GetTextPosition(14, 11);
			roundPosition = MyGame.Manager.TextManager.GetTextPosition(14, 12);

			delay1 = BaseData.LoadDelay - (BaseData.LoadDelay / 2.0f);
			delay2 = BaseData.LoadDelay - (BaseData.LoadDelay / 10.0f);

			candyKid = MyGame.Manager.EntityManager.GetBasePlayer(MyGame.Manager.ImageManager.GamerOneRectangle[BaseData.GamerSpriteIndex], BaseData.PlayerPosition, BaseData.PlayerVelocity);
			candyKid.Initialize(BaseData.MinTile, BaseData.MaxTile);
			basePosition = candyKid.Position;
			LoadTextData();
		}

		public override void LoadContent()
		{
			base.LoadContent();
			//LoadTextData();

			if (BaseData.TrialedGame && BaseData.ScoreLevel > BaseData.TrialLevel)
			{
				BaseData.SetMaxTrialLevel();
			}

			Byte world = BaseData.ScoreWorld;
			Byte round = BaseData.ScoreRound;
			MyGame.Manager.ScoreManager.InsertLevel(world, round);
			MyGame.Manager.BoardManager.LoadContent(world, round);

			UInt16 level = BaseData.ScoreLevel;
			MyGame.Manager.ScoreManager.InsertLevel(level);

			Boolean bonusFreePlayer = GetBonusFreePlayer(level, round);
			BaseData.SetBonusFreePlayer(bonusFreePlayer);
			if (bonusFreePlayer)
			{
				MyGame.Manager.BoardManager.XtraContent();
			}
			worldText = MyGame.Manager.ScoreManager.ScoreList[ScoreType.World].Text;
			roundText = MyGame.Manager.ScoreManager.ScoreList[ScoreType.Round].Text;

			MyGame.Manager.EntityManager.ResetPlayer();
			MyGame.Manager.EntityManager.ResetEnemies();

			// Set gamer crash for this level.
			MyGame.Manager.CollisionManager.SetGamerCrash(BaseData.GamerCrashMin, BaseData.GamerCrashMax, world);

			// Update gamer velocity + enemy attacker.
			UpdateEntities(world);
			flag1 = flag2 = true;

			candyKid.SetSource(MyGame.Manager.ImageManager.GamerOneRectangle[BaseData.GamerSpriteIndex]);
		}

		public ScreenType Update(GameTime gameTime)
		{
			Boolean board = MyGame.Manager.InputManager.Board();
			if (board)
			{
				// If player prompt then "forward".
				Timer = (UInt16)delay1;
			}

			if (flag1)
			{
				//http://johnnygizmo.blogspot.ie/2008/10/xna-sidebar-smoothstep-and-lerp.htmls
				var percentage = Timer / delay1;
				var input = MathHelper.SmoothStep(0, 1, percentage);
				input = 1 - input;
				scale = 3 * input + 1;
			}

			UpdateTimer(gameTime);
			if (Timer >= delay1)
			{
				if (flag1)
				{
					MyGame.Manager.SoundManager.PlayReadySoundEffect();
				}
				flag1 = false;
			}
			if (Timer >= delay2)
			{
				flag2 = false;
			}
			if (Timer > BaseData.LoadDelay)
			{
				MyGame.Manager.SoundManager.StartMusic();
				return ScreenType.Play;
			}

			return ScreenType.Load;
		}

		public override void Draw()
		{
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.BoardManager.Draw();

			MyGame.Manager.DeviceManager.DrawStripes();
			MyGame.Manager.DeviceManager.DrawGameArrows(Direction.None);

			if (flag2)
			{
				if (BaseData.LoadZoomOut)
				{
					DrawScalePlayer();
				}
				else
				{
					MyGame.Manager.EntityManager.DrawPlayer();
				}

				MyGame.Manager.BorderManager.DrawOver();
				MyGame.Manager.TextManager.Draw(TextDataList);

				Engine.SpriteBatch.DrawString(Assets.EmulogicFont, worldText, worldPosition, Color.White);
				Engine.SpriteBatch.DrawString(Assets.EmulogicFont, roundText, roundPosition, Color.White);
			}
			else
			{
				MyGame.Manager.EntityManager.DrawPlayer();
			}

			MyGame.Manager.EntityManager.DrawEnemies();

			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.ScoreManager.Draw();
		}

		private void DrawScalePlayer()
		{
			delta = (scale - 1) * (BaseData.GamerSize / 2.0f);
			Vector2 postion = basePosition;

			postion.X -= delta;
			postion.Y -= delta;
			if (postion.X > basePosition.X)
			{
				postion.X = basePosition.X;
			}
			if (postion.Y > basePosition.Y)
			{
				postion.Y = basePosition.Y;
			}

			candyKid.SetPosition(postion);
			candyKid.DrawScale(scale);
		}

		private static Boolean GetBonusFreePlayer(UInt16 level, Byte round)
		{
			// Trial game = no extra!
			if (BaseData.TrialedGame)
			{
				return false;
			}

			// Every fifth round make it 50-50 chance to get free man.
			Byte data = 2;
			if (0 != round % 5)
			{
				UInt16 temp = (UInt16)(((level / Constants.DIV_BONUS_VALUE) * 5) + 5);
				data = temp >= Constants.MAX_BONUS_VALUE ? Constants.MAX_BONUS_VALUE : (Byte)(temp);
			}

			return 0 == MyGame.Manager.NumberManager.Generate(data);
		}

		private static void UpdateEntities(Byte world)
		{
			Byte min = world;
			Byte max = (Byte)(world + 5);
			if (min > 5) { min = 5; }
			if (max > 15) { max = 15; }

			Byte gamerVelIndex = BaseData.GamerVelIndex;
			Byte enemyVelIndex = BaseData.EnemyVelIndex;

			Byte randomVelocity = Generate(min, max);
			Byte playerVelocity = BaseData.PlayerVelocityArr[gamerVelIndex];
			MyGame.Manager.EntityManager.UpdatePlayerVelocity((Byte)(playerVelocity + randomVelocity));

			randomVelocity = Generate(min, max);
			if (!BaseData.UseOpenExits)
			{
				randomVelocity /= 3;
			}
			Byte adrianaVelocity = BaseData.AdrianaVelocityArr[enemyVelIndex];
			MyGame.Manager.EntityManager.UpdateEnemyVelocity(EnemyType.Adriana, (Byte)(adrianaVelocity + randomVelocity));

			randomVelocity = Generate(min, max);
			Byte suzanneVelocity = BaseData.SuzanneVelocityArr[enemyVelIndex];
			MyGame.Manager.EntityManager.UpdateEnemyVelocity(EnemyType.Suzanne, (Byte)(suzanneVelocity + randomVelocity));

			randomVelocity = Generate(min, max);
			if (!BaseData.UseOpenExits)
			{
				randomVelocity /= 2;
			}
			Byte steveproVelocity = BaseData.SteveProVelocityArr[enemyVelIndex];
			MyGame.Manager.EntityManager.UpdateEnemyVelocity(EnemyType.StevePro, (Byte)(steveproVelocity + randomVelocity));

			randomVelocity = Generate(min, max);
			Byte adrianaAttacker = BaseData.AdrianaAttackerArr[enemyVelIndex];
			MyGame.Manager.EntityManager.UpdateEnemyAttacker(EnemyType.Adriana, (Byte)(adrianaAttacker + randomVelocity));

			randomVelocity = Generate(min, max);
			Byte suzanneAttacker = BaseData.SuzanneAttackerArr[enemyVelIndex];
			MyGame.Manager.EntityManager.UpdateEnemyAttacker(EnemyType.Suzanne, (Byte)(suzanneAttacker + randomVelocity));

			randomVelocity = Generate(min, max);
			Byte steveproAttacker = BaseData.SteveProAttackerArr[enemyVelIndex];
			MyGame.Manager.EntityManager.UpdateEnemyAttacker(EnemyType.StevePro, (Byte)(steveproAttacker + randomVelocity));
		}

		private static Byte Generate(Byte min, Byte max)
		{
			return (Byte)MyGame.Manager.NumberManager.Generate(min, max);
		}

	}
}