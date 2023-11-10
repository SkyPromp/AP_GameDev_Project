using AP_GameDev_Project.State_handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Input_devices
{
    internal class RunningKeyboardEventHandler
    {
        private RunningKeyboardHandler keyboardHandler;
        private RunningStateHandler stateHandler;
        private double max_debug_cooldown;
        private double debug_cooldown;

        public RunningKeyboardEventHandler(RunningKeyboardHandler keyboardHandler, RunningStateHandler stateHandler) 
        { 
            this.keyboardHandler = keyboardHandler;
            this.stateHandler = stateHandler;
            this.debug_cooldown = 0;
            this.max_debug_cooldown = 0.3;
        }

        public void Update()
        {
            if (this.debug_cooldown <= 0 && keyboardHandler.is_debug())
            {
                this.debug_cooldown = this.max_debug_cooldown;
                this.stateHandler.ToggleDebug();
            }

            this.stateHandler.MovePlayer(keyboardHandler.Move());

            this.keyboardHandler.HandleState();
            
        }
    }
}
