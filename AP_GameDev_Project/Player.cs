using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project
{
    internal class Player
    {
        private readonly Animate stand_animation;
        private Vector2 position;
        public readonly float max_speed;
        private Vector2 speed;
        private float speed_damping_factor;

        public Player(Animate stand_animation, float max_speed, float speed_damping_factor=0.95f) 
        { 
            this.stand_animation = stand_animation;
            this.speed = Vector2.Zero;
            this.max_speed = max_speed;
            this.speed_damping_factor = speed_damping_factor;
        }

        public void Update(GameTime gameTime)
        {
            this.speed *= this.speed_damping_factor;
            this.stand_animation.Update(gameTime);
            this.position += this.speed;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.stand_animation.Draw(spriteBatch, this.position);
        }

        public void SpeedUp(Vector2 add_speed)
        {
            this.speed += add_speed;

            if(this.speed.Length() >= this.max_speed)
            {
                this.speed = Vector2.Normalize(this.speed) * this.max_speed;
            }
        }
    }
}
