﻿using AP_GameDev_Project.Input_devices;
using AP_GameDev_Project.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private bool show_current_brush;
        private SpriteFont font;
        private Room room;
        private MapMakingKeyboardEventHandler keyboardHandler;
        private ContentManager contentManager;

        private int player_spawnpoint;

        // DRAW VERTECES VARIABLES
        private GraphicsDevice graphicsDevice;
        private BasicEffect basicEffect;

        public MapMakingStateHandler(GraphicsDevice graphicsDevice, int tile_size=64) {
            this.contentManager = ContentManager.getInstance;
            this.is_init = false;
            this.tile_size = tile_size;
            this.tiles = new List<Byte>();
            this.tilemap = this.contentManager.GetTextures["TILEMAP"];
            this.font = this.contentManager.Font;
            this.keyboardHandler = new MapMakingKeyboardEventHandler(this);
            this.player_spawnpoint = -1;

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
            this.show_current_brush = true;
            this.player_spawnpoint = -1;

            // FIX Without Math.Ceiling
            UInt16 room_width = (UInt16) Math.Ceiling((double)GlobalConstants.SCREEN_WIDTH / this.tile_size);
            UInt16 room_height = (UInt16) Math.Ceiling((double)GlobalConstants.SCREEN_HEIGHT / this.tile_size);

            int tile_amount = (int)Math.Ceiling((double)room_width * (double)room_height);
            this.tiles = Enumerable.Repeat((Byte) 0, tile_amount).ToList();
            this.room = new Room(this.tiles, room_width);

            this.mouseHandler = MouseHandler.getInstance.Init();
            this.mouseHandler.LeftClickHook = () => { this.PlaceTile(this, this.current_tile_brush); };
            this.mouseHandler.RightClickHook = () => { this.PlaceTile(this, 0); };
            this.mouseHandler.MiddleClickHook = () => { this.PlaceSpawnpoint(this); };
        }

        public void Update(GameTime gameTime)
        {
            this.mouseHandler.Update();
            this.keyboardHandler.Update(gameTime);
        }

        private void PlaceTile(MapMakingStateHandler map_maker, Byte brush)
        {
            if (this.mouseHandler.IsOnScreen)
            {
                int tile_row = (int)map_maker.mouseHandler.MousePos.Y / map_maker.tile_size;
                int tile_column = (int)map_maker.mouseHandler.MousePos.X / map_maker.tile_size;
                int tile_index = tile_column + tile_row * GlobalConstants.SCREEN_WIDTH / map_maker.tile_size;

                Debug.Assert((tile_column + 1) * (tile_row + 1) <= map_maker.tiles.Count,
                    message: string.Format("Error: Tile X:{0} Y:{1} is out of scope {2}", tile_column, tile_row, map_maker.tiles.Count));

                map_maker.tiles[tile_index] = brush;
            }
        }

        private void PlaceSpawnpoint(MapMakingStateHandler map_maker)
        {
            int tile_row = (int)map_maker.mouseHandler.MousePos.Y / map_maker.tile_size;
            int tile_column = (int)map_maker.mouseHandler.MousePos.X / map_maker.tile_size;
            int tile_index = tile_column + tile_row * GlobalConstants.SCREEN_WIDTH / map_maker.tile_size;

            this.player_spawnpoint = tile_index;
        }

        private Vector2 IndexToXY(int index)
        {
            int width = GlobalConstants.SCREEN_WIDTH / this.tile_size;
            return new Vector2(index % width, index / width);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.DrawGrid(spriteBatch);
            if (this.room != null) this.room.Draw(spriteBatch);

            if (this.player_spawnpoint != -1)
            {
                Vector2 tile_center_coords = IndexToXY(player_spawnpoint) * this.tile_size + new Vector2(32, 32);
                Rectangle sprite_rectangle = new Rectangle(0, 0, 128, 192);  // DO MORE DYNAMICALLY
                spriteBatch.Draw(this.contentManager.GetTextures["PLAYER_STANDSTILL"], tile_center_coords - new Vector2(sprite_rectangle.Width / 2, 116), sprite_rectangle, Color.White);
            }

            if (this.show_current_brush)
            {
                if(this.current_tile_brush != (Byte)0)
                {
                    spriteBatch.Draw(
                    texture: this.tilemap,
                    position: new Vector2(this.tile_size / 2, this.tile_size / 2),
                    sourceRectangle: new Rectangle(0, (this.current_tile_brush - 1) * this.tile_size, this.tile_size, this.tile_size),
                    color: Color.White
                    );
                }

                Vector2 half_text_size = this.font.MeasureString(this.current_tile_brush.ToString()) / 2;
                spriteBatch.DrawString(this.font, this.current_tile_brush.ToString(), new Vector2(64 - half_text_size.X, 64 - half_text_size.Y), Color.White);
            }
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

        public void ChangeBrush(Byte delta)
        {
            this.current_tile_brush += delta;
        }

        public void ToggleFont()
        {
            this.show_current_brush = !this.show_current_brush;
        }

        public void SaveFile()
        {
            if(this.player_spawnpoint == -1) throw new InvalidOperationException("There is no spawnpoint for the player set, or it is outside of the room");  // Find better solution (draw text to screen)
            List<Byte> trimmed_tiles;
            int width;
            int spawnpoint;
            (trimmed_tiles, width, spawnpoint) = (new Trimmer()).GetTrimmedRoom(new List<Byte>(this.tiles), this.tile_size, this.player_spawnpoint);

            if (spawnpoint == -1) throw new InvalidOperationException("The spawnpoint is outside of the room");  // Find better solution (draw text to screen)

            // Write to file
            Byte[] spawnpoint_bytes = BitConverter.GetBytes(spawnpoint);
            trimmed_tiles.Insert(0, spawnpoint_bytes[0]);
            trimmed_tiles.Insert(0, spawnpoint_bytes[1]);

            Byte[] header = BitConverter.GetBytes(width);

            trimmed_tiles.Insert(0, header[0]);
            trimmed_tiles.Insert(0, header[1]);

            FileSaver.SaveFile(trimmed_tiles);
        }
    }
}
