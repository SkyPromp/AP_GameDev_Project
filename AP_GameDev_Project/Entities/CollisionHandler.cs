using AP_GameDev_Project.Entities.Collectables;
using AP_GameDev_Project.Entities.Mobs;
using AP_GameDev_Project.Input_devices;
using AP_GameDev_Project.State_handlers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;


/*
 E:  Enemy
 P:  Player
 B:  Bullet
 C:  Collectable
 T:  Tile
 Pb: Player bullet
 Eb: Enemy bullet
 */

namespace AP_GameDev_Project.Entities
{
    internal class CollisionHandler
    {
        private HitboxCollisionHelper hitboxCollisionHelper;
        private ContentManager contentManager;
        private StateHandler stateHandler;
        public CollisionHandler()
        {
            this.contentManager = ContentManager.getInstance;
            this.hitboxCollisionHelper = new HitboxCollisionHelper();
            this.stateHandler = StateHandler.getInstance;
        }

        public List<AEntity> HandleCollision(GameTime gameTime, Player player, List<AEntity> entities, List<ACollectables> collectables, List<Rectangle> tile_hitboxes, MouseHandler mouseHandler)
        {
            foreach (AEntity entity in new List<AEntity>(entities))  // One big foreach, for performance reasons
            {
                EECollision(entity, entities);
                ETBCollision(entity, tile_hitboxes);

                if (entity is Player) continue;

                PbECollision(entity, player);
                EbPCollision(entity, player);
            }

            PCCollision(player, collectables);

            return entities;
        }

        private void EECollision(AEntity entity, List<AEntity> entities)
        {
            List<AEntity> entities_list = new List<AEntity>(entities);
            foreach (AEntity entity2 in entities)
            {
                if ((entity2.GetHitbox.Center - entity.GetHitbox.Center).ToVector2().Length() > 1)
                    this.hitboxCollisionHelper.HandleHardCollison(entity, entity2); // average movement out  
            }
        }

        private void ETBCollision(AEntity entity, List<Rectangle> tile_hitboxes)
        {
            List<Bullet> entity_bullets = new List<Bullet>(entity.Bullets);
            foreach (Rectangle hitbox in tile_hitboxes)
            {
                this.hitboxCollisionHelper.HandleHardCollison(entity, hitbox);

                foreach (Bullet bullet in entity.Bullets)
                {
                    if (!bullet.GetHitboxHitbox.DoesCollideR(hitbox).IsEmpty) entity_bullets.Remove(bullet);
                }
            }

            entity.Bullets = entity_bullets;
        }

        private void PbECollision(AEntity entity, Player player)
        {
            List<Bullet> player_bullets = new List<Bullet>(player.Bullets);
            Hitbox hitbox = entity.GetHitboxHitbox;

            foreach (Bullet bullet in player.Bullets)
            {
                if (!bullet.GetHitboxHitbox.DoesCollideR(hitbox).Item1.IsEmpty)
                {
                    entity.DoDamage(bullet.Damage);
                    player_bullets.Remove(bullet);
                }
            }

            player.Bullets = player_bullets;
        }

        private void EbPCollision(AEntity entity, Player player)
        {
            List<Bullet> entity_bullets = new List<Bullet>(entity.Bullets);
            Hitbox hitbox = player.GetHitboxHitbox;

            foreach (Bullet bullet in entity.Bullets)
            {
                if (!bullet.GetHitboxHitbox.DoesCollideR(hitbox).Item1.IsEmpty)
                {
                    player.DoDamage(bullet.Damage);
                    entity_bullets.Remove(bullet);
                }
            }

            entity.Bullets = entity_bullets;
        }

        private void PCCollision(Player player, List<ACollectables> collectables)
        {
            Hitbox player_hitbox = player.GetHitboxHitbox;

            foreach (ACollectables collectable in new List<ACollectables>(collectables))
            {
                if (!collectable.GetHitboxHitbox.DoesCollideR(player_hitbox).Item1.IsEmpty)
                {
                    collectable.OnCollision(player);
                    collectables.Remove(collectable);
                }
            }
        }
    }
}
