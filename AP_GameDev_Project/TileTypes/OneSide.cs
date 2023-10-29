using System;
using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class OneSide : ATileType
    {
        public override int GetRotation()
        {
            int left = this.tileHelper.getLeftIndex(this.i);
            int right = this.tileHelper.getRightIndex(this.i);
            int top = this.tileHelper.getTopIndex(this.i);

            if (tileHelper.IsCorrectTileAtPos(left)) return 1;
            else if (tileHelper.IsCorrectTileAtPos(right)) return 3;
            else if (tileHelper.IsCorrectTileAtPos(top)) return 2;

            return 0;
        }

        public override int GetImage(int rotation)
        {
            return 1;
        }
    }
}
