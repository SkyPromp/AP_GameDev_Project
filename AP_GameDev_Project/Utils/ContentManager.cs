using AP_GameDev_Project.Entities;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace AP_GameDev_Project.Utils
{
    internal class ContentManager: IContentManager
    {
        private Dictionary<string, SoundEffect> sound_effects;
        public Dictionary<string, SoundEffect> GetSoundEffects { get { return sound_effects; } }
        private SpriteFont font;
        public SpriteFont Font { get { return font; } set { font = value; } }
        private Dictionary<string, Texture2D> textures;
        public Dictionary<string, Texture2D> GetTextures { get { return textures; } }
        private Dictionary<string, Animate> animations;
        public Dictionary<string, Animate> GetAnimations { get { return animations; } }
        private List<Room> rooms;
        public List<Room> GetRooms { get { return rooms; } }

        // Singleton vars
        private volatile static ContentManager instance;
        private static object syncRoot = new object();

        private ContentManager() { }

        public static ContentManager getInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null) instance = new ContentManager();
                    }
                }

                return instance;
            }
        }

        public void Init()
        {
            sound_effects = new Dictionary<string, SoundEffect>();
            textures = new Dictionary<string, Texture2D>();
            animations = new Dictionary<string, Animate>();
            rooms = new List<Room>();
        }

        public void AddSoundEffect(string name, SoundEffect sound_effect)
        {
            sound_effects.Add(name, sound_effect);
        }

        public void AddTexture(string name, Texture2D texture)
        {
            textures.Add(name, texture);
        }

        public void AddAnimation(string name, Animate animation)
        {
            animations.Add(name, animation);
        }

        public void AddRoom(Room room)
        {
            rooms.Add(room);
        }
    }
}
