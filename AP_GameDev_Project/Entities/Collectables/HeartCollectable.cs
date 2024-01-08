using AP_GameDev_Project.Entities.Mobs;
using AP_GameDev_Project.Utils;
using Microsoft.Xna.Framework;


namespace AP_GameDev_Project.Entities.Collectables
{
    internal class HeartCollectable : ACollectables
    {
        private readonly int healing_amount;

        public HeartCollectable(Vector2 position, ContentManager contentManager) : base(position, contentManager.GetAnimations["HEART_COLLECTABLE"], new Hitbox().AddChild(new Rectangle(16, 11, 32, 35)))
        {
            this.healing_amount = 20;
        }

        public override void OnCollision(Player player)
        {
            player.Heal(this.healing_amount);
            base.OnCollision(player);
        }
    }
}
