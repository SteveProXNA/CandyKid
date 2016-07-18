using System;
using System.IO;
using Microsoft.Xna.Framework;
using WindowsGame.Library.Interfaces;

namespace WindowsGame.Implementation
{
	public class RealFileProxy : IFileProxy
	{
		public Stream GetStream(String path)
		{
			return TitleContainer.OpenStream(path);
		}
	}
}