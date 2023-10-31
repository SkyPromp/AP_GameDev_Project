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

        public Player(Animate stand_animation) 
        { 
            this.stand_animation = stand_animation;
        }

        public void Update(GameTime gameTime)
        {
            this.stand_animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.stand_animation.Draw(spriteBatch, this.position);
        }
    }
}
