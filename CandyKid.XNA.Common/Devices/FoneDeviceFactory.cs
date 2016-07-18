using System;
using System.Collections.Generic;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Devices
{
	public class FoneDeviceFactory : BaseDeviceFactory, IDeviceFactory
	{
		public void Initialize()
		{
			LocalizationDict = new Dictionary<LocalizeType, String>
			{
				{LocalizeType.SelectText1, Constants.SELECT_FONE},
				{LocalizeType.SelectText2, Constants.SELECT_TEXT}
			};
		}

		public override void LoadContent()
		{
			LoadContent(BaseData.TileRatio);
		}

		public override void DrawGameArrows(Direction direction)
		{
			MyGame.Manager.NewArrowManager.Draw(direction);
		}

		public override Platform GetPlatform()
		{
			return Platform.Port;
		}
		public override Byte GetNewArrowIndex(Byte theIndex)
		{
			Byte index = theIndex;
			if (index > Constants.NEWARROW_INDEX)
			{
				index = Constants.NEWARROW_INDEX;
			}

			return index;
		}
		public override Boolean GetIsFullScreen(Boolean theIsFullScreen)
		{
			return theIsFullScreen;
		}
		public override Boolean ChooseIsFullScreen()
		{
			return Constants.IsFullScreen;
		}
		public override Byte GetOptionThree()
		{
			return BaseData.NewArrowIndex;
		}
		public override void ActionOptionThree(Byte option)
		{
			BaseData.SetNewArrowIndex(option);
			MyGame.Manager.NewArrowManager.SetQuadrants(option);
		}

	}
}