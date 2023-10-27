using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AP_GameDev_Project
{
    internal class StartStateHandler//: IStateHandler
    {
        private readonly Rectangle startButtonRect;
        public StartStateHandler() { 
            this.startButtonRect = new Rectangle(0, 0, 1920, 1080);
        }

        public void Update(GameTime gameTime, MouseHandler inputDevice)
        {
            if (((inputDevice.MouseActive & 1) == 1) && this.startButtonRect.Contains(inputDevice.MousePos)) {
                inputDevice.LeftClickHook = null;
                inputDevice.RightClickHook = null;
                Game1.current_state = Game1.states.RUNNING;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
