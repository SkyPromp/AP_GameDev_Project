using AP_GameDev_Project.Entities;
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
            this._spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.stateHandler = StateHandler.getInstance;
            this.stateHandler.InitStateHandler();
            this.contentManager = ContentManager.getInstance;
            this.contentManager.Init();

            SoundEffect.MasterVolume = 1.0f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.contentManager.AddSoundEffect("BULLET_SHOOT", Content.Load<SoundEffect>("shootsound"));
            this.contentManager.AddSoundEffect("PLAYER_DEATH", Content.Load<SoundEffect>("deathsound"));
            this.contentManager.Font = Content.Load<SpriteFont>("Font");
            this.contentManager.AddTexture("STARTSCREEN", Content.Load<Texture2D>("startscreen"));
            this.contentManager.AddTexture("TILEMAP", Content.Load<Texture2D>("gamedev_tilemap_2"));
            this.contentManager.AddTexture("BULLET", Content.Load<Texture2D>("bullet"));
            this.contentManager.AddTexture("TILEMAP_ENTITIES", Content.Load<Texture2D>("tilemap_entities"));
            this.contentManager.AddTexture("COLLECTABLES", Content.Load<Texture2D>("collectables"));

            Animate enemy1_standstill = new Animate(1, 2, new Rectangle(0, 0, 64, 64), this.contentManager.GetTextures["TILEMAP_ENTITIES"]);
            Animate player_standstill = new Animate(1, 2, new Rectangle(0, 128, 128, 192), this.contentManager.GetTextures["TILEMAP_ENTITIES"]);
            Animate player_walk = new Animate(0.5, 4, new Rectangle(0, 320, 128, 192), this.contentManager.GetTextures["TILEMAP_ENTITIES"]);
            Animate enemy1_walk = new Animate(2, 4, new Rectangle(0, 64, 64, 64), this.contentManager.GetTextures["TILEMAP_ENTITIES"]);
            Animate heart_collectable = new Animate(3.5, 4, new Rectangle(0, 0, 64, 64), this.contentManager.GetTextures["COLLECTABLES"]);

            this.contentManager.AddAnimation("ENEMY1_STANDSTILL", enemy1_standstill);
            this.contentManager.AddAnimation("PLAYER_STANDSTILL", player_standstill);
            this.contentManager.AddAnimation("PLAYER_WALK", player_walk);
            this.contentManager.AddAnimation("ENEMY1_WALK", enemy1_walk);
            this.contentManager.AddAnimation("HEART_COLLECTABLE", heart_collectable);

            this.contentManager.AddRoom(new Room("Rooms\\BigRoom.room"));

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
