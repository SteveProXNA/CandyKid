using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame.Static
{
	public static class Assets
	{
		// Fonts.
		public static SpriteFont EmulogicFont;

		// Textures.
		public static Texture2D NewArrowTexture;
		public static Texture2D TilemapsTexture;
		public static Texture2D SpritemapsTexture;
		public static Texture2D SteveProTexture;

		public static Texture2D HorizontalTexture;
		public static Texture2D VerticalTexture;

		// Banners.
		public static Texture2D AboutTexture;
		public static Texture2D CreditsTexture;
		public static Texture2D HistoryTexture;
		public static Texture2D OptionsTexture;
		public static Texture2D TitleTexture;
		public static Texture2D UnlockTexture;

		// Sound.
		public static IDictionary<SoundEffectType, SoundEffectInstance> SoundEffectDictionary;
		public static Song Song;
		public static Song Winner;
	}
}