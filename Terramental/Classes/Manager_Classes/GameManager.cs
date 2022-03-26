﻿using Microsoft.Xna.Framework;
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

        public static bool gameInProgress;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputManager _inputManager;
        private SpriteManager _spriteManager;

        public PlayerCharacter playerCharacter = new PlayerCharacter();

        private CameraController _mainCam;

        public static int screenHeight = 540;
        public static int screenWidth = 960;

        private MapManager _mapManager;

        private MenuManager _menuManager;

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
            _menuManager = new MenuManager(this, _graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            InitialiseManagers();
        }

        protected override void Update(GameTime gameTime)
        {

            UpdateManagers(gameTime);

            if(gameInProgress)
            {
                playerCharacter.UpdateCharacter(gameTime);
                playerCharacter.UpdatePlayerCharacter(gameTime);
            }
            

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

        public void ExitGame()
        {
            Exit();
        }

        public void LoadNewGame()
        {
            _mapManager = new MapManager(this);

            playerCharacter.Initialise(new Vector2(128, 128), GetTexture("Sprites/Player/Idle/Idle_Fire_SpriteSheet"), new Vector2(64, 64));
            playerCharacter.InitialisePlayerAnimations(this);
            _inputManager.playerCharacter = playerCharacter;

            CameraController.cameraActive = true;

            gameInProgress = true;
        }

        private void InitialiseManagers()
        {
            _spriteManager = new SpriteManager();
            _mainCam = new CameraController();

            _inputManager = new InputManager(_mainCam, _menuManager);
        }

        private void UpdateManagers(GameTime gameTime)
        {
            _inputManager.Update(gameTime);
            _spriteManager.Update(gameTime);
            _mainCam.MoveCamera(playerCharacter);
            SpawnManager.Update(gameTime);
        }

        #endregion
    }
}
