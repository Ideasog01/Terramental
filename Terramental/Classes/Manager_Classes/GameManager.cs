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

        public static bool gameInProgress;

        public enum GameState { SplashScreen, MainMenu, Options, Credits, NewGame, LoadGame, Level, Respawn, LevelSelect, LevelSelectConfirm, LevelPause, LevelComplete};

        public enum ButtonName { NewGameButton, LoadGameButton, OptionsButton, AchievementsButton, CreditsButton, ExitGameButton, RespawnButton, DialogueNextButton, LevelSelectExit, LevelSelectConfirm, ReturnMainMenu, ResumeGame };

        public enum GameData { Game1, Game2, Game3, Game4};

        public enum LevelButton { Level1Button, Level2Button };

        public static GameState currentGameState = GameState.SplashScreen;

        public static int screenWidth = 960;
        public static int screenHeight = 540;

        public static int levelIndex;
        public static Vector2 playerCheckpoint;

        public MapManager mapManager;
        public PlayerInterface playerInterface;
        public MenuManager menuManager;
        public DialogueManager dialogueManager;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputManager _inputManager;
        private SpriteManager _spriteManager;

        public PlayerCharacter playerCharacter;

        private CameraController _mainCam;

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
           
            menuManager = new MenuManager(this, _graphics);

            InitialiseGame();
        }

        protected override void Update(GameTime gameTime)
        {
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

                if(DialogueManager.dialogueActive)
                {
                    dialogueManager.UpdatePosition();
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

            if(DialogueManager.dialogueActive && currentGameState == GameState.Level)
            {
                dialogueManager.DrawDialogueInterface(_spriteBatch);
            }

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

        public static void PauseGame()
        {
            currentGameState = GameState.LevelPause;
        }

        public void LoadNewGame(string filePath)
        {
            if(!gameInProgress)
            {
                playerCharacter = new PlayerCharacter(this);
                playerCharacter.Initialise(new Vector2(200, 128), GetTexture("Sprites/Player/Idle/Idle_Fire_SpriteSheet"), new Vector2(64, 64));
                playerCharacter.InitialisePlayerAnimations(this);
                playerCharacter.LayerOrder = -1;
                playerInterface = new PlayerInterface(this);


                dialogueManager = new DialogueManager(this, menuManager);
                mapManager = new MapManager(this);

                mapManager.LoadMapData(filePath);

                CameraController.playerCharacter = playerCharacter;

                gameInProgress = true;

                LoadAudioLibrary();
            }
            else
            {
                playerCharacter.ResetPlayer();
                playerCharacter.DisplayPlayerLives();
                GameManager.currentGameState = GameManager.GameState.Level;
            }
        }

        private void InitialiseGame()
        {
            SaveManager.gameManager = this;
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
        }

        #endregion
    }
}
