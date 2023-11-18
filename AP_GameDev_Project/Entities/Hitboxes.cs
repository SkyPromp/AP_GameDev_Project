using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AP_GameDev_Project.Entities
{
    internal class Hitboxes
    {
        private Hitboxes parent;
        private List<Hitboxes> children;

        private Rectangle hitbox;

        public Hitboxes(Rectangle hitbox, Hitboxes parent=null)
        {
            this.hitbox = hitbox;
            this.parent = parent;
        }

        public void AddChild(Rectangle child_hitbox)
        {
            this.children.Add(new Hitboxes(child_hitbox, this));
        }

        public void RemoveChild(Hitboxes hitbox)
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
