using AP_GameDev_Project.State_handlers;
using AP_GameDev_Project.Utils;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Input_devices
{
    internal class MapMakingKeyboardHandler
    {
        private readonly StateHandler stateHandler;

        public MapMakingKeyboardHandler()
        {
            this.stateHandler = StateHandler.getInstance;
        }

        public (int, double, double, bool) HandleKeyboard(int current_tile_brush, double change_brush_cooldown, double toggle_font_cooldown, List<Byte> tiles, int tile_size, bool show_current_brush)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.stateHandler.SetCurrentState(StateHandler.states_enum.START).Init();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && change_brush_cooldown <= 0)
            {
                current_tile_brush++;
                change_brush_cooldown = 0.5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && change_brush_cooldown <= 0)
            {
                current_tile_brush--;
                change_brush_cooldown = 0.5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.B) && toggle_font_cooldown <= 0)
            {
                show_current_brush = !show_current_brush;
                toggle_font_cooldown = 0.5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                List<Byte> trimmed_tiles;
                int width;
                (trimmed_tiles, width) = Trimmer.GetTrimmedRoom(new List<Byte>(tiles), tile_size);

                // Write to file
                Byte[] header = BitConverter.GetBytes(width);

                trimmed_tiles.Insert(0, header[0]);
                trimmed_tiles.Insert(0, header[1]);

                FileSaver.SaveFile(trimmed_tiles);
            }

            return (current_tile_brush, change_brush_cooldown, toggle_font_cooldown, show_current_brush);
        }
    }
}
