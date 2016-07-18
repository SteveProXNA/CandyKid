using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WindowsGame.Inputs.Types;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Inputs
{
	public class WorkInputFactory : BaseInputFactory, IInputFactory
	{
		public WorkInputFactory(IJoystickInput joystickInput, IKeyboardInput keyboardInput, IMouseScreenInput mouseScreenInput)
		{
			JoystickInput = joystickInput;
			KeyboardInput = keyboardInput;
			MouseScreenInput = mouseScreenInput;
		}

		public void Update(GameTime gameTime)
		{
			JoystickInput.Update(gameTime);
			KeyboardInput.Update(gameTime);
			MouseScreenInput.Update(gameTime);
		}

		protected override Boolean HoldUp()
		{
			return KeyboardInput.KeyHold(Keys.Up) || JoyHoldUp();
		}

		protected override Boolean HoldDown()
		{
			return KeyboardInput.KeyHold(Keys.Down) || JoyHoldDown();
		}

		protected override Boolean HoldLeft()
		{
			return KeyboardInput.KeyHold(Keys.Left) || JoyHoldLeft();
		}

		protected override Boolean HoldRight()
		{
			return KeyboardInput.KeyHold(Keys.Right) || JoyHoldRight();
		}

		protected override Boolean MoveUp()
		{
			return KeyboardInput.KeyPress(Keys.Up) || JoyMoveUp();
		}

		protected override Boolean MoveDown()
		{
			return KeyboardInput.KeyPress(Keys.Down) || JoyMoveDown();
		}

		protected override Boolean MoveLeft()
		{
			return KeyboardInput.KeyPress(Keys.Left) || JoyMoveLeft();
		}

		protected override Boolean MoveRight()
		{
			return KeyboardInput.KeyPress(Keys.Right) || JoyMoveRight();
		}

		public Boolean Escape()
		{
			return KeyboardInput.KeyHold(Keys.Escape) || JoyEscape();
		}
		public Boolean Pause()
		{
			return KeyboardInput.KeyHold(Keys.Escape) || JoyHold(Buttons.Start) || MouseDetect(PauseLeft, PauseRight);
		}
		public Boolean Next()
		{
			return KeyboardInput.KeyHold(Keys.Enter) || JoyHold(Buttons.A) || MouseScreenInput.ButtonHold();
		}
		public Boolean Board()
		{
			return KeyboardInput.KeyHold(Keys.Enter) || JoyHold(Buttons.A) || MouseDetect(ArrowsLeft, PopupRight);
		}
		public Boolean Released()
		{
			return false;
		}
		public Boolean Space()
		{
			return KeyboardInput.KeyPress(Keys.Space);
		}

		public Boolean MenuSelect()
		{
			return KeyboardInput.KeyHold(Keys.Enter) || JoyHold(Buttons.A);
		}

		public Byte MenuChoose()
		{
			Byte keyboard = MenuChooseKeyboard();
			if (0 != keyboard)
			{
				return keyboard;
			}
			if (!MouseScreenInput.ButtonHold())
			{
				return 0;
			}
			if (0 == MouseScreenInput.CurrMouseX && 0 == MouseScreenInput.CurrMouseY)
			{
				return 0;
			}

			var mousePosition = new Vector2(MouseScreenInput.CurrMouseX, MouseScreenInput.CurrMouseY);
			Vector2 checkPosition = MyGame.Manager.ResolutionManager.VeiwPortVector2;
			Vector2 deltaPosition = mousePosition - checkPosition;
			Matrix invertMatrix = MyGame.Manager.ResolutionManager.InvertTransformationMatrix;
			deltaPosition = Vector2.Transform(deltaPosition, invertMatrix);

			if (deltaPosition.X < MenuUp || deltaPosition.Y > MenuDown)
			{
				return 0;
			}

			Single y = deltaPosition.Y;
			if (y < MenuTop || y > MenuBottom)
			{
				return 0;
			}

			Single a = y - MenuTop;
			Single b = a / Distance;

			return (Byte)(b + Constants.MENUS_TOPEND);
		}

		private Byte MenuChooseKeyboard()
		{
			if (KeyboardInput.KeyHold(Keys.D1) || KeyboardInput.KeyHold(Keys.NumPad1))
			{
				return Constants.MENUS_TOPEND + 0;
			}
			if (KeyboardInput.KeyHold(Keys.D2) || KeyboardInput.KeyHold(Keys.NumPad2))
			{
				return Constants.MENUS_TOPEND + 1;
			}
			if (KeyboardInput.KeyHold(Keys.D3) || KeyboardInput.KeyHold(Keys.NumPad3))
			{
				return Constants.MENUS_TOPEND + 2;
			}
			if (KeyboardInput.KeyHold(Keys.D4) || KeyboardInput.KeyHold(Keys.NumPad4))
			{
				return Constants.MENUS_TOPEND + 3;
			}
			if (KeyboardInput.KeyHold(Keys.D5) || KeyboardInput.KeyHold(Keys.NumPad5))
			{
				return Constants.MENUS_TOPEND + 4;
			}
			return 0;
		}
		public Boolean PopupOk()
		{
			return KeyboardInput.KeyHold(Keys.Enter) || JoyHold(Buttons.A) || MouseDetect(ArrowsLeft, PopupLeft);
		}
		public Boolean PopupNo()
		{
			return KeyboardInput.KeyHold(Keys.Escape) || JoyEscape() || MouseDetect(PopupLeft, PopupRight);
		}

		private Boolean MouseDetect(UInt16 lhs, UInt16 rhs)
		{
			if (!MouseScreenInput.ButtonHold())
			{
				return false;
			}

			return MouseScreenInput.CurrMouseX > lhs && MouseScreenInput.CurrMouseX < rhs;
		}

	}
}