using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AP_GameDev_Project.State_handlers
{
    internal class StartStateHandler : IStateHandler
    {
        private readonly Rectangle startButtonRect;
        private MouseHandler mouseHandler;
        public StartStateHandler()
        {
            startButtonRect = new Rectangle(0, 0, 1920, 1080);
            mouseHandler = MouseHandler.getInstance;
        }

        public void Update(GameTime gameTime)
        {
            mouseHandler.Update();

            if ((mouseHandler.MouseActive & 1) == 1 && startButtonRect.Contains(mouseHandler.MousePos))
            {
                mouseHandler.LeftClickHook = null;
                mouseHandler.RightClickHook = null;
                Game1.current_state = Game1.states.RUNNING;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
