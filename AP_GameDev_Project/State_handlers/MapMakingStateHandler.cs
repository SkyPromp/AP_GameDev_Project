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
            for (int x = this.tile_size; x < GlobalConstants.SCREEN_WIDTH; x += this.tile_size)
            {
                basicEffect.CurrentTechnique.Passes[0].Apply();
                var vertices = new VertexPositionColor[2];
                vertices[0].Position = new Vector3(x, 0, 0);
                vertices[0].Color = Color.Black;
                vertices[1].Position = new Vector3(x, GlobalConstants.SCREEN_HEIGHT, 0);
                vertices[1].Color = Color.Black;

                this.graphicsDevice.DrawUserPrimitives<VertexPositionColor>(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineStrip, vertices, 0, 1);
            }

            for (int y = this.tile_size; y < GlobalConstants.SCREEN_HEIGHT; y += this.tile_size)
            {
                basicEffect.CurrentTechnique.Passes[0].Apply();
                var vertices = new VertexPositionColor[2];
                vertices[0].Position = new Vector3(0, y, 0);
                vertices[0].Color = Color.Black;
                vertices[1].Position = new Vector3(GlobalConstants.SCREEN_WIDTH, y, 0);
                vertices[1].Color = Color.Black;

                this.graphicsDevice.DrawUserPrimitives<VertexPositionColor>(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineStrip, vertices, 0, 1);
            }
            
            // MERGE INTO 1 FOR LOOP WITH Math.Min(width, height) and an if
        }
    }
}

