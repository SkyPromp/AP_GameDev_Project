using AP_GameDev_Project.Entities.Collectables;
using AP_GameDev_Project.Entities.Mobs;
using AP_GameDev_Project.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AP_GameDev_Project.Entities
{
    internal class EntityFactory
    {
        IContentManager contentManager;
        Random random;

        private List<AEntity> entities;
        private List<ACollectables> collectables; 
        private Room current_room;

        public EntityFactory(List<AEntity> entities, List<ACollectables> collectables, Room current_room, IContentManager contentManager)
        {
            random = new Random();
            this.contentManager = contentManager;

            this.entities = entities;
            this.collectables = collectables;
            this.current_room = current_room;
        }

        public void StartSpawn(ushort difficulty = 0)
        {
            List<Rectangle> tiles = this.current_room.GetHitboxes((byte tile) => { return tile == 1 || tile == 3; });  // TODO: remove player spawnpoint tile (and the one above)

            ushort spawn_amount = (ushort)(4 + difficulty);
            ushort total_spawnable_types = 3;

            for (int i = 0; i < spawn_amount; i++)
            {
                switch (random.Next(0, total_spawnable_types))
                {
                    case 0:
                        tiles = this.Spawn<Enemy1, AEntity>(1, tiles, this.entities, new object[] { 5f, (int)(100 * (1 + 0.2 * difficulty)), 0.8f });  // Speed, health, damping factor 
                        break;
                    case 1:
                        tiles = this.Spawn<Enemy2, AEntity>(1, tiles, this.entities, new object[] { 7f, (int)(500 * (1 + 0.2 * difficulty)), 0.8f });
                        break;
                    case 2:
                        tiles = this.Spawn<Enemy3, AEntity>(1, tiles, this.entities, new object[] { 3f, (int)(300 * (1 + 0.2 * difficulty)), 0.8f });
                        break;
                }
            }

            spawn_amount = 1;
            total_spawnable_types = 2;

            for (int i = 0; i < spawn_amount; i++)
            {
                switch (random.Next(0, total_spawnable_types))
                {
                    case 0:
                        tiles = this.Spawn<HeartCollectable, ACollectables>(1, tiles, this.collectables, new object[] { });
                        break;
                    case 1:
                        tiles = this.Spawn<StrengthCollectable, ACollectables>(1, tiles, this.collectables, new object[] { });
                        break;
                }
            }
        }

        private List<Rectangle> Spawn<T, A>(int amount, List<Rectangle> tiles, List<A> collection, object[] constructor_parameters) where T : A where A : ISpawnable
        {
            amount = Math.Min(amount, tiles.Count);

            object[] default_parameters = { contentManager };
            object[] parameters = new object[default_parameters.Length + constructor_parameters.Length + 1];
            default_parameters.CopyTo(parameters, 1);
            constructor_parameters.CopyTo(parameters, default_parameters.Length + 1);

            parameters[0] = Vector2.Zero;
            Vector2 offset = ((T)Activator.CreateInstance(typeof(T), parameters)).GetCenter;

            for (int i = 0; i < amount; i++)
            {
                Rectangle random_rect = tiles[random.Next(0, tiles.Count)];
                parameters[0] = random_rect.Center.ToVector2() - offset;
                T collectable = (T)Activator.CreateInstance(typeof(T), parameters);
                collection.Add(collectable);
                tiles.Remove(random_rect);
            }

            return tiles;
        }

        public void SpawnRandomCollectable(Vector2 position)
        {
            int total_spawnable_types = 2;

            switch (this.random.Next(0, total_spawnable_types + 1))
            {
                case 0:
                    this.collectables.Add(new HeartCollectable(position, this.contentManager));
                    break;
                case 1:
                    this.collectables.Add(new StrengthCollectable(position, this.contentManager));
                    break;
                case 2:
                    break;
            }
        }
    }
}
