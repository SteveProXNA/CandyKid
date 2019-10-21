using System;
using System.Collections.Generic;
using System.Globalization;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface ICommandManager
	{
		void Initialize();
		void Initialize(String root);
		void LoadContent();
		void LoadCommandData(Byte commandId);
		void ParseCommandData(IList<String> lines, Byte commandId);

		IDictionary<Byte, IList<Single>> CommandTimeList { get; }
		IDictionary<Byte, IList<String>> CommandTypeList { get; }
		IDictionary<Byte, IList<String>> CommandArgsList { get; }
	}

	public class CommandManager : ICommandManager
	{
		private String commandRoot;
		private Char[] delim;

		public void Initialize()
		{
			Initialize(String.Empty);
		}
		public void Initialize(String root)
		{
			commandRoot = root;
			delim = new[] { ',' };

			CommandTimeList = new Dictionary<Byte, IList<Single>>();
			CommandTypeList = new Dictionary<Byte, IList<String>>();
			CommandArgsList = new Dictionary<Byte, IList<String>>();
		}

		public void LoadContent()
		{
			LoadCommandData(0);
			LoadCommandData(1);
		}

		public void LoadCommandData(Byte commandId)
		{
			if (CommandTimeList.ContainsKey(commandId))
			{
				return;
			}

			String file = GetCommandFile(commandId + ".txt");
			var lines = MyGame.Manager.FileManager.LoadTxt(file);
			ParseCommandData(lines, commandId);
		}

		public void ParseCommandData(IList<String> lines, Byte commandId)
		{
			if (CommandTimeList.ContainsKey(commandId))
			{
				return;
			}

			IList<Single> eventTimeList = new List<Single>();
			IList<String> eventTypeList = new List<String>();
			IList<String> eventArgsList = new List<String>();

			UInt16 count = (UInt16)(lines.Count);
			for (UInt16 index = 0; index < count; ++index)
			{
				String line = lines[index];
				String[] items = line.Split(delim);

				eventTimeList.Add(Convert.ToSingle(items[0], CultureInfo.InvariantCulture));
				eventTypeList.Add(items[1]);
				eventArgsList.Add(items[2]);
			}

			CommandTimeList.Add(commandId, eventTimeList);
			CommandTypeList.Add(commandId, eventTypeList);
			CommandArgsList.Add(commandId, eventArgsList);
		}

		public IDictionary<Byte, IList<Single>> CommandTimeList { get; private set; }
		public IDictionary<Byte, IList<String>> CommandTypeList { get; private set; }
		public IDictionary<Byte, IList<String>> CommandArgsList { get; private set; }

		private String GetCommandFile(String commandFile)
		{
			return String.Format("{0}{1}/{2}/{3}/{4}", commandRoot, Constants.CONTENT_DIRECTORY, Constants.DATA_DIRECTORY, Constants.COMMAND_DIRECTORY, commandFile);
		}
	}

}