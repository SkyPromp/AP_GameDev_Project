using System;
using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class ThreeSide : ATileType
    {
        public override int GetRotation()
        {
            int left = this.tileHelper.getLeftIndex(i);
            int right = this.tileHelper.getRightIndex(i);
            int top = this.tileHelper.getTopIndex(i);

            if (!this.tileHelper.IsCorrectTileAtPos(left)) return 3;
            else if (!this.tileHelper.IsCorrectTileAtPos(right)) return 1;
            else if (!this.tileHelper.IsCorrectTileAtPos(top)) return 0;

            return 2;
        }

        public override int GetImage(int rotation)
        {
            int bottom_left = this.tileHelper.getRotatedCorner((int)TileHelper.corners.BOTTOM_LEFT, this.i, rotation);
            int bottom_right = this.tileHelper.getRotatedCorner((int)TileHelper.corners.BOTTOM_RIGHT, this.i, rotation);

            if (!this.tileHelper.IsCorrectTileAtPos(bottom_left))
            {
                if (!this.tileHelper.IsCorrectTileAtPos(bottom_right)) return 8;
                else return 6;
            }
            else if (!this.tileHelper.IsCorrectTileAtPos(bottom_right)) return 7;

            return 5;
        }
    }
}
