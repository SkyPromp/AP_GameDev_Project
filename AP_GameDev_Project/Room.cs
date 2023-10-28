using AP_GameDev_Project.TileTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AP_GameDev_Project
{
    internal class Room
    {
        List<Byte> tiles;
        private Int16 room_width;
        private readonly Texture2D tilemap;
        private readonly int tile_size;

        public Room(Texture2D tilemap, string tilesFilename, int tile_size=64) {
            try
            {
                tilesFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tilesFilename);
                List<Byte> bytelist = File.ReadAllBytes(tilesFilename).ToList();
                this.room_width = BitConverter.ToInt16(bytelist.ToArray(), 0);
                this.room_width = (Int16)((this.room_width << 8) + (this.room_width >> 8));  // use Big-Endian
                bytelist.RemoveRange(0, 2);
                this.tiles = bytelist;

            } catch
            {
                throw new Exception("ERROR: File reading failed");
            }

            this.tilemap = tilemap;
            this.tile_size = tile_size;
        }

        public Room(Texture2D tilemap, List<Byte> tiles, Int16 room_width, int tile_size=64)
        {
            this.room_width = room_width;
            this.tiles = tiles;
            this.tilemap = tilemap;
            this.tile_size = tile_size;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.tiles.Count; i++)
            {
                // Place the tile at the correct position relative to eachother
                int screen_x = (i % this.room_width) * this.tile_size;
                int screen_y = (i / this.room_width) * this.tile_size;

                // Move reference point to place the room in the center of the screen
                //screen_x += (GlobalConstants.SCREEN_WIDTH - this.tile_size * this.room_width) / 2;
                //screen_y += (GlobalConstants.SCREEN_HEIGHT - this.tile_size * this.tiles.Count / this.room_width) / 2;


                (int pattern, int angle) = this.GetPattern(i).GetileTile(i, this.tiles, this.room_width);
                if (pattern == -1) continue;
                
                int tilemap_x = pattern;
                int tilemap_y = 0;

                // TODO: Draw to a texture2D
                spriteBatch.Draw(
                    texture: this.tilemap,
                    position: new Vector2(screen_x + this.tile_size / 2, screen_y + this.tile_size / 2),  // The origin changes because of the rotation
                    sourceRectangle: new Rectangle(tilemap_x * this.tile_size, tilemap_y * this.tile_size, this.tile_size, this.tile_size),
                    color: Color.White,
                    rotation: (float) Math.PI / 2.0f * (float) angle,
                    origin: new Vector2(this.tile_size / 2, this.tile_size / 2),
                    scale: 1.0f,
                    effects: SpriteEffects.None,
                    layerDepth: 0
                    );
            }
        }

        private ITileType GetPattern(int i)
        {
            Byte center_tile = this.tiles[i];

            if (center_tile == (Byte) 0) return new BlankTileType();

            TileHelper tileHelper = new TileHelper(room_width, tiles);

            int left_i = tileHelper.getLeftIndex(i);
            int right_i = tileHelper.getRightIndex(i);
            int top_i = tileHelper.getTopIndex(i);
            int bottom_i = tileHelper.getBottomIndex(i);

            Byte left = tileHelper.DoesTileMatch(left_i, center_tile);
            Byte right = tileHelper.DoesTileMatch(right_i, center_tile);
            Byte top = tileHelper.DoesTileMatch(top_i, center_tile);
            Byte bottom = tileHelper.DoesTileMatch(bottom_i, center_tile);

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
