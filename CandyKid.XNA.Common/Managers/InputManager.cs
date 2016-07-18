using System;
using Microsoft.Xna.Framework;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IInputManager
	{
		void Initialize();
		void Initialize2();
		void Update(GameTime gameTime);

		Single Rotate();
		Boolean Escape();
		Boolean Pause();
		Boolean Next();
		Boolean Board();
		Boolean Sides();
		Boolean Released();
		Boolean Space();

		Boolean MenuSelect();
		Byte MenuChoose();

		Boolean PopupOk();
		Boolean PopupNo();

		Direction HoldDirection();
		Direction MoveDirection();
		Direction MenuDirection();

		Quadrant HoldQuadrant();
		Quadrant MoveQuadrant();

		void SetMotors(Single leftMotor, Single rightMotor);
		void ResetMotors();
	}

	public class InputManager : IInputManager
	{
		private readonly IInputFactory inputFactory;

		public InputManager(IInputFactory inputFactory)
		{
			this.inputFactory = inputFactory;
		}

		public void Initialize()
		{
			inputFactory.Initialize();
		}
		public void Initialize2()
		{
			inputFactory.Initialize2();
		}

		public void Update(GameTime gameTime)
		{
			inputFactory.Update(gameTime);
		}

		public Single Rotate()
		{
			return inputFactory.Rotate();
		}

		public Boolean Escape()
		{
			return inputFactory.Escape();
		}
		public Boolean Pause()
		{
			return inputFactory.Pause();
		}
		public Boolean Next()
		{
			return inputFactory.Next();
		}
		public Boolean Board()
		{
			return inputFactory.Board();
		}
		public Boolean Sides()
		{
			return inputFactory.Sides();
		}
		public Boolean Released()
		{
			return inputFactory.Released();
		}
		public Boolean Space()
		{
			return inputFactory.Space();
		}

		public Boolean MenuSelect()
		{
			return inputFactory.MenuSelect();
		}
		public Byte MenuChoose()
		{
			return inputFactory.MenuChoose();
		}

		public Boolean PopupOk()
		{
			return inputFactory.PopupOk();
		}

		public Boolean PopupNo()
		{
			return inputFactory.PopupNo();
		}

		public Direction HoldDirection()
		{
			return inputFactory.HoldDirection();
		}
		public Direction MoveDirection()
		{
			return inputFactory.MoveDirection();
		}
		public Direction MenuDirection()
		{
			return inputFactory.MenuDirection();
		}

		public Quadrant HoldQuadrant()
		{
			return inputFactory.HoldQuadrant();
		}
		public Quadrant MoveQuadrant()
		{
			return inputFactory.MoveQuadrant();
		}

		public void SetMotors(Single leftMotor, Single rightMotor)
		{
			inputFactory.SetMotors(leftMotor, rightMotor);
		}
		public void ResetMotors()
		{
			inputFactory.ResetMotors();
		}

	}
}