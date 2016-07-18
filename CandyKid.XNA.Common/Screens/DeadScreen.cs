using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class DeadScreen : KillScreen, IScreen
	{
		private Byte lives;

		public override void LoadContent()
		{
			base.LoadContent();

			MyGame.Manager.InputManager.ResetMotors();
			MyGame.Manager.InputManager.SetMotors(1, 0);

			if (BaseData.UseKillTrees)
			{
				CandyKid player = MyGame.Manager.EntityManager.Player;
				SByte col = player.CurrX;
				SByte row = player.CurrY;

				// Check if player in game play area before check if tree.
				if (col >= BaseData.MinTile && col <= BaseData.MaxTile &&
					row >= BaseData.MinTile && row <= BaseData.MaxTile)
				{
					// If death tree in play area then remove for player compensation!!
					TileType tileType = MyGame.Manager.BoardManager.BoardData[row, col];
					if (TileType.Trees == tileType)
					{
						Byte location = MyGame.Manager.BoardManager.CalcLocation((Byte)col, (Byte)row);
						MyGame.Manager.BoardManager.ClearTreesTile(location);
					}
				}
			}

			MyGame.Manager.ScoreManager.UpdateLives(-1);
			lives = (Byte)MyGame.Manager.ScoreManager.ScoreList[ScoreType.Lives].Value;
			if (lives <= 0)
			{
				MyGame.Manager.SoundManager.PlayKilldSoundEffect();
			}
			else
			{
				MyGame.Manager.SoundManager.PlayDeathSoundEffect();
			}
		}

		public ScreenType Update(GameTime gameTime)
		{
			// If game over but continues enabled then go straight to continue popup.
			if (lives <= 0)
			{
				return BaseData.CanContinue ? ScreenType.Cont : ScreenType.Over;
			}

			return ScreenType.DeadX;
		}

		public override void Draw()
		{
			// MUST draw everything in this order!
			MyGame.Manager.BorderManager.DrawGame();
			MyGame.Manager.BoardManager.Draw();

			// Draw death skull n' crossbones.
			base.Draw();
			MyGame.Manager.EntityManager.DrawEnemies();

			MyGame.Manager.DeviceManager.DrawStripes();
			MyGame.Manager.DeviceManager.DrawGameArrows(Direction.None);

			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.ScoreManager.Draw();
		}
	}
}