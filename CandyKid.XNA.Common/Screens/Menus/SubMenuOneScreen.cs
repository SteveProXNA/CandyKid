using System;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Screens.Menus
{
	public class SubMenuOneScreen : SubMenuAllScreen, IScreen
	{
		public override void Initialize()
		{
			BaseScreenType = ScreenType.SubMenuOne;
			base.Initialize();
			base.Initialize2();
		}

		protected override ScreenType GetNextScreen(Byte row)
		{
			if (Constants.MENUS_BOTTOM == row)
			{
				return MenuScreenType;
			}

			MyGame.Manager.SoundManager.PlayBonusSoundEffect();
			if (Constants.MENUS_TOPEND == row)
			{
				return ScreenType.Instruct;
			}
			if (Constants.MENUS_TOPEND + 1 == row)
			{
				BaseData.PrevScreen = ScreenType.SubMenuOne;
				return ScreenType.Demo;
			}
			if (Constants.MENUS_TOPEND + 2 == row)
			{
				return ScreenType.History;
			}
			if (Constants.MENUS_TOPEND + 3 == row)
			{
				return ScreenType.Credits;
			}

			return BaseScreenType;
		}

		protected override byte[] GetOptions()
		{
			return new Byte[Constants.MENUS_NUMBER];
		}

	}
}