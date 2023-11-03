using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace AP_GameDev_Project.Entities
{
    internal class Player: AEntity
    {
        public Player(Vector2 position, Animate stand_animation, float max_speed, float speed_damping_factor=0.95f): base(position, stand_animation, max_speed, new Rectangle(56, 35, 35, 142), speed_damping_factor)
        {

        }
    }
}
