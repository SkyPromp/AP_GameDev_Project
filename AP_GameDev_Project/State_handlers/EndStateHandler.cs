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
    internal class EndStateHandler: IStateHandler
    {
        private readonly Rectangle homeButtonRect;
        private readonly Rectangle ExitButtonRect;
        private MouseHandler mouseHandler;
        private bool is_init;
        public bool IsInit { get { return this.is_init; } }
        private StateHandler stateHandler;
        private ContentManager contentManager;
        private double click_cooldown;
        private bool won;
        public bool Won {  get { return this.won; } set {
                if (value) { if (difficulty < 255) this.difficulty++; }
                else difficulty = 0;

                this.won = value; 
            } }
        private ushort difficulty;

        public EndStateHandler()
        {
            this.contentManager = ContentManager.getInstance;
            this.homeButtonRect = new Rectangle(248, 386, 1423, 253);
            this.ExitButtonRect = new Rectangle(248, 712, 1423, 253);
            this.won = false;
            this.mouseHandler = MouseHandler.getInstance.Init();
            this.is_init = false;
            this.stateHandler = StateHandler.getInstance;
            difficulty = 0;
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

        private void MenuClickHandler(EndStateHandler gameOverState)
        {
            if (gameOverState.homeButtonRect.Contains(gameOverState.mouseHandler.MousePos) && this.click_cooldown <= 0)
            {
                this.stateHandler.ResetState(StateHandler.states_enum.RUNNING, new RunningStateHandler(this.difficulty));
                this.stateHandler.SetCurrentState(StateHandler.states_enum.START).Init();
            }
            else if (gameOverState.ExitButtonRect.Contains(gameOverState.mouseHandler.MousePos) && this.click_cooldown <= 0)
            {
                this.stateHandler.ExitState = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.contentManager.GetTextures[this.won ? "WINSCREEN" : "GAMEOVERSCREEN"], new Vector2(0, 0), Color.White);
        }
    }
}
