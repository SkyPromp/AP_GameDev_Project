using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.TileTypes
{
    internal class TileHelper
    {
        private int room_width;
        private List<Byte> tiles;
        private Byte correct_tile;

        public TileHelper(int room_width, List<byte> tiles, int correct_i)
        {
            this.room_width = room_width;
            this.tiles = tiles;
            this.correct_tile = this.tiles[correct_i];
        }

        public int getLeftIndex(int i)
        {
            return ((i - 1) % this.room_width) < ((i) % this.room_width) && i != -1 ? i - 1 : -1;
        }

        public int getRightIndex(int i)
        {
            return ((i + 1) % this.room_width) > ((i) % this.room_width) && i != -1 ? i + 1 : -1;
        }

        public int getTopIndex(int i)
        {
            return i - this.room_width >= 0 && i != -1 ? i - this.room_width : -1;
        }

        public int getBottomIndex(int i)
        {
            return i + this.room_width < this.tiles.Count && i != -1 ? i + this.room_width : -1;
        }

        public Byte getTile(int i)
        {
            return (Byte)(i != -1 && this.tiles[i] == this.correct_tile ? 1 : 0);
        }
    }
}
