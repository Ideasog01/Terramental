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
        public static bool gameInitialised; //Set to true when game is intialised. E.g player is instantiated. Avoids multiple players/managers.
        public static bool levelLoaded; //States whether a level is loaded.

        public static int levelsComplete = 6; //When each level is completed, this value is incremented. Handles available levels and when to show end credits. Set this value to 6 to make all levels available

        public enum GameState { SplashScreen, CreditsVideo, MainMenu, Options, Credits, Level, Respawn, LevelSelect, LevelSelectConfirm, LevelPause, LevelComplete, StartScreen, HelpMenu, LoadingScreen }; //Controls the current textures and logic being activated

        public enum ButtonName { StartGameButton, OptionsButton, AchievementsButton, CreditsButton, ExitGameButton, RespawnButton, LevelSelectExit, LevelSelectConfirm, ReturnMainMenu, ResumeGame, Replay, Continue, ResolutionButton, OptionsReturn, MusicButton, ControlsButton, HelpScreenButton, SFXVolumeUp, SFXVolumeDown, MusicVolumeUp, MusicVolumeDown, ResolutionUp, ResolutionDown, DashButton }; //Each button has this value attributed

        public enum LevelButton { Level1Button, Level2Button, Level3Button, Level4Button, Level5Button, Level6Button, Level8Button }; //Each button that represents a level on the level select screen has this value attributed

        public static GameState currentGameState = GameState.SplashScreen; //The current state of the game.
        public static GameState previousGameState = GameState.SplashScreen; //The previous state of the game. Useful for return buttons in menus.

        public static int screenWidth = 960; //The game window width in pixels.
        public static int screenHeight = 540; //The game window height in pixels.

        public static int levelIndex; //The current level index. Level 1 = 1, Level 2 = 2 etc.

        public ObjectiveManager objectiveManager; //The Objective Manager handles level objectives. Includes displaying the current objective, updating the objective progress, and setting the end of level object active when the objective is complete.
        public MapManager mapManager; //The MapManager handles all processes concerning levels. Involves spawning tiles, entities and environment assets.
        public PlayerInterface playerInterface; //The player interface that handles the heads-up-display
        public MenuManager menuManager; //The MenuManager handles everything regarding menus, including drawing, checking button interaction and loading.
        public TutorialManager tutorialManager; //The TutorialManager displays information to the player in the form of text. This class also notifies the player if they use the wrong element.
        public InputManager inputManager; //The input manager controls the player's input and activates functions in other classes accordingly.
        public SpriteManager spriteManager; //The sprite manager renders all objects of type Sprite.

        public static float actualWidth; //The width of the device screen
        public static float actualHeight; //The height of the device screen

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public PlayerCharacter playerCharacter; //The player character is instantiated when a level is loaded for the first time
        public CameraController mainCam; //The main camera that follows the player's location

        private bool skipToLevel = false; //TESTING PURPOSES ONLY. SET TO FALSE ON BUILD

        public bool useDoubleTapDash; //Customisation for the dash ability. If this boolean is false, the B button or left shift key can be used instead to activate the dash ability.

        private int[] screenWidths = { 960, 1920 }; //The possible screen widths for changing resolution
        private int[] screenHeights = { 540, 1080 }; //The possible screen heights for changing resolution
        private int currentResolutionIndex; //The current index for the screenWidth and screenHeight array
        private Sprite particleSprite; //Weather particle.

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = false;

            _graphics.PreferredBackBufferHeight = screenHeight; //Gets the window's height
            _graphics.PreferredBackBufferWidth = screenWidth; //Gets the window's width
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
            actualWidth = _graphics.GraphicsDevice.Viewport.Width; //Set the actualWidth to the device screen width
            actualHeight = _graphics.GraphicsDevice.Viewport.Height;//Set the actualWidth to the device screen height

            InitialiseGame(); //
            LoadAudioLibrary();
            AudioManager.SetVolumes();
            AudioManager.PlaySound("Intro_Music");

            if (skipToLevel) //Skips the splash screen and main menu and instantly. USED FOR TESTING
            {
                LoadNewGame(@"Content/Level1Map.json");
                currentGameState = GameState.Level;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateManagers(gameTime);

            // System.Diagnostics.Debug.WriteLine("Camera Position = " + mainCam.CameraTopLeftAnchor.ToString());

            if (currentGameState == GameState.Level && playerInterface != null) //If level is active, update the appropriate objects
            {
                playerInterface.UpdatePlayerInterface();
                playerCharacter.UpdateCharacter(gameTime);
                playerCharacter.UpdatePlayerCharacter(gameTime);

                if (mapManager != null)
                {
                    mapManager.CheckActiveTiles();
                }

                if (tutorialManager != null)
                {
                    tutorialManager.UpdateDisplayMessageTimer(gameTime);
                }
            }

            menuManager.UpdateMenu(gameTime);
            mainCam.UpdateCamera(gameTime);

            if (particleSprite != null)
            {
                particleSprite.SpritePosition = CameraController.cameraWorldPos;
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, mainCam.cameraTransform);

            if (currentGameState == GameState.Level || currentGameState == GameState.LevelPause && playerInterface != null)
            {
                spriteManager.Draw(gameTime, _spriteBatch);
            }

            menuManager.DrawMenus(_spriteBatch);

            if(currentGameState == GameState.Level)
            {
                if (tutorialManager != null)
                {
                    playerInterface.DrawInterface(_spriteBatch);
                    playerInterface.DrawCooldownTexts(_spriteBatch);
                    tutorialManager.DrawMessage(_spriteBatch);
                }

                if (objectiveManager != null)
                {
                    objectiveManager.DrawObjectiveString(_spriteBatch);
                }
            }

            _spriteBatch.End();


            // System.Diagnostics.Debug.WriteLine("Draw Count = " + GraphicsDevice.Metrics.DrawCount.ToString());

            base.Draw(gameTime);
        }

        #endregion

        #region Property Tools

        public Texture2D GetTexture(string path) //Easily load textures
        {
            return Content.Load<Texture2D>(path);
        }

        #endregion

        #region Managers

        public void ExitGame() //Closes the application. Called when the main menu exit button is interacted with.
        {
            Exit();
        }

        public static void PauseGame() //Pauses the game during the level. Called when the escape key or start controller button is pressed.
        {
            currentGameState = GameState.LevelPause;
        }

        public void LoadNewGame(string filePath) //Loads the approriate level using the data located at the file path.
        {
            menuManager.ActivateLoadingScreen(5, GameState.Level);









            if (!gameInitialised) //Loads neccessary managers and the player if a level has not yet been loaded
            {
                playerCharacter = new PlayerCharacter(this);
                playerCharacter.Initialise(new Vector2(200, 128), GetTexture("Sprites/Player/Idle/Idle_Fire_SpriteSheet"), new Vector2(64, 64));
                playerCharacter.InitialisePlayerAnimations(this); //Initialise Player Animations
                playerCharacter.LayerOrder = -1;
                playerInterface = new PlayerInterface(this); //Loads the player's heads-up-display
                mapManager = new MapManager(this);
                tutorialManager = new TutorialManager(Content.Load<SpriteFont>("SpriteFont/DefaultFont"), playerCharacter); //Initialises the tutorial manager
                objectiveManager = new ObjectiveManager(Content.Load<SpriteFont>("SpriteFont/DefaultFont"));
                CameraController.playerCharacter = playerCharacter;
                gameInitialised = true;

                particleSprite = new Sprite();

                particleSprite.Initialise(Vector2.Zero, GetTexture("Sprites/Effects/Rain"), new Vector2(screenWidth, screenHeight));
                Animation rainParticle = new Animation(GetTexture("Sprites/Effects/Rain"), 4, 90f, true, new Vector2(screenWidth, screenHeight));
                particleSprite.AddAnimation(rainParticle);
                particleSprite.SetAnimation(0);
                rainParticle.AnimationActive = true;
                particleSprite.LayerOrder = -2;







            }

            if (levelIndex == 1)
            {
                objectiveManager.SetObjective(ObjectiveManager.Objective.DefeatEnemies);
            }

            if (levelIndex == 2)
            {
                objectiveManager.SetObjective(ObjectiveManager.Objective.CollectGems);
            }

            if (levelIndex == 3)
            {
                objectiveManager.SetObjective(ObjectiveManager.Objective.DefeatEnemies);
            }

            if (levelIndex == 4)
            {
                objectiveManager.SetObjective(ObjectiveManager.Objective.DefeatEnemies);
            }

            if (levelIndex == 5)
            {
                objectiveManager.SetObjective(ObjectiveManager.Objective.CollectGems);
            }

            if (levelIndex == 8)
            {
                objectiveManager.SetObjective(ObjectiveManager.Objective.CollectGems);
            }

            playerCharacter.ResetPlayer(); //Resets the player's variables and properties
            playerCharacter.DisplayPlayerLives();
            CameraController.cameraWorldPos = playerCharacter.SpritePosition;
            mapManager.LoadMapData(filePath); //Loads all tiles, entities and environment assets
            levelLoaded = true;


            if (levelIndex < 4)
            {
                particleSprite.Animations[particleSprite.AnimationIndex].SpriteSheet = GetTexture("Sprites/Effects/Rain");
            }
            if (levelIndex == 4)
            {
                particleSprite.Animations[particleSprite.AnimationIndex].SpriteSheet = GetTexture("Sprites/Effects/Snow");
            }

            if (levelIndex == 5)
            {
                particleSprite.Animations[particleSprite.AnimationIndex].SpriteSheet = GetTexture("Sprites/Effects/Ashes");
            }

            if (levelIndex == 6)
            {
                particleSprite.Animations[particleSprite.AnimationIndex].SpriteSheet = GetTexture("Sprites/Effects/Ashes");
            }

        }

        private void InitialiseGame() //Loads the appropriate managers at the start of runtime
        {
            spriteManager = new SpriteManager();
            mainCam = new CameraController(_graphics.GraphicsDevice.Viewport);
            SpawnManager.gameManager = this;
            inputManager = new InputManager(mainCam, menuManager, this);
        }

        private void UpdateManagers(GameTime gameTime)
        {
            inputManager.Update(gameTime);

            if (currentGameState == GameState.Level)
            {
                spriteManager.Update(gameTime);
                SpawnManager.Update(gameTime);
            }
        }


        private void LoadAudioLibrary()//Loads all sounds effects and music
        {
            AudioClip audioClip = new AudioClip("PlayerJump_SFX", "SFXs/Jump_SFX", false, this);
            AudioManager.AddSound(audioClip);

            AudioClip beepSound = new AudioClip("BeepTone_SFX", "SFXs/BeepTone", false, this);
            AudioManager.AddSound(beepSound);

            AudioClip menuButtonSound = new AudioClip("MenuButton_SFX", "SFXs/MenuButton_SFX", false, this);
            AudioManager.AddSound(menuButtonSound);

            AudioClip healthPickupSound = new AudioClip("HealthPickup_SFX", "SFXs/HealthPickup_SFX", false, this);
            AudioManager.AddSound(healthPickupSound);

            AudioClip fireProjectileSound = new AudioClip("FireProjectile_SFX", "SFXs/FireProjectile_SFX", false, this);
            AudioManager.AddSound(fireProjectileSound);

            AudioClip waterProjectileSound = new AudioClip("WaterProjectile_SFX", "SFXs/WaterProjectile_SFX", false, this);
            AudioManager.AddSound(waterProjectileSound);

            AudioClip snowProjectileSound = new AudioClip("SnowProjectile_SFX", "SFXs/SnowProjectile_SFX", false, this);
            AudioManager.AddSound(snowProjectileSound);

            AudioClip ultimateActivationSound = new AudioClip("UltimateActivation_SFX", "SFXs/UltimateActivation_SFX", false, this);
            AudioManager.AddSound(ultimateActivationSound);

            AudioClip hitSound = new AudioClip("Hit_SFX", "SFXs/Damage_SFX", false, this);
            AudioManager.AddSound(hitSound);

            AudioClip scorePickupSound = new AudioClip("ScorePickup_SFX", "SFXs/ScorePickup_SFX", false, this);
            AudioManager.AddSound(scorePickupSound);

            AudioClip jumpSound = new AudioClip("Jump_SFX", "SFXs/Jump_SFX", false, this);
            AudioManager.AddSound(jumpSound);

            AudioClip swordSound = new AudioClip("Sword_SFX", "SFXs/Sword_SFX", false, this);
            AudioManager.AddSound(swordSound);

            AudioClip defeatSound = new AudioClip("Defeat_SFX", "SFXs/Defeat_SFX", false, this);
            AudioManager.AddSound(defeatSound);

            AudioClip wallBreakSound = new AudioClip("WallBreak_SFX", "SFXs/ElementWallBreak_SFX", false, this);
            AudioManager.AddSound(wallBreakSound);

            AudioClip dashSound = new AudioClip("Dash_SFX", "SFXs/Dash_SFX", false, this);
            AudioManager.AddSound(dashSound);

            AudioClip fireExplosion = new AudioClip("FireExplosion_SFX", "SFXs/FireExplosion_SFX", false, this);
            AudioManager.AddSound(fireExplosion);

            AudioClip waterExplosion = new AudioClip("WaterExplosion_SFX", "SFXs/WaterExplosion_SFX", false, this);
            AudioManager.AddSound(waterExplosion);

            AudioClip snowExplosion = new AudioClip("SnowExplosion_SFX", "SFXs/SnowExplosion_SFX", false, this);
            AudioManager.AddSound(snowExplosion);

            AudioClip negativeElementSound = new AudioClip("NegativeElement_SFX", "SFXs/NegativeElement_SFX", false, this);
            AudioManager.AddSound(negativeElementSound);

            AudioClip cannonSound = new AudioClip("CannonFire_SFX", "SFXs/CannonFire_SFX", false, this);
            AudioManager.AddSound(cannonSound);

            AudioClip positiveSound = new AudioClip("Positive_SFX", "SFXs/Positive_SFX", false, this);
            AudioManager.AddSound(positiveSound);

            //Music

            AudioClip introMusic = new AudioClip("Intro_Music", "Music/far-from-home-acoustic-version-13463", true, this);
            AudioManager.AddSound(introMusic);

            AudioClip levelMusic = new AudioClip("Level_Music", "Music/ambient-piano-10781", true, this);
            AudioManager.AddSound(levelMusic);
        }

        public void ChangeResolution(int amount) //Changes the resolution of the game
        {
            currentResolutionIndex += amount;

            if (currentResolutionIndex < 0)
            {
                currentResolutionIndex = screenWidths.Length - 1;
            }

            if (currentResolutionIndex > screenWidths.Length - 1)
            {
                currentResolutionIndex = 0;
            }

            _graphics.PreferredBackBufferWidth = screenWidths[currentResolutionIndex];
            _graphics.PreferredBackBufferHeight = screenHeights[currentResolutionIndex];
            screenWidth = screenWidths[currentResolutionIndex];
            screenHeight = screenHeights[currentResolutionIndex];

            if (screenWidth == _graphics.GraphicsDevice.Viewport.Width && screenHeight == _graphics.GraphicsDevice.Viewport.Height)
            {
                _graphics.IsFullScreen = true;
            }
            else
            {
                _graphics.IsFullScreen = false;
            }

            _graphics.ApplyChanges();
            System.Console.WriteLine("New resolution: {0} x {1}", _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }


        #endregion
    }
}
