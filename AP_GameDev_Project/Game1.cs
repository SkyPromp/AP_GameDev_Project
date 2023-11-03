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
        public static IStateHandler current_state;
        private static Dictionary<Game1.states_enum, IStateHandler> states;
        public static Dictionary<Game1.states_enum, IStateHandler> States { get { return states; } }
        public enum states_enum
        {
            START,
            RUNNING,
            MAPMAKING,
            PAUSED,
            GAME_OVER
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = GlobalConstants.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = GlobalConstants.SCREEN_HEIGHT;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D tilemap = Content.Load<Texture2D>("gamedev_tilemap");
            SpriteFont font = Content.Load<SpriteFont>("Font");
            Bullet base_bullet = new Bullet(Vector2.Zero, Vector2.Zero, Content.Load<Texture2D>("bullet"));
            Player player = new Player(new Vector2(180, 180), new Animate(1, 2, new Rectangle(0, 0, 128, 192), Content.Load<Texture2D>("stand_still0")), 5f, base_bullet);

            Game1.states = new Dictionary<Game1.states_enum, IStateHandler>();
            Game1.states.Add(Game1.states_enum.START, new StartStateHandler());
            Game1.states.Add(Game1.states_enum.RUNNING, new RunningStateHandler(tilemap, player));
            Game1.states.Add(Game1.states_enum.MAPMAKING, new MapMakingStateHandler(GraphicsDevice, tilemap, font));
            Game1.current_state = Game1.states[Game1.states_enum.START];
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F4))
                Exit();

            if (!Game1.current_state.IsInit) Game1.InitCurrentState();

            Game1.current_state.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            Game1.current_state.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void InitCurrentState()
        {
            Game1.current_state.Init();
        }
    }
}
