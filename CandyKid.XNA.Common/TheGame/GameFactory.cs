using WindowsGame.Library.IoC;

namespace WindowsGame.TheGame
{
	/// <summary>
	/// GameFactory is responsible for constructing the GameManager.
	/// </summary>
	public static class GameFactory
	{
		public static IGameManager Resolve()
		{
			return IoCContainer.Resolve<IGameManager>();
		}

		public static void Release()
		{
			IoCContainer.Release();
		}
	}
}