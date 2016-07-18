using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame.Interfaces;
using WindowsGame.Library;
using WindowsGame.Static;
using WindowsGame.Screens;
using WindowsGame.Screens.Menus;

namespace WindowsGame.Managers
{
	public interface IScreenManager
	{
		void Initialize();
		void LoadContent();
		void Update(GameTime gameTime);
		void Draw();
	}

	public class ScreenManager : IScreenManager
	{
		private IDictionary<ScreenType, IScreen> screens;
		private ScreenType currScreen = ScreenType.Splash;
		private ScreenType nextScreen = ScreenType.Splash;

		public void Initialize()
		{
			screens = GetScreens();
			screens[ScreenType.Splash].Initialize();
			screens[ScreenType.Init].Initialize();
		}

		public void LoadContent()
		{
			foreach (var key in screens.Keys)
			{
				if (ScreenType.Splash == key || ScreenType.Init == key)
				{
					continue;
				}

				screens[key].Initialize();
			}
		}

		public void Update(GameTime gameTime)
		{
			if (currScreen != nextScreen)
			{
				currScreen = nextScreen;
				screens[currScreen].LoadContent();
			}

			nextScreen = screens[currScreen].Update(gameTime);
		}

		public void Draw()
		{
			MyGame.Manager.ResolutionManager.BeginDraw();
			Engine.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, MyGame.Manager.ResolutionManager.TransformationMatrix);
			screens[currScreen].Draw();
			Engine.SpriteBatch.End();
		}

		private static Dictionary<ScreenType, IScreen> GetScreens()
		{
			return new Dictionary<ScreenType, IScreen>
			{
				{ScreenType.Splash, new SplashScreen()},
				{ScreenType.Init, new InitScreen()},
				{ScreenType.Title, new TitleScreen()},
				{ScreenType.Demo, new DemoScreen()},
				{ScreenType.Load, new LoadScreen()},
				{ScreenType.Options, new OptionsScreen()},
				{ScreenType.Instruct, new InstructScreen()},
				{ScreenType.Select, new SelectScreen()},
				{ScreenType.Play, new PlayScreen()},
				{ScreenType.Stop, new StopScreen()},
				{ScreenType.StopX, new StopXScreen()},
				{ScreenType.Comp, new CompScreen()},
				{ScreenType.Beat, new BeatScreen()},
				{ScreenType.Dead, new DeadScreen()},
				{ScreenType.DeadX, new DeadXScreen()},
				{ScreenType.Cont, new ContScreen()},
				{ScreenType.Over, new OverScreen()},
				{ScreenType.Exit, new ExitScreen()},
				{ScreenType.Menu, new MenuScreen()},
				{ScreenType.SubMenuOne, new SubMenuOneScreen()},
				{ScreenType.SubMenuTwo, new SubMenuTwoScreen()},
				{ScreenType.SubMenuWee, new SubMenuWeeScreen()},
				{ScreenType.SubMenuXyz, new SubMenuXyzScreen()},
				{ScreenType.Credits, new CreditsScreen()},
				{ScreenType.History, new HistoryScreen()},
				{ScreenType.Unlock, new UnlockScreen()},
			};
		}

		/* Same method using Reflection instead (hardcode namespace).
		private static Dictionary<ScreenType, IScreen> GetScreensX()
		{
			var screens = new Dictionary<ScreenType, IScreen>();
			for (ScreenType key = ScreenType.Splash; key <= ScreenType.Exit; ++key)
			{
				String typeName = String.Format("{0}.{1}Screen", "WindowsGame.Screens", key);
				Type type = Type.GetType(typeName);

				if (null == type)
				{
					typeName = String.Format("{0}.{1}Screen", "WindowsGame.Screens.Menus", key);
					type = Type.GetType(typeName);
				}

				if (null == type)
				{
					continue;
				}

				IScreen value = (IScreen)Activator.CreateInstance(type);
				screens.Add(key, value);
			}

			return screens;
		}
		*/

	}
}