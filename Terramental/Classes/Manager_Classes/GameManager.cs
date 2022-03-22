using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Terramental
{
    /// <summary>
    /// The Game1 Class
    /// </summary>

    public class GameManager : Game
    {
        public const int gravity = 3;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputManager _inputManager;
        private SpriteManager _spriteManager;

        private PlayerCharacter _playerCharacter = new PlayerCharacter();

        private CameraController _mainCam;

        public static int screenHeight = 540;
        public static int screenWidth = 960;

        private MapManager _mapManager;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            IsMouseVisible = false;

            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.IsFullScreen = false;
        }

        #region Main

        protected override void Initialize()
        {
            _mapManager = new MapManager(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            InitialiseManagers();

            _mainCam = new CameraController();

            _playerCharacter.Initialise(new Vector2(128, 128), GetTexture("Sprites/Player/Idle/Idle_Fire_SpriteSheet"), new Vector2(64, 64));
            _playerCharacter.InitialisePlayerAnimations(this);


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            UpdateManagers(gameTime);
            _playerCharacter.UpdateCharacter(gameTime);
            _playerCharacter.UpdatePlayerCharacter(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin(transformMatrix: _mainCam.Transform);

            _spriteManager.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region Property Tools

        public Texture2D GetTexture(string path)
        {
            return Content.Load<Texture2D>(path);
        }

        #endregion

        #region Managers

        private void InitialiseManagers()
        {
            _inputManager = new InputManager(_playerCharacter);
            _spriteManager = new SpriteManager();
        }

        private void UpdateManagers(GameTime gameTime)
        {
            _inputManager.Update(gameTime);
            _spriteManager.Update(gameTime);
            _mainCam.MoveCamera(_playerCharacter);
            SpawnManager.Update(gameTime);
        }

        #endregion
    }
}
