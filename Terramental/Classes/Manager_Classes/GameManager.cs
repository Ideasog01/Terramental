using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Terramental
{
    public class GameManager : Game
    {
        public const int gravity = 3;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputManager _inputManager;
        private SpriteManager _spriteManager;
        private SpawnManager _spawnManager;

        private PlayerCharacter _playerCharacter = new PlayerCharacter();
        private BaseCharacter _testEnemy = new BaseCharacter();

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

        public Texture2D GetTexture(string path)
        {
            return Content.Load<Texture2D>(path);
        }

        protected override void Initialize()
        {
            _mapManager = new MapManager(14, 10, 0, this, _spawnManager, _playerCharacter);

            Animation animation = new Animation(GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), 4, 120f, true);
            Sprite test = new Sprite();
            test.Initialise(new Vector2(576, 128), GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), new Vector2(64, 64), _spawnManager);
            test.Animations.Add(animation);

            Animation waterAnim = new Animation(GetTexture("Sprites/Pickups/WaterPickup_SpriteSheet"), 4, 200f, true);
            Sprite test2 = new Sprite();
            test2.Initialise(new Vector2(640, 128), GetTexture("Sprites/Pickups/WaterPickup_SpriteSheet"), new Vector2(64, 64), _spawnManager);
            test2.Animations.Add(waterAnim);

            Animation snowAnim = new Animation(GetTexture("Sprites/Pickups/SnowPickup_SpriteSheet"), 4, 120f, true);
            Sprite test3 = new Sprite();
            test3.Initialise(new Vector2(512, 128), GetTexture("Sprites/Pickups/SnowPickup_SpriteSheet"), new Vector2(64, 64), _spawnManager);
            test3.Animations.Add(snowAnim);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            InitialiseManagers();

            _mainCam = new CameraController();
            _playerCharacter.Initialise(new Vector2(128, 128), GetTexture("Sprites/Player/PlayerCharacter_Sprite_Fire"), new Vector2(64, 64), _spawnManager);
            _testEnemy.Initialise(new Vector2(196, 96), GetTexture("Sprites/Enemies/Knight/Knight_Character_Idle_SpriteSheet"), new Vector2(96, 96), _spawnManager);

            Animation knightIdle = new Animation(GetTexture("Sprites/Enemies/Knight/Knight_Character_Attack_SpriteSheet"), 8, 120f, true);
            _testEnemy.Animations.Add(knightIdle);
            _spawnManager.enemyCharacters.Add(_testEnemy);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _mainCam.MoveCamera(_playerCharacter);
            UpdateManagers(gameTime);

            _mapManager.Update();

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
            _spawnManager = new SpawnManager(this);
        }

        private void UpdateManagers(GameTime gameTime)
        {
            _inputManager.Update(gameTime);
            _spriteManager.Update(gameTime);
            _spawnManager.Update(gameTime);
        }
    }
}
