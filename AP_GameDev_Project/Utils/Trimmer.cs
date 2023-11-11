using System;
using System.Collections.Generic;
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
            (trimmed_room, player_spawnpoint) = Trimmer.TrimBottom(new List<Byte>(tiles), width, player_spawnpoint);
            trimmed_room.Reverse();
            (trimmed_room, _) = Trimmer.TrimBottom(trimmed_room, width);
            trimmed_room.Reverse();

            // Trim horizontally
            (trimmed_room, width, player_spawnpoint) = Trimmer.TrimSide(trimmed_room, width, (int i, int width) => { return i * width; }, player_spawnpoint);  // Left
            (trimmed_room, width, _) = Trimmer.TrimSide(trimmed_room, width, (int i, int width) => { return (i + 1) * width - 1; });  // Right

            return (trimmed_room, width, player_spawnpoint);
        }

        private static (List<Byte>, int, int) TrimSide(List<Byte> trimmed_room, int width, Func<int, int, int> pick_index, int player_spawnpoint=0)
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
                if(player_spawnpoint != -1) player_spawnpoint = (player_spawnpoint - 1) % width < player_spawnpoint % width ? player_spawnpoint - 1: -1;

                width--;
            }

            return (trimmed_room, width, player_spawnpoint);
        }

        private static (List<Byte>, int) TrimBottom(List<Byte> trimmed_room, int width, int player_spawnpoint=0)
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
                if (player_spawnpoint != -1) player_spawnpoint = player_spawnpoint - width > 0 ? player_spawnpoint - width : -1;
            }

            return (trimmed_room, player_spawnpoint);
        }
    }
}
