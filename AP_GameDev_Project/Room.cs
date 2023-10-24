using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AP_GameDev_Project
{
    internal class Room
    {
        List<Byte> tiles;
        private int room_width;
        private readonly Texture2D tilemap;
        private readonly int tile_size;

        public Room(Texture2D tilemap ,int room_width, string tilesFilename, int tile_size=64) {
            try
            {
                tilesFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tilesFilename);
                this.tiles = File.ReadAllBytes(tilesFilename).ToList();

            } catch
            {
                throw new Exception("ERROR: File reading failed");
            }


            this.tilemap = tilemap;

            this.room_width = room_width;
            this.tile_size = tile_size;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.tiles.Count; i++)
            {
                int screen_x = (i % this.room_width) * this.tile_size;  // TODO Center
                int screen_y = (i / this.room_width) * this.tile_size;  // TODO Center

                int tilemap_x;
                int tilemap_y;

                switch (this.tiles[i])
                {
                    case 1:
                        tilemap_x = 0;
                        tilemap_y = 0;
                        break;
                    default:
                        continue;
                }

                // Todo find correct tile and rotate correctly
                spriteBatch.Draw(this.tilemap, new System.Numerics.Vector2(screen_x, screen_y), new Rectangle(tilemap_x * this.tile_size, tilemap_y * this.tile_size, this.tile_size, this.tile_size), Color.White);
            }
        }
    }
}
