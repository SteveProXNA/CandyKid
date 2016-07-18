using System;
using WindowsGame.Static;
using Microsoft.Xna.Framework;

namespace WindowsGame.Objects
{
	public class CandyMama : BaseObject
	{
		private Single frameDelta;
		private readonly Single frameTimer;

		public CandyMama(Byte baseX, Byte baseY, Vector2 basePosition, Rectangle baseSource, Byte increase, Byte velocity, Byte distance, Byte gamerSize, Byte attacker) :
			base(baseX, baseY, basePosition, baseSource, increase, velocity, distance, gamerSize)
		{
			Attacker = attacker;
			frameDelta = 0.0f;
			frameTimer = 1000;
		}

		public void Initialize(BehaveType behaveType)
		{
			BehaveType = behaveType;
		}

		public override void Update(GameTime gameTime)
		{
			UpdatePosition(gameTime);
			UpdateLifeCycle(gameTime);
		}

		public void UpdateFrame(GameTime gameTime)
		{
			frameDelta += gameTime.ElapsedGameTime.Milliseconds;
			if (frameDelta > frameTimer)
			{
				frameDelta -= frameTimer;
				SwapFrame();
			}
		}

		public override void Stop()
		{
			None();
		}

		public void SetAttacker(Byte attacker)
		{
			Attacker = attacker;
		}

		public Byte Attacker { get; private set; }
		public BehaveType BehaveType { get; private set; }
	}
}