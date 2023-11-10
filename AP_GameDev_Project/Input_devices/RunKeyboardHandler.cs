using AP_GameDev_Project.Entities;
using AP_GameDev_Project.State_handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Input_devices
{
    internal class RunKeyboardHandler
    {
        private readonly StateHandler stateHandler;

        public RunKeyboardHandler() 
        {
            this.stateHandler = StateHandler.getInstance;
        }

        public List<AEntity> HandleKeyboard(RunningStateHandler runningState, List<AEntity> entities)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.stateHandler.SetCurrentState(StateHandler.states_enum.START).Init();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F3) && runningState.Debug_cooldown <= 0)
            {
                runningState.Debug_cooldown = runningState.Max_debug_cooldown;

                foreach (AEntity entity in entities) entity.show_hitbox = !entity.show_hitbox;
            }

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

            entities[0].SpeedUp(addSpeed / 3);

            return entities;
        }
    }
}
