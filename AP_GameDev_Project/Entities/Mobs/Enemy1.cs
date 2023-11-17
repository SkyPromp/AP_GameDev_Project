﻿using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace AP_GameDev_Project.Entities.Mobs
{
    internal class Enemy1 : AEntity
    {
        public Enemy1(Vector2 position, ContentManager contentManager, float max_speed, int base_health, float speed_damping_factor = 0.8f) :
            base(position, max_speed, new Rectangle(22, 10, 17, 43), 10f, 1f, contentManager.GetAnimations["ENEMY1_STANDSTILL"], base_health: base_health, speed_damping_factor: speed_damping_factor)
        {
        }

        public override void Update(GameTime gameTime, Vector2 player_center)
        {
            Attack(player_center);  // Add condition
            base.SpeedUp(Vector2.Normalize(player_center - GetCenter));
            base.Update(gameTime, player_center);
        }

        public override void Attack(Vector2 player_center)
        {
            if (bullet_cooldown <= 0)  // TODO: if no walls using ray marching
            {
                Vector2 enemy_center = GetCenter;
                Vector2 weapon_offset = new Vector2(Position.X < player_center.X ? -19.5f : 19.5f, -7);
                Vector2 bullet_position = weapon_offset + enemy_center;
                Vector2 angle = Vector2.Normalize(player_center - bullet_position);    // TODO: correct bullet_position to the center of the bullet?

                bullets.Add(new Bullet(bullet_position, angle * bullet_speed));
                bullet_cooldown = bullet_max_cooldown;
            }
        }
    }
}