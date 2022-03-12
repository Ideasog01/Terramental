using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Terramental
{
    public class Game1 : Game
    {
        public const int gravity = 3;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputManager _inputManager;
        private SpriteManager _spriteManager;

        private PlayerCharacter _playerCharacter = new PlayerCharacter();

        private Texture2D _playerTexture;

        private CameraController _mainCam;

        public static int screenHeight = 540;
        public static int screenWidth = 960;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            IsMouseVisible = false;

            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.IsFullScreen = false;
        }
        

        protected override void Initialize()
        {
            _playerTexture = Content.Load<Texture2D>("Sprites/Player/Knight_Side");

            //Test Animation
            Texture2D animTexture = Content.Load<Texture2D>("Sprites/SpriteSheets/Effects/Flame_SpriteSheet");
            Sprite test = new Sprite();
            test.Initialise(new Vector2(100, 100), animTexture, new Vector2(64, 128));

            Animation testAnim = new Animation(animTexture, 4, 120f);
            test.Animations.Add(testAnim);
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            InitialiseManagers();

            _mainCam = new CameraController();
            _playerCharacter.Initialise(Vector2.Zero, _playerTexture, new Vector2(96, 96));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _mainCam.MoveCamera(_playerCharacter);
            UpdateManagers(gameTime);

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

        private void InitialiseManagers()
        {
            _inputManager = new InputManager(_playerCharacter);
            _spriteManager = new SpriteManager();
        }

        private void UpdateManagers(GameTime gameTime)
        {
            _inputManager.Update(gameTime);
            _spriteManager.Update(gameTime);
        }
    }
}
