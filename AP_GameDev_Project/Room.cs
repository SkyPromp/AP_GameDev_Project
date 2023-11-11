using AP_GameDev_Project.TileTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AP_GameDev_Project
{
    internal class Room
    {
        List<Byte> tiles;
        private UInt16 room_width;
        private readonly Texture2D tilemap;
        private readonly int tile_size;
        private ContentManager contentManager;
        private Vector2 offset;
        private Vector2 player_spawnpoint;
        public Vector2 GetPlayerSpawnpoint { get { return this.player_spawnpoint; } }

        public Room(string tilesFilename, int tile_size=64) {
            UInt16 player_spawnpoint;

            try
            {
                tilesFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tilesFilename);
                List<Byte> bytelist = File.ReadAllBytes(tilesFilename).ToList();
                this.room_width = BitConverter.ToUInt16(bytelist.ToArray(), 0);
                this.room_width = (UInt16)((this.room_width << 8) + (this.room_width >> 8));  // use Big-Endian
                bytelist.RemoveRange(0, 2);

                player_spawnpoint = BitConverter.ToUInt16(bytelist.ToArray(), 0);
                player_spawnpoint = (UInt16)((player_spawnpoint << 8) + (player_spawnpoint >> 8));  // use Big-Endian


                bytelist.RemoveRange(0, 2);
                this.tiles = bytelist;

            } catch
            {
                throw new Exception("ERROR: File reading failed");
            }
            this.contentManager = ContentManager.getInstance;
            this.tilemap = this.contentManager.GetTextures["TILEMAP"];
            this.tile_size = tile_size;

            Vector2 tile_center_coords = this.IndexToXY(player_spawnpoint) * this.tile_size + new Vector2(32, 32);
            Rectangle sprite_rectangle = new Rectangle(0, 0, 128, 192);  // DO MORE DYNAMICALLY
            this.player_spawnpoint = tile_center_coords - new Vector2(sprite_rectangle.Width / 2, 152);  // DO MORE DYNAMICALLY
        }

        public Room(List<Byte> tiles, UInt16 room_width, int tile_size=64)
        {
            this.contentManager = ContentManager.getInstance;
            this.room_width = room_width;
            this.tiles = tiles;
            this.tilemap = this.contentManager.GetTextures["TILEMAP"];
            this.tile_size = tile_size;
        }

        public List<Rectangle> GetHitboxes()  // TODO Refactor away List<Byte>
        {
            List<Rectangle> result = new List<Rectangle>();

            for (int i = 0; i < this.tiles.Count; i++)
            {
                int screen_x = (i % this.room_width) * this.tile_size;
                int screen_y = (i / this.room_width) * this.tile_size;

                // Move reference point to place the room in the center of the screen
                screen_x += (int)offset.X;
                screen_y += (int)offset.Y;

                (int pattern, int angle) = this.GetPattern(i).GetileTile(i, this.tiles, this.room_width);
                if (pattern == -1) continue;

                result.Add(new Rectangle(screen_x, screen_y, this.tile_size, this.tile_size));
            }

            return result;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.tiles.Count; i++)
            {
                // Place the tile at the correct position relative to eachother
                int screen_x = (i % this.room_width) * this.tile_size;
                int screen_y = (i / this.room_width) * this.tile_size;

                // Move reference point to place the room in the center of the screen
                screen_x += (int) offset.X;
                screen_y += (int) offset.Y;

                (int pattern, int angle) = this.GetPattern(i).GetileTile(i, this.tiles, this.room_width);
                if (pattern == -1) continue;
                
                int tilemap_x = pattern;
                int tilemap_y = this.tiles[i] - 1;

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

        public void Center()
        {
            this.offset = new Vector2(
                (GlobalConstants.SCREEN_WIDTH - this.tile_size * this.room_width) / 2,
                (GlobalConstants.SCREEN_HEIGHT - this.tile_size * this.tiles.Count / this.room_width) / 2);
            this.player_spawnpoint += this.offset;
        }

        private ATileType GetPattern(int i)
        {
            Byte center_tile = this.tiles[i];

            if (center_tile == (Byte) 0) return new BlankTileType();

            TileHelper tileHelper = new TileHelper(room_width, tiles, i);

            int left_i = tileHelper.getLeftIndex(i);
            int right_i = tileHelper.getRightIndex(i);
            int top_i = tileHelper.getTopIndex(i);
            int bottom_i = tileHelper.getBottomIndex(i);

            int left = tileHelper.IsCorrectTileAtPos(left_i) ? 1 : 0;
            int right = tileHelper.IsCorrectTileAtPos(right_i) ? 1 : 0;
            int top = tileHelper.IsCorrectTileAtPos(top_i) ? 1 : 0;
            int bottom = tileHelper.IsCorrectTileAtPos(bottom_i) ? 1 : 0;

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

        private Vector2 IndexToXY(int index)
        {
            int width = GlobalConstants.SCREEN_WIDTH / this.tile_size;
            return new Vector2(index % width, index / width);
        }
    }
}
