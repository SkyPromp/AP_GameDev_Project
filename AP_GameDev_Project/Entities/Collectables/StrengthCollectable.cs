using AP_GameDev_Project.Entities.Mobs;
using AP_GameDev_Project.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Entities.Collectables
{
    internal class StrengthCollectable: ACollectables
    {
        private readonly int strengthen_amount;

        public StrengthCollectable(Vector2 position, ContentManager contentManager) : base(position, contentManager.GetAnimations["STRENGTH_COLLECTABLE"], new Hitbox().AddChild(new Rectangle(16, 11, 32, 35)))
        {
            this.strengthen_amount = 10;
        }

        public override void OnCollision(Player player)
        {
            player.AddStrength(this.strengthen_amount);
            base.OnCollision(player);
        }
    }
}
