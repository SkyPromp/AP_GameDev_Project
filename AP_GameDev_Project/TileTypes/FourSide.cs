using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;


namespace AP_GameDev_Project.TileTypes
{
    internal class FourSide : ATileType
    {
        protected override int GetRotation()
        {
            bool bottom_right = this.tileHelper.IsCorrectTileAtPos(this.tileHelper.getRotatedCorner((int)TileHelper.corners.BOTTOM_RIGHT, this.i, 0));
            bool bottom_left = this.tileHelper.IsCorrectTileAtPos(this.tileHelper.getRotatedCorner((int)TileHelper.corners.BOTTOM_LEFT, this.i, 0));
            bool top_right = this.tileHelper.IsCorrectTileAtPos(this.tileHelper.getRotatedCorner((int)TileHelper.corners.TOP_RIGHT, this.i, 0));
            bool top_left = this.tileHelper.IsCorrectTileAtPos(this.tileHelper.getRotatedCorner((int)TileHelper.corners.TOP_LEFT, this.i, 0));

            if (!bottom_left)
            {
                if (!top_left)
                {
                    if (!top_right) return 2;  // Check if rotating sprite 13 removes nested if
                    return 1;
                }
                return 0;
            } 
            if (!bottom_right) return 3;
            if (!top_right) return 2;
            if (!top_left) return 1;

            return 0;
        }

        protected override int GetImage(int rotation)
        {
            bool bottom_right = this.tileHelper.IsCorrectTileAtPos(this.tileHelper.getRotatedCorner((int)TileHelper.corners.BOTTOM_RIGHT, this.i, rotation));
            bool bottom_left = this.tileHelper.IsCorrectTileAtPos(this.tileHelper.getRotatedCorner((int)TileHelper.corners.BOTTOM_LEFT, this.i, rotation));
            bool top_right = this.tileHelper.IsCorrectTileAtPos(this.tileHelper.getRotatedCorner((int)TileHelper.corners.TOP_RIGHT, this.i, rotation));
            bool top_left = this.tileHelper.IsCorrectTileAtPos(this.tileHelper.getRotatedCorner((int)TileHelper.corners.TOP_LEFT, this.i, rotation));

            int total = (bottom_right ? 0 : 1) + (bottom_left ? 0 : 1) + (top_right ? 0 : 1) + (top_left ? 0 : 1);

            switch (total)
            {
                case 0:
                    return 9;
                case 1:
                    return 10;
                case 2:
                    if (top_left == bottom_right) return 12;
                    return 11;
                case 3:
                    return 13;
                case 4:
                    return 14;
            }

            throw new InvalidOperationException(string.Format("Unexpected sum of corners: ", total));
        }
    }
}
