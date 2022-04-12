using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

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

        public static List<Button> loadGameButtonList = new List<Button>();

        public Button levelSelectReturnButton;

        public Button creditsReturnButton;
        public MenuComponent creditsBackground;

        public Button loadGameReturnButton;

        public MenuComponent loadGameBackground;

        private GameManager _gameManager;
        private GraphicsDeviceManager _graphics;

        private SpriteFont _defaultFont;
        private SpriteFont _levelTitleFont;

        private string _levelNameText;
        private string _levelDescriptionText;
        private string _levelDataFilePath;

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

                    levelSelectReturnButton.DrawMenuComponent(spriteBatch);

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

                    levelSelectReturnButton.DrawMenuComponent(spriteBatch);

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
                    loadGameReturnButton.DrawMenuComponent(spriteBatch);

                    foreach (Button loadButton in loadGameButtonList)
                    {
                        loadButton.DrawMenuComponent(spriteBatch);
                    }

                    

                    break;
                
            }
        }

        public void MouseClick(Vector2 mousePos)
        {
            if(GameManager.currentGameState == GameManager.GameState.MainMenu)
            {
                foreach (Button button in mainMenuButtonList)
                {
                    button.CheckInteraction(mousePos);
                }
            }

            if(GameManager.currentGameState == GameManager.GameState.Respawn)
            {
                foreach(Button button in respawnMenuButtonList)
                {
                    button.CheckInteraction(mousePos);
                }
            }

            if(GameManager.currentGameState == GameManager.GameState.LevelSelect)
            {
                foreach(Button button in levelSelectButtonList)
                {
                    button.CheckInteractionLevel(mousePos);
                    levelSelectReturnButton.CheckInteraction(mousePos);
                }
            }

            if (GameManager.currentGameState == GameManager.GameState.LevelSelectConfirm)
            {
                foreach (Button button in confirmLevelButtonList)
                {
                    button.CheckInteraction(mousePos);
                    levelSelectReturnButton.CheckInteraction(mousePos);
                }
            }

            if(GameManager.currentGameState == GameManager.GameState.LevelPause)
            {
                foreach(Button button in pauseMenuButtonList)
                {
                    button.CheckInteraction(mousePos);
                }
            }

            if (GameManager.currentGameState == GameManager.GameState.Credits)
            {
                creditsReturnButton.CheckInteraction(mousePos);
            }

            if(GameManager.currentGameState == GameManager.GameState.LoadGame)
            {
                foreach(Button button in loadGameButtonList)
                {
                    button.CheckInteractionLoad(mousePos);
                }

                loadGameReturnButton.CheckInteraction(mousePos);
            }
        }

        public void ButtonInteraction(GameManager.ButtonName buttonName)
        {
            switch(buttonName)
            {
                case GameManager.ButtonName.NewGameButton: //Display New Game Menu
                    break;
                case GameManager.ButtonName.ExitGameButton: _gameManager.ExitGame();
                    break;
                case GameManager.ButtonName.RespawnButton: DisplayRespawnScreen(false);
                    _gameManager.playerCharacter.ResetPlayer();
                    break;
                case GameManager.ButtonName.LevelSelectConfirm: LoadLevel();
                    break;
                case GameManager.ButtonName.LevelSelectExit: GameManager.currentGameState = GameManager.GameState.LevelSelect;
                    break;
                case GameManager.ButtonName.ReturnMainMenu: GameManager.currentGameState = GameManager.GameState.MainMenu;
                    break;
                case GameManager.ButtonName.ResumeGame: GameManager.currentGameState = GameManager.GameState.Level;
                    _gameManager.IsMouseVisible = false;
                    break;
                case GameManager.ButtonName.CreditsButton: GameManager.currentGameState = GameManager.GameState.Credits;
                    break;
                case GameManager.ButtonName.LoadGameButton: GameManager.currentGameState = GameManager.GameState.LoadGame;
                    break;
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

            levelSelectComponentList.Add(terraMap);

            Button levelOneSelect = new Button(GameManager.LevelButton.Level1Button, this);
            levelOneSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/LevelSelectButton"), new Vector2(200, 400), new Vector2(24, 24));

            levelSelectButtonList.Add(levelOneSelect);

            Button levelTwoSelect = new Button(GameManager.LevelButton.Level2Button, this);
            levelTwoSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/LevelSelectButton"), new Vector2(600, 420), new Vector2(24, 24));

            levelSelectButtonList.Add(levelTwoSelect);

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
            levelSelectReturnButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            levelSelectReturnButton.InitialiseMenuComponent(returnButtonTexture, new Vector2(0, 0), new Vector2(returnButtonTexture.Width / 2, returnButtonTexture.Height / 2));


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
            loadGameReturnButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
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
        }
    }
}
