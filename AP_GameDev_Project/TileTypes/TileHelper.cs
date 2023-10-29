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

        public enum corners: int
        {
            /*TOP_LEFT,
            BOTTOM_LEFT,
            BOTTOM_RIGHT,
            TOP_RIGHT*/
            TOP_LEFT,
            TOP_RIGHT,
            BOTTOM_RIGHT,
            BOTTOM_LEFT
        }

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

        public bool IsCorrectTileAtPos(int pos)
        {
            return pos != -1 && this.tiles[pos] == this.correct_tile;
        }

        public int getRotatedCorner(int corner, int i, int rotation)
        {
            corner = (int)(corner + rotation) % 4;

            switch (corner)
            {
                case (int)corners.TOP_LEFT:
                    return this.getTopIndex(this.getLeftIndex(i));
                case (int)corners.BOTTOM_LEFT:
                    return this.getBottomIndex(this.getLeftIndex(i)); ;
                case (int)corners.BOTTOM_RIGHT:
                    return this.getBottomIndex(this.getRightIndex(i));
                case (int)corners.TOP_RIGHT:
                    return this.getTopIndex(this.getRightIndex(i));
            }

            throw new InvalidOperationException(string.Format("Invalid corner rotation: {0}", corner));
        }
    }
}
