using AP_GameDev_Project.State_handlers;
using Microsoft.Xna.Framework;


namespace AP_GameDev_Project.Input_devices
{
    internal class MapMakingKeyboardEventHandler
    {
        private MapMakingKeyboardHandler keyboardHandler;
        private MapMakingStateHandler stateHandler;
        private double max_change_brush_cooldown;
        private double change_brush_cooldown;
        private double max_toggle_font_cooldown;
        private double toggle_font_cooldown;

        public MapMakingKeyboardEventHandler(MapMakingStateHandler stateHandler)
        {
            this.keyboardHandler = new MapMakingKeyboardHandler();
            this.stateHandler = stateHandler;
            this.change_brush_cooldown = 0;
            this.max_change_brush_cooldown = 0.3;
            this.max_toggle_font_cooldown = 0.3;
            this.toggle_font_cooldown = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (this.change_brush_cooldown > 0) this.change_brush_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            if (this.toggle_font_cooldown > 0) this.toggle_font_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            if (this.change_brush_cooldown <= 0)
            {
                this.change_brush_cooldown = this.max_change_brush_cooldown;
                this.stateHandler.ChangeBrush(this.keyboardHandler.Change_brush());
            }

            if (this.toggle_font_cooldown <= 0 && this.keyboardHandler.ToggleFont())
            {
                this.toggle_font_cooldown = this.max_toggle_font_cooldown;
                this.stateHandler.ToggleFont();
            }

            if (this.keyboardHandler.SaveFile()) this.stateHandler.SaveFile();

            this.keyboardHandler.HandleState();
        }
    }
}
