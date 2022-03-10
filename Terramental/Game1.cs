using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terramental
{
    public class Game1 : Game
    {
        public const int gravity = 3;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

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
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _mainCam = new CameraController();
            _playerCharacter.Initialise(Vector2.Zero, _playerTexture);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _playerCharacter.Update(gameTime);
            _mainCam.MoveCamera(_playerCharacter);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin(transformMatrix: _mainCam.Transform);
            _playerCharacter.Draw(gameTime, _spriteBatch);
            _spriteBatch.Draw(_playerTexture, new Rectangle(10, 10, 100, 100), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
