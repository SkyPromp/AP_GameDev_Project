using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace AP_GameDev_Project
{
    internal class ContentManager
    {
        private Dictionary<string, SoundEffect> sound_effects;
        public Dictionary<string, SoundEffect> GetSoundEffects { get { return this.sound_effects; } }
        private SpriteFont font;
        public SpriteFont Font {  get { return this.font; } set { this.font = value; } }
        private Dictionary<string, Texture2D> textures;
        public Dictionary<string, Texture2D> GetTextures { get { return this.textures; } }
        private Dictionary<string, Animate> animations;
        public Dictionary<string, Animate> GetAnimations { get { return this.animations; } }

        // Singleton vars
        private volatile static ContentManager instance;
        private static object syncRoot = new object();

        private ContentManager() { }

        public static ContentManager getInstance
        {
            get
            {
                if (ContentManager.instance == null)
                {
                    lock (ContentManager.syncRoot)
                    {
                        if (ContentManager.instance == null) ContentManager.instance = new ContentManager();
                    }
                }

                return ContentManager.instance;
            }
        }

        public void Init()
        {
            this.sound_effects = new Dictionary<string, SoundEffect>();
            this.textures = new Dictionary<string, Texture2D>();
            this.animations = new Dictionary<string, Animate>();
        }

        public void AddSoundEffect(string name, SoundEffect sound_effect)
        {
            this.sound_effects.Add(name, sound_effect);
        }

        public void AddTexture(string name, Texture2D texture)
        {
            this.textures.Add(name, texture);
        }

        public void AddAnimation(string name, Animate animation)
        {
            this.animations.Add(name, animation);
        }
    }
}
