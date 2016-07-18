using System;
using System.IO;

namespace WindowsGame.Library.Interfaces
{
	public interface IFileProxy
	{
		Stream GetStream(String path);
	}
}
