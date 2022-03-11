﻿using Microsoft.Xna.Framework;
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

        private PlayerCharacter _playerCharacter = new PlayerCharacter();

        private Texture2D _playerTexture;

        private CameraController _mainCam;

        private static List<Sprite> _spriteList = new List<Sprite>();

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

        public static List<Sprite> SpriteList
        {
            get { return _spriteList; }
        }
        

        protected override void Initialize()
        {
            _playerTexture = Content.Load<Texture2D>("Sprites/Player/Knight_Side");

            Texture2D animTexture = Content.Load<Texture2D>("Sprites/SpriteSheets/Effects/Flame_SpriteSheet");
            AnimationTest test = new AnimationTest();
            test.Initialise(new Vector2(100, 100), animTexture, new Vector2(64, 128));
            test.AddAnimation(animTexture);
            test.SetAnimationIndex(0, 4, 120f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _mainCam = new CameraController();
            _playerCharacter.Initialise(Vector2.Zero, _playerTexture, new Vector2(64, 64));
            _inputManager = new InputManager(_playerCharacter);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach(Sprite sprite in _spriteList)
            {
                sprite.Update(gameTime);
            }

            _mainCam.MoveCamera(_playerCharacter);
            _inputManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin(transformMatrix: _mainCam.Transform);

            foreach(Sprite sprite in _spriteList)
            {
                sprite.Draw(gameTime, _spriteBatch);
            }

            //_spriteBatch.Draw(_playerTexture, new Rectangle(10, 10, 100, 100), Color.White); //Test

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
