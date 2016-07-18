using System;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Static;

namespace WindowsGame.Screens.Menus
{
	public class SubMenuXyzScreen : SubMenuAllScreen, IScreen
	{
		public override void Initialize()
		{
			BaseScreenType = ScreenType.SubMenuXyz;
			base.Initialize();
			base.Initialize2();
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
				if (Constants.MENUS_TOPEND == row || Constants.MENUS_TOPEND + 3 == row)
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
				BaseData.SetIsInGodMode();
			}
			if (Constants.MENUS_TOPEND + 1 == row)
			{
				BaseData.SetResetEnemies();
			}
			if (Constants.MENUS_TOPEND + 2 == row)
			{
				BaseData.SetEnemyVelIndex(Options[2]);
			}
			if (Constants.MENUS_TOPEND + 3 == row)
			{
				BaseData.SetMoveAdriana(false);
				BaseData.SetMoveSuzanne(false);
				BaseData.SetMoveStevePro(false);

				Byte enemy = Options[3];
				if (0 == enemy || 1 == enemy || 2 == enemy || 4 == enemy)
				{
					BaseData.SetMoveAdriana(true);
				}
				if (0 == enemy || 1 == enemy || 3 == enemy || 5 == enemy)
				{
					BaseData.SetMoveSuzanne(true);
				}
				if (0 == enemy || 2 == enemy || 3 == enemy || 6 == enemy)
				{
					BaseData.SetMoveStevePro(true);
				}

				MyGame.Manager.EntityManager.UpdateMoveEnemies();
			}

			return BaseScreenType;
		}

		protected override byte[] GetOptions()
		{
			var theOptions = new Byte[Constants.MENUS_NUMBER];
			theOptions[0] = Convert.ToByte(BaseData.IsInGodMode);
			theOptions[1] = Convert.ToByte(BaseData.ResetEnemies);
			theOptions[2] = Convert.ToByte(BaseData.EnemyVelIndex);
			theOptions[3] = GetEnemies(BaseData.MoveAdriana, BaseData.MoveSuzanne, BaseData.MoveStevePro);
			return theOptions;
		}

		private static Byte GetEnemies(Boolean adriana, Boolean suzanne, Boolean stevepro)
		{
			if (adriana && suzanne && stevepro)
			{
				return 0;
			}
			if (adriana && suzanne)
			{
				return 1;
			}
			if (adriana && stevepro)
			{
				return 2;
			}
			if (!adriana && suzanne && stevepro)
			{
				return 3;
			}

			if (adriana)
			{
				return 4;
			}
			if (suzanne)
			{
				return 5;
			}
			if (stevepro)
			{
				return 6;
			}

			return 7;
		}

	}
}