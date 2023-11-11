using AP_GameDev_Project.Entities;
using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace AP_GameDev_Project.State_handlers
{
    internal class RunningStateHandler : IStateHandler
    {
        private Room current_room;
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
            this.mouseHandler = MouseHandler.getInstance;
            this.entities = new List<AEntity>();
            this.keyboardHandler = new RunningKeyboardEventHandler(this);

            // TEST (REMOVE)
            Player player = new Player(new Vector2(180, 180), 5f);
            this.entities.Add(player);
            Enemy1 enemy1 = new Enemy1(new Vector2(300, 300), 5f, 5);
            this.entities.Add(enemy1);
            // END TEST
        }

        public void Init()
        {
            this.is_init = true;
            this.mouseHandler.LeftClickHook = () => {
                if (new Rectangle(0, 0, GlobalConstants.SCREEN_WIDTH, GlobalConstants.SCREEN_HEIGHT).Contains(mouseHandler.MousePos))
                {
                    this.Player.Attack();
                    this.contentManager.GetSoundEffects["BULLET_SHOOT"].Play();
                }
            };
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
                entity.Update(gameTime);
                List<Bullet> entity_bullets = new List<Bullet>(entity.Bullets);

                foreach (Rectangle hitbox in this.current_room.GetHitboxes())
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
