using AP_GameDev_Project.Entities;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AP_GameDev_Project.Utils
{
    internal interface IContentManager
    {
        public Dictionary<string, SoundEffect> GetSoundEffects { get ; }
        public SpriteFont Font { get; set; }
        public Dictionary<string, Texture2D> GetTextures { get; }
        public Dictionary<string, Animate> GetAnimations { get; }
        public List<Room> GetRooms { get ; }
        public static ContentManager getInstance { get; }

        public void Init();

        public void AddSoundEffect(string name, SoundEffect sound_effect);

        public void AddTexture(string name, Texture2D texture);

        public void AddAnimation(string name, Animate animation);

        public void AddRoom(Room room);
    }
}
