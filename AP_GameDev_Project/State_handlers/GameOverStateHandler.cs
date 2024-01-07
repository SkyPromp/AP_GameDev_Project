using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.State_handlers
{
    internal class GameOverStateHandler: IStateHandler
    {
        private readonly Rectangle homeButtonRect;
        private readonly Rectangle ExitButtonRect;
        private MouseHandler mouseHandler;
        private bool is_init;
        public bool IsInit { get { return this.is_init; } }
        private StateHandler stateHandler;
        private ContentManager contentManager;
        private double click_cooldown;

        public GameOverStateHandler()
        {
            this.contentManager = ContentManager.getInstance;
            this.homeButtonRect = new Rectangle(248, 386, 1423, 253);
            this.ExitButtonRect = new Rectangle(248, 712, 1423, 253);
            this.mouseHandler = MouseHandler.getInstance.Init();
            this.is_init = false;
            this.stateHandler = StateHandler.getInstance;
        }
        public void Init()
        {
            this.is_init = true;
            this.mouseHandler.LeftClickHook = () => { this.MenuClickHandler(this); };
            this.click_cooldown = 0.4;
        }

        public void Update(GameTime gameTime)
        {
            if (this.click_cooldown > 0) this.click_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            this.mouseHandler.Update();
        }

        private void MenuClickHandler(GameOverStateHandler gameOverState)
        {
            if (gameOverState.homeButtonRect.Contains(gameOverState.mouseHandler.MousePos) && this.click_cooldown <= 0)
            {
                this.stateHandler.ResetState(StateHandler.states_enum.RUNNING, new RunningStateHandler());
                this.stateHandler.SetCurrentState(StateHandler.states_enum.START).Init();
            }
            else if (gameOverState.ExitButtonRect.Contains(gameOverState.mouseHandler.MousePos) && this.click_cooldown <= 0)
            {
                this.stateHandler.ExitState = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.contentManager.GetTextures["GAMEOVERSCREEN"], new Vector2(0, 0), Color.White);
        }
    }
}
