using System;
using Microsoft.Xna.Framework.Input;
using WindowsGame.Data;
using WindowsGame.Inputs.Types;
using WindowsGame.Static;

namespace WindowsGame.Inputs
{
	public abstract class BaseInputFactory
	{
		protected UInt16 ArrowsLeft, ArrowsRight, PopupLeft, PopupRight, PauseLeft, PauseRight;
		protected UInt16 Distance, MenuUp, MenuDown, MenuTop, MenuBottom;

		protected virtual Boolean HoldUp() { return false; }
		protected virtual Boolean HoldDown() { return false; }
		protected virtual Boolean HoldLeft() { return false; }
		protected virtual Boolean HoldRight() { return false; }

		protected virtual Boolean MoveUp() { return false; }
		protected virtual Boolean MoveDown() { return false; }
		protected virtual Boolean MoveLeft() { return false; }
		protected virtual Boolean MoveRight() { return false; }

		protected IJoystickInput JoystickInput;
		protected IKeyboardInput KeyboardInput;
		protected IMouseScreenInput MouseScreenInput;
		protected ITouchScreenInput TouchScreenInput;

		public virtual void Initialize()
		{
		}
		public virtual void Initialize2()
		{
			JoystickInput.Initialize();

			ArrowsLeft = (UInt16)(2 * BaseData.TilesSize);
			ArrowsRight = (UInt16)(BaseData.ScreenWide - (2 * BaseData.TilesSize));
			PopupLeft = (UInt16)(BaseData.GameOffsetX + Constants.POPUP_COLUMN * BaseData.TilesSize);
			PopupRight = (UInt16)(BaseData.GameOffsetX + (2 * Constants.POPUP_COLUMN) * BaseData.TilesSize);

			UInt16 delta = (UInt16)((PopupRight - PopupLeft) / 2.0f);
			PauseLeft = (UInt16)(PopupLeft - delta);
			PauseRight = (UInt16)(PopupLeft + delta);

			Distance = (Byte)(Constants.MENUS_SPACES * BaseData.TextsSize);
			MenuUp = (UInt16)(BaseData.GameOffsetX + Constants.MENUS_LEFCOL * BaseData.TilesSize);
			MenuDown = (UInt16)(BaseData.GameOffsetX + (Constants.MENUS_RGTCOL + 1) * BaseData.TilesSize);
			MenuTop = (UInt16)(Constants.MENUS_TOPEND * BaseData.TextsSize - BaseData.TextsSize);
			MenuBottom = (UInt16)(MenuTop + (Constants.MENUS_BOTTOM - Constants.MENUS_TOPEND + 1) * Distance);
		}

		public Single Rotate()
		{
			return JoystickInput.Rotate();
		}

		public virtual Boolean Sides()
		{
			return false;
		}
		protected Boolean JoyHold(Buttons button)
		{
			return JoystickInput.JoyHold(button);
		}
		protected Boolean JoyMove(Buttons button)
		{
			return JoystickInput.JoyMove(button);
		}
		protected Boolean JoyEscape()
		{
			return JoystickInput.JoyHold(Buttons.B) || JoystickInput.JoyHold(Buttons.Back);
		}

		public virtual Direction HoldDirection()
		{
			Direction direction = JoystickInput.HoldDirection();
			if (Direction.None != direction)
			{
				return direction;
			}

			if (HoldUp())
			{
				return Direction.Up;
			}
			if (HoldDown())
			{
				return Direction.Down;
			}
			if (HoldLeft())
			{
				return Direction.Left;
			}
			if (HoldRight())
			{
				return Direction.Right;
			}

			return Direction.None;
		}

		public virtual Direction MoveDirection()
		{
			Direction direction = JoystickInput.MoveDirection();
			if (Direction.None != direction)
			{
				return direction;
			}

			if (MoveUp())
			{
				return Direction.Up;
			}
			if (MoveDown())
			{
				return Direction.Down;
			}
			if (MoveLeft())
			{
				return Direction.Left;
			}
			if (MoveRight())
			{
				return Direction.Right;
			}

			return Direction.None;
		}

		public virtual Direction MenuDirection()
		{
			return HoldDirection();
		}

		public virtual Quadrant HoldQuadrant()
		{
			Direction direction = HoldDirection();
			if (Direction.None == direction)
			{
				return Quadrant.None;
			}

			return GetQuadrant(direction);
		}
		public virtual Quadrant MoveQuadrant()
		{
			Direction direction = MoveDirection();
			if (Direction.None == direction)
			{
				return Quadrant.None;
			}

			return GetQuadrant(direction);
		}
		private Quadrant GetQuadrant(Direction direction)
		{
			if (Direction.Up == direction)
			{
				return Quadrant.TopLeft;
			}
			if (Direction.Down == direction)
			{
				return Quadrant.BotLeft;
			}
			if (Direction.Left == direction)
			{
				return Quadrant.TopRight;
			}
			if (Direction.Right == direction)
			{
				return Quadrant.BotRight;
			}

			return Quadrant.None;
		}

		public virtual void SetMotors(Single leftMotor, Single rightMotor)
		{
			JoystickInput.SetMotors(leftMotor, rightMotor);
		}
		public virtual void ResetMotors()
		{
			JoystickInput.ResetMotors();
		}

		protected Boolean JoyMoveUp()
		{
			return JoyMove(Buttons.DPadUp);
		}
		protected Boolean JoyMoveDown()
		{
			return JoyMove(Buttons.DPadDown);
		}
		protected Boolean JoyMoveLeft()
		{
			return JoyMove(Buttons.DPadLeft);
		}
		protected Boolean JoyMoveRight()
		{
			return JoyMove(Buttons.DPadRight);
		}

		protected Boolean JoyHoldUp()
		{
			return JoyHold(Buttons.DPadUp);
		}
		protected Boolean JoyHoldDown()
		{
			return JoyHold(Buttons.DPadDown);
		}
		protected Boolean JoyHoldLeft()
		{
			return JoyHold(Buttons.DPadLeft);
		}
		protected Boolean JoyHoldRight()
		{
			return JoyHold(Buttons.DPadRight);
		}

	}
}