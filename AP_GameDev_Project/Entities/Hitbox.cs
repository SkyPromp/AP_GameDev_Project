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

        public Hitbox AddChild(Rectangle child_hitbox)
        {
            this.children.Add(new Hitbox(child_hitbox, this));
            if (this.parent != null) this.parent.UpdateParentHitbox(child_hitbox);

            return this;
        }

        public Hitbox RemoveChild(Hitbox hitbox)  // TODO Auto adjust hitboxes
        {
            this.children.Remove(hitbox);

            return this;
        }

        private void UpdateParentHitbox(Rectangle child_hitbox)
        {
            if (this.hitbox.Left > child_hitbox.Left)
            {
                this.hitbox = new Rectangle(child_hitbox.Left, this.hitbox.Top, this.hitbox.Right - child_hitbox.Left, this.hitbox.Height);
            }

            if (this.hitbox.Right < child_hitbox.Right)
            {
                this.hitbox = new Rectangle(this.hitbox.Left, this.hitbox.Top, child_hitbox.Right - this.hitbox.Left, this.hitbox.Height);
            }

            if (this.hitbox.Top > child_hitbox.Top)
            {
                this.hitbox = new Rectangle(this.hitbox.Left, child_hitbox.Top, this.hitbox.Width, this.hitbox.Bottom - child_hitbox.Top);
            }

            if (this.hitbox.Bottom < child_hitbox.Bottom)
            {
                this.hitbox = new Rectangle(this.hitbox.Left, this.hitbox.Top, this.hitbox.Width, child_hitbox.Bottom - this.hitbox.Top);
            }

            if (this.parent != null) this.parent.UpdateParentHitbox(this.hitbox);
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
