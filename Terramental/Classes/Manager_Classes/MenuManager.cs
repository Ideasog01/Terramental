using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Terramental
{
    public class MenuManager
    {
        public static List<Button> mainMenuButtonList = new List<Button>();
        public static List<MenuComponent> mainMenuComponentList = new List<MenuComponent>();

        public static List<Button> respawnMenuButtonList = new List<Button>();
        public static List<MenuComponent> respawnMenuComponentList = new List<MenuComponent>();

        public static List<Button> levelSelectButtonList = new List<Button>();
        public static List<MenuComponent> levelSelectComponentList = new List<MenuComponent>();

        public static List<Button> confirmLevelButtonList = new List<Button>();
        public static List<MenuComponent> confirmLevelComponentList = new List<MenuComponent>();

        public static List<Button> pauseMenuButtonList = new List<Button>();
        public static List<MenuComponent> pauseMenuComponentList = new List<MenuComponent>();

        public static List<MenuComponent> completeMenuComponentList = new List<MenuComponent>();
        public static List<Button> completeMenuButtonList = new List<Button>();

        public static List<Button> loadGameButtonList = new List<Button>();

        public static List<Button> newGameButtonList = new List<Button>();

        public static List<Button> difficultyButtonList = new List<Button>();

        public int currentButtonIndex;

        public Video splashScreenVideo;
        public VideoPlayer videoPlayer;
        public Texture2D videoTexture;
        public Rectangle videoRectangle;

        public Button creditsReturnButton;
        public MenuComponent creditsBackground;

        public MenuComponent loadGameBackground;

        public MenuComponent newGameBackground;

        public MenuComponent difficultyBackground;

        private GameManager _gameManager;
        private GraphicsDeviceManager _graphics;

        private SpriteFont _defaultFont;
        private SpriteFont _levelTitleFont;

        private string _levelNameText;
        private string _levelDescriptionText;
        private string _levelDataFilePath;

        private bool _verticalMenu;
        private float _gamePadButtonTimer;

        private float videoTimer;
        private bool videoPlaying;

        private float _buttonBuffer;

        private List<Button> _buttonList = new List<Button>();

        public MenuManager(GameManager gameManager, GraphicsDeviceManager graphics)
        {
            _gameManager = gameManager;
            _graphics = graphics;

            _defaultFont = _gameManager.Content.Load<SpriteFont>("SpriteFont/LevelNameFont");
            _levelTitleFont = _gameManager.Content.Load<SpriteFont>("SpriteFont/LevelTitleFont");

            LoadMainMenu();
            LoadRespawnScreen();
            LoadLevelSelect();
            LoadPauseMenu();
            LoadCreditsMenu();
            LoadLoadGameMenu();
            LoadSplashScreens();
            LoadLevelCompleteMenu();
            LoadNewGameMenu();
            LoadDifficultySelectMenu();

            currentButtonIndex = 0;
            mainMenuButtonList[0].ComponentColor = Color.Gray;
        }

        public void DrawMenus(SpriteBatch spriteBatch)
        {
            switch(GameManager.currentGameState)
            {
                case GameManager.GameState.MainMenu:
                     foreach(MenuComponent mainComponent in mainMenuComponentList)
                     {
                        mainComponent.DrawMenuComponent(spriteBatch);
                     }

                     foreach (Button mainButton in mainMenuButtonList)
                     {
                        mainButton.DrawMenuComponent(spriteBatch);
                     }
                 break;

                case GameManager.GameState.Respawn:
                    foreach (MenuComponent respawnComponent in respawnMenuComponentList)
                    {
                        respawnComponent.DrawMenuComponent(spriteBatch);
                    }

                    foreach (Button respawnButton in respawnMenuButtonList)
                    {
                        respawnButton.DrawMenuComponent(spriteBatch);
                    }
                 break;

                case GameManager.GameState.LevelSelect:
                    foreach(MenuComponent selectComponent in levelSelectComponentList)
                    {
                        selectComponent.DrawMenuComponent(spriteBatch);
                    }

                    foreach(Button selectButton in levelSelectButtonList)
                    {
                        selectButton.DrawMenuComponent(spriteBatch);
                    }

                    spriteBatch.DrawString(_defaultFont, "The Golden Shores", new Vector2(150, 380), Color.Black);
                    spriteBatch.DrawString(_defaultFont, "1", new Vector2(207, 403), Color.White);

                    spriteBatch.DrawString(_defaultFont, "The Fire Lands", new Vector2(565, 400), Color.Black);
                    spriteBatch.DrawString(_defaultFont, "2", new Vector2(609, 423), Color.White);
                    break;

                case GameManager.GameState.LevelSelectConfirm:
                    foreach (MenuComponent selectComponent in levelSelectComponentList)
                    {
                        selectComponent.DrawMenuComponent(spriteBatch);
                    }

                    foreach (Button selectButton in levelSelectButtonList)
                    {
                        selectButton.DrawMenuComponent(spriteBatch);
                    }

                    spriteBatch.DrawString(_defaultFont, "The Golden Shores", new Vector2(150, 380), Color.Black);
                    spriteBatch.DrawString(_defaultFont, "1", new Vector2(207, 403), Color.White);

                    spriteBatch.DrawString(_defaultFont, "The Fire Lands", new Vector2(550, 430), Color.Black);
                    spriteBatch.DrawString(_defaultFont, "2", new Vector2(607, 453), Color.White);


                    foreach (MenuComponent confirmComponent in confirmLevelComponentList)
                    {
                        confirmComponent.DrawMenuComponent(spriteBatch);
                    }

                    foreach (Button confrimButton in confirmLevelButtonList)
                    {
                        confrimButton.DrawMenuComponent(spriteBatch);
                    }

                    spriteBatch.DrawString(_levelTitleFont, _levelNameText, new Vector2((GameManager.screenWidth / 2) - 130, 120), Color.White);
                    spriteBatch.DrawString(_defaultFont, _levelDescriptionText, new Vector2((GameManager.screenWidth / 2) - 150, 180), Color.White);

                    break;

                case GameManager.GameState.LevelPause:

                    foreach (MenuComponent pauseComponent in pauseMenuComponentList)
                    {
                        pauseComponent.DrawMenuComponent(spriteBatch);
                        pauseComponent.FollowCamera();
                    }

                    foreach (Button pauseButton in pauseMenuButtonList)
                    {
                        pauseButton.DrawMenuComponent(spriteBatch);
                        pauseButton.FollowCamera();
                    }

                    break;

                case GameManager.GameState.Credits:

                    creditsBackground.DrawMenuComponent(spriteBatch);
                    creditsReturnButton.DrawMenuComponent(spriteBatch);

                    break;

                case GameManager.GameState.LoadGame:

                    loadGameBackground.DrawMenuComponent(spriteBatch);

                    foreach (Button loadButton in loadGameButtonList)
                    {
                        loadButton.DrawMenuComponent(spriteBatch);
                    }

                    break;

                case GameManager.GameState.SplashScreen:

                    if (videoPlayer.State == MediaState.Stopped)
                    {
                        videoPlayer.Play(splashScreenVideo);
                    }

                    if (videoPlayer.State == MediaState.Playing)
                    {
                        videoTexture = videoPlayer.GetTexture();
                        spriteBatch.Draw(videoTexture, videoRectangle, Color.White);
                    }
                    
                    break;

                case GameManager.GameState.LevelComplete:

                    foreach(MenuComponent completeComponent in completeMenuComponentList)
                    {
                        completeComponent.DrawMenuComponent(spriteBatch);
                    }

                    foreach (Button completeButton in completeMenuButtonList)
                    {
                        completeButton.DrawMenuComponent(spriteBatch);
                    }

                    spriteBatch.DrawString(_levelTitleFont, "Score: " + _gameManager.playerCharacter.PlayerScore.ToString(), new Vector2(200, 200), Color.White);

                    break;

                case GameManager.GameState.NewGame:

                    newGameBackground.DrawMenuComponent(spriteBatch);

                    foreach(Button newGameButton in newGameButtonList)
                    {
                        newGameButton.DrawMenuComponent(spriteBatch);
                    }

                    break;

                case GameManager.GameState.DifficultySelect:

                    difficultyBackground.DrawMenuComponent(spriteBatch);

                    foreach (Button difficultyButton in difficultyButtonList)
                    {
                        difficultyButton.DrawMenuComponent(spriteBatch);
                    }

                    break;
            }
        }

        public void UpdateMenuButtons(GameTime gameTime)
        {
            if(_buttonBuffer > 0)
            {
                _buttonBuffer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(_gamePadButtonTimer > 0)
            {
                _gamePadButtonTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(videoPlaying && GameManager.currentGameState == GameManager.GameState.SplashScreen)
            {
                if (videoTimer > 0)
                {
                    videoTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    GameManager.currentGameState = GameManager.GameState.MainMenu;
                    videoPlaying = false;
                }
            }
            
        }

        public void EndLevel()
        {
            GameManager.currentGameState = GameManager.GameState.LevelComplete;
            _gameManager.IsMouseVisible = true;
        }

        public void ChangeSelectedButton(int amount, bool vertical)
        {
            if (GameManager.currentGameState != GameManager.GameState.SplashScreen && GameManager.currentGameState != GameManager.GameState.Level && _gamePadButtonTimer <= 0)
            {
                switch (GameManager.currentGameState)
                {
                    case GameManager.GameState.MainMenu:
                        _buttonList = mainMenuButtonList;
                        _verticalMenu = true;
                        break;
                    case GameManager.GameState.LevelSelect:
                        _buttonList = levelSelectButtonList;
                        _verticalMenu = false;
                        break;
                    case GameManager.GameState.LevelSelectConfirm:
                        _buttonList = confirmLevelButtonList;
                        _verticalMenu = true;
                        break;
                    case GameManager.GameState.LoadGame:
                        _buttonList = loadGameButtonList;
                        _verticalMenu = true;
                        break;
                    case GameManager.GameState.LevelPause:
                        _buttonList = pauseMenuButtonList;
                        _verticalMenu = true;
                        break;
                    case GameManager.GameState.Respawn:
                        _buttonList = respawnMenuButtonList;
                        _verticalMenu = false;
                        break;
                    case GameManager.GameState.LevelComplete:
                        _buttonList = completeMenuButtonList;
                        _verticalMenu = true;
                        break;
                    case GameManager.GameState.NewGame:
                        _buttonList = newGameButtonList;
                        _verticalMenu = false;
                        break;
                    case GameManager.GameState.DifficultySelect:
                        _buttonList = difficultyButtonList;
                        _verticalMenu = true;
                        break;
                }

                if (_verticalMenu == vertical)
                {
                    _buttonList[currentButtonIndex].ComponentColor = Color.White;

                    currentButtonIndex -= amount;

                    if (currentButtonIndex >= _buttonList.Count)
                    {
                        currentButtonIndex = 0;
                    }

                    if (currentButtonIndex < 0)
                    {
                        currentButtonIndex = _buttonList.Count - 1;
                    }

                    _buttonList[currentButtonIndex].ComponentColor = Color.Gray;
                }

                _gamePadButtonTimer = 0.25f;
            }
        }

        public void InteractSelectedButton()
        {
            if(GameManager.currentGameState != GameManager.GameState.Level)
            {
                if (currentButtonIndex < _buttonList.Count && _buttonBuffer <= 0)
                {
                    Button button = _buttonList[currentButtonIndex];
                    button.ComponentColor = Color.White;

                    if (GameManager.currentGameState == GameManager.GameState.LoadGame)
                    {
                        button.LoadGame();
                    }
                    else if (GameManager.currentGameState == GameManager.GameState.LevelSelect)
                    {
                        LevelSelectButtonInteraction(button.LevelButtonName);
                    }

                    ButtonInteraction(button.ButtonName);

                    currentButtonIndex = 0;
                }
            }
        }

        public void MouseClick(Vector2 mousePos)
        {
            if(_buttonBuffer <= 0)
            {
                if (GameManager.currentGameState == GameManager.GameState.NewGame)
                {
                    foreach (Button button in newGameButtonList)
                    {
                        button.SetPlayerData(mousePos);
                    }
                }

                if (GameManager.currentGameState == GameManager.GameState.DifficultySelect)
                {
                    foreach (Button button in difficultyButtonList)
                    {
                        button.CheckInteraction(mousePos);
                    }
                }

                if (GameManager.currentGameState == GameManager.GameState.MainMenu)
                {
                    foreach (Button button in mainMenuButtonList)
                    {
                        button.CheckInteraction(mousePos);
                    }
                }

                if (GameManager.currentGameState == GameManager.GameState.Respawn)
                {
                    foreach (Button button in respawnMenuButtonList)
                    {
                        button.CheckInteraction(mousePos);
                    }
                }

                if (GameManager.currentGameState == GameManager.GameState.LevelSelect)
                {
                    foreach (Button button in levelSelectButtonList)
                    {
                        button.CheckInteractionLevel(mousePos);
                    }
                }

                if (GameManager.currentGameState == GameManager.GameState.LevelSelectConfirm)
                {
                    foreach (Button button in confirmLevelButtonList)
                    {
                        button.CheckInteraction(mousePos);
                    }
                }

                if (GameManager.currentGameState == GameManager.GameState.LevelPause)
                {
                    foreach (Button button in pauseMenuButtonList)
                    {
                        button.CheckInteraction(mousePos);
                    }
                }

                if (GameManager.currentGameState == GameManager.GameState.Credits)
                {
                    creditsReturnButton.CheckInteraction(mousePos);
                }

                if (GameManager.currentGameState == GameManager.GameState.LoadGame && _buttonBuffer <= 0)
                {
                    foreach (Button button in loadGameButtonList)
                    {
                        button.CheckInteractionLoad(mousePos);
                    }
                }

                if(GameManager.currentGameState == GameManager.GameState.LevelComplete)
                {
                    foreach(Button button in completeMenuButtonList)
                    {
                        button.CheckInteraction(mousePos);
                    }
                }
            }

        }

        public void SetPlayerData(GameManager.GameData gameData)
        {
            if(gameData == GameManager.GameData.Game1)
            {
                GameManager.playerCheckpoint = Vector2.Zero;
                SaveManager.currentData = GameManager.GameData.Game1;
                SaveManager.SaveGame();
            }

            GameManager.currentGameState = GameManager.GameState.DifficultySelect;
        }

        public void ButtonInteraction(GameManager.ButtonName buttonName)
        {
            if(_buttonBuffer <= 0)
            {
                switch (buttonName)
                {
                    case GameManager.ButtonName.NewGameButton: //Display New Game Menu
                        GameManager.currentGameState = GameManager.GameState.NewGame;
                        break;
                    case GameManager.ButtonName.ExitGameButton:
                        _gameManager.ExitGame();
                        break;
                    case GameManager.ButtonName.RespawnButton:
                        DisplayRespawnScreen(false);
                        _gameManager.playerCharacter.ResetPlayer();
                        break;
                    case GameManager.ButtonName.LevelSelectConfirm:
                        LoadLevel();
                        break;
                    case GameManager.ButtonName.LevelSelectExit:
                        GameManager.currentGameState = GameManager.GameState.LevelSelect;
                        break;

                    case GameManager.ButtonName.ReturnMainMenu:
                        GameManager.currentGameState = GameManager.GameState.MainMenu;
                        break;

                    case GameManager.ButtonName.ResumeGame:
                        GameManager.currentGameState = GameManager.GameState.Level;
                        _gameManager.IsMouseVisible = false;
                        break;
                    case GameManager.ButtonName.CreditsButton:
                        GameManager.currentGameState = GameManager.GameState.Credits;
                        break;
                    case GameManager.ButtonName.LoadGameButton:
                        GameManager.currentGameState = GameManager.GameState.LoadGame;
                        break;
                    case GameManager.ButtonName.Replay:
                        GameManager.currentGameState = GameManager.GameState.Level;
                        _gameManager.mapManager.ResetLevel();
                        break;
                    case GameManager.ButtonName.Continue:
                        GameManager.currentGameState = GameManager.GameState.LevelSelect;
                        break;
                    case GameManager.ButtonName.ApprenticeButton:
                        GameManager.currentGameState = GameManager.GameState.LevelSelect;
                        _gameManager.difficultyIndex = 0;
                        break;
                    case GameManager.ButtonName.HeroButton:
                        GameManager.currentGameState = GameManager.GameState.LevelSelect;
                        _gameManager.difficultyIndex = 1;
                        break;
                    case GameManager.ButtonName.MasterButton:
                        GameManager.currentGameState = GameManager.GameState.LevelSelect;
                        _gameManager.difficultyIndex = 2;
                        break;
                }

                _buttonBuffer = 0.5f;
            }
            
        }

        public void LevelSelectButtonInteraction(GameManager.LevelButton buttonName)
        {
            switch(buttonName)
            {
                case GameManager.LevelButton.Level1Button:
                    _levelDataFilePath = @"MapData.json";
                    _levelNameText = "The Golden Shores";
                    _levelDescriptionText = "Explore the golden shores, and defeat \nthe armies of the Fire Lands.";
                    GameManager.levelIndex = 0;
                    GameManager.currentGameState = GameManager.GameState.LevelSelectConfirm;
                    break;
                case GameManager.LevelButton.Level2Button:
                    _levelDataFilePath = @"MapData.json";
                    _levelNameText = "The Fire Lands";
                    _levelDescriptionText = "Venture to the fire lands, and use the \nelements to defeat the armies \nof Magnus.";
                    GameManager.currentGameState = GameManager.GameState.LevelSelectConfirm;
                    GameManager.levelIndex = 1;
                    break;
            }
        }

        public void DisplayMainMenu(bool isActive)
        {
            if(isActive)
            {
                GameManager.currentGameState = GameManager.GameState.MainMenu;
                _gameManager.IsMouseVisible = true;
            }
            else
            {
                GameManager.currentGameState = GameManager.GameState.Level;
                _gameManager.IsMouseVisible = false;
            }
        }

        public void DisplayRespawnScreen(bool isActive)
        {
            if(isActive)
            {
                GameManager.currentGameState = GameManager.GameState.Respawn;
                _gameManager.IsMouseVisible = true;
            }
            else
            {
                GameManager.currentGameState = GameManager.GameState.Level;
                _gameManager.IsMouseVisible = false;
            }
        }

        private void LoadLevel()
        {
            _gameManager.LoadNewGame(@"MapData.json");
        }

        private void LoadMainMenu()
        {
            int viewportCentreX = _graphics.PreferredBackBufferWidth / 2;

            Texture2D mainMenuBackgroundTexture = _gameManager.GetTexture("UserInterface/MainMenu/MainMenu_FireBackground");
            MenuComponent mainMenuBackground = new MenuComponent();
            mainMenuBackground.InitialiseMenuComponent(mainMenuBackgroundTexture, new Vector2(0, 0), new Vector2(GameManager.screenWidth, GameManager.screenHeight));
            mainMenuComponentList.Add(mainMenuBackground);

            Texture2D titleTextTexture = _gameManager.GetTexture("UserInterface/MainMenu/GameTitleText");
            MenuComponent titleText = new MenuComponent();
            titleText.InitialiseMenuComponent(titleTextTexture, new Vector2(viewportCentreX - (titleTextTexture.Width / 2), 0), new Vector2(titleTextTexture.Width, titleTextTexture.Height));
            mainMenuComponentList.Add(titleText);

            Texture2D newGameButtonTexture = _gameManager.GetTexture("UserInterface/MainMenu/NewGame_MainMenu_Button");
            Button newGameButton = new Button(GameManager.ButtonName.NewGameButton, this);
            newGameButton.InitialiseMenuComponent(newGameButtonTexture, new Vector2(viewportCentreX - (newGameButtonTexture.Width / 2), 125), new Vector2(256, 64));
            mainMenuButtonList.Add(newGameButton);

            Texture2D loadGameButtonTexture = _gameManager.GetTexture("UserInterface/MainMenu/LoadGame_MainMenu_Button");
            Button loadGameButton = new Button(GameManager.ButtonName.LoadGameButton, this);
            loadGameButton.InitialiseMenuComponent(loadGameButtonTexture, new Vector2(viewportCentreX - (loadGameButtonTexture.Width / 2), 205), new Vector2(256, 64));
            mainMenuButtonList.Add(loadGameButton);

            Texture2D optionsButtonTexture = _gameManager.GetTexture("UserInterface/MainMenu/Options_MainMenu_Button");
            Button optionsButton = new Button(GameManager.ButtonName.OptionsButton, this);
            optionsButton.InitialiseMenuComponent(optionsButtonTexture, new Vector2(viewportCentreX - (optionsButtonTexture.Width / 2), 285), new Vector2(256, 64));
            mainMenuButtonList.Add(optionsButton);

            Texture2D achievementsButtonTexture = _gameManager.GetTexture("UserInterface/MainMenu/Achievements_MainMenu_Button");
            Button achievementsButton = new Button(GameManager.ButtonName.AchievementsButton, this);
            achievementsButton.InitialiseMenuComponent(achievementsButtonTexture, new Vector2(viewportCentreX - (achievementsButtonTexture.Width / 2), 285), new Vector2(256, 64));
            mainMenuButtonList.Add(achievementsButton);

            Texture2D creditsButtonTexture = _gameManager.GetTexture("UserInterface/MainMenu/Credits_MainMenu_Button");
            Button creditsButton = new Button(GameManager.ButtonName.CreditsButton, this);
            creditsButton.InitialiseMenuComponent(creditsButtonTexture, new Vector2(viewportCentreX - (creditsButtonTexture.Width / 2), 365), new Vector2(256, 64));
            mainMenuButtonList.Add(creditsButton);

            Texture2D exitButtonTexture = _gameManager.GetTexture("UserInterface/MainMenu/ExitGame_MainMenu_Button");
            Button exitButton = new Button(GameManager.ButtonName.ExitGameButton, this);
            exitButton.InitialiseMenuComponent(exitButtonTexture, new Vector2(viewportCentreX - (exitButtonTexture.Width / 2), 445), new Vector2(256, 64));
            mainMenuButtonList.Add(exitButton);

            _gameManager.IsMouseVisible = true;
        }

        private void LoadRespawnScreen()
        {
            Texture2D respawnBackgroundTexture = _gameManager.GetTexture("UserInterface/RespawnScreen/RespawnScreen");
            MenuComponent respawnBackground = new MenuComponent();
            respawnBackground.InitialiseMenuComponent(respawnBackgroundTexture, new Vector2(0, 0), new Vector2(respawnBackgroundTexture.Width / 2, respawnBackgroundTexture.Height / 2));
            respawnMenuComponentList.Add(respawnBackground);

            Texture2D respawnButtonTexture = _gameManager.GetTexture("UserInterface/RespawnScreen/RespawnButton");
            Button respawnButton = new Button(GameManager.ButtonName.RespawnButton, this);
            respawnButton.InitialiseMenuComponent(respawnButtonTexture, new Vector2(_graphics.PreferredBackBufferWidth / 2 - (respawnButtonTexture.Width / 2), 365), new Vector2(256, 64));
            respawnMenuButtonList.Add(respawnButton);

            _gameManager.IsMouseVisible = true;
        }

        private void LoadLevelSelect()
        {
            Texture2D terraMapTexture = _gameManager.GetTexture("UserInterface/LevelSelect/Terra_Map");

            MenuComponent terraMap = new MenuComponent();
            terraMap.InitialiseMenuComponent(terraMapTexture, new Vector2(0, 0), new Vector2(terraMapTexture.Width / 2, terraMapTexture.Height / 2));

            Button levelOneSelect = new Button(GameManager.LevelButton.Level1Button, this);
            levelOneSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/LevelSelectButton"), new Vector2(200, 400), new Vector2(24, 24));

            Button levelTwoSelect = new Button(GameManager.LevelButton.Level2Button, this);
            levelTwoSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/LevelSelectButton"), new Vector2(600, 420), new Vector2(24, 24));

            Texture2D confirmPanelTexture = _gameManager.GetTexture("UserInterface/LevelSelect/LevelDetailsPanel");
            MenuComponent confirmPanel = new MenuComponent();
            confirmPanel.InitialiseMenuComponent(confirmPanelTexture, new Vector2((GameManager.screenWidth / 2) - confirmPanelTexture.Width / 2, (GameManager.screenHeight / 2) - confirmPanelTexture.Height / 2), new Vector2(confirmPanelTexture.Width, confirmPanelTexture.Height));

            Texture2D confirmExitTexture = _gameManager.GetTexture("UserInterface/LevelSelect/ExitButton");
            Button confirmExitButton = new Button(GameManager.ButtonName.LevelSelectExit, this);
            confirmExitButton.InitialiseMenuComponent(confirmExitTexture, new Vector2((GameManager.screenWidth / 2) - confirmPanelTexture.Width / 2, (GameManager.screenHeight / 2) - confirmPanelTexture.Height / 2), new Vector2(confirmExitTexture.Width / 2, confirmExitTexture.Height / 2));

            Texture2D confirmButtonTexture = _gameManager.GetTexture("UserInterface/LevelSelect/StartButton");
            Button confirmButton = new Button(GameManager.ButtonName.LevelSelectConfirm, this);
            confirmButton.InitialiseMenuComponent(confirmButtonTexture, new Vector2(((GameManager.screenWidth / 2) - confirmPanelTexture.Width / 2) + (confirmPanelTexture.Width / 2) - (confirmButtonTexture.Width / 2), 380), new Vector2(confirmButtonTexture.Width, confirmButtonTexture.Height));

            Texture2D returnButtonTexture = _gameManager.GetTexture("UserInterface/LevelSelect/ExitButton");
            Button levelSelectReturnButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            levelSelectReturnButton.InitialiseMenuComponent(returnButtonTexture, new Vector2(0, 0), new Vector2(returnButtonTexture.Width / 2, returnButtonTexture.Height / 2));

            levelSelectComponentList.Add(terraMap);

            levelSelectButtonList.Add(levelOneSelect);
            levelSelectButtonList.Add(levelTwoSelect);
            levelSelectButtonList.Add(levelSelectReturnButton);

            confirmLevelComponentList.Add(confirmPanel);

            confirmLevelButtonList.Add(confirmExitButton);
            confirmLevelButtonList.Add(confirmButton);

        }

        private void LoadPauseMenu()
        {
            Texture2D pauseMenuPanelTexture = _gameManager.GetTexture("UserInterface/PauseMenu/PauseMenu");
            MenuComponent pauseMenuPanel = new MenuComponent();
            pauseMenuPanel.InitialiseMenuComponent(pauseMenuPanelTexture, new Vector2((GameManager.screenWidth / 2) - (pauseMenuPanelTexture.Width / 2), (GameManager.screenHeight / 2) - (pauseMenuPanelTexture.Height / 2)), new Vector2(pauseMenuPanelTexture.Width, pauseMenuPanelTexture.Height));

            Texture2D resumeButtonTexture = _gameManager.GetTexture("UserInterface/PauseMenu/ResumeButton");
            Button resumeButton = new Button(GameManager.ButtonName.ResumeGame, this);
            resumeButton.InitialiseMenuComponent(resumeButtonTexture, new Vector2((GameManager.screenWidth / 2) - (pauseMenuPanelTexture.Width / 2) + 50, (GameManager.screenHeight / 2) - (pauseMenuPanelTexture.Height / 2) + 157), new Vector2(resumeButtonTexture.Width, resumeButtonTexture.Height));

            Texture2D optionsButtonTexture = _gameManager.GetTexture("UserInterface/PauseMenu/OptionsButton");
            Button optionsButton = new Button(GameManager.ButtonName.OptionsButton, this);
            optionsButton.InitialiseMenuComponent(optionsButtonTexture, new Vector2((GameManager.screenWidth / 2) - (pauseMenuPanelTexture.Width / 2) + 49, (GameManager.screenHeight / 2) - (pauseMenuPanelTexture.Height / 2) + 245), new Vector2(optionsButtonTexture.Width, optionsButtonTexture.Height));

            Texture2D mainMenuTexture = _gameManager.GetTexture("UserInterface/PauseMenu/MainMenuButton");
            Button mainMenuButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            mainMenuButton.InitialiseMenuComponent(mainMenuTexture, new Vector2((GameManager.screenWidth / 2) - (pauseMenuPanelTexture.Width / 2) + 50, (GameManager.screenHeight / 2) - (pauseMenuPanelTexture.Height / 2) + 332), new Vector2(mainMenuTexture.Width, mainMenuTexture.Height));

            pauseMenuButtonList.Add(resumeButton);
            pauseMenuButtonList.Add(optionsButton);
            pauseMenuButtonList.Add(mainMenuButton);

            pauseMenuComponentList.Add(pauseMenuPanel);
        
        }

        private void LoadCreditsMenu()
        {
            Texture2D creditsBackgroundTexture = _gameManager.GetTexture("UserInterface/CreditsMenu/Credits");
            creditsBackground = new MenuComponent();
            creditsBackground.InitialiseMenuComponent(creditsBackgroundTexture, new Vector2(0, 0), new Vector2(GameManager.screenWidth, GameManager.screenHeight));

            Texture2D creditsReturnTexture = _gameManager.GetTexture("UserInterface/CreditsMenu/ReturnButton");
            creditsReturnButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            creditsReturnButton.InitialiseMenuComponent(creditsReturnTexture, new Vector2(10, 460), new Vector2(creditsReturnTexture.Width, creditsReturnTexture.Height));
        }

        private void LoadLoadGameMenu()
        {
            Texture2D loadGameBackgroundTexture = _gameManager.GetTexture("UserInterface/LoadGameMenu/LoadGameBackground");
            loadGameBackground = new MenuComponent();
            loadGameBackground.InitialiseMenuComponent(loadGameBackgroundTexture, new Vector2(0, 0), new Vector2(GameManager.screenWidth, GameManager.screenHeight));

            Texture2D loadGameReturnButtonTexture = _gameManager.GetTexture("UserInterface/CreditsMenu/ReturnButton");
            Button loadGameReturnButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            loadGameReturnButton.InitialiseMenuComponent(loadGameReturnButtonTexture, new Vector2((GameManager.screenWidth / 2) - (loadGameReturnButtonTexture.Width / 2), 470), new Vector2(loadGameReturnButtonTexture.Width, loadGameReturnButtonTexture.Height));

            Texture2D game1ButtonTexture = _gameManager.GetTexture("UserInterface/LoadGameMenu/Game1Button");
            Button game1Button = new Button(GameManager.GameData.Game1, this);
            game1Button.InitialiseMenuComponent(game1ButtonTexture, new Vector2((GameManager.screenWidth / 2) - (loadGameReturnButtonTexture.Width / 2), 140), new Vector2(game1ButtonTexture.Width, game1ButtonTexture.Height));

            Texture2D game2ButtonTexture = _gameManager.GetTexture("UserInterface/LoadGameMenu/Game2Button");
            Button game2Button = new Button(GameManager.GameData.Game2, this);
            game2Button.InitialiseMenuComponent(game2ButtonTexture, new Vector2((GameManager.screenWidth / 2) - (loadGameReturnButtonTexture.Width / 2), 210), new Vector2(game2ButtonTexture.Width, game2ButtonTexture.Height));

            Texture2D game3ButtonTexture = _gameManager.GetTexture("UserInterface/LoadGameMenu/Game3Button");
            Button game3Button = new Button(GameManager.GameData.Game3, this);
            game3Button.InitialiseMenuComponent(game3ButtonTexture, new Vector2((GameManager.screenWidth / 2) - (loadGameReturnButtonTexture.Width / 2), 280), new Vector2(game3ButtonTexture.Width, game3ButtonTexture.Height));

            Texture2D game4ButtonTexture = _gameManager.GetTexture("UserInterface/LoadGameMenu/Game4Button");
            Button game4Button = new Button(GameManager.GameData.Game4, this);
            game4Button.InitialiseMenuComponent(game4ButtonTexture, new Vector2((GameManager.screenWidth / 2) - (loadGameReturnButtonTexture.Width / 2), 350), new Vector2(game4ButtonTexture.Width, game4ButtonTexture.Height));

            loadGameButtonList.Add(game1Button);
            loadGameButtonList.Add(game2Button);
            loadGameButtonList.Add(game3Button);
            loadGameButtonList.Add(game4Button);
            loadGameButtonList.Add(loadGameReturnButton);
        }

        private void LoadSplashScreens()
        {
            videoPlayer = new VideoPlayer();
            splashScreenVideo = _gameManager.Content.Load<Video>("Videos/SplashScreen_Video");
            videoRectangle = new Rectangle(0, 0, GameManager.screenWidth, GameManager.screenHeight);
            videoPlayer.Play(splashScreenVideo);

            videoTimer = 13;
            videoPlaying = true;
        }

        private void LoadLevelCompleteMenu()
        {
            Texture2D backgroundTexture = _gameManager.GetTexture("UserInterface/LevelCompleteMenu/LevelCompleteBackground");
            MenuComponent background = new MenuComponent();
            background.InitialiseMenuComponent(backgroundTexture, Vector2.Zero, new Vector2(GameManager.screenWidth, GameManager.screenHeight));

            completeMenuComponentList.Add(background);

            Texture2D replayButtonTexture = _gameManager.GetTexture("UserInterface/LevelCompleteMenu/ReplayButton");
            Button replayButton = new Button(GameManager.ButtonName.Replay, this);
            replayButton.InitialiseMenuComponent(replayButtonTexture, new Vector2((GameManager.screenWidth / 2) - (replayButtonTexture.Width / 2) - 160, 380), new Vector2(replayButtonTexture.Width, replayButtonTexture.Height));

            Texture2D continueButtonTexture = _gameManager.GetTexture("UserInterface/LevelCompleteMenu/ContinueButton");
            Button continueButton = new Button(GameManager.ButtonName.Continue, this);
            continueButton.InitialiseMenuComponent(continueButtonTexture, new Vector2((GameManager.screenWidth / 2) - (replayButtonTexture.Width / 2) + 160, 380), new Vector2(continueButtonTexture.Width, continueButtonTexture.Height));

            completeMenuButtonList.Add(replayButton);
            completeMenuButtonList.Add(continueButton);
        }

        private void LoadNewGameMenu()
        {
            newGameBackground = new MenuComponent();
            newGameBackground.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/NewGameMenu/NewGameMenuBackground"), Vector2.Zero, new Vector2(GameManager.screenWidth, GameManager.screenHeight));

            Texture2D loadGameReturnButtonTexture = _gameManager.GetTexture("UserInterface/CreditsMenu/ReturnButton");
            Button loadGameReturnButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            loadGameReturnButton.InitialiseMenuComponent(loadGameReturnButtonTexture, new Vector2((GameManager.screenWidth / 2) - (loadGameReturnButtonTexture.Width / 2), 470), new Vector2(loadGameReturnButtonTexture.Width, loadGameReturnButtonTexture.Height));

            Texture2D game1ButtonTexture = _gameManager.GetTexture("UserInterface/LoadGameMenu/Game1Button");
            Button game1Button = new Button(GameManager.GameData.Game1, this);
            game1Button.InitialiseMenuComponent(game1ButtonTexture, new Vector2((GameManager.screenWidth / 2) - (game1ButtonTexture.Width / 2), 140), new Vector2(game1ButtonTexture.Width, game1ButtonTexture.Height));

            Texture2D game2ButtonTexture = _gameManager.GetTexture("UserInterface/LoadGameMenu/Game2Button");
            Button game2Button = new Button(GameManager.GameData.Game2, this);
            game2Button.InitialiseMenuComponent(game2ButtonTexture, new Vector2((GameManager.screenWidth / 2) - (game2ButtonTexture.Width / 2), 210), new Vector2(game2ButtonTexture.Width, game2ButtonTexture.Height));

            Texture2D game3ButtonTexture = _gameManager.GetTexture("UserInterface/LoadGameMenu/Game3Button");
            Button game3Button = new Button(GameManager.GameData.Game3, this);
            game3Button.InitialiseMenuComponent(game3ButtonTexture, new Vector2((GameManager.screenWidth / 2) - (game3ButtonTexture.Width / 2), 280), new Vector2(game3ButtonTexture.Width, game3ButtonTexture.Height));

            Texture2D game4ButtonTexture = _gameManager.GetTexture("UserInterface/LoadGameMenu/Game4Button");
            Button game4Button = new Button(GameManager.GameData.Game4, this);
            game4Button.InitialiseMenuComponent(game4ButtonTexture, new Vector2((GameManager.screenWidth / 2) - (game4ButtonTexture.Width / 2), 350), new Vector2(game4ButtonTexture.Width, game4ButtonTexture.Height));

            newGameButtonList.Add(game1Button);
            newGameButtonList.Add(game2Button);
            newGameButtonList.Add(game3Button);
            newGameButtonList.Add(game4Button);

        }

        private void LoadDifficultySelectMenu()
        {
            difficultyBackground = new MenuComponent();
            difficultyBackground.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/DifficultySelectMenu/SelectDifficultyBackground"), new Vector2(0, 0), new Vector2(GameManager.screenWidth, GameManager.screenHeight));

            Button apprenticeButton = new Button(GameManager.ButtonName.ApprenticeButton, this);
            apprenticeButton.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/DifficultySelectMenu/Apprentice_Button"), new Vector2(10, 250), new Vector2(300, 60));
            
            Button heroButton = new Button(GameManager.ButtonName.HeroButton, this);
            heroButton.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/DifficultySelectMenu/Hero_Button"), new Vector2((GameManager.screenWidth / 2) - 150, 250), new Vector2(300, 60));

            Button masterButton = new Button(GameManager.ButtonName.MasterButton, this);
            masterButton.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/DifficultySelectMenu/Master_Button"), new Vector2(650, 250), new Vector2(300, 60));

            difficultyButtonList.Add(apprenticeButton);
            difficultyButtonList.Add(heroButton);
            difficultyButtonList.Add(masterButton);
        }
    }
}
