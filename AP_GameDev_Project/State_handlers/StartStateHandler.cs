using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AP_GameDev_Project.State_handlers
{
    internal class StartStateHandler : IStateHandler
    {
        private readonly Rectangle startButtonRect;
        private readonly Rectangle mapMakeButtonRect;
        private MouseHandler mouseHandler;
        private bool is_init;
        public bool IsInit {  get;}

        public StartStateHandler()
        {
            this.startButtonRect = new Rectangle(0, 0, 1920, 540);
            this.mapMakeButtonRect = new Rectangle(0, 540, 1920, 540);
            this.mouseHandler = MouseHandler.getInstance;
            this.is_init = false;
        }
        public void Init()
        {
            this.is_init = true;
        }

        public void Update(GameTime gameTime)
        {
            mouseHandler.Update();

            if ((mouseHandler.MouseActive & 1) == 1)
            {
                if (startButtonRect.Contains(mouseHandler.MousePos))
                {
                    mouseHandler.LeftClickHook = null;
                    mouseHandler.RightClickHook = null;
                    Game1.current_state = Game1.states.RUNNING;
                } else if (mapMakeButtonRect.Contains(mouseHandler.MousePos))
                {
                    mouseHandler.LeftClickHook = null;
                    mouseHandler.RightClickHook = null;
                    Game1.current_state = Game1.states.MAPMAKING;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
