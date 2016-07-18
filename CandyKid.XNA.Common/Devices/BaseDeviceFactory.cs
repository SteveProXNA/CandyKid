using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Devices
{
	public class BaseDeviceFactory
	{
		private Vector2[] positions;

		public virtual void LoadContent()
		{
			LoadContent(0);
		}
		protected void LoadContent(Byte delta)
		{
			Byte space = (Byte)(BaseData.TreesSize / 2);
			LoadStripes(delta, space);
		}

		public virtual void DrawGameArrows(Direction direction)
		{
		}
		public void DrawStripes()
		{
			for (Byte index = 0; index < Constants.STRIP_NUMBER; ++index)
			{
				Engine.SpriteBatch.Draw(Assets.NewArrowTexture, positions[index], MyGame.Manager.ImageManager.BlackStripRectangle, Color.White);
			}
		}

		public virtual void SerializeAllEvents()
		{
		}

		public virtual Platform GetPlatform()
		{
			return Platform.Desk;
		}
		public virtual Byte GetNewArrowIndex(Byte theIndex)
		{
			return 1;
		}
		public virtual Boolean GetIsFullScreen(Boolean theIsFullScreen)
		{
			Boolean isFullScreen = theIsFullScreen;
#if !DEBUG
			isFullScreen = !isFullScreen;
#endif
			return isFullScreen;
		}
		public virtual Boolean ChooseIsFullScreen()
		{
			return BaseData.IsFullScreen;
		}
		public virtual Byte GetOptionThree()
		{
			return Convert.ToByte(BaseData.IsFullScreen);
		}
		public virtual void ActionOptionThree(Byte option)
		{
			BaseData.SetIsFullScreen();
			MyGame.Manager.ResolutionManager.ApplyFullScreen(BaseData.IsFullScreen);
		}

		private void LoadStripes(Byte delta, Byte space)
		{
			positions = new Vector2[Constants.STRIP_NUMBER];

			// Left hand side.
			positions[0] = GetLeftVector2(0, 0, delta, space, BaseData.ExitLower); positions[1] = GetLeftVector2(0, 1, delta, space, BaseData.ExitLower);
			positions[2] = GetLeftVector2(1, 0, delta, space, BaseData.ExitLower); positions[3] = GetLeftVector2(1, 1, delta, space, BaseData.ExitLower);
			positions[4] = GetLeftVector2(0, 0, delta, space, BaseData.ExitUpper); positions[5] = GetLeftVector2(0, 1, delta, space, BaseData.ExitUpper);
			positions[6] = GetLeftVector2(1, 0, delta, space, BaseData.ExitUpper); positions[7] = GetLeftVector2(1, 1, delta, space, BaseData.ExitUpper);

			// Right hand side.
			positions[8] = GetRightVector2(0, 0, delta, space, BaseData.ExitLower); positions[9] = GetRightVector2(0, 1, delta, space, BaseData.ExitLower);
			positions[10] = GetRightVector2(1, 0, delta, space, BaseData.ExitLower); positions[11] = GetRightVector2(1, 1, delta, space, BaseData.ExitLower);
			positions[12] = GetRightVector2(0, 0, delta, space, BaseData.ExitUpper); positions[13] = GetRightVector2(0, 1, delta, space, BaseData.ExitUpper);
			positions[14] = GetRightVector2(1, 0, delta, space, BaseData.ExitUpper); positions[15] = GetRightVector2(1, 1, delta, space, BaseData.ExitUpper);
		}
		private static Vector2 GetLeftVector2(Byte x, Byte y, Byte delta, Byte space, Byte level)
		{
			return new Vector2(BaseData.GameOffsetX -(x + 1) * space - delta, level * BaseData.TreesSize + y * space);
		}
		private static Vector2 GetRightVector2(Byte x, Byte y, Byte delta, Byte space, Byte level)
		{
			return new Vector2(BaseData.GameOffsetX + BaseData.BorderGame * BaseData.TreesSize + x * space + delta, level * BaseData.TreesSize + y * space);
		}

		public IDictionary<LocalizeType, String> LocalizationDict { get; protected set; }
	}
}