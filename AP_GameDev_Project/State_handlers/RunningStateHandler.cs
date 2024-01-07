using AP_GameDev_Project.Entities;
using AP_GameDev_Project.Entities.Collectables;
using AP_GameDev_Project.Entities.Mobs;
using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AP_GameDev_Project.State_handlers
{
    internal class RunningStateHandler : IStateHandler
    {
        private Room current_room;
        private List<Rectangle> tile_hitboxes;
        private List<Vector2> walkable_tile_centers;
        private bool is_init;
        public bool IsInit { get { return this.is_init; } }
        private List<AEntity> entities;
        private Player Player { get { return (Player)this.entities[0]; } set { this.entities[0] = value; } }
        private List<ACollectables> collectables;
        private MouseHandler mouseHandler;
        private RunningKeyboardEventHandler keyboardHandler;
        private ContentManager contentManager;
        private CollisionHandler collisionHandler;
        private Random random;

        public RunningStateHandler()
        {
            this.contentManager = ContentManager.getInstance;
            this.collisionHandler = new CollisionHandler();
            this.current_room = this.contentManager.GetRooms[0];
            this.tile_hitboxes = this.current_room.GetHitboxes((Byte tile) => { return tile > 1; });
            this.mouseHandler = MouseHandler.getInstance.Init();
            this.entities = new List<AEntity>();
            this.keyboardHandler = new RunningKeyboardEventHandler(this);
            this.collectables = new List<ACollectables>();

            this.entities.Add(new Player(this.current_room.GetPlayerSpawnpoint, contentManager, 5f));

            List <Rectangle> tiles = this.current_room.GetHitboxes((Byte tile) => { return tile == 1; });  // TODO: remove player spawnpoint tile (and the one above)

            this.random = new Random();

            Vector2 offset = new Enemy2(Vector2.Zero, this.contentManager, 0, 0).GetCenter;
            this.walkable_tile_centers = tiles.Select(tile => tile.Center.ToVector2() - offset).ToList();

            ushort total_enemies = 5;
            ushort total_enemy_types = 3;

            for (int i = 1; i < total_enemies; i++)
            {
                switch (this.random.Next(0, total_enemy_types))
                {
                    case 0:
                        tiles = this.Spawn<Enemy1, AEntity>(1, tiles, this.entities, new object[] { 5f, 5, 0.8f });
                        break;
                    case 1:
                        tiles = this.Spawn<Enemy2, AEntity>(1, tiles, this.entities, new object[] { 5f, 5, 0.8f });
                        break; 
                    case 2:
                        tiles = this.Spawn<Enemy3, AEntity>(1, tiles, this.entities, new object[] { 5f, 5, 0.8f });
                        break;
                }
            }

            tiles = this.Spawn<HeartCollectable, ACollectables>(4, tiles, this.collectables, new object[] {});
            tiles = this.Spawn<StrengthCollectable, ACollectables>(1, tiles, this.collectables, new object[] { });
        }

        public void Init()
        {
            this.is_init = true;
            this.mouseHandler.LeftClickHook = () => { if (this.mouseHandler.IsOnScreen) this.Player.Attack(this.mouseHandler.MousePos); };
        }

        private List<Rectangle> Spawn<T, A>(int amount, List<Rectangle> tiles, List<A> collection, Object[] constructor_parameters) where T: A where A : ISpawnable
        {
            amount = Math.Min(amount, tiles.Count);

            Object[] default_parameters = { this.contentManager };
            Object[] parameters = new object[default_parameters.Length + constructor_parameters.Length + 1];
            default_parameters.CopyTo(parameters, 1);
            constructor_parameters.CopyTo(parameters, default_parameters.Length + 1);

            parameters[0] = Vector2.Zero;
            Vector2 offset = ((T)Activator.CreateInstance(typeof(T), parameters)).GetCenter;

            for (int i = 0; i < amount; i++)
            {
                Rectangle random_rect = tiles[this.random.Next(0, tiles.Count)];
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

            this.collisionHandler.HandleCollision(gameTime, this.Player, this.entities, this.collectables, this.tile_hitboxes);

            foreach (AEntity entity in new List<AEntity>(this.entities))
            {
                if (entity.Health <= 0)
                {
                    entity.Die(this.contentManager);
                    this.entities.Remove(entity);
                    if (entity is Player) return;
                }
                else
                {
                    Vector2 target;
                    if (entity is Player) target = mouseHandler.MousePos;
                    else if (entity is Enemy2) target = this.walkable_tile_centers[random.Next(0, this.walkable_tile_centers.Count)];
                    else target = this.Player.GetCenter;

                    entity.Update(gameTime, target);

                }
            }

            if (entities.Count == 1)
            {
                StateHandler stateHandler = StateHandler.getInstance;
                stateHandler.SetCurrentState(StateHandler.states_enum.END).Init();
                ((EndStateHandler)stateHandler.States[StateHandler.states_enum.END]).Won = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.current_room.Draw(spriteBatch);
            foreach (ACollectables collectable in this.collectables) collectable.Draw(spriteBatch);
            foreach (AEntity entity in this.entities) entity.Draw(spriteBatch);

            String health_display = new StringBuilder()
                .Append("Health: ")
                .Append(this.Player.Health)
                .Append("/")
                .Append(this.Player.MaxHealth)
                .ToString();

            Vector2 half_text_size = this.contentManager.Font.MeasureString(health_display) / 2;
            spriteBatch.DrawString(this.contentManager.Font, health_display, new Vector2(64 - half_text_size.Y, 64 - half_text_size.Y), Color.Black);
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
