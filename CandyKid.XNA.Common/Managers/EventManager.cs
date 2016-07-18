using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Objects;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IEventManager
	{
		void Initialize();
		void ClearEvents();
		void AddPlayerMoveEvent(Direction direction);
		void AddPlayerCandyEvent(Byte location, UInt16 score);
		void AddPlayerBonusEvent(Byte location, UInt16 bonus);
		void AddEnemyMoveEvent(EnemyType enemyType, Direction direction);
		void ProcessEvents(GameTime gameTime);
		void ProcessEvent(EventType eventType, ValueType eventArgs);

		void SerializeAllEvents();
		String SerializeTypeData(IEnumerable<EventType> theEventTypeData);
		String SerializeArgsData(IEnumerable<ValueType> theEventArgsData);
		IList<EventType> DeserializeTypeText(String theEventTypeText);
		IList<ValueType> DeserializeArgsText(String theEventArgsText);
	}

	public class EventManager : IEventManager
	{
		private IList<Single> eventTimeList;
		private IList<String> eventTypeList;
		private IList<String> eventArgsList;

		private IList<EventType> eventTypeData;
		private IList<ValueType> eventArgsData;

		private Char[] delim;
		private Single delta;

		public void Initialize()
		{
			eventTimeList = new List<Single>();
			eventTypeList = new List<String>();
			eventArgsList = new List<String>();

			eventTypeData = new List<EventType>();
			eventArgsData = new List<ValueType>();

			delim = new[] { '|' };
			delta = 0.0f;
		}

		public void ClearEvents()
		{
			eventTypeData.Clear();
			eventArgsData.Clear();
		}

		public void ProcessEvents(GameTime gameTime)
		{
			delta += (Single)gameTime.ElapsedGameTime.TotalSeconds;
			if (0 == eventTypeData.Count)
			{
				return;
			}

			Byte count = (Byte)(eventTypeData.Count);
			for (Byte index = 0; index < count; ++index)
			{
				EventType eventType = eventTypeData[index];
				ValueType eventArgs = eventArgsData[index];

				ProcessEvent(eventType, eventArgs);
			}

			// Save events for later.
			String eventTypeText = SerializeTypeData(eventTypeData);
			String eventArgsText = SerializeArgsData(eventArgsData);

			eventTimeList.Add(delta);
			eventTypeList.Add(eventTypeText);
			eventArgsList.Add(eventArgsText);

			delta = 0.0f;
		}

		public void ProcessEvent(EventType eventType, ValueType eventArgs)
		{
			switch (eventType)
			{
				case EventType.PlayerMove:
					PlayerMove(eventArgs);
					break;
				case EventType.BadOneMove:
					EnemyMove(EnemyType.Adriana, eventArgs);
					break;
				case EventType.BadTwoMove:
					EnemyMove(EnemyType.Suzanne, eventArgs);
					break;
				case EventType.BadThrMove:
					EnemyMove(EnemyType.StevePro, eventArgs);
					break;
				case EventType.BonusTile:
					ClearBonusTile(eventArgs);
					break;
				case EventType.CandyTile:
					ClearCandyTile(eventArgs);
					break;
				case EventType.EatCandy:
				case EventType.EatBonus:
				case EventType.EatBonus1:
				case EventType.EatBonus2:
				case EventType.EatBonus3:
				case EventType.EatBonus4:
					UpdateScore(eventArgs);
					break;
			}
		}

		public void SerializeAllEvents()
		{
			//UInt16 count = (UInt16)(eventTimeList.Count);
			//for (UInt16 index = 0; index < count; ++index)
			//{
			//    Single time = (Single)Math.Round(eventTimeList[index], 2);
			//    String type = eventTypeList[index];
			//    String args = eventArgsList[index];
			//    String value = String.Format("{0},{1},{2}", time, type, args);
			//    System.Diagnostics.Debug.WriteLine(value);
			//}
			//System.Diagnostics.Debug.WriteLine(String.Empty);
		}

		public String SerializeTypeData(IEnumerable<EventType> theEventTypeData)
		{
#if XBOX
			return String.Empty;
#endif
#if !XBOX
			return String.Join("|", theEventTypeData);
#endif
		}
		public String SerializeArgsData(IEnumerable<ValueType> theEventArgsData)
		{
#if XBOX
			return String.Empty;
#endif
#if !XBOX
			return String.Join("|", theEventArgsData);
#endif
		}
		public IList<EventType> DeserializeTypeText(String theEventTypeText)
		{
			eventTypeData.Clear();

			String[] theList = theEventTypeText.Split(delim);
			for (Byte index = 0; index < theList.Length; ++index)
			{
				String theText = theList[index];
				if (0 == theText.Length)
				{
					continue;
				}

				EventType eventType = (EventType)Enum.Parse(typeof(EventType), theText, true);
				eventTypeData.Add(eventType);
			}

			return eventTypeData;
		}
		public IList<ValueType> DeserializeArgsText(String theEventArgsText)
		{
			eventArgsData.Clear();

			String[] theList = theEventArgsText.Split(delim);
			for (Byte index = 0; index < theList.Length; ++index)
			{
				String theText = theList[index];
				if (0 == theText.Length)
				{
					continue;
				}

				UInt16 result;
				if (UInt16.TryParse(theText, out result))
				{
					eventArgsData.Add(result);
				}
				else
				{
					Direction direction = (Direction)Enum.Parse(typeof(Direction), theText, true);
					eventArgsData.Add(direction);
				}
			}

			return eventArgsData;
		}


		public void AddPlayerMoveEvent(Direction direction)
		{
			AddEvent(EventType.PlayerMove, direction);
		}
		public void AddPlayerCandyEvent(Byte location, UInt16 score)
		{
			AddEvent(EventType.CandyTile, location);
			AddEvent(EventType.EatCandy, score);
		}

		public void AddPlayerBonusEvent(Byte location, UInt16 bonus)
		{
			AddEvent(EventType.BonusTile, location);
			AddEvent(EventType.EatBonus, bonus);
		}

		public void AddEnemyMoveEvent(EnemyType enemyType, Direction direction)
		{
			switch (enemyType)
			{
				case EnemyType.Adriana:
					AddEvent(EventType.BadOneMove, direction);
					break;
				case EnemyType.Suzanne:
					AddEvent(EventType.BadTwoMove, direction);
					break;
				case EnemyType.StevePro:
					AddEvent(EventType.BadThrMove, direction);
					break;
			}
		}

		private void AddEvent(EventType type, ValueType args)
		{
			eventTypeData.Add(type);
			eventArgsData.Add(args);
		}

		private static void UpdateScore(ValueType eventArgs)
		{
			UInt16 score = Convert.ToUInt16(eventArgs);
			MyGame.Manager.ScoreManager.UpdateScore(score);
		}

		private static void PlayerMove(ValueType eventArgs)
		{
			BaseObject entity = MyGame.Manager.EntityManager.Player;
			EntityMove(entity, eventArgs);
		}
		private static void EnemyMove(EnemyType enemyType, ValueType eventArgs)
		{
			BaseObject entity = MyGame.Manager.EntityManager.Enemies[enemyType];
			EntityMove(entity, eventArgs);
		}
		private static void EntityMove(BaseObject entity, ValueType eventArgs)
		{
			Direction direction = (Direction)Enum.Parse(typeof(Direction), eventArgs.ToString(), true);
			MyGame.Manager.EntityManager.Move(entity, direction);
		}
		private static void ClearBonusTile(ValueType eventArgs)
		{
			Byte location = Convert.ToByte(eventArgs);
			MyGame.Manager.BoardManager.ClearBonusTile(location);
		}
		private static void ClearCandyTile(ValueType eventArgs)
		{
			Byte location = Convert.ToByte(eventArgs);
			MyGame.Manager.BoardManager.ClearCandyTile(location);
		}

	}
}