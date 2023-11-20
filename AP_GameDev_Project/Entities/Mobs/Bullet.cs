using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;


namespace AP_GameDev_Project.Entities.Mobs
{
    internal class Bullet
    {
        private Vector2 position;
        private readonly Vector2 speed;
        private readonly Hitbox hitbox;
        public Hitbox GetHitboxHitbox { get { return this.hitbox; } }
        private readonly Rectangle normalized_hitbox;
        private readonly Texture2D texture;
        public bool show_hitbox;

        public Rectangle GetHitbox
        {
            get
            {
                return new Rectangle((int)(this.position.X + this.normalized_hitbox.X), (int)(this.position.Y + this.normalized_hitbox.Y), this.normalized_hitbox.Width, this.normalized_hitbox.Height);
            }
        }

        public Bullet(Vector2 position, Vector2 speed)
        {
            ContentManager contentManager = ContentManager.getInstance;
            this.position = position;
            this.speed = speed;
            this.texture = contentManager.GetTextures["BULLET"];
            this.hitbox = new Hitbox().AddChild(new Rectangle(-2, -2, 4, 4));
            this.show_hitbox = false;
        }

        public void Update(GameTime gameTime)
        {
            this.position += this.speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
            texture: this.texture,
            position: this.position,  // The origin changes because of the rotation
            sourceRectangle: new Rectangle(4, 6, 9, 4),
            color: Color.White,
            rotation: (float)Math.Atan2(this.speed.Y, this.speed.X),
            origin: new Vector2(13 / 2f, 10 / 2f),
            scale: 1.0f,
            effects: SpriteEffects.None,
            layerDepth: 0
            );

            if (this.show_hitbox) { this.hitbox.Draw(spriteBatch); }
        }
    }
}
