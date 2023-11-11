using AP_GameDev_Project.Input_devices;
using Microsoft.Xna.Framework;


namespace AP_GameDev_Project.Entities
{
    internal class Player: AEntity
    {
        private readonly MouseHandler mouseHandler;
        private ContentManager contentManager;
        private readonly int max_health;
        private bool has_invincibility;
        
        public Player(Vector2 position, float max_speed=5f, int max_health=3, float speed_damping_factor=0.10f) : 
            base(position, max_speed, new Rectangle(45, 35, 35, 142), 10f, 0.2f, max_health, speed_damping_factor)
        {
            this.contentManager = ContentManager.getInstance;
            base.stand_animation = this.contentManager.GetAnimations["PLAYER_STANDSTILL"];
            this.mouseHandler = MouseHandler.getInstance.Init();
            this.max_health = max_health;
            this.has_invincibility = true;  // TODO set to false
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle hitbox = base.GetHitbox;
            base.flip_texture = mouseHandler.MousePos.X < hitbox.X + hitbox.Width / 2;

            base.Update(gameTime);
        }

        public void Attack()
        {
            if (base.bullet_cooldown <= 0)
            {
                Vector2 center = base.GetCenter;
                Vector2 bullet_position = center + new Vector2(this.mouseHandler.MousePos.X < center.X ? -40 : 40, -7);  
                Vector2 angle = Vector2.Normalize(this.mouseHandler.MousePos - bullet_position);  // TODO: correct bullet_position to the center of the bullet?

                base.bullets.Add(new Bullet(bullet_position, angle * base.bullet_speed));
                base.bullet_cooldown = base.bullet_max_cooldown;
                this.contentManager.GetSoundEffects["BULLET_SHOOT"].Play();
            }
        }
        public override void Attack(Vector2 player_center)
        {
            throw new System.NotImplementedException();
        }

        public void Heal(int heal_amount=1)
        {
            base.health += heal_amount;
            
            if(base.health > this.max_health) base.health = this.max_health;
        }


        public override int DoDamage(int damage=1)
        {
            if(!this.has_invincibility) base.DoDamage(damage);
            
            return base.health;
        }
    }
}
