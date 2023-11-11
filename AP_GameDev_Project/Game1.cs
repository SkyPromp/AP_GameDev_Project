using AP_GameDev_Project.State_handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AP_GameDev_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;

        private StateHandler stateHandler;
        private ContentManager contentManager;

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
            this.stateHandler = StateHandler.getInstance;
            this.stateHandler.InitStateHandler();
            this.contentManager = ContentManager.getInstance;
            this.contentManager.Init();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this._spriteBatch = new SpriteBatch(this.GraphicsDevice);

            Animate enemy1_standstill = new Animate(1, 2, new Rectangle(0, 0, 64, 64), Content.Load<Texture2D>("enemy1_stand_still_0"));
            Animate player_standstill = new Animate(1, 2, new Rectangle(0, 0, 128, 192), Content.Load<Texture2D>("stand_still1"));

            this.contentManager.AddSoundEffect("BULLET_SHOOT", Content.Load<SoundEffect>("shootsound"));
            this.contentManager.AddSoundEffect("DEATH_EFFECT", Content.Load<SoundEffect>("deathsound"));
            this.contentManager.Font = Content.Load<SpriteFont>("Font");
            this.contentManager.AddTexture("TILEMAP", Content.Load<Texture2D>("gamedev_tilemap"));
            this.contentManager.AddTexture("BULLET", Content.Load<Texture2D>("bullet"));
            this.contentManager.AddAnimation("ENEMY1_STANDSTILL", enemy1_standstill);
            this.contentManager.AddAnimation("PLAYER_STANDSTILL", player_standstill);

            this.stateHandler.Add(StateHandler.states_enum.START, new StartStateHandler());
            this.stateHandler.Add(StateHandler.states_enum.RUNNING, new RunningStateHandler());
            this.stateHandler.Add(StateHandler.states_enum.MAPMAKING, new MapMakingStateHandler(this.GraphicsDevice));
            this.stateHandler.SetCurrentState(StateHandler.states_enum.START).Init();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F4))
                Exit();

            if (!this.stateHandler.IsInit) this.stateHandler.Init();

            this.stateHandler.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Wheat);

            this._spriteBatch.Begin();

            this.stateHandler.Draw(_spriteBatch);

            this._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
