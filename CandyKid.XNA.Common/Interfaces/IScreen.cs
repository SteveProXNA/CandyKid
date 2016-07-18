using Microsoft.Xna.Framework;
using WindowsGame.Static;

namespace WindowsGame.Interfaces
{
	public interface IScreen
	{
		void Initialize();
		void LoadContent();
		ScreenType Update(GameTime gameTime);
		void Draw();
	}
}