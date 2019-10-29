using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IScoreManager
	{
		void Initialize();
		void LoadContent();

		void ResetHigh();
		void ResetLevel();
		void ResetLives();
		void ResetScore();
		void InsertLevel(UInt16 value);
		void InsertLevel(Byte world, Byte round);
		void Insert(ScoreType scoreType, UInt32 value);

		void UpdateExtra();
		void UpdateScore(UInt32 value);
		void UpdateLives(Int16 value);
		void SetHighScore(UInt32 value);

		UInt16 ConvertEventToBonus(EventType eventType);
		void Draw();

		IDictionary<ScoreType, ScoreObject> ScoreList { get; }
	}

	public class ScoreManager : IScoreManager
	{
		private UInt32 extraObjValue;

		public IDictionary<ScoreType, ScoreObject> ScoreList { get; private set; }

		public void Initialize()
		{
			ScoreList = new Dictionary<ScoreType, ScoreObject>
			{
				{ScoreType.HighX, new ScoreObject(Constants.DEF_HIGH_SCORE, 30, 5, 30, 4)},
				{ScoreType.Score, new ScoreObject(0, 30, 9, 30, 7)},
				{ScoreType.Lives, new ScoreObject(3, 30, 13, -2, 19)},
				{ScoreType.Level, new ScoreObject(1, 30, 17, -2, 23)},
				{ScoreType.World, new ScoreObject(1, 30, 24, 30, 24)},
				{ScoreType.Round, new ScoreObject(1, 31, 24, 31, 24)}
			};
		}

		public void LoadContent()
		{
			ResetLives();

			InsertLevel(BaseData.ScoreLevel);
			foreach (var key in ScoreList.Keys)
			{
				ScoreObject score = ScoreList[key];
				UpdatePosition(score);
			}
		}

		public void ResetHigh()
		{
			// Must "reset" high score as layout changed??
			ScoreObject score = ScoreList[ScoreType.HighX];
			Insert(ScoreType.HighX, score.Value);
		}
		public void ResetLevel()
		{
			InsertLevel(1);
			InsertLevel(1, 1);
		}
		public void ResetLives()
		{
			InsertLives(BaseData.ScoreLives);
		}
		public void ResetScore()
		{
			Insert(ScoreType.Score, 0);
			extraObjValue = 0;
		}
		public void InsertLives(UInt16 value)
		{
			Insert(ScoreType.Lives, value);
		}
		public void InsertLevel(UInt16 value)
		{
			Insert(ScoreType.Level, value);
		}

		public void InsertLevel(Byte world, Byte round)
		{
			Insert(ScoreType.World, world);
			Insert(ScoreType.Round, round);
		}
		public void Insert(ScoreType scoreType, UInt32 value)
		{
			ScoreObject score = ScoreList[scoreType];
			score.InsertScore(value);
			UpdatePosition(score);
		}

		public void UpdateExtra()
		{
			// Trial game = no extra!
			if (BaseData.TrialedGame)
			{
				return;
			}

			MyGame.Manager.SoundManager.PlayExtraSoundEffect();
			UpdateLives(1);
		}
		public void UpdateScore(UInt32 value)
		{
			Update(ScoreType.Score, (Int32)value);
			var highXObj = ScoreList[ScoreType.HighX];
			var scoreObjValue = ScoreList[ScoreType.Score].Value;

			extraObjValue += value;
			if (extraObjValue >= BaseData.ExtraLives)
			{
				extraObjValue -= BaseData.ExtraLives;
				Byte lives = (Byte)ScoreList[ScoreType.Lives].Value;
				if (lives < Constants.SCORE_LIVES_MAX)
				{
					UpdateExtra();
				}
			}

			if (scoreObjValue < highXObj.Value)
			{
				return;
			}

			highXObj.InsertScore(scoreObjValue);
			UpdatePosition(highXObj);
		}
		public void UpdateLives(Int16 value)
		{
			Update(ScoreType.Lives, value);
		}

		public void SetHighScore(UInt32 value)
		{
			if (null == ScoreList)
			{
				return;
			}
			var highXObj = ScoreList[ScoreType.HighX];
			if (value < highXObj.Value)
			{
				return;
			}

			highXObj.InsertScore(value);
			UpdatePosition(highXObj);
		}
		public void UpdateLevel(UInt16 value)
		{
			Update(ScoreType.Level, (Int16)value);
		}
		public void UpdateLevel(Byte world, Byte round)
		{
			Update(ScoreType.World, world);
			Update(ScoreType.Round, round);
		}

		private void Update(ScoreType scoreType, Int32 value)
		{
			ScoreObject score = ScoreList[scoreType];
			score.UpdateScore(value);
			UpdatePosition(score);
		}

		public UInt16 ConvertEventToBonus(EventType eventType)
		{
			switch (eventType)
			{
				case EventType.EatBonus1:
					return Constants.BONUS_SCORE[0];
				case EventType.EatBonus2:
					return Constants.BONUS_SCORE[1];
				case EventType.EatBonus3:
					return Constants.BONUS_SCORE[2];
				case EventType.EatBonus4:
					return Constants.BONUS_SCORE[3];
				default:
					return 0;
			}
		}

		public void Draw()
		{
			foreach (ScoreObject score in ScoreList.Keys.Select(key => ScoreList[key]))
			{
				score.Draw();
			}
		}

		private static void UpdatePosition(ScoreObject score)
		{
			SByte deltaX = (SByte)(score.X - score.Text.Length + 1);

			Vector2 position = MyGame.Manager.TextManager.GetTextPosition(deltaX, score.Y);
			score.UpdatePosition(position);
		}
	}
}