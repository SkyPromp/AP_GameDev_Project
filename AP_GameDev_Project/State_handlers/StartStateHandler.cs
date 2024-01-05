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
        public bool IsInit { get { return this.is_init; } }
        private StateHandler stateHandler;
        private ContentManager contentManager;

        public StartStateHandler()
        {
            this.contentManager = ContentManager.getInstance;
            this.startButtonRect = new Rectangle(248, 386, 1423, 253);
            this.mapMakeButtonRect = new Rectangle(248, 712, 1423, 253);
            this.mouseHandler = MouseHandler.getInstance.Init();
            this.is_init = false;
            this.stateHandler = StateHandler.getInstance;
        }
        public void Init()
        {
            this.is_init = true;
            this.mouseHandler.LeftClickHook = () => { this.MenuClickHandler(this); };
        }

        public void Update(GameTime gameTime)
        {
            this.mouseHandler.Update();
        }

        private void MenuClickHandler(StartStateHandler startState)
        {
            if (startState.startButtonRect.Contains(startState.mouseHandler.MousePos))
            {
                stateHandler.SetCurrentState(StateHandler.states_enum.RUNNING).Init();
            }
            else if (startState.mapMakeButtonRect.Contains(startState.mouseHandler.MousePos))
            {
                stateHandler.SetCurrentState(StateHandler.states_enum.MAPMAKING).Init();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.contentManager.GetTextures["STARTSCREEN"], new Vector2(0, 0), Color.White);
        }
    }
}
