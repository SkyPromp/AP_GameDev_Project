using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace AP_GameDev_Project.Entities.Mobs
{
    internal class Bullet
    {
        private Vector2 position;
        private readonly Vector2 speed;
        private readonly Rectangle normalized_hitbox;
        private readonly Texture2D texture;
        private readonly ContentManager contentManager;

        public Rectangle GetHitbox
        {
            get
            {
                return new Rectangle((int)(position.X + normalized_hitbox.X), (int)(position.Y + normalized_hitbox.Y), normalized_hitbox.Width, normalized_hitbox.Height);
            }
        }

        public Bullet(Vector2 position, Vector2 speed)
        {
            contentManager = ContentManager.getInstance;
            this.position = position;
            this.speed = speed;
            texture = contentManager.GetTextures["BULLET"];
            normalized_hitbox = new Rectangle(-2, -2, 4, 4);
        }

        public Bullet(Vector2 position, Vector2 speed, Bullet base_bullet)
        {
            this.position = position;
            this.speed = speed;
            texture = base_bullet.texture;
        }

        public void Update(GameTime gameTime)
        {
            position += speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
            texture: texture,
            position: position,  // The origin changes because of the rotation
            sourceRectangle: new Rectangle(4, 6, 9, 4),
            color: Color.White,
            rotation: (float)Math.Atan2(speed.Y, speed.X),
            origin: new Vector2(13 / 2f, 10 / 2f),
            scale: 1.0f,
            effects: SpriteEffects.None,
            layerDepth: 0
            );
        }
    }
}
