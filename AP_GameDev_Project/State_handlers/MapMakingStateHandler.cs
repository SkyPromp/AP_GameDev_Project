using AP_GameDev_Project.Input_devices;
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

            room = new Room(this.tilemap, this.tiles, room_width);
        }

        public void Update(GameTime gameTime)
        {
            mouseHandler.Update();
            // TODO: change current tile brush with keyboard
            // TODO: store to .room file with keyboard

            if (mouseHandler.MouseActive == 0) return;

            int tile_row = (int) mouseHandler.MousePos.Y / this.tile_size;
            int tile_column = (int) mouseHandler.MousePos.X / this.tile_size;
            int tile_index = tile_column + tile_row * GlobalConstants.SCREEN_WIDTH / this.tile_size;

            if ((mouseHandler.MouseActive & (short) MouseHandler.mouseEnum.LEFT_CLICK) == 1)
            {
                if (new Rectangle(0, 0, GlobalConstants.SCREEN_WIDTH, GlobalConstants.SCREEN_HEIGHT).Contains(mouseHandler.MousePos))
                {
                    Debug.Assert((tile_column + 1) * (tile_row + 1) <= this.tiles.Count, 
                        message: string.Format("Error: Tile X:{0} Y:{1} is out of scope {2}", tile_column, tile_row, this.tiles.Count));

                    this.tiles[tile_index] = this.current_tile_brush;
                }
            } else if ((mouseHandler.MouseActive & (short)MouseHandler.mouseEnum.RIGHT_CLICK) == 1)  // Else if because leftclick has priority
            {
                if (new Rectangle(0, 0, GlobalConstants.SCREEN_WIDTH, GlobalConstants.SCREEN_HEIGHT).Contains(mouseHandler.MousePos))
                { // Avoid duplicated code?
                    Debug.Assert((tile_column + 1) * (tile_row + 1) <= this.tiles.Count, 
                        message: string.Format("Error: Tile X:{0} Y:{1} is out of scope {2}", tile_column, tile_row, this.tiles.Count));

                    this.tiles[tile_index] = 0;
                }
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
