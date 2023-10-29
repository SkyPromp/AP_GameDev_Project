using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.TileTypes
{
    internal abstract class ATileType
    {
        protected int i;
        protected TileHelper tileHelper;

        public (int, int) GetileTile(int i, List<byte> tiles, int room_width)
        {
            this.tileHelper = new TileHelper(room_width, tiles, i);
            this.i = i;

            int rotate = this.GetRotation();

            return (this.GetImage(rotate), rotate);
        }

        public abstract int GetRotation();
        public abstract int GetImage(int rotation);
    }
}
