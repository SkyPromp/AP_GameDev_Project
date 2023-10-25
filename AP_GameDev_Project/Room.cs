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
                int screen_x = (i % this.room_width) * this.tile_size + 64*3;  // TODO Center
                int screen_y = (i / this.room_width) * this.tile_size + 64*3;  // TODO Center


                (int pattern, int angle) = this.GetPattern(i);
                if (pattern == -1) continue;
                
                int tilemap_x = pattern;
                int tilemap_y = 0;

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

            Byte left = DoesTileMatch(this.getLeftIndex(i), center_tile);
            Byte right = DoesTileMatch(this.getRightIndex(i), center_tile);
            Byte top = DoesTileMatch(this.getTopIndex(i), center_tile); ;
            Byte bottom = DoesTileMatch(this.getBottomIndex(i), center_tile);

            Byte bottom_right = DoesTileMatch(this.getRightIndex(this.getBottomIndex(i)), center_tile);
            Byte bottom_left = DoesTileMatch(this.getLeftIndex(this.getBottomIndex(i)), center_tile);
            Byte top_right = DoesTileMatch(this.getRightIndex(this.getTopIndex(i)), center_tile);
            Byte top_left = DoesTileMatch(this.getLeftIndex(this.getTopIndex(i)), center_tile);

            int image = 0;
            int rotate = 0;

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
                        if(right == left)  // Parallel
                        {
                            image = 2;
                            rotate = 1;
                        }
                        else if(top == (Byte)0)
                        {
                            rotate = 0;

                            if(bottom_right == (Byte)0)
                            {
                                image = 4;
                            }
                            else
                            {
                                image = 3;
                            }
                        }
                        else {
                            rotate = 3;

                            if (top_right == (Byte)0)
                            {
                                image = 4;
                            }
                            else
                            {
                                image = 3;
                            }
                        }
                    }
                    else
                    {
                        if (left == right)  // Parallel
                        {
                            image = 2;
                            rotate = 0;
                        }
                        else if(top == (Byte)0)
                        {
                            rotate = 1;

                            if (bottom_left == (Byte)0)
                            {
                                image = 4;
                            }
                            else
                            {
                                image = 3;
                            }
                        }
                        else
                        {
                            rotate = 2;

                            if (top_left == (Byte)0)
                            {
                                image = 4;
                            }
                            else
                            {
                                image = 3;
                            }
                        }
                    }
                    break;
                case 3:
                    image = 5;

                    if (left == (Byte)0)
                    {
                        rotate = 3;
                    }
                    else if (right == (Byte)0)
                    {
                        rotate = 1;
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
                    image = 6;
                    rotate = 0;
                    break;
            }

            return (image, rotate);
        }

        private int getLeftIndex(int i)
        {
            return ((i - 1) % this.room_width) < ((i) % this.room_width) && i != -1 ? i - 1 : -1;
        }

        private int getRightIndex(int i)
        {
            return ((i + 1) % this.room_width) > ((i) % this.room_width) && i != -1 ? i + 1 : -1;
        }

        private int getTopIndex(int i)
        {
            return i - this.room_width >= 0 && i != -1 ? i - this.room_width : -1;
        }

        private int getBottomIndex(int i)
        {
            return i + this.room_width < tiles.Count && i != -1 ? i + this.room_width : -1;
        }

        private Byte DoesTileMatch(int i, Byte correct_tile)
        {
            return (Byte)(i != -1 && this.tiles[i] == correct_tile ? 1 : 0);
        }
    }
}
