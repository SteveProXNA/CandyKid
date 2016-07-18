using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Interfaces;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Screens
{
	public class CompScreen : BaseScreen, IScreen
	{
		private UInt16 timer;
		private Boolean flash;
		private Byte count;
		private TextData baseTextData, perfTextData;
		private IList<TextData> baseDataList, perfDataList, textDataList;

		public override void Initialize()
		{
			LoadTextData();

			baseDataList = TextDataList;
			baseTextData = baseDataList[1];

			perfTextData = new TextData(baseTextData.Position, Constants.PERFECT_TEXT);
			perfDataList = new List<TextData> { baseDataList[0], perfTextData };
		}

		public override void LoadContent()
		{
			base.LoadContent();

			timer = 0;
			flash = false;
			count = 0;

			MyGame.Manager.SoundManager.StopMusic();
			MyGame.Manager.InputManager.ResetMotors();

			Boolean bonusCount = MyGame.Manager.BoardManager.BonusCount <= 0;
			if (bonusCount)
			{
				MyGame.Manager.SoundManager.PlayMetalSoundEffect();
				UInt32 value = Constants.COMPLETE_ROUND;
				if (0 == BaseData.ScoreRound % 5)
				{
					// Every fifth round there are tons of bonus.
					// So you get double round score; only fair!!
					value *= 2;
				}

				MyGame.Manager.ScoreManager.UpdateScore(value);
				textDataList = perfDataList;
			}
			else
			{
				MyGame.Manager.SoundManager.PlayHappySoundEffect();
				textDataList = baseDataList;
			}

			// Finally increment the level.
			BaseData.IncrementScoreLevel();
		}

		public ScreenType Update(GameTime gameTime)
		{
			UpdateTimer(gameTime);
			if (count < 6)
			{
				timer += (UInt16)(gameTime.ElapsedGameTime.Milliseconds);
				UInt16 delay = flash ? (UInt16)250 : (UInt16)750;
				if (timer > delay)
				{
					timer = 0;
					flash = !flash;
					count++;
				}
			}

			Boolean time = Timer > BaseData.CompDelay;
			Boolean next = MyGame.Manager.InputManager.Next();
			if (time || next)
			{
				UInt16 clockLevel = BaseData.TrialedGame ? BaseData.TrialLevel : BaseData.ClockLevel;
				return BaseData.ScoreLevel > clockLevel ? ScreenType.Beat : ScreenType.Load;
			}

			return ScreenType.Comp;
		}

		public override void Draw()
		{
			if (!flash)
			{
				MyGame.Manager.BorderManager.DrawGame();
			}

			MyGame.Manager.BoardManager.Draw();
			MyGame.Manager.EntityManager.Draw();

			MyGame.Manager.DeviceManager.DrawStripes();
			MyGame.Manager.DeviceManager.DrawGameArrows(Direction.None);
			MyGame.Manager.BorderManager.DrawOver();

			MyGame.Manager.TextManager.DrawPlay();
			MyGame.Manager.TextManager.Draw(textDataList);
			MyGame.Manager.ScoreManager.Draw();
		}

	}
}