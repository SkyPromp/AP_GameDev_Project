using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Utils
{
    internal class Trimmer
    {
        public static (List<Byte>, int, int) GetTrimmedRoom(List<Byte> tiles, int tile_size, int player_spawnpoint)
        {
            // Trim vertically
            int width = GlobalConstants.SCREEN_WIDTH / tile_size;
            List<Byte> trimmed_room;

            (trimmed_room) = Trimmer.TrimBottom(new List<Byte>(tiles), width);
            player_spawnpoint -= tiles.Count - trimmed_room.Count;

            trimmed_room.Reverse();
            (trimmed_room) = Trimmer.TrimBottom(trimmed_room, width);
            trimmed_room.Reverse();

            // Trim horizontally
            int old_width = width;
            (trimmed_room, width) = Trimmer.TrimSide(trimmed_room, width, (int i, int width) => { return i * width; });  // Left
            player_spawnpoint -= old_width - width;

            (trimmed_room, width) = Trimmer.TrimSide(trimmed_room, width, (int i, int width) => { return (i + 1) * width - 1; });  // Right

            return (trimmed_room, width, player_spawnpoint);
        }

        private static (List<Byte>, int) TrimSide(List<Byte> trimmed_room, int width, Func<int, int, int> pick_index)
        {
            int height = trimmed_room.Count / width;

            while (trimmed_room.Count > 0)
            {
                bool is_empty = true;

                for (int i = 0; i < height; i += 1)
                {
                    if (i >= trimmed_room.Count || trimmed_room[pick_index(i, width)] != (Byte)0)
                    {
                        is_empty = false;
                        break;
                    }
                }

                if (!is_empty) break;
                for (int i = height - 1; i >= 0; i--) trimmed_room.RemoveAt(pick_index(i, width));

                width--;
            }

            return (trimmed_room, width);
        }

        private static List<Byte> TrimBottom(List<Byte> trimmed_room, int width)
        {
            while (trimmed_room.Count > 0)
            {
                bool is_empty = true;

                for (int i = 0; i < width; i++)
                {
                    if (i >= trimmed_room.Count || trimmed_room[i] != (Byte)0)
                    {
                        is_empty = false;
                        break;
                    }
                }

                if (!is_empty) break;
                trimmed_room.RemoveRange(0, width);
            }

            return (trimmed_room);
        }
    }
}
