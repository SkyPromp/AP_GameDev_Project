using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AP_GameDev_Project.State_handlers
{
    internal class StateHandler: IStateHandler
    {
        // Singleton vars
        private volatile static StateHandler instance;
        private static object syncRoot = new object();

        // Class Specific vars
        private IStateHandler current_state;
        public IStateHandler Current_state { get { return this.current_state; } }
        private Dictionary<states_enum, IStateHandler> states;
        public Dictionary<states_enum, IStateHandler> States { get { return states; } }
        public enum states_enum
        {
            START,
            RUNNING,
            MAPMAKING,
            PAUSED,
            END
        }
        public bool IsInit { get { return current_state.IsInit; } }
        private bool exit_state;
        public bool ExitState { get { return exit_state; } set { this.exit_state = value; } }

        private StateHandler() { }

        public static StateHandler getInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null) instance = new StateHandler();
                    }
                }

                return instance;
            }
        }

        public void InitStateHandler()
        {
            this.exit_state = false;
            this.states = new Dictionary<states_enum, IStateHandler>();
        }

        public StateHandler SetCurrentState(states_enum state)
        {
            this.current_state = this.states[state];

            return this;
        }

        public void Add(states_enum state_enum, IStateHandler state)
        {
            this.states.Add(state_enum, state);
        }

        public void ResetState(states_enum state_enum, IStateHandler state)
        {
            this.states[state_enum] = state;
        }

        public void Init()
        {
            current_state.Init();
        }

        public void Update(GameTime gameTime)
        {
            this.current_state.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.current_state.Draw(spriteBatch);
        }
    }
}
