using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;


namespace AP_GameDev_Project.State_handlers
{
    internal class MapMakingStateHandler : IStateHandler
    {
        private MouseHandler mouseHandler;
        private bool is_init;
        public bool IsInit { get; }
        private int tile_size;

        // DRAW VERTECES VARIABLES
        private GraphicsDevice graphicsDevice;
        private BasicEffect basicEffect;

        public MapMakingStateHandler(GraphicsDevice graphicsDevice, int tile_size=64) {
            this.mouseHandler = MouseHandler.getInstance;
            this.tile_size = tile_size;

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
            mouseHandler.LeftClickHook = () => { Debug.WriteLine("MapMaker Left"); };
            mouseHandler.RightClickHook = () => { Debug.WriteLine("MapMaker Right"); };
        }

        public void Update(GameTime gameTime)
        {
            mouseHandler.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.DrawGrid(spriteBatch);
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
                for (int tile_i_pos = GlobalConstants.SCREEN_WIDTH - (GlobalConstants.SCREEN_WIDTH % this.tile_size); tile_i_pos < GlobalConstants.SCREEN_HEIGHT; tile_i_pos += this.tile_size)
                {
                    this.DrawLine(vertices, new Vector2(0, tile_i_pos), new Vector2(GlobalConstants.SCREEN_WIDTH, tile_i_pos));
                }
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
