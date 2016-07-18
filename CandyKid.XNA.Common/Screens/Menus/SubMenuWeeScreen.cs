using System;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Screens.Menus
{
	public class SubMenuWeeScreen : SubMenuAllScreen, IScreen
	{
		public override void Initialize()
		{
			String screen = String.Format("{0}{1}", GetType().Name, (Byte)BaseData.Platform);
			LoadMenuData(screen);

			BaseScreenType = ScreenType.SubMenuWee;
			base.Initialize();
		}

		protected override ScreenType GetNextScreen(Byte row)
		{
			if (Constants.MENUS_BOTTOM == row)
			{
				return MenuScreenType;
			}

			// Options not configured.
			if (BaseData.TrialedGame)
			{
				if (Constants.MENUS_TOPEND == row || Constants.MENUS_TOPEND + 1 == row)
				{
					MyGame.Manager.SoundManager.PlayTrialBuzzdEffect();
					return BaseScreenType;
				}
			}

			Byte index = (Byte)(row - Constants.MENUS_TOPEND);
			Options[index]++;
			if (Options[index] >= TextMenuList[index].List.Count)
			{
				Options[index] = 0;
			}

			MyGame.Manager.SoundManager.PlayBonusSoundEffect();
			if (Constants.MENUS_TOPEND == row)
			{
				BaseData.SetCanContinue();
			}
			if (Constants.MENUS_TOPEND + 1 == row)
			{
				Byte lives = 3;
				Byte value = Options[1];
				if (0 == value)
				{
					lives = 3;
				}
				if (1 == value)
				{
					lives = 5;
				}
				if (2 == value)
				{
					lives = 10;
				}
				if (3 == value)
				{
					lives = 25;
				}
				BaseData.SetScoreLives(lives);
			}
			if (Constants.MENUS_TOPEND + 2 == row)
			{
				BaseData.SetGamerVelIndex(Options[2]);
			}
			if (Constants.MENUS_TOPEND + 3 == row)
			{
				Byte option = Options[3];
				MyGame.Manager.DeviceManager.ActionOptionThree(option);
			}

			return BaseScreenType;
		}

		protected override byte[] GetOptions()
		{
			var theOptions = new Byte[Constants.MENUS_NUMBER];
			theOptions[0] = Convert.ToByte(BaseData.CanContinue);
			theOptions[1] = GetNumLives(BaseData.ScoreLives);
			theOptions[2] = BaseData.GamerVelIndex;
			theOptions[3] = MyGame.Manager.DeviceManager.GetOptionThree();
			return theOptions;
		}

		private static Byte GetNumLives(Byte lives)
		{
			if (lives >= 25)
			{
				return 3;
			}
			if (lives >= 10)
			{
				return 2;
			}
			if (lives >= 5)
			{
				return 1;
			}

			return 0;
		}
	}
}