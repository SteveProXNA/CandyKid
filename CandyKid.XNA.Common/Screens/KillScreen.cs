using System;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Library;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public abstract class KillScreen : BaseScreen
	{
		private Vector2[] stripePositions;
		private Vector2 deathPosition;

		private Rectangle deathSource, stripSource;

		public override void Initialize()
		{
			stripePositions = new Vector2[Constants.DEATH_NUMBER];
			deathSource = MyGame.Manager.ImageManager.DeathStandardRectangle;
			stripSource = MyGame.Manager.ImageManager.BlackStripRectangle;
		}

		public override void LoadContent()
		{
			CandyKid player = MyGame.Manager.EntityManager.Player;
			deathPosition = player.Position;

			GetStripePositions();
			base.LoadContent();
		}

		public override void Draw()
		{
			// Draw death skull n' crossbones.
			for (Byte index = 0; index < Constants.DEATH_NUMBER; ++index)
			{
				Engine.SpriteBatch.Draw(Assets.NewArrowTexture, stripePositions[index], stripSource, Color.White);
			}

			Engine.SpriteBatch.Draw(Assets.TilemapsTexture, deathPosition, deathSource, Color.White);
		}

		private void GetStripePositions()
		{
			stripePositions[0].X = deathPosition.X - BaseData.EntityOffset;
			stripePositions[0].Y = deathPosition.Y - BaseData.EntityOffset;
			stripePositions[1].X = stripePositions[0].X;
			stripePositions[1].Y = stripePositions[0].Y + BaseData.TextsSize;
			stripePositions[2].X = stripePositions[0].X + BaseData.TextsSize;
			stripePositions[2].Y = stripePositions[0].Y;
			stripePositions[3].X = stripePositions[0].X + BaseData.TextsSize;
			stripePositions[3].Y = stripePositions[0].Y + BaseData.TextsSize;
		}

	}
}