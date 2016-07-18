using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WindowsGame.Static;

namespace WindowsGame.Inputs.Types
{
	public interface IJoystickInput
	{
		void Initialize();
		void Update(GameTime gameTime);

		Single Rotate();
		Boolean JoyHold(Buttons button);
		Boolean JoyMove(Buttons button);
		Direction HoldDirection();
		Direction MoveDirection();

		void SetMotors(Single leftMotor, Single rightMotor);
		void ResetMotors();
	}

	public class JoystickInput : IJoystickInput
	{
		private Single tolerance;

		private GamePadState currGamePadState;
		private GamePadState prevGamePadState;

		const float Deadzone = 0.8f;
		const float DiagonalAvoidance = 0.2f;

		public void Initialize()
		{
			tolerance = Constants.JOYSTICK_TOLERANCE;
		}

		public Single Rotate()
		{
			Single rotate = currGamePadState.ThumbSticks.Left.LengthSquared();
			if (Math.Abs(rotate) < 0.00001f)
			{
				return 0;
			}
			return -(Single)Math.Atan2(currGamePadState.ThumbSticks.Left.Y, currGamePadState.ThumbSticks.Left.X);
		}

		public void Update(GameTime gameTime)
		{
			// http://xona.com/2010/05/03.html.
			prevGamePadState = currGamePadState;
			currGamePadState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.IndependentAxes);
		}

		public Boolean JoyHold(Buttons button)
		{
			return currGamePadState.IsButtonDown(button) && prevGamePadState.IsButtonUp(button);

		}
		public Boolean JoyMove(Buttons button)
		{
			return currGamePadState.IsButtonDown(button);
		}

		public Direction HoldDirection()
		{
			if (currGamePadState.ThumbSticks.Left.X < -tolerance && prevGamePadState.ThumbSticks.Left.X > -tolerance)
			{
				return Direction.Left;
			}
			if (currGamePadState.ThumbSticks.Left.X > tolerance && prevGamePadState.ThumbSticks.Left.X < tolerance)
			{
				return Direction.Right;
			}
			if (currGamePadState.ThumbSticks.Left.Y > tolerance && prevGamePadState.ThumbSticks.Left.Y < tolerance)
			{
				return Direction.Up;
			}
			if (currGamePadState.ThumbSticks.Left.Y < -tolerance && prevGamePadState.ThumbSticks.Left.Y > -tolerance)
			{
				return Direction.Down;
			}

			return Direction.None;
		}

		public Direction MoveDirection()
		{
			//http://gamedev.stackexchange.com/questions/22826/how-can-i-map-thumbsticks-to-cardinal-directions
			Vector2 gamepadThumbStick = currGamePadState.ThumbSticks.Left;

			// Get the length and prevent something from happening
			// if it's in our deadzone.
			var length = gamepadThumbStick.Length();
			if (length < Deadzone)
				return Direction.None;

			var absX = Math.Abs(gamepadThumbStick.X);
			var absY = Math.Abs(gamepadThumbStick.Y);
			var absDiff = Math.Abs(absX - absY);

			// We don't like values that are too close to each other
			// i.e. borderline diagonal.
			if (absDiff < length * DiagonalAvoidance)
				return Direction.None;

			if (absX > absY)
			{
				if (gamepadThumbStick.X > 0)
					return Direction.Right;
				else
					return Direction.Left;
			}
			else
			{
				if (gamepadThumbStick.Y < 0)
					return Direction.Down;
				else
					return Direction.Up;
			}
		}

		public void SetMotors(Single leftMotor, Single rightMotor)
		{
			if (!currGamePadState.IsConnected)
			{
				return;
			}

			GamePad.SetVibration(PlayerIndex.One, leftMotor, rightMotor);
		}

		public void ResetMotors()
		{
			SetMotors(0, 0);
		}
	}
}