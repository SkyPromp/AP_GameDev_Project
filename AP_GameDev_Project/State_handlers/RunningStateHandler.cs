using AP_GameDev_Project.Entities;
using AP_GameDev_Project.Entities.Collectables;
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
        private List<ACollectables> collectables;
        private MouseHandler mouseHandler;
        private RunningKeyboardEventHandler keyboardHandler;
        private ContentManager contentManager;
        private CollisionHandler collisionHandler;

        public RunningStateHandler()
        {
            this.contentManager = ContentManager.getInstance;
            this.collisionHandler = new CollisionHandler();
            this.current_room = this.contentManager.GetRooms[0];
            this.current_room.Center();
            this.tile_hitboxes = this.current_room.GetHitboxes((Byte tile) => { return tile > 1; });
            this.mouseHandler = MouseHandler.getInstance.Init();
            this.entities = new List<AEntity>();
            this.keyboardHandler = new RunningKeyboardEventHandler(this);
            this.collectables = new List<ACollectables>();

            this.entities.Add(new Player(this.current_room.GetPlayerSpawnpoint, contentManager, 5f));

            List <Rectangle> tiles = this.current_room.GetHitboxes((Byte tile) => { return tile == 1; });  // remove player spawnpoint tile (and the one above)

            int max_enemies = 4;
            tiles = this.SpawnEnemies(max_enemies, tiles);

            int max_collectables = 4;
            tiles = this.SpawnCollectables(max_collectables, tiles);
        }

        public void Init()
        {
            this.is_init = true;
            this.mouseHandler.LeftClickHook = () => { if (this.mouseHandler.IsOnScreen) this.Player.Attack(this.mouseHandler.MousePos); };
        }

        private List<Rectangle> SpawnEnemies(int enemy_amount, List<Rectangle> tiles)
        {
            Random random = new Random();
            enemy_amount = Math.Min(enemy_amount, tiles.Count);
            Vector2 enemy1_offset = new Enemy1(Vector2.Zero, contentManager, 0, 0).GetCenter;

            for (int i = 0; i < enemy_amount; i++)
            {
                Rectangle random_rect = tiles[random.Next(0, tiles.Count)];
                Enemy1 enemy1 = new Enemy1(random_rect.Center.ToVector2() - enemy1_offset, this.contentManager, 5f, 5);
                this.entities.Add(enemy1);
                tiles.Remove(random_rect);
            }

            return tiles;
        }

        private List<Rectangle> SpawnCollectables(int collectable_amount, List<Rectangle> tiles)
        {
            Random random = new Random();
            collectable_amount = Math.Min(collectable_amount, tiles.Count);
            Vector2 collectable_offset = new HeartCollectable(Vector2.Zero, this.contentManager).GetCenter;

            for (int i = 0; i < collectable_amount; i++)
            {
                Rectangle random_rect = tiles[random.Next(0, tiles.Count)];
                ACollectables collectable = new HeartCollectable(random_rect.Center.ToVector2() - collectable_offset, this.contentManager);
                this.collectables.Add(collectable);
                tiles.Remove(random_rect);
            }

            return tiles;
        }

        public void Update(GameTime gameTime)
        {
            this.mouseHandler.Update();
            this.keyboardHandler.Update(gameTime);
            foreach (ACollectables collectable in this.collectables) collectable.Update(gameTime);
            this.collisionHandler.HandleCollision(gameTime, this.Player, this.entities, this.collectables, this.tile_hitboxes, this.mouseHandler);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            current_room.Draw(spriteBatch);
            foreach (ACollectables collectable in this.collectables) collectable.Draw(spriteBatch);
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
    }
}
