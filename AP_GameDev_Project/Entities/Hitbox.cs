using Microsoft.Xna.Framework;
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
        }

        public void AddChild(Rectangle child_hitbox)
        {
            this.children.Add(new Hitbox(child_hitbox, this));
        }

        public void RemoveChild(Hitbox hitbox)
        {
            this.children.Remove(hitbox);
        }

        public bool DoesCollide(Rectangle other_hitbox)
        {
            if(this.children.Count == 0) return this.hitbox.Intersects(other_hitbox);

            return this.children.Any(child_hitbox => child_hitbox.DoesCollide(other_hitbox));
        }
    }
}
