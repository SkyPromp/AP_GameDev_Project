using AP_GameDev_Project.TileTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Utils
{
    internal class TileSelector
    {
        List<byte> tiles;
        private ushort room_width;

        public TileSelector(List<byte> tiles, ushort room_width) 
        {
            this.tiles = tiles;
            this.room_width = room_width;
        }

        public ATileType GetPattern(int i)
        {
            byte center_tile = this.tiles[i];

            if (center_tile == 0) return new BlankTileType();

            TileHelper tileHelper = new TileHelper(this.room_width, this.tiles, i);

            int left = tileHelper.IsCorrectTileAtPos(tileHelper.getLeftIndex(i)) ? 1 : 0;
            int right = tileHelper.IsCorrectTileAtPos(tileHelper.getRightIndex(i)) ? 1 : 0;
            int top = tileHelper.IsCorrectTileAtPos(tileHelper.getTopIndex(i)) ? 1 : 0;
            int bottom = tileHelper.IsCorrectTileAtPos(tileHelper.getBottomIndex(i)) ? 1 : 0;

            switch (left + right + top + bottom)
            {
                case 0:
                    return new ZeroSide();
                case 1:
                    return new OneSide();
                case 2:
                    return new TwoSide();
                case 3:
                    return new ThreeSide();
                case 4:
                    return new FourSide();
            }

            throw new InvalidOperationException(string.Format("Unexpected sum of sides: ", left + right + top + bottom));
        }
    }
}
