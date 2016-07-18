using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Objects
{
	public abstract class BaseObject
	{
		protected Single Delta;
		private readonly Vector2 basePosition;
		private readonly Byte gamerSize;
		private Rectangle baseSource;

		protected BaseObject(Byte baseX, Byte baseY, Vector2 basePosition, Rectangle baseSource, Byte increase, Byte velocity, Byte distance, Byte gamerSize)
		{
			BaseX = baseX;
			BaseY = baseY;
			this.basePosition = basePosition;
			this.baseSource = baseSource;
			Reset();

			Increase = increase;
			Velocity = velocity;
			Distance = distance;

			this.gamerSize = gamerSize;
		}

		public virtual void Move(Direction direction)
		{
			Direction = direction;
			Lifecycle = Lifecycle.Move;
		}

		public void None()
		{
			PrevDirection = Direction;
			Direction = Direction.None;
		}

		public void Dead()
		{
			Direction = Direction.None;
			Lifecycle = Lifecycle.Dead;
		}

		public void Draw()
		{
			Engine.SpriteBatch.Draw(Assets.SpritemapsTexture, Position, Source, Color.White);
		}
		public void DrawScale(Single scale)
		{
			Engine.SpriteBatch.Draw(Assets.SpritemapsTexture, Position, Source, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 1.0f);
		}

		public abstract void Update(GameTime gameTime);

		public abstract void Stop();

		public void Reset()
		{
			CurrX = (SByte)BaseX;
			CurrY = (SByte)BaseY;
			PrevX = CurrX;
			PrevY = CurrY;
			Position = basePosition;
			Source = baseSource;
			Direction = Direction.None;
			Lifecycle = Lifecycle.Idle;
			PrevDirection = Direction;
			Delta = 0.0f;
			Frame = 0;
		}

		public void SetVelocity(Byte velocity)
		{
			Velocity = velocity;
		}
		
		public void SetSource(Rectangle rectangle)
		{
			if (rectangle == baseSource)
			{
				return;
			}

			baseSource = rectangle;
			Source = baseSource;
		}

		protected void UpdatePosition(GameTime gameTime)
		{
			Single secs = (Single)gameTime.ElapsedGameTime.TotalSeconds;
			Single secs2 = secs * Velocity;
			Single vals = Increase * secs2;

			// Correcting algorithm.
			if (Delta + vals >= Distance)
			{
				vals = Distance - Delta;
			}

			Delta += vals;
			Vector2 position = Position;
			switch (Direction)
			{
				case Direction.Left:
					position.X -= vals;
					break;
				case Direction.Right:
					position.X += vals;
					break;
				case Direction.Up:
					position.Y -= vals;
					break;
				case Direction.Down:
					position.Y += vals;
					break;
			}

			Position = position;
		}
		protected void UpdateLifeCycle(GameTime gameTime)
		{
			if (Delta >= Distance)
			{
				PrevX = CurrX;
				PrevY = CurrY;

				Delta -= Distance;
				switch (Direction)
				{
					case Direction.Left:
						CurrX -= 1;
						break;
					case Direction.Right:
						CurrX += 1;
						break;
					case Direction.Up:
						CurrY -= 1;
						break;
					case Direction.Down:
						CurrY += 1;
						break;
				}

				Lifecycle = Lifecycle.Idle;
			}
			else
			{
				Lifecycle = Lifecycle.Move;
			}
		}

		protected void SwapFrame()
		{
			Frame = (Byte)(1 - Frame);
			Rectangle source = Source;
			source.X = baseSource.X + gamerSize * Frame;
			Source = source;
		}

		public Byte BaseX { get; protected set; }
		public Byte BaseY { get; protected set; }
		public SByte PrevX { get; protected set; }
		public SByte PrevY { get; protected set; }
		public SByte CurrX { get; protected set; }
		public SByte CurrY { get; protected set; }
		public Vector2 Position { get; protected set; }
		public Direction Direction { get; protected set; }
		public Direction PrevDirection { get; protected set; }
		public Lifecycle Lifecycle { get; protected set; }
		public Rectangle Source { get; private set; }
		protected Byte Distance { get; private set; }
		protected Byte Frame { get; private set; }
		private Byte Increase { get; set; }
		private Byte Velocity { get; set; }
	}
}