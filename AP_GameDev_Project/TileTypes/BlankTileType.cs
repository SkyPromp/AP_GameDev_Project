using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.TileTypes
{
    internal class BlankTileType : ITileType
    {
        public (int, int) GetileTile(int i, List<byte> tiles, int room_width)
        {
            return (-1, -1);
        }
    }
}
