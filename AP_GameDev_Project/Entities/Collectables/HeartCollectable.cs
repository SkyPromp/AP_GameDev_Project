using AP_GameDev_Project.Entities.Mobs;
using Microsoft.Xna.Framework;


namespace AP_GameDev_Project.Entities.Collectables
{
    internal class HeartCollectable : ACollectables
    {
        public HeartCollectable(Vector2 position, ContentManager contentManager) : base(position, contentManager.GetAnimations["HEART_COLLECTABLE"], new Hitbox().AddChild(new Rectangle(16, 11, 32, 35)))
        {
        }

        public override void OnCollision(Player player)
        {
            player.Heal(1);
        }
    }
}
