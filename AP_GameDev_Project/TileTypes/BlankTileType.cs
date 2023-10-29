using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.TileTypes
{
    internal class BlankTileType : ATileType
    {
        public override int GetRotation()
        {
            return -1;
        }

        public override int GetImage(int rotation)
        {
            return -1;
        }
    }
}
