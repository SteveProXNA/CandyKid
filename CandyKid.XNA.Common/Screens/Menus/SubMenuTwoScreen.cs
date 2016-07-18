using System;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Screens.Menus
{
	public class SubMenuTwoScreen : SubMenuAllScreen, IScreen
	{
		public override void Initialize()
		{
			BaseScreenType = ScreenType.SubMenuTwo;
			base.Initialize();
			base.Initialize2();
		}

		protected override ScreenType GetNextScreen(Byte row)
		{
			if (Constants.MENUS_BOTTOM == row)
			{
				return MenuScreenType;
			}

			Byte index = (Byte)(row - Constants.MENUS_TOPEND);
			Options[index]++;
			if (Options[index] >= TextMenuList[index].List.Count)
			{
				Options[index] = 0;
			}

			if (Constants.MENUS_TOPEND + 3 == row)
			{
				BaseData.SetUsePlaySound();
			}
			MyGame.Manager.SoundManager.PlayBonusSoundEffect();
			if (Constants.MENUS_TOPEND == row)
			{
				BaseData.SetUseKillTrees();
			}
			if (Constants.MENUS_TOPEND + 1 == row)
			{
				BaseData.SetUseOpenExits();
			}
			if (Constants.MENUS_TOPEND + 2 == row)
			{
				BaseData.SetUsePlayMusic();
			}
			

			return BaseScreenType;
		}

		protected override byte[] GetOptions()
		{
			var theOptions = new Byte[Constants.MENUS_NUMBER];
			theOptions[0] = Convert.ToByte(BaseData.UseKillTrees);
			theOptions[1] = Convert.ToByte(BaseData.UseOpenExits);
			theOptions[2] = Convert.ToByte(BaseData.UsePlayMusic);
			theOptions[3] = Convert.ToByte(BaseData.UsePlaySound);
			return theOptions;
		}

	}
}