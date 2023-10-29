using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.TileTypes
{
    internal class ZeroSide : ATileType
    {
        protected override int GetRotation()
        {
            return 0;
        }

        protected override int GetImage(int rotation)
        {
            return 0;
        }
    }
}
