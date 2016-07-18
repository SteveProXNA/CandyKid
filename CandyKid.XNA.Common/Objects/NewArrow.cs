using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Objects
{
	public class NewArrow
	{
		private readonly Rectangle normalRect;
		private readonly Rectangle invertRect;
		private Vector2 position;
		private readonly SpriteEffects effects;

		public NewArrow(Direction direction, Rectangle normalRect, Rectangle invertRect, SpriteEffects effects)
		{
			Direction = direction;

			this.position = Vector2.Zero;
			this.normalRect = normalRect;
			this.invertRect = invertRect;
			this.effects = effects;
		}

		public void SetPosition(Vector2 newPostion)
		{
			Vector2 tmpPosition = Vector2.Zero;
			tmpPosition.X = newPostion.X;
			tmpPosition.Y = newPostion.Y;
			position = tmpPosition;
		}

		public void DrawNormal()
		{
			Draw(normalRect);
		}
		public void DrawInvert()
		{
			Draw(invertRect);
		}
		private void Draw(Rectangle rect)
		{
			Engine.SpriteBatch.Draw(Assets.NewArrowTexture, position, rect, Color.White, 0.0f, Vector2.Zero, 1.0f, effects, 1.0f);
		}

		public Direction Direction { get; private set; }
	}
}