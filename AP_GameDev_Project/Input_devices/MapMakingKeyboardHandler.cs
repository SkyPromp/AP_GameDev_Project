using AP_GameDev_Project.State_handlers;
using Microsoft.Xna.Framework.Input;
using System;


namespace AP_GameDev_Project.Input_devices
{
    internal class MapMakingKeyboardHandler
    {
        private readonly StateHandler stateHandler;

        public MapMakingKeyboardHandler()
        {
            this.stateHandler = StateHandler.getInstance;
        }

        public void HandleState()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.stateHandler.SetCurrentState(StateHandler.states_enum.START).Init();
            }
        }

        public Byte Change_brush()
        {
            Byte delta = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Up)) delta++;
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) delta--;

            return delta;
        }

        public bool ToggleFont()
        {
            return Keyboard.GetState().IsKeyDown(Keys.B);
        }

        public bool SaveFile()
        {
            return Keyboard.GetState().IsKeyDown(Keys.S);
        }
    }
}
