using System;
using System.Collections.Generic;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Devices
{
	public class XboxDeviceFactory : BaseDeviceFactory, IDeviceFactory
	{
		public void Initialize()
		{
			LocalizationDict = new Dictionary<LocalizeType, String>
			{
				{LocalizeType.SelectText1, Constants.SELECT_XBOX},
				{LocalizeType.SelectText2, Constants.SELECT_TEXT}
			};
		}
	}
}