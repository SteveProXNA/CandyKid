using System;
using System.Collections.Generic;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IDeviceManager
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

	public class DeviceManager : IDeviceManager
	{
		private readonly IDeviceFactory deviceFactory;

		public DeviceManager(IDeviceFactory deviceFactory)
		{
			this.deviceFactory = deviceFactory;
		}

		public void Initialize()
		{
			deviceFactory.Initialize();
		}
		public void LoadContent()
		{
			deviceFactory.LoadContent();
		}
		public void DrawGameArrows(Direction direction)
		{
			deviceFactory.DrawGameArrows(direction);
		}

		public void DrawStripes()
		{
			deviceFactory.DrawStripes();
		}

		public void SerializeAllEvents()
		{
			deviceFactory.SerializeAllEvents();
		}

		public Platform GetPlatform()
		{
			return deviceFactory.GetPlatform();
		}
		public Byte GetNewArrowIndex(Byte theIndex)
		{
			return deviceFactory.GetNewArrowIndex(theIndex);
		}
		public Boolean GetIsFullScreen(Boolean theIsFullScreen)
		{
			return deviceFactory.GetIsFullScreen(theIsFullScreen);
		}
		public Boolean ChooseIsFullScreen()
		{
			return deviceFactory.ChooseIsFullScreen();
		}
		public Byte GetOptionThree()
		{
			return deviceFactory.GetOptionThree();
		}
		public void ActionOptionThree(Byte option)
		{
			deviceFactory.ActionOptionThree(option);
		}

		public IDictionary<LocalizeType, String> LocalizationDict
		{
			get
			{
				return deviceFactory.LocalizationDict;
			}
		}
	}

}