using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Terramental
{
    /// <summary>
    /// The Game1 Class
    /// </summary>

    public class GameManager : Game
    {
        public const int gravity = 3;

        public static bool gameInProgress;
        public static bool gameLoaded;

        public static int levelsComplete = 5;

        public enum GameState { SplashScreen, MainMenu, Options, Credits, Level, Respawn, LevelSelect, LevelSelectConfirm, LevelPause, LevelComplete, StartScreen, HelpMenu, LoadingScreen};

        public enum ButtonName { StartGameButton, OptionsButton, AchievementsButton, CreditsButton, ExitGameButton, RespawnButton, DialogueNextButton, LevelSelectExit, LevelSelectConfirm, ReturnMainMenu, ResumeGame, Replay, Continue, ResolutionButton, OptionsReturn, MusicButton, ControlsButton, HelpScreenButton, SFXVolumeUp, SFXVolumeDown, MusicVolumeUp, MusicVolumeDown, ResolutionUp, ResolutionDown };

        public enum GameData { Game1, Game2, Game3, Game4};

        public enum LevelButton { Level1Button, Level2Button, Level3Button, Level4Button, Level5Button };

        public static GameState currentGameState = GameState.SplashScreen;
        public static GameState previousGameState = GameState.SplashScreen;

        public static int screenWidth = 960;
        public static int screenHeight = 540;

        public static float spriteWidth = 1.0f; 
        public static float spriteHeight = 1.0f;

        public static int levelIndex;
        public static Vector2 playerCheckpoint;

        public MapManager mapManager;
        public PlayerInterface playerInterface;
        public MenuManager menuManager;

        public static float actualWidth;
        public static float actualHeight;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public InputManager _inputManager;
        private SpriteManager _spriteManager;

        public PlayerCharacter playerCharacter;

        public CameraController _mainCam;

        private bool skipToLevel = false;

        private int[] screenWidths = { 960, 3840, 2560, 2560, 1920, 1366, 1280, 1280 };
        private int[] screenHeights = { 540, 2160, 1440, 1080, 1080, 768, 1024, 720 };
        private int currentResolutionIndex;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            IsMouseVisible = false;

            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }

        #region Main

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
           
            menuManager = new MenuManager(this, _graphics);
            actualWidth = _graphics.GraphicsDevice.Viewport.Width;
            actualHeight = _graphics.GraphicsDevice.Viewport.Height;

            InitialiseGame();
            LoadAudioLibrary();
            AudioManager.SetVolumes();
            AudioManager.PlaySound("Intro_Music");

            if(skipToLevel)
            {
                LoadNewGame(@"MapData.json");
                currentGameState = GameState.Level;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            spriteWidth = screenWidth / 960;
            spriteHeight = screenHeight / 540;

            UpdateManagers(gameTime);

            if(currentGameState == GameState.Level && playerInterface != null)
            {
                playerInterface.UpdatePlayerInterface();
                playerCharacter.UpdateCharacter(gameTime);
                playerCharacter.UpdatePlayerCharacter(gameTime);

                if(mapManager != null)
                {
                    mapManager.CheckActiveTiles();
                }
            }

            menuManager.UpdateMenuButtons(gameTime);

            _mainCam.UpdateCamera(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, _mainCam.cameraTransform);

            if(currentGameState == GameState.Level || currentGameState == GameState.LevelPause && playerInterface != null)
            {
                _spriteManager.Draw(gameTime, _spriteBatch);
                playerInterface.DrawInterface(_spriteBatch);
                playerInterface.DrawCooldownTexts(_spriteBatch);
            }

            menuManager.DrawMenus(_spriteBatch);

            _spriteBatch.End();


            System.Diagnostics.Debug.WriteLine("Draw Count = " + GraphicsDevice.Metrics.DrawCount.ToString());

            base.Draw(gameTime);
        }

        #endregion

        #region Property Tools

        public Texture2D GetTexture(string path)
        {
            return Content.Load<Texture2D>(path);
        }

        public void ChangeResolution(int amount)
        {
            currentResolutionIndex += amount;

            if(currentResolutionIndex < 0)
            {
                currentResolutionIndex = screenWidths.Length - 1;
            }

            if(currentResolutionIndex >= screenWidths.Length)
            {
                currentResolutionIndex = 0;
            }

            _graphics.PreferredBackBufferWidth = screenWidths[currentResolutionIndex];
            _graphics.PreferredBackBufferHeight = screenHeights[currentResolutionIndex];
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;

            if(screenWidth == _graphics.GraphicsDevice.Viewport.Width && screenHeight == _graphics.GraphicsDevice.Viewport.Height)
            {
                _graphics.IsFullScreen = true;
            }
            else
            {
                _graphics.IsFullScreen = false;
            }

            // Apply the changes
            _graphics.ApplyChanges();
            System.Console.WriteLine("New resolution: {0} x {1}", _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }

        #endregion

        #region Managers

        public void ExitGame()
        {
            Exit();
        }

        public static void PauseGame()
        {
            currentGameState = GameState.LevelPause;
        }

        public void LoadNewGame(string filePath)
        {
            menuManager.ActivateLoadingScreen(5, GameState.Level);

            if (!gameInProgress)
            {
                playerCharacter = new PlayerCharacter(this);
                playerCharacter.Initialise(new Vector2(200, 128), GetTexture("Sprites/Player/Idle/Idle_Fire_SpriteSheet"), new Vector2(64, 64));
                playerCharacter.InitialisePlayerAnimations(this);
                playerCharacter.LoadStatusEffects();
                playerCharacter.LayerOrder = -1;
                playerInterface = new PlayerInterface(this);

                mapManager = new MapManager(this);

                CameraController.playerCharacter = playerCharacter;
                gameInProgress = true;
            }

            playerCharacter.ResetPlayer();
            playerCharacter.DisplayPlayerLives();

            mapManager.LoadMapData(filePath);
            gameLoaded = true;
        }

        private void InitialiseGame()
        {
            _spriteManager = new SpriteManager();
            _mainCam = new CameraController(_graphics.GraphicsDevice.Viewport);
            SpawnManager._gameManager = this;
            _inputManager = new InputManager(_mainCam, menuManager, this);
        }

        private void UpdateManagers(GameTime gameTime)
        {
            _inputManager.Update(gameTime);

            if(currentGameState == GameState.Level)
            {
                _spriteManager.Update(gameTime);
                SpawnManager.Update(gameTime);
            }
        }

        private void LoadAudioLibrary()
        {
            AudioClip audioClip = new AudioClip("PlayerJump_SFX", "SFXs/PlayerJump_SFX", false, this);
            AudioManager.AddSound(audioClip);

            AudioClip beepSound = new AudioClip("BeepTone_SFX", "SFXs/BeepTone", false, this);
            AudioManager.AddSound(beepSound);

            AudioClip introMusic = new AudioClip("Intro_Music", "Music/far-from-home-acoustic-version-13463", true, this);
            AudioManager.AddSound(introMusic);

            AudioClip levelMusic = new AudioClip("Level_Music", "Music/ambient-piano-10781", true, this);
            AudioManager.AddSound(levelMusic);
        }

        #endregion
    }
}
