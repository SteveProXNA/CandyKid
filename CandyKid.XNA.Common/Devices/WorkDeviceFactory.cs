using System;
using System.Collections.Generic;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Devices
{
	public class WorkDeviceFactory : BaseDeviceFactory, IDeviceFactory
	{
		public void Initialize()
		{
			LocalizationDict = new Dictionary<LocalizeType, String>
			{
				{LocalizeType.SelectText1, Constants.SELECT_WORK},
				{LocalizeType.SelectText2, Constants.SELECT_TEXT}
			};
		}

		public override void SerializeAllEvents()
		{
			MyGame.Manager.EventManager.SerializeAllEvents();
		}
	}
}