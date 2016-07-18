using System;
using System.Collections.Generic;
using WindowsGame.Managers;
using WindowsGame.Static;
using NUnit.Framework;

namespace WindowsGame.UnitTests.Managers
{
	[TestFixture]
	public class EventManagerUnitTests : BaseUnitTests
	{
		private IList<EventType> evenTypeList;
		private IList<ValueType> evenTypeArgs;

		[SetUp]
		public new void SetUp()
		{
			// System under test.
			EventManager = new EventManager();
			EventManager.Initialize();
			base.SetUp();
		}

		[Test]
		public void SerializeTypeDataTest()
		{
			evenTypeList = new List<EventType>();
			Assert.That(String.Empty, Is.EqualTo(EventManager.SerializeTypeData(evenTypeList)));

			evenTypeList = new List<EventType>{ EventType.PlayerMove };
			Assert.That("PlayerMove", Is.EqualTo(EventManager.SerializeTypeData(evenTypeList)));

			evenTypeList = new List<EventType> { EventType.EatCandy, EventType.BadOneMove };
			Assert.That("EatCandy|BadOneMove", Is.EqualTo(EventManager.SerializeTypeData(evenTypeList)));
		}

		[Test]
		public void SerializeArgsDataTest()
		{
			evenTypeArgs = new List<ValueType>();
			Assert.That(String.Empty, Is.EqualTo(EventManager.SerializeArgsData(evenTypeArgs)));

			evenTypeArgs = new List<ValueType> { Direction.Left };
			Assert.That("Left", Is.EqualTo(EventManager.SerializeArgsData(evenTypeArgs)));

			evenTypeArgs = new List<ValueType> { 10, Direction.Down };
			Assert.That("10|Down", Is.EqualTo(EventManager.SerializeArgsData(evenTypeArgs)));
		}

		[Test]
		public void DeserializeTypeTextTest()
		{
			evenTypeList = EventManager.DeserializeTypeText(String.Empty);
			Assert.That(0, Is.EqualTo(evenTypeList.Count));

			evenTypeList = EventManager.DeserializeTypeText("PlayerMove");
			Assert.That(EventType.PlayerMove, Is.EqualTo(evenTypeList[0]));

			evenTypeList = EventManager.DeserializeTypeText("EatCandy|BadOneMove");
			Assert.That(2, Is.EqualTo(evenTypeList.Count));
			Assert.That(EventType.EatCandy, Is.EqualTo(evenTypeList[0]));
			Assert.That(EventType.BadOneMove, Is.EqualTo(evenTypeList[1]));
		}

		[Test]
		public void DeserializeArgsTextTest()
		{
			evenTypeArgs = EventManager.DeserializeArgsText(String.Empty);
			Assert.That(0, Is.EqualTo(evenTypeArgs.Count));

			evenTypeArgs = EventManager.DeserializeArgsText("Left");
			Assert.That(Direction.Left, Is.EqualTo(evenTypeArgs[0]));

			evenTypeArgs = EventManager.DeserializeArgsText("10|Down");
			Assert.That(2, Is.EqualTo(evenTypeArgs.Count));
			Assert.That(10, Is.EqualTo(evenTypeArgs[0]));
			Assert.That(Direction.Down, Is.EqualTo(evenTypeArgs[1]));
		}

		[TearDown]
		public void TearDown()
		{
			EventManager = null;
		}

	}
}