using AP_GameDev_Project.Entities.Collectables;
using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Entities
{
    internal class CollisionHandler
    {
        private CollisionHelper collisionHelper;
        public CollisionHandler()
        {
            this.collisionHelper = new CollisionHelper();
        }

        public void HandleCollision(GameTime gameTime, Player player, List<AEntity> entities, List<ACollectables> collectables, List<Rectangle> tile_hitboxes, MouseHandler mouseHandler)
        {
            Vector2 player_center = player.GetCenter;
            bool is_player = true;

            foreach (AEntity entity in new List<AEntity>(entities))  // One big foreach, for performance reasons
            {
                entity.Update(gameTime, is_player ? mouseHandler.MousePos : player_center);
                List<Bullet> entity_bullets = new List<Bullet>(entity.Bullets);

                EECollision(entity, new List<AEntity>(entities));
                ETBCollision(entity, tile_hitboxes);

                if (is_player)
                {
                    is_player = false;
                    continue;
                }

                if (PbECollision(entity, player)) entities.Remove(entity);

                EbPCollision(entity, player);
            }

            PCCollision(player, collectables);
        }

        private void EECollision(AEntity entity, List<AEntity> entities)
        {
            foreach (AEntity entity2 in entities)
            {
                if ((entity2.GetHitbox.Center - entity.GetHitbox.Center).ToVector2().Length() > 1)
                    this.collisionHelper.HandleHardCollison(entity, entity2);
            }
        }

        private void ETBCollision(AEntity entity, List<Rectangle> tile_hitboxes)
        {
            List<Bullet> entity_bullets = new List<Bullet>(entity.Bullets);
            foreach (Rectangle hitbox in tile_hitboxes)
            {
                this.collisionHelper.HandleHardCollison(entity, hitbox);

                foreach (Bullet bullet in entity.Bullets)
                {
                    if (hitbox.Intersects(bullet.GetHitbox)) entity_bullets.Remove(bullet);
                }
            }

            entity.Bullets = entity_bullets;
        }

        private bool PbECollision(AEntity entity, Player player)
        {
            List<Bullet> player_bullets = new List<Bullet>(player.Bullets);
            bool remove_entity = false;

            foreach (Bullet bullet in player.Bullets)
            {
                if (bullet.GetHitbox.Intersects(entity.GetHitbox))
                {
                    int health = entity.DoDamage();
                    player_bullets.Remove(bullet);
                    remove_entity = health <= 0;
                }
            }

            player.Bullets = player_bullets;

            return remove_entity;
        }

        private void EbPCollision(AEntity entity, Player player)
        {
            List<Bullet> entity_bullets = new List<Bullet>(entity.Bullets);

            foreach (Bullet bullet in entity.Bullets)
            {
                if (bullet.GetHitbox.Intersects(player.GetHitbox))
                {
                    int health = player.DoDamage();
                    entity_bullets.Remove(bullet);
                    if (health <= 0)
                    {
                        //this.contentManager.GetSoundEffects["PLAYER_DEATH"].Play();
                        throw new NotImplementedException("The player has died, a game over screen has not been implemented yet.");
                    }
                }
            }

            entity.Bullets = entity_bullets;
        }

        private void PCCollision(Player player, List<ACollectables> collectables)
        {
            Rectangle player_hitbox = player.GetHitbox;

            foreach (ACollectables collectable in new List<ACollectables>(collectables))
            {
                if (player_hitbox.Intersects(collectable.GetHitbox))
                {
                    collectable.OnCollision(player);
                    collectables.Remove(collectable);
                }
            }
        }
    }
}
