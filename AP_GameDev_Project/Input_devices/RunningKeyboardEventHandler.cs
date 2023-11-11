using AP_GameDev_Project.State_handlers;
using Microsoft.Xna.Framework;

namespace AP_GameDev_Project.Input_devices
{
    internal class RunningKeyboardEventHandler: IInputHandler
    {
        private RunningKeyboardHandler keyboardHandler;
        private RunningStateHandler stateHandler;
        private double max_debug_cooldown;
        private double debug_cooldown;
        private double max_sound_cooldown;
        private double sound_cooldown;

        public RunningKeyboardEventHandler(RunningStateHandler stateHandler) 
        { 
            this.keyboardHandler = new RunningKeyboardHandler();
            this.stateHandler = stateHandler;
            this.debug_cooldown = 0;
            this.max_debug_cooldown = 0.3;
            this.sound_cooldown = 0;
            this.max_sound_cooldown = 0.3;
        }

        public void Update(GameTime gameTime)
        {
            if (this.debug_cooldown > 0) this.debug_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            if (this.sound_cooldown > 0) this.sound_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            if (this.debug_cooldown <= 0 && this.keyboardHandler.is_debug())
            {
                this.debug_cooldown = this.max_debug_cooldown;
                this.stateHandler.ToggleDebug();
            }

            if (this.sound_cooldown <= 0)
            {
                this.sound_cooldown = this.max_sound_cooldown;
                this.keyboardHandler.HandleSound();
            }

            this.stateHandler.MovePlayer(this.keyboardHandler.Move());

            this.keyboardHandler.HandleState();
        }
    }
}
