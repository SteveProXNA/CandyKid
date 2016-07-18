using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WindowsGame.Inputs.Types;
using WindowsGame.Interfaces;

namespace WindowsGame.Inputs
{
	public class XboxInputFactory : BaseInputFactory, IInputFactory
	{
		public XboxInputFactory(IJoystickInput joystickInput)
		{
			JoystickInput = joystickInput;
		}

		public void Update(GameTime gameTime)
		{
			JoystickInput.Update(gameTime);
		}

		protected override Boolean HoldUp()
		{
			return JoyHoldUp();
		}

		protected override Boolean HoldDown()
		{
			return JoyHoldDown();
		}

		protected override Boolean HoldLeft()
		{
			return JoyHoldLeft();
		}

		protected override Boolean HoldRight()
		{
			return JoyHoldRight();
		}

		protected override Boolean MoveUp()
		{
			return JoyMoveUp();
		}

		protected override Boolean MoveDown()
		{
			return JoyMoveDown();
		}

		protected override Boolean MoveLeft()
		{
			return JoyMoveLeft();
		}

		protected override Boolean MoveRight()
		{
			return JoyMoveRight();
		}

		public Boolean Escape()
		{
			return JoyHold(Buttons.Back);
		}
		public Boolean Pause()
		{
			return JoyHold(Buttons.Start);
		}
		public Boolean Next()
		{
			return JoyMove(Buttons.A);
		}
		public Boolean Board()
		{
			return Next();
		}
		public Boolean Released()
		{
			return false;
		}
		public Boolean Space()
		{
			return false;
		}

		public Boolean MenuSelect()
		{
			return Next();
		}
		public Byte MenuChoose()
		{
			return 0;
		}

		public Boolean PopupOk()
		{
			return Next();
		}
		public Boolean PopupNo()
		{
			return Escape();
		}

	}
}