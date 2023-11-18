using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AP_GameDev_Project.Entities
{
    internal class HitboxDrawer
    {
        // Singleton vars
        private volatile static HitboxDrawer instance;
        private static object syncRoot = new object();

        private GraphicsDevice graphicsDevice;
        private BasicEffect basicEffect;

        private HitboxDrawer() { }

        public static HitboxDrawer getInstance
        {
            get
            {
                if (HitboxDrawer.instance == null)
                {
                    lock (HitboxDrawer.syncRoot)
                    {
                        if (HitboxDrawer.instance == null) HitboxDrawer.instance = new HitboxDrawer();
                    }
                }

                return HitboxDrawer.instance;
            }
        }

        public HitboxDrawer Init(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            this.basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
            (0, graphicsDevice.Viewport.Width,     // left, right
            graphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1);

            return this;
        }

        public void DrawHitbox(Rectangle hitbox, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin();

            this.basicEffect.CurrentTechnique.Passes[0].Apply();

            VertexPositionColor[] vertices = new VertexPositionColor[5];
            vertices[0].Position = new Vector3(hitbox.Left, hitbox.Top, 0);
            vertices[0].Color = Color.Red;
            vertices[1].Position = new Vector3(hitbox.Right, hitbox.Top, 0);
            vertices[1].Color = Color.Red;
            vertices[2].Position = new Vector3(hitbox.Right, hitbox.Bottom, 0);
            vertices[2].Color = Color.Red;
            vertices[3].Position = new Vector3(hitbox.Left, hitbox.Bottom, 0);
            vertices[3].Color = Color.Red;
            vertices[4] = vertices[0];

            this.graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, 4);
        }
    }
}
