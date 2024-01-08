using AP_GameDev_Project.State_handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace AP_GameDev_Project.Input_devices
{
    internal class RunningKeyboardHandler
    {
        private readonly StateHandler stateHandler;

        public RunningKeyboardHandler() 
        {
            this.stateHandler = StateHandler.getInstance;
        }

        public bool is_debug()
        {
            return Keyboard.GetState().IsKeyDown(Keys.F3);
        }

        public void HandleState()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.stateHandler.SetCurrentState(StateHandler.states_enum.START).Init();
            }
        }

        public Vector2 Move()
        {
            Vector2 addSpeed = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                addSpeed += new Vector2(0, -1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                addSpeed += new Vector2(0, 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                addSpeed += new Vector2(-1, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                addSpeed += new Vector2(1, 0);
            }

            return addSpeed;
        }
    }
}
