using AP_GameDev_Project.Entities;
using AP_GameDev_Project.Entities.Collectables;
using AP_GameDev_Project.Entities.Mobs;
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

            /*tiles = this.Spawn<Enemy1, AEntity>(4, tiles, this.entities, new object[] { 5f, 5, 0.8f});
            tiles = this.Spawn<HeartCollectable, ACollectables>(4, tiles, this.collectables, new object[] {});*/
        }

        public void Init()
        {
            this.is_init = true;
            this.mouseHandler.LeftClickHook = () => { if (this.mouseHandler.IsOnScreen) this.Player.Attack(this.mouseHandler.MousePos); };
        }

        private List<Rectangle> Spawn<T, A>(int amount, List<Rectangle> tiles, List<A> collection, Object[] constructor_parameters) where T: A where A : ISpawnable
        {
            Random random = new Random();
            amount = Math.Min(amount, tiles.Count);

            Object[] default_parameters = { this.contentManager };
            Object[] parameters = new object[default_parameters.Length + constructor_parameters.Length + 1];
            default_parameters.CopyTo(parameters, 1);
            constructor_parameters.CopyTo(parameters, default_parameters.Length + 1);

            parameters[0] = Vector2.Zero;
            Vector2 offset = ((T)Activator.CreateInstance(typeof(T), parameters)).GetCenter;

            for (int i = 0; i < amount; i++)
            {
                Rectangle random_rect = tiles[random.Next(0, tiles.Count)];
                parameters[0] = random_rect.Center.ToVector2() - offset;
                T collectable = (T)Activator.CreateInstance(typeof(T), parameters);
                collection.Add((A)collectable);
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
            this.current_room.Draw(spriteBatch);
            foreach (ACollectables collectable in this.collectables) collectable.Draw(spriteBatch);
            foreach (AEntity entity in this.entities) entity.Draw(spriteBatch);
        }

        public void MovePlayer(Vector2 speed)
        {
            this.Player.SpeedUp(speed);
        }

        public void ToggleDebug()
        {
            foreach(AEntity entity in this.entities)
            {
                entity.show_hitbox = !entity.show_hitbox;
                foreach(Bullet bullet in entity.Bullets) bullet.show_hitbox = !bullet.show_hitbox;
            }

            foreach(ACollectables collectable in this.collectables) collectable.show_hitbox = !collectable.show_hitbox;
        }
    }
}
