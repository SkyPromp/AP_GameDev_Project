using AP_GameDev_Project.TileTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AP_GameDev_Project.Utils
{
    internal class Room
    {
        List<byte> tiles;
        private ushort room_width;
        private readonly Texture2D tilemap;
        private readonly int tile_size;
        private ContentManager contentManager;
        private Vector2 offset;
        private Vector2 player_spawnpoint;
        public Vector2 GetPlayerSpawnpoint { get { return player_spawnpoint; } }
        private TileSelector tileSelector;

        public Room(string tilesFilename, int tile_size = 64)
        {
            ushort player_spawnpoint;

            try
            {
                tilesFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tilesFilename);
                List<byte> bytelist = File.ReadAllBytes(tilesFilename).ToList();

                room_width = BitConverter.ToUInt16(bytelist.ToArray(), 0);
                room_width = (ushort)((room_width << 8) + (room_width >> 8));  // use Big-Endian
                bytelist.RemoveRange(0, 2);

                player_spawnpoint = BitConverter.ToUInt16(bytelist.ToArray(), 0);
                player_spawnpoint = (ushort)((player_spawnpoint << 8) + (player_spawnpoint >> 8));  // use Big-Endian
                bytelist.RemoveRange(0, 2);

                tiles = bytelist;
            }
            catch
            {
                throw new Exception("ERROR: File reading failed");
            }

            contentManager = ContentManager.getInstance;
            tilemap = contentManager.GetTextures["TILEMAP"];
            this.tile_size = tile_size;

            Vector2 tile_center_coords = IndexToXY(player_spawnpoint) * this.tile_size + new Vector2(32, 32);
            Rectangle sprite_rectangle = new Rectangle(0, 0, 128, 192);  // DO MORE DYNAMICALLY
            this.player_spawnpoint = tile_center_coords - new Vector2(sprite_rectangle.Width / 2, 116);  // DO MORE DYNAMICALLY

            this.tileSelector = new TileSelector(this.tiles, this.room_width);
        }

        public Room(List<byte> tiles, ushort room_width, int tile_size = 64)
        {
            contentManager = ContentManager.getInstance;
            this.room_width = room_width;
            this.tiles = tiles;
            tilemap = contentManager.GetTextures["TILEMAP"];
            this.tile_size = tile_size;

            this.tileSelector = new TileSelector(this.tiles, this.room_width);
        }

        public List<Rectangle> GetHitboxes(Func<byte, bool> filter = null)  // TODO Refactor away List<Byte>
        {
            List<Rectangle> result = new List<Rectangle>();

            for (int i = 0; i < tiles.Count; i++)
            {
                int screen_x = i % room_width * tile_size;
                int screen_y = i / room_width * tile_size;

                // Move reference point to place the room in the center of the screen
                screen_x += (int)offset.X;
                screen_y += (int)offset.Y;

                (int pattern, int angle) = this.tileSelector.GetPattern(i).GetileTile(i, tiles, room_width);
                if (pattern == -1) continue;
                if (filter != null && !filter(tiles[i])) continue;

                result.Add(new Rectangle(screen_x, screen_y, tile_size, tile_size));
            }

            return result;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                // Place the tile at the correct position relative to eachother
                int screen_x = i % room_width * tile_size;
                int screen_y = i / room_width * tile_size;

                // Move reference point to place the room in the center of the screen
                screen_x += (int)offset.X;
                screen_y += (int)offset.Y;

                (int pattern, int angle) = this.tileSelector.GetPattern(i).GetileTile(i, tiles, room_width);
                if (pattern == -1) continue;

                int tilemap_x = pattern;
                int tilemap_y = tiles[i] - 1;

                // TODO: Draw to a texture2D
                Vector2 origin_offset = Vector2.One * tile_size / 2;

                spriteBatch.Draw(
                    texture: tilemap,
                    position: new Vector2(screen_x, screen_y) + origin_offset,  // The origin changes because of the rotation
                    sourceRectangle: new Rectangle(tilemap_x * tile_size, tilemap_y * tile_size, tile_size, tile_size),
                    color: Color.White,
                    rotation: (float)Math.PI / 2.0f * angle,
                    origin: origin_offset,
                    scale: 1.0f,
                    effects: SpriteEffects.None,
                    layerDepth: 0
                    );
            }
        }

        public void Center()
        {
            offset = new Vector2(
                (GlobalConstants.SCREEN_WIDTH - tile_size * room_width) / 2,
                (GlobalConstants.SCREEN_HEIGHT - tile_size * tiles.Count / room_width) / 2);
            player_spawnpoint += offset;
        }

        private Vector2 IndexToXY(int index)
        {
            int width = GlobalConstants.SCREEN_WIDTH / tile_size;
            return new Vector2(index % width, index / width);
        }
    }
}
