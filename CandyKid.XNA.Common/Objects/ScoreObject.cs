using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using WindowsGame.Library;
using WindowsGame.Static;
using WindowsGame.Data;

namespace WindowsGame.Objects
{
	public class ScoreObject
	{
		public UInt32 Value { get; private set; }
		public String Text { get; private set; }
		public SByte X { get { return xVals[(Byte)BaseData.GameLayout]; } }
		public SByte Y { get { return yVals[(Byte)BaseData.GameLayout]; } }

		private Vector2 Position { get; set; }
		private readonly SByte[] xVals;
		private readonly SByte[] yVals;

		public ScoreObject(UInt32 value, SByte x0, SByte y0, SByte x1, SByte y1)
		{
			Value = value;
			Text = GetText(Value);

			xVals = new SByte[Constants.MAX_LAYOUT];
			xVals[(Byte)LayoutType.Custom] = x0; xVals[(Byte)LayoutType.BotRight] = x1;

			yVals = new SByte[Constants.MAX_LAYOUT];
			yVals[(Byte)LayoutType.Custom] = y0; yVals[(Byte)LayoutType.BotRight] = y1;
		}

		public void UpdatePosition(Vector2 position)
		{
			Position = position;
		}

		public void InsertScore(UInt32 value)
		{
			Value = value;
			Text = GetText(Value);
		}
		public void UpdateScore(Int32 value)
		{
			Value += (UInt32)value;
			if (Value >= Constants.MAX_HIGH_SCORE)
			{
				Value = Constants.MAX_HIGH_SCORE;
			}

			Text = GetText(Value);
		}

		public void Draw()
		{
			Engine.SpriteBatch.DrawString(Assets.EmulogicFont, Text, Position, Color.White);
		}

		private static String GetText(UInt32 value)
		{
			return value.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
		}

	}
}