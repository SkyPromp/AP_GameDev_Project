using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Entities
{
    internal class Player
    {
        private readonly Animate stand_animation;
        private Vector2 position;
        public readonly float max_speed;
        private Vector2 speed;
        private float speed_damping_factor;

        public Player(Animate stand_animation, float max_speed, float speed_damping_factor = 0.95f)
        {
            this.stand_animation = stand_animation;
            speed = Vector2.Zero;
            this.max_speed = max_speed;
            this.speed_damping_factor = speed_damping_factor;
        }

        public void Update(GameTime gameTime)
        {
            speed *= speed_damping_factor;
            stand_animation.Update(gameTime);
            position += speed;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            stand_animation.Draw(spriteBatch, position);
        }

        public void SpeedUp(Vector2 add_speed)
        {
            speed += add_speed;

            if (speed.Length() >= max_speed)
            {
                speed = Vector2.Normalize(speed) * max_speed;
            }
        }
    }
}
