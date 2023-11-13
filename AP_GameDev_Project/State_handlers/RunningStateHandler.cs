using AP_GameDev_Project.Entities;
using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AP_GameDev_Project.State_handlers
{
    internal class RunningStateHandler : IStateHandler
    {
        private Room current_room;
        private List<Rectangle> tile_hitboxes;
        private bool is_init;
        public bool IsInit { get { return this.is_init; } }
        private List<AEntity> entities;
        private Player Player { get { return (Player)this.entities[0]; } set { this.entities[0] = value; } }
        private MouseHandler mouseHandler;
        private RunningKeyboardEventHandler keyboardHandler;
        private ContentManager contentManager;

        public RunningStateHandler()
        {
            this.contentManager = ContentManager.getInstance;
            this.current_room = new Room("Rooms\\BigRoom.room");
            this.current_room.Center();
            this.tile_hitboxes = this.current_room.GetHitboxes((Byte tile) => { return tile > 1; });
            this.mouseHandler = MouseHandler.getInstance.Init();
            this.entities = new List<AEntity>();
            this.keyboardHandler = new RunningKeyboardEventHandler(this);
            this.entities.Add(new Player(this.current_room.GetPlayerSpawnpoint, contentManager, 5f));

            // TEST (REMOVE)
            Vector2 enemy1_offset = new Enemy1(Vector2.Zero, contentManager, 0, 0).GetCenter;

            List <Rectangle> tiles = this.current_room.GetHitboxes((Byte tile) => { return tile == 1; });  // remove player spawnpoint

            int max_enemies = 7;
            int enemy_amount = Math.Min(max_enemies, tiles.Count);
            Random random = new Random();

            for (int i = 0; i < enemy_amount; i++)
            {
                Rectangle random_rect = tiles[random.Next(0, tiles.Count)];
                Enemy1 enemy1 = new Enemy1(random_rect.Center.ToVector2() - enemy1_offset, contentManager,5f, 5);
                this.entities.Add(enemy1);
                tiles.Remove(random_rect);
            }
            // END TEST
        }

        public void Init()
        {
            this.is_init = true;
            this.mouseHandler.LeftClickHook = () => { if (this.mouseHandler.IsOnScreen) this.Player.Attack(); };
        }

        public void Update(GameTime gameTime)
        {
            this.mouseHandler.Update();
            this.keyboardHandler.Update(gameTime);
            this.HandleCollision(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            current_room.Draw(spriteBatch);

            foreach (AEntity entity in this.entities) entity.Draw(spriteBatch);
        }

        public void MovePlayer(Vector2 speed)
        {
            this.Player.SpeedUp(speed);
        }

        public void ToggleDebug()
        {
            foreach(AEntity entity in this.entities) entity.show_hitbox = !entity.show_hitbox;
        }

        private void HandleCollision(GameTime gameTime)
        {
            Vector2 player_center = this.Player.GetCenter;
            List<AEntity> entities_new = new List<AEntity>(this.entities);
            bool is_player = true;

            foreach (AEntity entity in this.entities)  // One big foreach, for performance reasons
            {
                entity.Update(gameTime, is_player ? this.mouseHandler.MousePos : player_center);
                List<Bullet> entity_bullets = new List<Bullet>(entity.Bullets);

                foreach (Rectangle hitbox in this.tile_hitboxes)
                {
                    if (hitbox.Intersects(entity.GetHitbox)) entity.HandleCollison(hitbox);

                    foreach (Bullet bullet in entity.Bullets)
                    {
                        if (hitbox.Intersects(bullet.GetHitbox)) entity_bullets.Remove(bullet);
                    }
                }

                entity.Bullets = entity_bullets;

                if (is_player)
                {
                    is_player = false;
                    continue;
                }

                entity.Attack(player_center);  // Add condition

                // Check if player hits enemies
                List<Bullet> player_bullets = new List<Bullet>(this.Player.Bullets);

                foreach (Bullet bullet in this.Player.Bullets)
                {
                    if (bullet.GetHitbox.Intersects(entity.GetHitbox))
                    {
                        int health = entity.DoDamage();
                        player_bullets.Remove(bullet);
                        if (health <= 0) entities_new.Remove(entity);
                    }
                }

                this.Player.Bullets = player_bullets;

                // Check if enemies hit player
                entity_bullets = new List<Bullet>(entity.Bullets);

                foreach (Bullet bullet in entity.Bullets)
                {
                    if (bullet.GetHitbox.Intersects(this.Player.GetHitbox))
                    {
                        int health = this.Player.DoDamage();
                        entity_bullets.Remove(bullet);
                        if (health <= 0)
                        {
                            this.contentManager.GetSoundEffects["PLAYER_DEATH"].Play();
                            throw new NotImplementedException("The player has died, a game over screen has not been implemented yet.");
                        }
                    }
                }

                entity.Bullets = entity_bullets;
            }

            this.entities = entities_new;
        }
    }
}
