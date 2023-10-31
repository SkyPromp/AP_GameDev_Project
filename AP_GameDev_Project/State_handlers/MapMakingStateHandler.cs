using AP_GameDev_Project.Input_devices;
using AP_GameDev_Project.Utils;
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
        private bool show_current_brush;
        private double toggle_font_cooldown;
        private double change_brush_cooldown;
        private SpriteFont font;
        private Room room;

        // DRAW VERTECES VARIABLES
        private GraphicsDevice graphicsDevice;
        private BasicEffect basicEffect;

        public MapMakingStateHandler(GraphicsDevice graphicsDevice, Texture2D tilemap, SpriteFont font, int tile_size=64) {
            this.is_init = false;
            this.mouseHandler = MouseHandler.getInstance;
            this.tile_size = tile_size;
            this.tiles = new List<Byte>();
            this.tilemap = tilemap;
            this.font = font;


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
            this.toggle_font_cooldown = 0;
            this.change_brush_cooldown = 0;

            // FIX Without Math.Ceiling
            Int16 room_width = (Int16) Math.Ceiling((double)GlobalConstants.SCREEN_WIDTH / this.tile_size);
            Int16 room_height = (Int16) Math.Ceiling((double)GlobalConstants.SCREEN_HEIGHT / this.tile_size);

            int tile_amount = (int)Math.Ceiling((double)room_width * (double)room_height);
            this.tiles = Enumerable.Repeat((Byte) 0, tile_amount).ToList();

            this.room = new Room(this.tilemap, this.tiles, room_width);

            this.mouseHandler.LeftClickHook = () => { this.PlaceTile(this); };
            this.mouseHandler.RightClickHook = () => { 
                Byte old_brush = this.current_tile_brush;
                this.current_tile_brush = 0;
                this.PlaceTile(this); 
                this.current_tile_brush = old_brush;
            };
        }

        public void Update(GameTime gameTime)
        {
            this.mouseHandler.Update();
            if(this.toggle_font_cooldown >= 0) this.toggle_font_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            if (this.change_brush_cooldown >= 0) this.change_brush_cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && this.change_brush_cooldown <= 0)
            {
                this.current_tile_brush++;
                this.change_brush_cooldown = 0.5;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Down) && this.change_brush_cooldown <= 0)
            {
                this.current_tile_brush--;
                this.change_brush_cooldown = 0.5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.B) && this.toggle_font_cooldown <= 0) 
            {
                this.show_current_brush = !this.show_current_brush;
                this.toggle_font_cooldown = 0.5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                List<Byte> trimmed_tiles;
                int width;
                (trimmed_tiles, width) = Trimmer.GetTrimmedRoom(new List<Byte>(this.tiles), this.tile_size);

                // Write to file
                Byte[] header = BitConverter.GetBytes(width);

                trimmed_tiles.Insert(0, header[0]);
                trimmed_tiles.Insert(0, header[1]);

                FileSaver.SaveFile(trimmed_tiles);
            }
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

        public void Draw(SpriteBatch spriteBatch)
        {
            this.DrawGrid(spriteBatch);
            if (this.room != null) this.room.Draw(spriteBatch);

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
    }
}
