using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;


namespace AP_GameDev_Project.Entities
{
    internal class Hitbox
    {
        private Hitbox parent;
        private List<Hitbox> children;

        private Rectangle hitbox;
        public Rectangle GetHitbox { get { return this.hitbox; } }

        public Hitbox(Rectangle hitbox, Hitbox parent=null)
        {
            this.hitbox = hitbox;
            this.parent = parent;
            this.children = new List<Hitbox>();
        }

        public void AddChild(Rectangle child_hitbox)
        {
            this.children.Add(new Hitbox(child_hitbox, this));
        }

        public void RemoveChild(Hitbox hitbox)
        {
            this.children.Remove(hitbox);
        }

        public bool DoesCollide(Rectangle other_hitbox)
        {
            if(this.children.Count == 0) return this.hitbox.Intersects(other_hitbox);

            return this.children.Any(child_hitbox => child_hitbox.DoesCollide(other_hitbox));
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.End();
            spriteBatch.Begin();

            GraphicsDevice graphicsDevice = spriteBatch.GraphicsDevice;

            BasicEffect basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
            (0, graphicsDevice.Viewport.Width,     // left, right
            graphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1);

            this.DrawHitboxes(graphicsDevice, basicEffect, position);
        }

        private void DrawHitboxes(GraphicsDevice graphicsDevice, BasicEffect basicEffect, Vector2 position)
        {
            if(this.children.Count == 0)
            {
                this.DrawHitbox(graphicsDevice, basicEffect, position);
            }
            else
            {
                foreach (Hitbox hitbox in this.children)
                {
                    hitbox.DrawHitboxes(graphicsDevice, basicEffect, position);
                }
            }

       
        }

        private void DrawHitbox(GraphicsDevice graphicsDevice, BasicEffect basicEffect, Vector2 position)
        {


                basicEffect.CurrentTechnique.Passes[0].Apply();

                VertexPositionColor[] vertices = new VertexPositionColor[5];
                vertices[0].Position = new Vector3(this.hitbox.Left + position.X, this.hitbox.Top + position.Y, 0);
                vertices[0].Color = Color.Red;
                vertices[1].Position = new Vector3(this.hitbox.Right + position.X, this.hitbox.Top + position.Y, 0);
                vertices[1].Color = Color.Red;
                vertices[2].Position = new Vector3(this.hitbox.Right + position.X, this.hitbox.Bottom + position.Y, 0);
                vertices[2].Color = Color.Red;
                vertices[3].Position = new Vector3(this.hitbox.Left + position.X, this.hitbox.Bottom + position.Y, 0);
                vertices[3].Color = Color.Red;
                vertices[4] = vertices[0];

                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, 4);
        }
    }
}
