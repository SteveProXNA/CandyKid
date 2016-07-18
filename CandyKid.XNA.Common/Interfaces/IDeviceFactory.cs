using System;
using System.Collections.Generic;
using WindowsGame.Static;

namespace WindowsGame.Interfaces
{
	public interface IDeviceFactory
	{
		void Initialize();
		void LoadContent();
		void DrawGameArrows(Direction direction);
		void DrawStripes();
		void SerializeAllEvents();
		Platform GetPlatform();
		Byte GetNewArrowIndex(Byte theIndex);
		Boolean GetIsFullScreen(Boolean isFullScreen);
		Boolean ChooseIsFullScreen();
		Byte GetOptionThree();
		void ActionOptionThree(Byte option);

		IDictionary<LocalizeType, String> LocalizationDict { get; }
	}
}