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

        public enum GameState { MainMenu, Options, Credits, NewGame, LoadGame, Level, Respawn};
        public enum ButtonName { NewGameButton, LoadGameButton, OptionsButton, AchievementsButton, CreditsButton, ExitGameButton, RespawnButton, DialogueNextButton };

        public static GameState currentGameState = GameState.MainMenu;

        public PlayerInterface playerInterface;
        public MenuManager menuManager;
        public DialogueManager dialogueManager;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputManager _inputManager;
        private SpriteManager _spriteManager;

        public PlayerCharacter playerCharacter;

        private CameraController _mainCam;

        public static int screenHeight = 540;
        public static int screenWidth = 960;

        private MapManager _mapManager;

        private DialogueController _dialogueTrigger;
        private Dialogue _dialogue;

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

            InitialiseManagers();
            //LoadNewGame();
        }

        protected override void Update(GameTime gameTime)
        {

            UpdateManagers(gameTime);

            if(currentGameState == GameState.Level && playerInterface != null)
            {
                playerInterface.UpdatePlayerInterface();
                playerCharacter.UpdateCharacter(gameTime);
                playerCharacter.UpdatePlayerCharacter(gameTime);
                CameraController.playerPosition = playerCharacter.SpritePosition;

                if(_mapManager != null)
                {
                    _mapManager.CheckActiveTiles();
                }

                if(DialogueManager.dialogueActive)
                {
                    dialogueManager.UpdatePosition();
                }
                else
                {
                    _dialogueTrigger.CheckDialogueCollision();
                }
                
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            _spriteBatch.Begin(transformMatrix: _mainCam.Transform);

            if(currentGameState == GameState.Level && playerInterface != null)
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

        public void LoadNewGame()
        {
            playerCharacter = new PlayerCharacter(this);
            playerCharacter.Initialise(new Vector2(200, 128), GetTexture("Sprites/Player/Idle/Idle_Fire_SpriteSheet"), new Vector2(64, 64));
            playerCharacter.InitialisePlayerAnimations(this);
            playerCharacter.LayerOrder = -1;
            playerInterface = new PlayerInterface(this);

            playerCharacter.DisplayPlayerLives();
            GameManager.currentGameState = GameManager.GameState.Level;

            _mapManager = new MapManager(this);

            dialogueManager = new DialogueManager(this, menuManager);

            CameraController.cameraActive = true;

            string[] dialogue = { "Hello, my name is bob.", "How are you?", "That is great", "Bye now..." };

            _dialogue = new Dialogue(dialogue, "Bob");

            _dialogueTrigger = new DialogueController(playerCharacter, new Rectangle(1300, 1100, 64, 64), dialogueManager, _dialogue);




            gameInProgress = true;

            LoadAudioLibrary();
        }

        private void InitialiseManagers()
        {
            _spriteManager = new SpriteManager();
            _mainCam = new CameraController();
            CameraController.viewPort = _graphics.GraphicsDevice.Viewport;
            SpawnManager._gameManager = this;
            _inputManager = new InputManager(_mainCam, menuManager, this);
        }

        private void UpdateManagers(GameTime gameTime)
        {
            _inputManager.Update(gameTime);
            _spriteManager.Update(gameTime);
            _mainCam.MoveCamera(playerCharacter);
            SpawnManager.Update(gameTime);
        }

        private void LoadAudioLibrary()
        {
            AudioClip audioClip = new AudioClip("PlayerJump_SFX", "SFXs/PlayerJump_SFX", false, this);
            AudioManager.AddSound(audioClip);
        }

        #endregion
    }
}
