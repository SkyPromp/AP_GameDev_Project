﻿using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AP_GameDev_Project.State_handlers
{
    internal class MapMakingStateHandler : IStateHandler
    {
        private MouseHandler mouseHandler;
        private bool is_init;
        public bool IsInit { get { return this.is_init; } }
        private int tile_size;

        private Texture2D tilemap;
        private List<Byte> tiles;
        private Byte current_tile_brush;
        private Room room;

        // DRAW VERTECES VARIABLES
        private GraphicsDevice graphicsDevice;
        private BasicEffect basicEffect;

        public MapMakingStateHandler(GraphicsDevice graphicsDevice, Texture2D tilemap,int tile_size=64) {
            this.is_init = false;
            this.mouseHandler = MouseHandler.getInstance;
            this.tile_size = tile_size;
            this.tiles = new List<Byte>();
            this.tilemap = tilemap;

            // DRAW VERTICES SETUP
            this.graphicsDevice = graphicsDevice;
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
            (0, graphicsDevice.Viewport.Width,     // left, right
            graphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1);
        }

        public void Init()
        {
            this.is_init = true;
            this.current_tile_brush = 1;

            // FIX Without Math.Ceiling
            Int16 room_width = (Int16) Math.Ceiling((double)GlobalConstants.SCREEN_WIDTH / this.tile_size);
            Int16 room_height = (Int16) Math.Ceiling((double)GlobalConstants.SCREEN_HEIGHT / this.tile_size);

            int tile_amount = (int)Math.Ceiling((double)room_width * (double)room_height);
            this.tiles = Enumerable.Repeat((Byte) 0, tile_amount).ToList();

            this.room = new Room(this.tilemap, this.tiles, room_width);

            this.mouseHandler.LeftClickHook = () => { this.PlaceTile(this); };
            this.mouseHandler.RightClickHook = () => { this.RemoveTile(this); };
        }

        public void Update(GameTime gameTime)
        {
            this.mouseHandler.Update();
            // TODO: change current tile brush with keyboard
            // TODO: store to .room file with keyboard

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                // Trim this.tiles
                List<Byte> trimmed_tiles = GetTrimmedRoom();
                // Write to file
            }
        }

        private List<Byte> GetTrimmedRoom() // TODO: inject function to generalize trimleft, trimtop, trimright
        {  // TODO put in seperate class
            // Trim vertically
            int base_width = GlobalConstants.SCREEN_WIDTH / this.tile_size;

            List<Byte> trimmed_room = this.TrimTop(new List<Byte>(this.tiles), base_width);
            trimmed_room.Reverse();
            trimmed_room = this.TrimTop(trimmed_room, GlobalConstants.SCREEN_WIDTH / this.tile_size);
            trimmed_room.Reverse();

            // Trim horizontally
            int width;
            (trimmed_room, width) = this.TrimLeft(trimmed_room, base_width);
            trimmed_room = this.TrimRight(trimmed_room, width);

            return trimmed_room;
        }

        private (List<Byte>,int) TrimLeft(List<Byte> trimmed_room, int width)
        {
            int height = trimmed_room.Count / width;

            while (trimmed_room.Count > 0)
            {
                bool is_empty = true;

                for (int i = 0; i < height; i += 1)
                {
                    if (i >= trimmed_room.Count || trimmed_room[i * width] != (Byte)0)
                    {
                        is_empty = false;
                        break;
                    }
                }

                if (!is_empty) break;

                for (int i = height - 1; i >= 0; i--) trimmed_room.RemoveAt(i * width); 
                width--;
            }

            return (trimmed_room, width);
        }

        private List<Byte> TrimRight(List<Byte> trimmed_room, int width)
        {
            int height = trimmed_room.Count / width;

            while (trimmed_room.Count > 0)
            {
                bool is_empty = true;

                for (int i = 0; i < height; i += 1)
                {
                    if (i >= trimmed_room.Count || trimmed_room[(i + 1) * width - 1] != (Byte)0)
                    {
                        is_empty = false;
                        break;
                    }
                }

                if (!is_empty) break;

                for (int i = height - 1; i >= 0; i--) trimmed_room.RemoveAt((i + 1) * width - 1);
                width--;
            }

            return trimmed_room;
        }

        private List<Byte> TrimTop(List<Byte> trimmed_room, int width)
        {
            while (trimmed_room.Count > 0)
            {
                bool is_empty = true;

                for (int i = 0; i < width; i++)
                {
                    if (i >= trimmed_room.Count || trimmed_room[i] != (Byte)0)
                    {
                        is_empty = false;
                        break;
                    }
                }

                if (!is_empty) break;
                trimmed_room.RemoveRange(0, width);
            }

            return trimmed_room;
        }

        private void PlaceTile(MapMakingStateHandler mapMaker)
        {
            if (new Rectangle(0, 0, GlobalConstants.SCREEN_WIDTH, GlobalConstants.SCREEN_HEIGHT).Contains(mouseHandler.MousePos))
            {
                int tile_row = (int)mapMaker.mouseHandler.MousePos.Y / mapMaker.tile_size;
                int tile_column = (int)mapMaker.mouseHandler.MousePos.X / mapMaker.tile_size;
                int tile_index = tile_column + tile_row * GlobalConstants.SCREEN_WIDTH / mapMaker.tile_size;

                Debug.Assert((tile_column + 1) * (tile_row + 1) <= mapMaker.tiles.Count,
                    message: string.Format("Error: Tile X:{0} Y:{1} is out of scope {2}", tile_column, tile_row, mapMaker.tiles.Count));

                mapMaker.tiles[tile_index] = mapMaker.current_tile_brush;
            }
        }

        // Refactor into PlaceTile
        private void RemoveTile(MapMakingStateHandler mapMaker)
        {
            if (new Rectangle(0, 0, GlobalConstants.SCREEN_WIDTH, GlobalConstants.SCREEN_HEIGHT).Contains(mouseHandler.MousePos))
            {
                int tile_row = (int)mapMaker.mouseHandler.MousePos.Y / mapMaker.tile_size;
                int tile_column = (int)mapMaker.mouseHandler.MousePos.X / mapMaker.tile_size;
                int tile_index = tile_column + tile_row * GlobalConstants.SCREEN_WIDTH / mapMaker.tile_size;

                Debug.Assert((tile_column + 1) * (tile_row + 1) <= mapMaker.tiles.Count,
                    message: string.Format("Error: Tile X:{0} Y:{1} is out of scope {2}", tile_column, tile_row, mapMaker.tiles.Count));

                mapMaker.tiles[tile_index] = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.DrawGrid(spriteBatch);
            if (this.room != null) this.room.Draw(spriteBatch);
        }

        private void DrawGrid(SpriteBatch spriteBatch)
        {
            basicEffect.CurrentTechnique.Passes[0].Apply();
            var vertices = new VertexPositionColor[2];
            vertices[0].Color = Color.Black;
            vertices[1].Color = Color.Black;

            for (int tile_i_pos = this.tile_size; tile_i_pos < GlobalConstants.SCREEN_WIDTH; tile_i_pos += this.tile_size)
            {
                this.DrawLine(vertices, new Vector2(tile_i_pos, 0), new Vector2(tile_i_pos, GlobalConstants.SCREEN_HEIGHT));

                if(tile_i_pos < GlobalConstants.SCREEN_HEIGHT)
                {
                    this.DrawLine(vertices, new Vector2(0, tile_i_pos), new Vector2(GlobalConstants.SCREEN_WIDTH, tile_i_pos));
                }
            }

            if (GlobalConstants.SCREEN_WIDTH < GlobalConstants.SCREEN_HEIGHT)
            {
                #pragma warning disable CS0162 // Unreachable code detected
                for (int tile_i_pos = GlobalConstants.SCREEN_WIDTH - (GlobalConstants.SCREEN_WIDTH % this.tile_size); tile_i_pos < GlobalConstants.SCREEN_HEIGHT; tile_i_pos += this.tile_size)
                {
                    this.DrawLine(vertices, new Vector2(0, tile_i_pos), new Vector2(GlobalConstants.SCREEN_WIDTH, tile_i_pos));
                }
                #pragma warning restore CS0162 // Unreachable code detected
            }
        }

        private void DrawLine(VertexPositionColor[] vertices, Vector2 start, Vector2 end)
        {
            vertices[0].Position = new Vector3(start.X, start.Y, 0);
            vertices[1].Position = new Vector3(end.X, end.Y, 0);

            this.graphicsDevice.DrawUserPrimitives<VertexPositionColor>(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineStrip, vertices, 0, 1);
        }
    }
}
