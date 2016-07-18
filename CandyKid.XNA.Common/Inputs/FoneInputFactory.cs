using System;
using WindowsGame.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using WindowsGame.Inputs.Types;
using WindowsGame.Interfaces;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Inputs
{
	public class FoneInputFactory : BaseInputFactory, IInputFactory
	{
		private UInt16 halfScreenHigh;
 
		public FoneInputFactory(IJoystickInput joystickInput, ITouchScreenInput touchScreenInput)
		{
			JoystickInput = joystickInput;
			TouchScreenInput = touchScreenInput;
		}

		public override void Initialize()
		{
			TouchScreenInput.Initialize();
			base.Initialize();
		}
		public override void Initialize2()
		{
			halfScreenHigh = (UInt16)(BaseData.ScreenHigh / 2.0f);
			base.Initialize2();
		}

		public void Update(GameTime gameTime)
		{
			JoystickInput.Update(gameTime);
			TouchScreenInput.Update(gameTime);
		}

		public override Direction HoldDirection()
		{
#if ANDROID
			Direction holDirection = base.HoldDirection();
			if (Direction.None != holDirection)
			{
				return holDirection;
			}
#endif
			if (TouchLocationState.Pressed != TouchScreenInput.TouchState)
			{
				return Direction.None;
			}

			return MoveDirection();
		}
		public override Direction MoveDirection()
		{
#if ANDROID
			Direction moveDirection = base.MoveDirection();
			if (Direction.None != moveDirection)
			{
				return moveDirection;
			}
#endif
			if (Vector2.Zero == TouchScreenInput.TouchPosition)
			{
				return Direction.None;
			}

			if (LayoutType.Custom != BaseData.GameLayout)
			{
				return GetQuadrantOuter2(TouchScreenInput.TouchPosition.X, TouchScreenInput.TouchPosition.Y);
			}

			Quadrant quadrant = GetQuadrantOuter(TouchScreenInput.TouchPosition.X, TouchScreenInput.TouchPosition.Y);
			if (Quadrant.None == quadrant)
			{
				return Direction.None;
			}

			NewArrow newArrow = MyGame.Manager.NewArrowManager.QuadArrowDictionary[quadrant];
			return newArrow.Direction;
		}
		public override Direction MenuDirection()
		{
			return Direction.None;
		}

		public override Quadrant HoldQuadrant()
		{
			if (TouchLocationState.Pressed != TouchScreenInput.TouchState)
			{
				return Quadrant.None;
			}

			return MoveQuadrant();
		}
		public override Quadrant MoveQuadrant()
		{
			if (Vector2.Zero == TouchScreenInput.TouchPosition)
			{
				return Quadrant.None;
			}

			return GetQuadrantInner(TouchScreenInput.TouchPosition.X, TouchScreenInput.TouchPosition.Y);
		}

		private Quadrant GetQuadrantOuter(Single x, Single y)
		{
			if (x <= ArrowsLeft && y <= halfScreenHigh)
			{
				return Quadrant.TopLeft;
			}
			if (x <= ArrowsLeft && y > halfScreenHigh)
			{
				return Quadrant.BotLeft;
			}
			if (x >= PopupRight && y <= halfScreenHigh)
			{
				return Quadrant.TopRight;
			}
			if (x >= PopupRight && y > halfScreenHigh)
			{
				return Quadrant.BotRight;
			}

			return Quadrant.None;
		}

		private Direction GetQuadrantOuter2(Single x, Single y)
		{
			if (x >= 560 && x <= 680 && y >= 240 && y <= 360)
			{
				return Direction.Left;
			}
			if (x >= 680 && x <= 800 && y >= 240 && y <= 360)
			{
				return Direction.Right;
			}
			if (x >= 560 && x <= 800 && y >= 40 && y <= 240)
			{
				return Direction.Up;
			}
			if (x >= 560 && x <= 800 && y >= 360)
			{
				return Direction.Down;
			}

			return Direction.None;
		}

		private Quadrant GetQuadrantInner(Single x, Single y)
		{
			if (x > ArrowsLeft && x < PopupLeft && y < halfScreenHigh)
			{
				return Quadrant.TopLeft;
			}
			if (x > ArrowsLeft && x < PopupLeft && y > halfScreenHigh)
			{
				return Quadrant.BotLeft;
			}
			if (x > PopupLeft && x < PopupRight && y < halfScreenHigh)
			{
				return Quadrant.TopRight;
			}
			if (x > PopupLeft && x < PopupRight && y > halfScreenHigh)
			{
				return Quadrant.BotRight;
			}

			return Quadrant.None;
		}

		public Boolean Escape()
		{
			return JoystickInput.JoyMove(Buttons.Back);
		}
		public Boolean Pause()
		{
			Boolean pause = JoystickInput.JoyMove(Buttons.Back);
			if (!pause)
			{
				if (Vector2.Zero == TouchScreenInput.TouchPosition)
				{
					return false;
				}
				if (TouchLocationState.Pressed != TouchScreenInput.TouchState)
				{
					return false;
				}

				pause = TouchScreenInput.TouchPosition.X > PauseLeft && TouchScreenInput.TouchPosition.X < PauseRight;
			}

			return pause;
		}

		public Boolean Next()
		{
			return TouchLocationState.Pressed == TouchScreenInput.TouchState;
		}
		public Boolean Board()
		{
			if (Vector2.Zero == TouchScreenInput.TouchPosition)
			{
				return false;
			}
			if (TouchLocationState.Pressed != TouchScreenInput.TouchState)
			{
				return false;
			}

			return TouchScreenInput.TouchPosition.X > ArrowsLeft && TouchScreenInput.TouchPosition.X < PopupRight;
		}
		public override Boolean Sides()
		{
			if (Vector2.Zero == TouchScreenInput.TouchPosition)
			{
				return false;
			}
			if (TouchLocationState.Pressed != TouchScreenInput.TouchState)
			{
				return false;
			}

			return TouchScreenInput.TouchPosition.X <= ArrowsLeft || TouchScreenInput.TouchPosition.X >= ArrowsRight;
		}

		public Boolean Released()
		{
			return TouchLocationState.Released == TouchScreenInput.TouchState;
		}
		public Boolean Space()
		{
			return false;
		}

		public Boolean MenuSelect()
		{
			return false;
		}
		public Byte MenuChoose()
		{
			if (TouchLocationState.Pressed != TouchScreenInput.TouchState)
			{
				return 0;
			}
			if (Vector2.Zero == TouchScreenInput.TouchPosition)
			{
				return 0;
			}
			if (TouchScreenInput.TouchPosition.X < MenuUp || TouchScreenInput.TouchPosition.X > MenuDown)
			{
				return 0;
			}

			Single y = TouchScreenInput.TouchPosition.Y;
			if (y < MenuTop || y > MenuBottom)
			{
				return 0;
			}

			Single a = y - MenuTop;
			Single b = a / Distance;
			return (Byte)(b + Constants.MENUS_TOPEND);
		}

		public Boolean PopupOk()
		{
			if (TouchLocationState.Pressed != TouchScreenInput.TouchState)
			{
				return false;
			}
			if (Vector2.Zero == TouchScreenInput.TouchPosition)
			{
				return false;
			}

			return TouchScreenInput.TouchPosition.X > ArrowsLeft && TouchScreenInput.TouchPosition.X <= PopupLeft;
		}
		public Boolean PopupNo()
		{
			if (TouchLocationState.Pressed != TouchScreenInput.TouchState)
			{
				return false;
			}
			if (Vector2.Zero == TouchScreenInput.TouchPosition)
			{
				return false;
			}

			return TouchScreenInput.TouchPosition.X > PopupLeft && TouchScreenInput.TouchPosition.X <= PopupRight;
		}

		public override void SetMotors(float leftMotor, float rightMotor)
		{
		}
		public override void ResetMotors()
		{
		}

	}
}