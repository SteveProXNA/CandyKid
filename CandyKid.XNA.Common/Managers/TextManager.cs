using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using WindowsGame.Data;
using WindowsGame.Library;
using WindowsGame.Objects;
using WindowsGame.Screens;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface ITextManager
	{
		void Initialize();
		void Initialize(String root);
		void InitializeBuild();
		void InitializeBuild(String assemblyName);
		void LoadContent();
		IList<TextData> LoadTextData(String screen);
		IList<TextData> LoadTextData(String screen, Byte textsSize, UInt32 offsetX, Single fontX, Single modY);
		IList<TextData> LoadMenuData(String screen);
		IList<TextData> LoadMenuData(String screen, Byte textsSize, UInt32 offsetX, Single fontX, Single modY);
		Vector2 GetTextPosition(SByte x, SByte y);
		Vector2 GetTextPosition(SByte x, SByte y, Byte textsSize, UInt32 offsetX, Single fontX, Single fontY);
		void Draw(IEnumerable<TextData> textDataList);
		void DrawMenu(IList<TextData> textMenuList, Byte[] options);
		void DrawPlay();
		String BuildVersion { get; }
	}

	public class TextManager : BaseManager, ITextManager
	{
		private static Char[] DELIM;
		private static Char[] PIPES;

		public void Initialize()
		{
			Initialize(String.Empty);
		}
		public void Initialize(String root)
		{
			DELIM = new[] { ',' };
			PIPES = new[] { '|' };

			BaseData.Initialize(root);
		}
		public void InitializeBuild()
		{
#if !WINDOWS_PHONE
			Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
#else
			Assembly assembly = Assembly.GetCallingAssembly();
#endif
			// Get AssemblyVersion from calling AssemblyInfo.cs
			InitializeBuild(assembly.FullName);
		}
		public void InitializeBuild(String assemblyName)
		{
			BuildVersion = assemblyName.Split('=')[1].Split(',')[0];
			if (BuildVersion.EndsWith(".0"))
			{
				BuildVersion = BuildVersion.Substring(0, BuildVersion.Length - 2);
			}
		}

		public void LoadContent()
		{
			var screen = typeof(PlayScreen);
			PlayText = new IEnumerable<TextData>[Constants.MAX_LAYOUT];

			for (Byte index = (Byte)LayoutType.Custom; index <= (Byte)LayoutType.BotRight; ++index)
			{
				String name = String.Format("{0}{1}", screen.Name, index);
				PlayText[index] = LoadTextData(name);
			}
		}
		public IList<TextData> LoadTextData(String screen)
		{
			return LoadTextData(screen, BaseData.TextsSize, BaseData.GameOffsetX, BaseData.FontOffsetX, BaseData.FontOffsetY);
		}
		public IList<TextData> LoadTextData(String screen, Byte textsSize, UInt32 offsetX, Single fontX, Single fontY)
		{
			String file = GetTextFile(screen + ".txt");
			var lines = MyGame.Manager.FileManager.LoadTxt(file);

			var textDataList = new List<TextData>();
			foreach (string line in lines)
			{
				if (line.StartsWith("--"))
				{
					continue;
				}

				String[] items = line.Split(DELIM);
				SByte x = Convert.ToSByte(items[0]);
				SByte y = Convert.ToSByte(items[1]);
				String message = items[2];

				Vector2 postion = GetTextPosition(x, y, textsSize, offsetX, fontX, fontY);
				TextData item = new TextData(postion, message);

				textDataList.Add(item);
			}

			return textDataList;
		}

		public IList<TextData> LoadMenuData(String screen)
		{
			return LoadMenuData(screen, BaseData.TextsSize, BaseData.GameOffsetX, BaseData.FontOffsetX, BaseData.FontOffsetY);
		}
		public IList<TextData> LoadMenuData(String screen, Byte textsSize, UInt32 offsetX, Single fontX, Single fontY)
		{
			String file = GetTextFile(screen + ".txt");
			var lines = MyGame.Manager.FileManager.LoadTxt(file);

			var textDataList = new List<TextData>();
			foreach (string line in lines)
			{
				if (line.StartsWith("--"))
				{
					continue;
				}

				String[] items = line.Split(DELIM);
				SByte x = Convert.ToSByte(items[0]);
				SByte y = Convert.ToSByte(items[1]);
				String message = items[2];
				String[] list = message.Split(PIPES);

				Color color = Color.White;
				if (4 == items.Length && BaseData.TrialedGame)
				{
					String value = items[3];
					if (value.ToLower() == "lightgray")
					{
						color = Color.LightGray;
					}
				}

				Vector2 postion = GetTextPosition(x, y, textsSize, offsetX, fontX, fontY);
				TextData item = new TextData(postion, list, color);

				textDataList.Add(item);
			}

			return textDataList;
		}

		public Vector2 GetTextPosition(SByte x, SByte y)
		{
			return GetTextPosition(x, y, BaseData.TextsSize, BaseData.GameOffsetX, BaseData.FontOffsetX, BaseData.FontOffsetY);
		}
		public Vector2 GetTextPosition(SByte x, SByte y, Byte textsSize, UInt32 offsetX, Single fontX, Single fontY)
		{
			return new Vector2(x * textsSize + offsetX + fontX, y * textsSize + fontY);
		}

		public void Draw(IEnumerable<TextData> textDataList)
		{
			foreach (TextData data in textDataList)
			{
				Engine.SpriteBatch.DrawString(Assets.EmulogicFont, data.Text, data.Position, data.Color);
			}
		}

		public void DrawMenu(IList<TextData> textMenuList, Byte[] options)
		{
			for (Byte index = 0; index < textMenuList.Count; ++index)
			{
				Byte option = options[index];
				TextData data = textMenuList[index];
				IList<String> list = data.List;
				String text = list[option];

				Engine.SpriteBatch.DrawString(Assets.EmulogicFont, text, data.Position, data.Color);
			}
		}
		public void DrawPlay()
		{
			Draw(PlayText[(Byte)BaseData.GameLayout]);
		}

		private IEnumerable<TextData>[] PlayText { get; set; }
		public String BuildVersion { get; private set; }

		private static String GetTextFile(String textFile)
		{
			return String.Format("{0}{1}/{2}/{3}/{4}", BaseData.BaseRoot, Constants.CONTENT_DIRECTORY, Constants.DATA_DIRECTORY, Constants.TEXTS_DIRECTORY, textFile);
		}
	}
}