using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AP_GameDev_Project
{
    internal class StartStateHandler: IStateHandler
    {
        private readonly Rectangle startButtonRect;
        private MouseHandler mouseHandler;
        public StartStateHandler() { 
            this.startButtonRect = new Rectangle(0, 0, 1920, 1080);
            this.mouseHandler = MouseHandler.getInstance;
        }

        public void Update(GameTime gameTime)
        {
            mouseHandler.Update();

            if (((mouseHandler.MouseActive & 1) == 1) && this.startButtonRect.Contains(mouseHandler.MousePos)) {
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
