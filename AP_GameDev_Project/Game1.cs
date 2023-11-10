using AP_GameDev_Project.Entities;
using AP_GameDev_Project.State_handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace AP_GameDev_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;

        private StateHandler stateHandler;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = GlobalConstants.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = GlobalConstants.SCREEN_HEIGHT;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            stateHandler = StateHandler.getInstance;
            stateHandler.InitStateHandler();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D tilemap = Content.Load<Texture2D>("gamedev_tilemap");
            SpriteFont font = Content.Load<SpriteFont>("Font");
            Bullet base_bullet = new Bullet(Vector2.Zero, Vector2.Zero, Content.Load<Texture2D>("bullet"));

            List<AEntity> base_enemies = new List<AEntity>();
            Animate enemy_standstill = new Animate(1, 2, new Rectangle(0, 0, 64, 64), Content.Load<Texture2D>("enemy1_stand_still_0"));
            Enemy1 base_enemy1 = new Enemy1(new Vector2(300, 300), enemy_standstill, 5f, new Rectangle(22, 10, 17, 43), 5, base_bullet);
            base_enemies.Add(base_enemy1);

            Animate player_standstill = new Animate(1, 2, new Rectangle(0, 0, 128, 192), Content.Load<Texture2D>("stand_still1"));
            Player player = new Player(new Vector2(180, 180), player_standstill, 5f, base_bullet);

            stateHandler.Add(StateHandler.states_enum.START, new StartStateHandler());
            stateHandler.Add(StateHandler.states_enum.RUNNING, new RunningStateHandler(tilemap, player, base_enemies));
            stateHandler.Add(StateHandler.states_enum.MAPMAKING, new MapMakingStateHandler(GraphicsDevice, tilemap, font));
            stateHandler.SetCurrentState(StateHandler.states_enum.START).Init();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F4))
                Exit();

            if (!stateHandler.IsInit) stateHandler.Init();

            stateHandler.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Wheat);

            _spriteBatch.Begin();

            stateHandler.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
