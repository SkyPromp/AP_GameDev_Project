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


                (int pattern, int angle) = this.GetPattern(i);
                if (pattern == -1) continue;
                
                int tilemap_x = pattern;
                int tilemap_y = 0;

                //spriteBatch.Draw(this.tilemap, new Vector2(screen_x, screen_y), new Rectangle(tilemap_x * this.tile_size, tilemap_y * this.tile_size, this.tile_size, this.tile_size), Color.White, rotation: Math.PI * angle, origin);
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

        private (int, int) GetPattern(int i)
        {
            Byte center_tile = this.tiles[i];

            if (center_tile == (Byte) 0) return (-1, -1);

            Byte left = i - 1 >= 0 ? this.tiles[i - 1] : (Byte) 0;
            Byte right = i + 1 < this.tiles.Count ? this.tiles[i + 1] : (Byte) 0;
            Byte top = i - this.tiles.Count >= 0 ? this.tiles[i - this.tiles.Count] : (Byte) 0;
            Byte bottom = i + this.room_width < this.tiles.Count ? this.tiles[i + this.room_width] : (Byte) 0;

            left = (Byte)(left == center_tile ? 1 : 0);
            right = (Byte)(right == center_tile ? 1 : 0);
            top = (Byte)(top == center_tile ? 1 : 0);
            bottom = (Byte)(bottom == center_tile ? 1 : 0);

            int image = 0;
            int rotate = 0;  // Multiply by PI/2

            switch (left + right + top + bottom)
            {
                case 0:
                    image = 0;
                    rotate =0;
                    break;
                case 1:
                    image = 1;

                    if (left == (Byte) 1)
                    {
                        rotate = 3;
                    } else if (right == (Byte)1)
                    {
                        rotate = 1;
                    } else if(top == (Byte)1)
                    {
                        rotate = 2;
                    }
                    else
                    {
                        rotate = 0;
                    }

                    break;
                case 2:
                    if(left == (Byte)0)
                    {
                        if(right == left)
                        {
                            image = 2;
                            rotate = 1; // 90 grad
                        }
                        else
                        {
                            image = 3;
                            rotate = 0;
                        }
                    }
                    else
                    {
                        if (left == right)
                        {
                            image = 2;
                            rotate = 0;
                        }
                        else
                        {
                            image = 3;
                            rotate = 1; // 90 grad
                        }
                    }
                    break;
                case 3:
                    image = 4;

                    if (left == (Byte)0)
                    {
                        rotate = 1;
                    }
                    else if (right == (Byte)0)
                    {
                        rotate = 3;
                    }
                    else if (top == (Byte)0)
                    {
                        rotate = 0;
                    }
                    else
                    {
                        rotate = 2;
                    }
                    break;
                case 4:
                    image = 5;
                    rotate = 0;
                    break;
            }

            return (image, rotate);
        }
    }
}
