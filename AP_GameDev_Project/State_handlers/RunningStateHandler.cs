using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AP_GameDev_Project.State_handlers
{
    internal class RunningStateHandler : IStateHandler
    {
        private Room current_room;
        private bool is_init;
        public bool IsInit { get { return this.is_init; } }
        private Player player;

        public RunningStateHandler(Texture2D tilemap, Player player)
        {
            this.current_room = new Room(tilemap, "Rooms\\Room1.room");
            this.player = player;
        }

        public void Init()
        {
            this.is_init = true;
        }

        public void Update(GameTime gameTime)
        {
            this.HandleKeyboard();
            player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            current_room.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }

        private void HandleKeyboard()
        {
            Vector2 addSpeed = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                addSpeed += new Vector2(0, -1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                addSpeed += new Vector2(0, 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                addSpeed += new Vector2(-1, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                addSpeed += new Vector2(1, 0);
            }

            this.player.SpeedUp(addSpeed);
            //if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Game1.current_state = Game1.states.START;  // Doesn't work, because start isn't initialized
        }
    }
}
