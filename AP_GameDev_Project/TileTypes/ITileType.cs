using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.TileTypes
{
    internal interface ITileType
    {
        public (int, int) GetileTile(int i, List<Byte> tiles, int room_width);
    }
}
