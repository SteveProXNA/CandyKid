using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using WindowsGame.Library;
using WindowsGame.Static;

namespace WindowsGame.Managers
{
	public interface IContentManager
	{
		void Initialize();
		void LoadContent();
		void LoadContentSplash();
	}

	public class ContentManager : BaseManager, IContentManager
	{
		private String contentRoot, texturesRoot, bannersRoot, soundRoot;

		public void Initialize()
		{
			contentRoot = GetGlobalBaseContentRoot();
			texturesRoot = String.Format("{0}/{1}/", contentRoot, Constants.TEXTURES_DIRECTORY);
			bannersRoot = String.Format("{0}{1}/", texturesRoot, Constants.BANNERS_DIRECTORY);
			soundRoot = String.Format("{0}/{1}/", contentRoot, Constants.SOUND_DIRECTORY);
		}

		public void LoadContentSplash()
		{
			Assets.SteveProTexture = LoadTexture("StevePro");
		}

		public void LoadContent()
		{
			// Fonts.
			String fonts = String.Format("{0}/{1}/", contentRoot, Constants.FONTS_DIRECTORY);
			Assets.EmulogicFont = Engine.Content.Load<SpriteFont>(fonts + "Emulogic");

			// Textures.
			Assets.NewArrowTexture = LoadTexture("NewArrow");
			Assets.TilemapsTexture = LoadTexture("Tilemaps");
			Assets.SpritemapsTexture = LoadTexture("Spritemaps");
			Assets.SteveProTexture = LoadTexture("StevePro");

			// Banners.
			Assets.AboutTexture = LoadBanners("About");
			Assets.CreditsTexture = LoadBanners("Credits");
			Assets.HistoryTexture = LoadBanners("History");
			Assets.OptionsTexture = LoadBanners("Options");
			Assets.TitleTexture = LoadBanners("Title");
			Assets.UnlockTexture = LoadBanners("Unlock");

			// Songs.
			Assets.Song = Engine.Content.Load<Song>(soundRoot + "MrP7");
			Assets.Winner = Engine.Content.Load<Song>(soundRoot + "CKwinner");

			// Sound effects.
			Assets.SoundEffectDictionary = new Dictionary<SoundEffectType, SoundEffectInstance>();
			for (SoundEffectType key = SoundEffectType.Celebrate; key <= SoundEffectType.TrialBuzz; ++key)
			{
				SoundEffectInstance value = LoadSoundEffectInstance(key.ToString());
				Assets.SoundEffectDictionary.Add(key, value);
			}
		}

		private SoundEffectInstance LoadSoundEffectInstance(String assetName)
		{
			SoundEffect effect = ContentLoad<SoundEffect>(soundRoot + assetName);
			return effect.CreateInstance();
		}
		private Texture2D LoadTexture(String assetName)
		{
			return ContentLoad<Texture2D>(texturesRoot + assetName);
		}
		private Texture2D LoadBanners(String assetName)
		{
			return ContentLoad<Texture2D>(bannersRoot + assetName);
		}
		private static T ContentLoad<T>(String assetName)
		{
			return Engine.Content.Load<T>(assetName);
		}
	}
}