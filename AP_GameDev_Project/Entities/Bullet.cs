using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Entities
{
    internal class Bullet
    {
        private Vector2 position;
        private readonly Vector2 speed;
        public Bullet(Vector2 position, Vector2 speed) 
        { 
            this.position = position;
            this.speed = speed;
        }

        public void Update()
        {
            this.position += this.speed;
        }

        public void Draw()
        {

        }
    }
}
