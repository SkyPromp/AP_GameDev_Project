using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;


namespace AP_GameDev_Project.Entities
{
    internal class Hitbox
    {
        private Hitbox parent;
        private List<Hitbox> children;

        private Rectangle normalised_hitbox;
        private Rectangle hitbox;
        public Rectangle GetHitbox { get { return this.hitbox; } }

        private Vector2 position;
        public Vector2 Position { set {
                this.UpdatePosition(value);
            } }

        public Hitbox()
        {
            this.normalised_hitbox = Rectangle.Empty;
            this.children = new List<Hitbox>();
            this.parent = null;
            this.position = Vector2.Zero;
        }

        private Hitbox(Rectangle normalised_hitbox, Hitbox parent=null)
        {
            this.normalised_hitbox = normalised_hitbox;
            this.parent = parent;
            this.children = new List<Hitbox>();
            this.position = Vector2.Zero;
        }

        public Hitbox AddChild(Rectangle child_normalised_hitbox)
        {
            Hitbox child = new Hitbox(child_normalised_hitbox, this);
            child.UpdatePosition(this.position);
            this.children.Add(child);
            this.UpdateParentHitbox(child_normalised_hitbox);

            return this;
        }

        public Hitbox RemoveChild(Hitbox hitbox)  // TODO Auto adjust hitboxes
        {
            this.children.Remove(hitbox);

            return this;
        }

        private void UpdatePosition(Vector2 position)
        {
            this.position = position;
            this.hitbox = new Rectangle(this.normalised_hitbox.X + (int)this.position.X, this.normalised_hitbox.Y + (int)this.position.Y, this.normalised_hitbox.Width, this.normalised_hitbox.Height);
            foreach (Hitbox child in this.children) child.UpdatePosition(position);
        }

        private void UpdateParentHitbox(Rectangle child_normalised_hitbox)
        {
            if (this.normalised_hitbox == Rectangle.Empty) this.normalised_hitbox = child_normalised_hitbox;
            else
            {
                if (this.normalised_hitbox.Left > child_normalised_hitbox.Left)
                {
                    this.normalised_hitbox = new Rectangle(child_normalised_hitbox.Left, this.normalised_hitbox.Top, this.normalised_hitbox.Right - child_normalised_hitbox.Left, this.normalised_hitbox.Height);
                }

                if (this.normalised_hitbox.Right < child_normalised_hitbox.Right)
                {
                    this.normalised_hitbox = new Rectangle(this.normalised_hitbox.Left, this.normalised_hitbox.Top, child_normalised_hitbox.Right - this.normalised_hitbox.Left, this.normalised_hitbox.Height);
                }

                if (this.normalised_hitbox.Top > child_normalised_hitbox.Top)
                {
                    this.normalised_hitbox = new Rectangle(this.normalised_hitbox.Left, child_normalised_hitbox.Top, this.normalised_hitbox.Width, this.normalised_hitbox.Bottom - child_normalised_hitbox.Top);
                }

                if (this.normalised_hitbox.Bottom < child_normalised_hitbox.Bottom)
                {
                    this.normalised_hitbox = new Rectangle(this.normalised_hitbox.Left, this.normalised_hitbox.Top, this.normalised_hitbox.Width, child_normalised_hitbox.Bottom - this.normalised_hitbox.Top);
                }
            }

            if (this.parent != null) this.parent.UpdateParentHitbox(this.normalised_hitbox);
        }

        public (Rectangle, Rectangle) DoesCollideR(Hitbox other_hitbox)
        {
            if (this.children.Count == 0)
            {
                Rectangle other_rect = other_hitbox.DoesCollideR(this.hitbox);
                if (other_rect != Rectangle.Empty) return (this.hitbox, other_rect);
                else return (Rectangle.Empty, Rectangle.Empty);
            }

            if (!this.hitbox.Intersects(other_hitbox.hitbox)) return (Rectangle.Empty, Rectangle.Empty);  // For performance

            foreach (Hitbox child in this.children)
            {
                (Rectangle collision_box, Rectangle other_box) = child.DoesCollideR(other_hitbox);
                if (collision_box != Rectangle.Empty) return (collision_box, other_box);
            }


            return (Rectangle.Empty, Rectangle.Empty);
        }

        public Rectangle DoesCollideR(Rectangle other_hitbox)
        {
            if (this.children.Count == 0)
            {
                if (this.hitbox.Intersects(other_hitbox)) return this.hitbox;
                else return Rectangle.Empty;
            }

            if (!this.hitbox.Intersects(other_hitbox)) return Rectangle.Empty;  // For performance

            foreach (Hitbox child in this.children)
            {
                Rectangle collision_box = child.DoesCollideR(other_hitbox); 
                if (collision_box != Rectangle.Empty) return collision_box;
            }

            return Rectangle.Empty;
        }

        /*public bool DoesCollideB(Hitbox other_hitbox)
        {
            if(other_hitbox.children.Count == 0) return this.DoesCollideB(other_hitbox.hitbox);
            
            return other_hitbox.children.Any(child => this.DoesCollideB(child));
        }

        public bool DoesCollideB(Rectangle other_hitbox)
        {
            if(this.children.Count == 0) return this.hitbox.Intersects(other_hitbox);

            return this.children.Any(child_hitbox => child_hitbox.DoesCollide(other_hitbox));
        }*/

        public void Draw(SpriteBatch spriteBatch)
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

            this.DrawHitboxes(graphicsDevice, basicEffect);
        }

        private void DrawHitboxes(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            if(this.children.Count == 0)
            {
                this.DrawHitbox(graphicsDevice, basicEffect);
            }
            else
            {
                foreach (Hitbox hitbox in this.children)
                {
                    hitbox.DrawHitboxes(graphicsDevice, basicEffect);
                }
            }
        }

        private void DrawHitbox(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
                basicEffect.CurrentTechnique.Passes[0].Apply();

                VertexPositionColor[] vertices = new VertexPositionColor[5];
                vertices[0].Position = new Vector3(this.hitbox.Left, this.hitbox.Top, 0);
                vertices[0].Color = Color.Red;
                vertices[1].Position = new Vector3(this.hitbox.Right, this.hitbox.Top, 0);
                vertices[1].Color = Color.Red;
                vertices[2].Position = new Vector3(this.hitbox.Right, this.hitbox.Bottom, 0);
                vertices[2].Color = Color.Red;
                vertices[3].Position = new Vector3(this.hitbox.Left, this.hitbox.Bottom, 0);
                vertices[3].Color = Color.Red;
                vertices[4] = vertices[0];

                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, 4);
        }
    }
}
