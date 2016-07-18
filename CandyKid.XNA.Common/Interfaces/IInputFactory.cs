using System;
using Microsoft.Xna.Framework;
using WindowsGame.Static;

namespace WindowsGame.Interfaces
{
	public interface IInputFactory
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
}