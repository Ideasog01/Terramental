using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Terramental
{
    /// <summary>
    /// Loads and controls all menus, including handling button interactions. MenuManager also displays videos.
    /// </summary>

    public class MenuManager
    {
        //Main Menu Components
        public static List<Button> mainMenuButtonList = new List<Button>();
        public static List<MenuComponent> mainMenuComponentList = new List<MenuComponent>();

        //Respawn Menu Components
        public static List<Button> respawnMenuButtonList = new List<Button>();
        public static List<MenuComponent> respawnMenuComponentList = new List<MenuComponent>();

        //Options Menu Components
        public static List<Button> optionsButtonList = new List<Button>();
        public static List<MenuComponent> optionsComponentList = new List<MenuComponent>();

        //Level Select Menu Components
        public static List<Button> levelSelectButtonList = new List<Button>();
        public static List<MenuComponent> levelSelectComponentList = new List<MenuComponent>();
        public static Button levelSelectExitButton;

        //Confirm Level Menu Components
        public static List<Button> confirmLevelButtonList = new List<Button>();
        public static List<MenuComponent> confirmLevelComponentList = new List<MenuComponent>();

        //Pause Menu Components
        public static List<Button> pauseMenuButtonList = new List<Button>();
        public static List<MenuComponent> pauseMenuComponentList = new List<MenuComponent>();

        //Level Complete Menu Components
        public static List<MenuComponent> completeMenuComponentList = new List<MenuComponent>();
        public static List<Button> completeMenuButtonList = new List<Button>();

        //Credits Menu Components
        public Button creditsReturnButton;
        public MenuComponent creditsBackground;

        //Help Screen Menu Components
        private MenuComponent _helpScreen;
        private Button _helpScreenReturnButton;

        //Start Screen Menu Component
        private MenuComponent _startScreen;

        public static Button dashControlButton; //Controls if the dash ability is controlled by a double-tap in a direction of movement, or a single button (Keyboard Left Shift/ Controller B Button)

        public Video splashScreenVideo; //Introduction video
        public Video loadingScreenVideo; //The Loading Screen
        public Video creditsVideo; //The credits video that occurs at the end of the game after completing all levels.

        public VideoPlayer videoPlayer; //The class that plays the video
        public Texture2D videoTexture; //The texture of the video
        public Rectangle videoRectangle; //Size of the video
        private float videoTimer; //The duration of the video, this value is decreased overtime
        private bool videoPlaying; //If a video is playing, set this to true
        private GameManager.GameState videoPlayerAfterState; //The state to switch to after the videoTimer variable reaches zero.

        private SpriteFont _titleFont; //The font used for displaying statistics on the level complete menu

        private string _levelDataFilePath; //The file path of the .json file that contains the level/map data. This value is set when a level button is activated on the level select screen

        private bool _menuIsVertical; //If this value is true, controller menu naviation will require a directional input from the leftstick for the selected button to change
        private int currentButtonIndex; //The index of the currently selected button in the _buttonList variable
        private float _gamePadButtonTimer; //A timer that delays the switching of buttons as the gamepad is used to navigate menus
        private float _buttonBuffer; //A timer that ensures that an incorrect button is not activated during screen transition
        private List<Button> _buttonList = new List<Button>(); //The current button list to use for controller menu navigation and interaction

        private GameManager _gameManager; //Reference to GameManager
        private GraphicsDeviceManager _graphics; //Reference to GraphicsDeviceManager

        public MenuManager(GameManager gameManager, GraphicsDeviceManager graphics) //The MenuManager constructor that loads the menus and videos
        {
            _gameManager = gameManager;
            _graphics = graphics;

            _titleFont = _gameManager.Content.Load<SpriteFont>("SpriteFont/LevelTitleFont");

            //Load all Menus
            LoadStartScreen();
            LoadMainMenu();
            LoadOptionsMenu();
            LoadRespawnScreen();
            LoadLevelSelect();
            LoadPauseMenu();
            LoadCreditsMenu();
            LoadSplashScreens();
            LoadLevelCompleteMenu();
            LoadHelpScreen();
            LoadLoadingScreen();

            creditsVideo = _gameManager.Content.Load<Video>("Videos/CreditsVideo"); //Loads the end credits video
        }

        #region Activate Videos

        public void ActivateLoadingScreen(float duration, GameManager.GameState afterState) //Displays the loading screen for a given duration. After the video has finished playing, the current GameState will change to the given afterState value
        {
            videoTimer = duration;
            videoPlaying = true;
            videoPlayer.Play(loadingScreenVideo);
            videoPlayerAfterState = afterState;
            GameManager.currentGameState = GameManager.GameState.LoadingScreen;
        }

        public void ActivateCreditsScreen(float duration, GameManager.GameState afterState) //Displays the end credits screen for a given duration. After the video has finished playin, the current GameState will change to the given afterState value
        {
            AudioManager.StopMusic();
            AudioManager.PlaySound("Intro_Music");
            videoTimer = duration;
            videoPlaying = true;
            videoPlayer.Play(creditsVideo);
            videoPlayerAfterState = afterState;
            GameManager.currentGameState = GameManager.GameState.CreditsVideo;
        }

        #endregion

        #region Controller Menu Navigation

        public void ChangeButtonController(bool increase, bool verticalMotion) //Allows naviation of menus using a controller. If the motion aligns with the direction of the menu (vertical/horizontal), the button index will be incremented/decremented depending on the increase value
        {
            if(_buttonList.Count > 0)
            {
                if (_gamePadButtonTimer > 0) //Stops the function if the selected button was recently changed
                {
                    return;
                }

                _buttonList[currentButtonIndex].ComponentColor = Color.White; //Sets the old button color back to the original

                if (verticalMotion == _menuIsVertical)
                {
                    if (increase)
                    {
                        currentButtonIndex--;

                        if (currentButtonIndex < 0) //Ensures the index of the list is never below zero
                        {
                            currentButtonIndex = _buttonList.Count - 1;
                        }
                    }
                    else
                    {
                        currentButtonIndex++;

                        if (currentButtonIndex >= _buttonList.Count) //Ensures the index of the list is never above the value of the list count
                        {
                            currentButtonIndex = 0;
                        }
                    }

                    _buttonList[currentButtonIndex].ComponentColor = Color.Gray; //Set the currently selected button's color to gray to inform the user that it is selected
                }

                _gamePadButtonTimer = 0.2f;
            }
        }

        public void ResetMenu() //The ResetMenu() function changes the button list to store the appropriate buttons for the currently displayed menu. This function should be called whenever a new menu is loaded.
        {
            if(_gameManager.inputManager.IsGamePadConnected())
            {
                if(_buttonList.Count > 0)
                {
                    _buttonList[currentButtonIndex].ComponentColor = Color.White; //Reset the currently selected button's color
                }

                switch (GameManager.currentGameState)
                {
                    case GameManager.GameState.MainMenu:
                        _buttonList = mainMenuButtonList;
                        _menuIsVertical = true;
                        break;
                    case GameManager.GameState.Options:
                        _buttonList = optionsButtonList;
                        _menuIsVertical = true;
                        break;
                    case GameManager.GameState.LevelPause:
                        _buttonList = pauseMenuButtonList;
                        _menuIsVertical = true;
                        break;
                    case GameManager.GameState.LevelSelect:
                        _buttonList = levelSelectButtonList;
                        _menuIsVertical = false;
                        break;
                    case GameManager.GameState.LevelSelectConfirm:
                        _buttonList = confirmLevelButtonList;
                        _menuIsVertical = true;
                        break;
                    case GameManager.GameState.LevelComplete:
                        _buttonList = completeMenuButtonList;
                        _menuIsVertical = false;
                        break;
                    case GameManager.GameState.HelpMenu:
                        _buttonList.Clear();
                        _buttonList.Add(_helpScreenReturnButton);
                        _buttonList.Add(dashControlButton);
                        break;
                }

                currentButtonIndex = 0; //Set the selected button to be the first button in the list
                _buttonList[currentButtonIndex].ComponentColor = Color.Gray; //Set the selected button's color to gray
            }
            
        }

        public void ActivateButtonController() //Activates the currently selected button
        {
            if(_buttonBuffer <= 0)
            {
                if (GameManager.currentGameState == GameManager.GameState.Credits)
                {
                    ButtonInteraction(GameManager.ButtonName.ReturnMainMenu);
                    _buttonBuffer = 0.5f;
                    AudioManager.PlaySound("MenuButton_SFX");
                    return;
                }

                if(GameManager.currentGameState == GameManager.GameState.Respawn)
                {
                    ButtonInteraction(GameManager.ButtonName.RespawnButton);
                    _buttonBuffer = 0.5f;
                    AudioManager.PlaySound("MenuButton_SFX");
                    return;
                }

                if (_buttonList.Count > 0)
                {
                    if(GameManager.currentGameState == GameManager.GameState.LevelSelect)
                    {
                        if (_buttonList[currentButtonIndex] != levelSelectExitButton)
                        {
                            LevelSelectButtonInteraction(_buttonList[currentButtonIndex].LevelButtonName);
                        }
                        else
                        {
                            ButtonInteraction(_buttonList[currentButtonIndex].ButtonName);
                        }
                    }
                    else
                    {
                        ButtonInteraction(_buttonList[currentButtonIndex].ButtonName);
                    }

                    if (_buttonList[currentButtonIndex].ButtonName != GameManager.ButtonName.SFXVolumeUp && _buttonList[currentButtonIndex].ButtonName != GameManager.ButtonName.SFXVolumeDown)
                    {
                        AudioManager.PlaySound("MenuButton_SFX");
                    }
                }

                _buttonBuffer = 0.5f;
            }
        }

        #endregion

        #region DrawMenus

        public void DrawMenus(SpriteBatch spriteBatch) //Handles drawing of menu components, buttons, videos and text
        {
            switch (GameManager.currentGameState)
            {
                case GameManager.GameState.StartScreen:

                    _startScreen.DrawMenuComponent(spriteBatch);

                    break;

                case GameManager.GameState.HelpMenu:

                    _helpScreen.DrawMenuComponent(spriteBatch);
                    dashControlButton.DrawMenuComponent(spriteBatch);
                    _helpScreenReturnButton.DrawMenuComponent(spriteBatch);

                    break;

                case GameManager.GameState.MainMenu:
                    foreach (MenuComponent mainComponent in mainMenuComponentList)
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

                    foreach (MenuComponent selectComponent in levelSelectComponentList)
                    {
                        selectComponent.DrawMenuComponent(spriteBatch);
                    }

                    foreach (Button selectButton in levelSelectButtonList)
                    {
                        int levelButtonIndex = levelSelectButtonList.IndexOf(selectButton);

                        if (levelButtonIndex > 0 && levelButtonIndex < 7)
                        {
                            if (GameManager.levelsComplete >= levelButtonIndex)
                            {
                                selectButton.DrawMenuComponent(spriteBatch);
                                selectButton.ButtonActive = true;
                            }
                            else
                            {
                                selectButton.ButtonActive = false;
                            }
                        }
                        else
                        {
                            selectButton.DrawMenuComponent(spriteBatch);
                        }
                    }

                    levelSelectExitButton.DrawMenuComponent(spriteBatch);

                    break;

                case GameManager.GameState.LevelSelectConfirm:
                    foreach (MenuComponent selectComponent in levelSelectComponentList)
                    {
                        selectComponent.DrawMenuComponent(spriteBatch);
                    }

                    foreach (Button selectButton in levelSelectButtonList)
                    {
                        int levelButtonIndex = levelSelectButtonList.IndexOf(selectButton);

                        if (levelButtonIndex > 0 && levelButtonIndex < 6)
                        {
                            if (GameManager.levelsComplete >= levelButtonIndex)
                            {
                                selectButton.DrawMenuComponent(spriteBatch);
                                selectButton.ButtonActive = true;
                            }
                            else
                            {
                                selectButton.ButtonActive = false;
                            }
                        }
                        else
                        {
                            selectButton.DrawMenuComponent(spriteBatch);
                        }
                    }


                    foreach (MenuComponent confirmComponent in confirmLevelComponentList)
                    {
                        confirmComponent.DrawMenuComponent(spriteBatch);
                    }

                    foreach (Button confrimButton in confirmLevelButtonList)
                    {
                        confrimButton.DrawMenuComponent(spriteBatch);
                    }


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

                case GameManager.GameState.SplashScreen:

                    if (videoPlayer.State == MediaState.Stopped)
                    {
                        if (GameManager.currentGameState == GameManager.GameState.SplashScreen)
                        {
                            videoPlayer.Play(splashScreenVideo);
                        }
                        else if (GameManager.currentGameState == GameManager.GameState.LoadingScreen)
                        {
                            videoPlayer.Play(loadingScreenVideo);
                        }
                    }

                    if (videoPlayer.State == MediaState.Playing)
                    {
                        videoTexture = videoPlayer.GetTexture();
                        spriteBatch.Draw(videoTexture, videoRectangle, Color.White);
                    }

                    break;

                case GameManager.GameState.LoadingScreen:

                    if (videoPlayer.State == MediaState.Stopped)
                    {
                        videoPlayer.Play(loadingScreenVideo);
                    }

                    if (videoPlayer.State == MediaState.Playing)
                    {
                        videoTexture = videoPlayer.GetTexture();
                        spriteBatch.Draw(videoTexture, videoRectangle, Color.White);
                    }

                    break;

                case GameManager.GameState.CreditsVideo:

                    if (videoPlayer.State == MediaState.Stopped)
                    {
                        videoPlayer.Play(creditsVideo);
                    }

                    if (videoPlayer.State == MediaState.Playing)
                    {
                        videoTexture = videoPlayer.GetTexture();
                        spriteBatch.Draw(videoTexture, videoRectangle, Color.White);
                    }

                    break;

                case GameManager.GameState.LevelComplete:

                    foreach (MenuComponent completeComponent in completeMenuComponentList)
                    {
                        completeComponent.DrawMenuComponent(spriteBatch);
                    }

                    foreach (Button completeButton in completeMenuButtonList)
                    {
                        completeButton.DrawMenuComponent(spriteBatch);
                    }


                    spriteBatch.DrawString(_titleFont, _gameManager.playerCharacter.EnemiesDefeated.ToString(), new Vector2((GameManager.screenWidth / 2) + 5, 178), Color.White);
                    spriteBatch.DrawString(_titleFont, _gameManager.playerCharacter.PlayerScore.ToString(), new Vector2((GameManager.screenWidth / 2) - 60, 210), Color.White);

                    break;

                case GameManager.GameState.Options:

                    foreach (MenuComponent optionsComponent in optionsComponentList)
                    {
                        optionsComponent.DrawMenuComponent(spriteBatch);
                    }

                    foreach (Button optionsButton in optionsButtonList)
                    {
                        optionsButton.DrawMenuComponent(spriteBatch);
                    }

                    break;

            }
        }

        #endregion

        public void UpdateMenu(GameTime gameTime)
        {
            if(_buttonBuffer > 0)
            {
                _buttonBuffer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(_gamePadButtonTimer > 0)
            {
                _gamePadButtonTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(videoPlaying)
            {
                if (videoTimer > 0)
                {
                    videoTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    //Don't pause music if the current game state is equal to Level or LevelPause
                    if(GameManager.currentGameState == GameManager.GameState.LoadingScreen && GameManager.previousGameState != GameManager.GameState.Level && GameManager.previousGameState != GameManager.GameState.LevelPause) //If the video is not playing, and the loading screen is finished, stop the music.
                    {
                        AudioManager.StopMusic();
                    }

                    GameManager.currentGameState = videoPlayerAfterState; //Set the current start to the after state assigned when the video was activated
                    videoPlayer.Stop(); //Stops the video as the timer has depleted

                    if(GameManager.currentGameState == GameManager.GameState.Level) //When the loading screen is finished, play the level music
                    {
                        AudioManager.PlaySound("Level_Music");
                    }

                    videoPlaying = false;
                }
            }
            
        }

        #region Button Interactivity

        public void MouseClick(Vector2 mousePos) //Handles mouse clicking for button interaction detection
        {
            if(_buttonBuffer <= 0)
            {
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
                        if(button != levelSelectExitButton)
                        {
                            button.CheckInteractionLevel(mousePos);
                        }
                        else
                        {
                            levelSelectExitButton.CheckInteraction(mousePos);
                        }
                    }
                }

                if (GameManager.currentGameState == GameManager.GameState.LevelSelectConfirm)
                {
                    foreach (Button button in confirmLevelButtonList)
                    {
                        button.CheckInteraction(mousePos);
                    }

                    levelSelectExitButton.CheckInteraction(mousePos);
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

                if(GameManager.currentGameState == GameManager.GameState.LevelComplete)
                {
                    foreach(Button button in completeMenuButtonList)
                    {
                        button.CheckInteraction(mousePos);
                    }
                }

                if(GameManager.currentGameState == GameManager.GameState.Options)
                {
                    foreach(Button button in optionsButtonList)
                    {
                        button.CheckInteraction(mousePos);
                    }
                }

                if(GameManager.currentGameState == GameManager.GameState.HelpMenu)
                {
                    _helpScreenReturnButton.CheckInteraction(mousePos);
                    dashControlButton.CheckInteraction(mousePos);
                }
            }

        }

        public void ButtonInteraction(GameManager.ButtonName buttonName) //When a button has been activated/clicked, perform the appropriate actions depending on the buttonName value
        {
            if(_buttonBuffer <= 0) //Avoids unintended button activations
            {
                GameManager.previousGameState = GameManager.currentGameState;

                if (buttonName == GameManager.ButtonName.ReturnMainMenu) //Depending on the current game state, return to the 'MainMenu'. The Pause Menu acts as a Main Menu when a level is in progress
                {
                    if (GameManager.currentGameState == GameManager.GameState.Credits || GameManager.currentGameState == GameManager.GameState.LevelSelect) //Return to the main menu if the current screen is the credits or level select menu
                    {
                        GameManager.currentGameState = GameManager.GameState.MainMenu;
                        ResetMenu();
                        _buttonBuffer = 1;
                        return;
                    }

                    if (GameManager.currentGameState == GameManager.GameState.HelpMenu)
                    {
                        if (GameManager.levelLoaded) //If the level is loaded, display the pause menu
                        {
                            GameManager.PauseGame();
                            ResetMenu();
                        }
                        else //If the level is not loaded, display the main menu
                        {
                            GameManager.currentGameState = GameManager.GameState.MainMenu;
                            ResetMenu();
                        }

                        return;
                    }

                    if (GameManager.currentGameState == GameManager.GameState.Options)
                    {
                        if (GameManager.levelLoaded) //If the level is loaded, display the pause menu
                        {
                            GameManager.PauseGame();
                        }
                        else //If the level is not loaded, display the main menu
                        {
                            GameManager.currentGameState = GameManager.GameState.MainMenu;
                        }

                        ResetMenu();
                        return;
                    }

                    if (GameManager.currentGameState == GameManager.GameState.LevelComplete || GameManager.currentGameState == GameManager.GameState.LevelPause) //If the current game state is equal to the level complete menu or pause menu, display the loading screen.
                    {
                        ActivateLoadingScreen(2, GameManager.GameState.MainMenu); //Displays the loading screen. After the video is completed, the game will display the main menu.
                        AudioManager.PlaySound("Level_Music");
                        GameManager.levelLoaded = false;


                        ResetMenu();
                    }

                    return;
                }

                switch (buttonName) //Performs the appropriate action depending on the buttonName value
                {
                    case GameManager.ButtonName.StartGameButton:
                        GameManager.currentGameState = GameManager.GameState.LevelSelect;
                        ResetMenu();
                        break;
                    case GameManager.ButtonName.ExitGameButton:
                        _gameManager.ExitGame();
                        break;
                    case GameManager.ButtonName.RespawnButton:
                        DisplayRespawnScreen(false);
                        _gameManager.mapManager.ResetLevel();
                        _gameManager.playerCharacter.TeleportPlayer(SpawnManager.levelStartPosition);
                        break;
                    case GameManager.ButtonName.LevelSelectConfirm:

                        if(_gameManager.mapManager != null)
                        {
                            _gameManager.mapManager.UnloadLevel();
                        }
                        
                        _gameManager.LoadNewGame(_levelDataFilePath);
                        break;
                    case GameManager.ButtonName.LevelSelectExit:
                        GameManager.currentGameState = GameManager.GameState.LevelSelect;
                        ResetMenu();
                        break;
                    case GameManager.ButtonName.OptionsButton:
                        GameManager.currentGameState = GameManager.GameState.Options;
                        ResetMenu();
                        break;
                    case GameManager.ButtonName.ResumeGame:
                        GameManager.currentGameState = GameManager.GameState.Level;
                        _gameManager.IsMouseVisible = false;
                        break;
                    case GameManager.ButtonName.CreditsButton:
                        GameManager.currentGameState = GameManager.GameState.Credits;
                        ResetMenu();
                        break;
                    case GameManager.ButtonName.Replay:
                        GameManager.currentGameState = GameManager.GameState.Level;
                        _gameManager.mapManager.ResetLevel();
                        break;
                    case GameManager.ButtonName.Continue:
                        GameManager.currentGameState = GameManager.GameState.LevelSelect;

                        if (GameManager.levelsComplete < GameManager.levelIndex)
                        {
                            GameManager.levelsComplete = GameManager.levelIndex;
                        }

                        if(GameManager.levelsComplete >= 5)
                        {
                            ActivateCreditsScreen(60, GameManager.GameState.MainMenu);
                        }

                        _gameManager.mapManager.UnloadLevel();
                        ResetMenu();
                        break;
                    case GameManager.ButtonName.HelpScreenButton:
                        GameManager.currentGameState = GameManager.GameState.HelpMenu;
                        ResetMenu();
                        break;
                    case GameManager.ButtonName.MusicVolumeUp:
                        AudioManager.AdjustMusicVolume(true);
                        break;
                    case GameManager.ButtonName.MusicVolumeDown:
                        AudioManager.AdjustMusicVolume(false);
                        break;
                    case GameManager.ButtonName.SFXVolumeUp:
                        AudioManager.AdjustSFXVolume(true);
                        break;
                    case GameManager.ButtonName.SFXVolumeDown:
                        AudioManager.AdjustSFXVolume(false);
                        break;
                    case GameManager.ButtonName.ResolutionUp:
                        _gameManager.ChangeResolution(1);
                        break;
                    case GameManager.ButtonName.ResolutionDown:
                        _gameManager.ChangeResolution(-1);
                        break;
                    case GameManager.ButtonName.DashButton:

                        if(_gameManager.playerCharacter != null)
                        {
                            if (_gameManager.playerCharacter.useDoubleTapDash == true)
                            {
                                _gameManager.playerCharacter.useDoubleTapDash = false;
                                dashControlButton.ComponentTexture = _gameManager.GetTexture("UserInterface/HelpScreen/DoubleTap");

                            }
                            else if (_gameManager.playerCharacter.useDoubleTapDash == false)
                            {
                                _gameManager.playerCharacter.useDoubleTapDash = true;
                                dashControlButton.ComponentTexture = _gameManager.GetTexture("UserInterface/HelpScreen/ShiftButton");
                            }
                        }

                        break;
                }
            }

            _buttonBuffer = 0.5f;

        }

        public void LevelSelectButtonInteraction(GameManager.LevelButton buttonName) //Assigns the values to begin loading a level. The selected level will be loaded once the player uses the start button in the level select confirm menu.
        {
            switch(buttonName)
            {
                case GameManager.LevelButton.Level1Button:
                    _levelDataFilePath = @"Content/Level1Map.json";
                    GameManager.levelIndex = 1;
                    GameManager.currentGameState = GameManager.GameState.LevelSelectConfirm;
                    confirmLevelComponentList[0].ComponentTexture = _gameManager.GetTexture("UserInterface/LevelDescriptions/Level1Description"); //The level panel that displays a description and level name.
                    break;
                case GameManager.LevelButton.Level2Button:

                    if(GameManager.levelsComplete > 0)
                    {
                        _levelDataFilePath = @"Content/Level2Map.json";
                        GameManager.currentGameState = GameManager.GameState.LevelSelectConfirm;
                        confirmLevelComponentList[0].ComponentTexture = _gameManager.GetTexture("UserInterface/LevelDescriptions/Level2Description");
                        GameManager.levelIndex = 2;
                    }

                    break;
                case GameManager.LevelButton.Level3Button:

                    if(GameManager.levelsComplete > 1)
                    {
                        _levelDataFilePath = @"Content/Level3Map.json";
                        GameManager.currentGameState = GameManager.GameState.LevelSelectConfirm;
                        confirmLevelComponentList[0].ComponentTexture = _gameManager.GetTexture("UserInterface/LevelDescriptions/Level3Description");
                        GameManager.levelIndex = 3;
                    }
                    
                    break;
                case GameManager.LevelButton.Level4Button:

                    if(GameManager.levelsComplete > 2)
                    {
                        _levelDataFilePath = @"Content/Level4Map.json";
                        GameManager.currentGameState = GameManager.GameState.LevelSelectConfirm;
                        confirmLevelComponentList[0].ComponentTexture = _gameManager.GetTexture("UserInterface/LevelDescriptions/Level4Description");
                        GameManager.levelIndex = 4;
                    }
                    
                    break;
                case GameManager.LevelButton.Level5Button:

                    if(GameManager.levelsComplete > 3)
                    {
                        _levelDataFilePath = @"Content/Level5Map.json";
                        GameManager.currentGameState = GameManager.GameState.LevelSelectConfirm;
                        confirmLevelComponentList[0].ComponentTexture = _gameManager.GetTexture("UserInterface/LevelDescriptions/Level5Description");
                        GameManager.levelIndex = 5;
                    }
                    
                    break;

                case GameManager.LevelButton.Level6Button:

                    if (GameManager.levelsComplete > 4)
                    {
                        _levelDataFilePath = @"Content/Level6Map.json";
                        GameManager.currentGameState = GameManager.GameState.LevelSelectConfirm;
                        confirmLevelComponentList[0].ComponentTexture = _gameManager.GetTexture("UserInterface/LevelDescriptions/Level5Description");
                        GameManager.levelIndex = 6;
                    }

                    break;

                case GameManager.LevelButton.Level8Button:

                    if (GameManager.levelsComplete > 4)
                    {
                        _levelDataFilePath = @"Content/Level8Map.json";
                        confirmLevelComponentList[0].ComponentTexture = _gameManager.GetTexture("UserInterface/LevelDescriptions/Level5Description");
                        GameManager.levelIndex = 8;
                    }

                    break;
            }

            ResetMenu();

            _buttonBuffer = 0.5f;
        }

        #endregion

        #region Display End of Level Menus

        public void DisplayRespawnScreen(bool isActive) //Displays the Respawn Screen/Level depending on the passed in boolean value of isActive
        {
            if(isActive)
            {
                GameManager.currentGameState = GameManager.GameState.Respawn;
                _gameManager.IsMouseVisible = true;
                ResetMenu();
                if (_gameManager.inputManager.IsGamePadConnected())
                {
                    respawnMenuButtonList[0].ComponentColor = Color.Gray;
                }
                else
                {
                    respawnMenuButtonList[0].ComponentColor = Color.White;
                }
            }
            else
            {
                GameManager.currentGameState = GameManager.GameState.Level;
                _gameManager.IsMouseVisible = false;
            }
        }

        public void EndLevel() //Ends the level and displays the level complete menu. This menu displays when the player collides with the end of level pickup (campfire)
        {
            GameManager.currentGameState = GameManager.GameState.LevelComplete;
            _gameManager.IsMouseVisible = true;
        }

        #endregion

        #region LoadMenus

        private void LoadMainMenu()
        {
            int viewportCentreX = _graphics.PreferredBackBufferWidth / 2; //Gets the centre of the screen on the X axis

            Texture2D mainMenuBackgroundTexture = _gameManager.GetTexture("UserInterface/MainMenu/MainMenu_FireBackground");
            MenuComponent mainMenuBackground = new MenuComponent();
            mainMenuBackground.InitialiseMenuComponent(mainMenuBackgroundTexture, new Vector2(0, 0), new Vector2(GameManager.screenWidth, GameManager.screenHeight));
            mainMenuComponentList.Add(mainMenuBackground);

            Texture2D startGameButtonTexture = _gameManager.GetTexture("UserInterface/MainMenu/StartGame_MainMenu_Button");
            Button startGameButton = new Button(GameManager.ButtonName.StartGameButton, this);
            startGameButton.InitialiseMenuComponent(startGameButtonTexture, new Vector2(viewportCentreX - (startGameButtonTexture.Width / 2), 125), new Vector2(300, 60));
            mainMenuButtonList.Add(startGameButton);

            Texture2D optionsButtonTexture = _gameManager.GetTexture("UserInterface/MainMenu/Options_MainMenu_Button");
            Button optionsButton = new Button(GameManager.ButtonName.OptionsButton, this);
            optionsButton.InitialiseMenuComponent(optionsButtonTexture, new Vector2(viewportCentreX - (optionsButtonTexture.Width / 2), 205), new Vector2(300, 60));
            mainMenuButtonList.Add(optionsButton);

            Texture2D creditsButtonTexture = _gameManager.GetTexture("UserInterface/MainMenu/Credits_MainMenu_Button");
            Button creditsButton = new Button(GameManager.ButtonName.CreditsButton, this);
            creditsButton.InitialiseMenuComponent(creditsButtonTexture, new Vector2(viewportCentreX - (creditsButtonTexture.Width / 2), 285), new Vector2(300, 60));
            mainMenuButtonList.Add(creditsButton);

            Texture2D exitButtonTexture = _gameManager.GetTexture("UserInterface/MainMenu/ExitGame_MainMenu_Button");
            Button exitButton = new Button(GameManager.ButtonName.ExitGameButton, this);
            exitButton.InitialiseMenuComponent(exitButtonTexture, new Vector2(viewportCentreX - (exitButtonTexture.Width / 2), 365), new Vector2(300, 60));
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
            levelOneSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/Level1_Button"), new Vector2(200, 400), new Vector2(40, 40));

            Button levelTwoSelect = new Button(GameManager.LevelButton.Level2Button, this);
            levelTwoSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/Level2_Button"), new Vector2(100, 220), new Vector2(40, 40));

            Button levelThreeSelect = new Button(GameManager.LevelButton.Level3Button, this);
            levelThreeSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/Level3_Button"), new Vector2(400, 280), new Vector2(40, 40));

            Button levelFourSelect = new Button(GameManager.LevelButton.Level4Button, this);
            levelFourSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/Level4_Button"), new Vector2(620, 150), new Vector2(40, 40));

            Button levelFiveSelect = new Button(GameManager.LevelButton.Level5Button, this);
            levelFiveSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/Level5_Button"), new Vector2(700, 380), new Vector2(40, 40));

            Button levelSixSelect = new Button(GameManager.LevelButton.Level6Button, this);
            levelSixSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/Level6_Button"), new Vector2(550, 75), new Vector2(40, 40));

            Button levelEightSelect = new Button(GameManager.LevelButton.Level8Button, this);
            levelEightSelect.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/LevelSelect/Level6_Button"), new Vector2(600, 430), new Vector2(40, 40));

            Texture2D confirmPanelTexture = _gameManager.GetTexture("UserInterface/LevelSelect/LevelDetailsPanel");
            MenuComponent confirmPanel = new MenuComponent();
            confirmPanel.InitialiseMenuComponent(confirmPanelTexture, new Vector2((GameManager.screenWidth / 2) - confirmPanelTexture.Width / 2, (GameManager.screenHeight / 2) - confirmPanelTexture.Height / 2), new Vector2(confirmPanelTexture.Width, confirmPanelTexture.Height));

            Texture2D confirmExitTexture = _gameManager.GetTexture("UserInterface/LevelSelect/ExitButton");
            Button confirmExitButton = new Button(GameManager.ButtonName.LevelSelectExit, this);
            confirmExitButton.InitialiseMenuComponent(confirmExitTexture, new Vector2(confirmPanel.ComponentPosition.X, confirmPanel.ComponentPosition.Y), new Vector2(confirmExitTexture.Width / 2, confirmExitTexture.Height / 2));

            Texture2D confirmButtonTexture = _gameManager.GetTexture("UserInterface/LevelSelect/StartButton");
            Button confirmButton = new Button(GameManager.ButtonName.LevelSelectConfirm, this);
            confirmButton.InitialiseMenuComponent(confirmButtonTexture, new Vector2(((GameManager.screenWidth / 2) - confirmButtonTexture.Width / 2) + (confirmButtonTexture.Width / 2) - (confirmButtonTexture.Width / 2), 380), new Vector2(confirmButtonTexture.Width, confirmButtonTexture.Height));

            levelSelectExitButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            levelSelectExitButton.InitialiseMenuComponent(confirmExitTexture, new Vector2(0, 0), new Vector2(confirmExitTexture.Width / 2, confirmExitTexture.Height / 2));

            levelSelectComponentList.Add(terraMap);

            levelSelectButtonList.Add(levelOneSelect);
            levelSelectButtonList.Add(levelTwoSelect);
            levelSelectButtonList.Add(levelThreeSelect);
            levelSelectButtonList.Add(levelFourSelect);
            levelSelectButtonList.Add(levelFiveSelect);
            levelSelectButtonList.Add(levelSixSelect);
            levelSelectButtonList.Add(levelEightSelect);
            levelSelectButtonList.Add(levelSelectExitButton);

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
            resumeButton.InitialiseMenuComponent(resumeButtonTexture, new Vector2((GameManager.screenWidth / 2) - (pauseMenuPanelTexture.Width / 2) + 49, (GameManager.screenHeight / 2) - (pauseMenuPanelTexture.Height / 2) + 96), new Vector2(resumeButtonTexture.Width, resumeButtonTexture.Height));

            Texture2D optionsButtonTexture = _gameManager.GetTexture("UserInterface/PauseMenu/OptionsButton");
            Button optionsButton = new Button(GameManager.ButtonName.OptionsButton, this);
            optionsButton.InitialiseMenuComponent(optionsButtonTexture, new Vector2((GameManager.screenWidth / 2) - (pauseMenuPanelTexture.Width / 2) + 49, (GameManager.screenHeight / 2) - (pauseMenuPanelTexture.Height / 2) + 175), new Vector2(optionsButtonTexture.Width, optionsButtonTexture.Height));

            Texture2D mainMenuTexture = _gameManager.GetTexture("UserInterface/PauseMenu/MainMenuButton");
            Button mainMenuButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            mainMenuButton.InitialiseMenuComponent(mainMenuTexture, new Vector2((GameManager.screenWidth / 2) - (pauseMenuPanelTexture.Width / 2) + 49, (GameManager.screenHeight / 2) - (pauseMenuPanelTexture.Height / 2) + 254), new Vector2(mainMenuTexture.Width, mainMenuTexture.Height));

            Texture2D helpButtonTexture = _gameManager.GetTexture("UserInterface/PauseMenu/HelpMenu_Button");
            Button helpButton = new Button(GameManager.ButtonName.HelpScreenButton, this);
            helpButton.InitialiseMenuComponent(helpButtonTexture, new Vector2((GameManager.screenWidth / 2) - (pauseMenuPanelTexture.Width / 2) + 49, (GameManager.screenHeight / 2) - (pauseMenuPanelTexture.Height / 2) + 333), new Vector2(helpButtonTexture.Width, helpButtonTexture.Height));

            Texture2D exitGameButtonTexture = _gameManager.GetTexture("UserInterface/PauseMenu/ExitGame_Button");
            Button exitGameButton = new Button(GameManager.ButtonName.ExitGameButton, this);
            exitGameButton.InitialiseMenuComponent(exitGameButtonTexture, new Vector2((GameManager.screenWidth / 2) - (pauseMenuPanelTexture.Width / 2) + 50, (GameManager.screenHeight / 2) - (pauseMenuPanelTexture.Height / 2) + 412), new Vector2(exitGameButtonTexture.Width, exitGameButtonTexture.Height));

            pauseMenuButtonList.Add(resumeButton);
            pauseMenuButtonList.Add(optionsButton);
            pauseMenuButtonList.Add(mainMenuButton);
            pauseMenuButtonList.Add(helpButton);
            pauseMenuButtonList.Add(exitGameButton);
            
            pauseMenuComponentList.Add(pauseMenuPanel);
        
        }

        private void LoadCreditsMenu()
        {
            Texture2D creditsBackgroundTexture = _gameManager.GetTexture("UserInterface/CreditsMenu/Credits");
            creditsBackground = new MenuComponent();
            creditsBackground.InitialiseMenuComponent(creditsBackgroundTexture, new Vector2(0, 0), new Vector2(GameManager.screenWidth, GameManager.screenHeight));

            Texture2D creditsReturnTexture = _gameManager.GetTexture("UserInterface/OptionsMenu/ReturnButton");
            creditsReturnButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            creditsReturnButton.InitialiseMenuComponent(creditsReturnTexture, new Vector2(10, 460), new Vector2(creditsReturnTexture.Width, creditsReturnTexture.Height));
            creditsReturnButton.ComponentColor = Color.Gray;
        }

        private void LoadSplashScreens()
        {
            videoPlayer = new VideoPlayer();
            splashScreenVideo = _gameManager.Content.Load<Video>("Videos/SplashScreen_Terramental");
            videoRectangle = new Rectangle(0, 0, GameManager.screenWidth, GameManager.screenHeight);
            videoPlayer.Play(splashScreenVideo);
            videoPlayerAfterState = GameManager.GameState.StartScreen;
            videoTimer = 10;
            videoPlaying = true;
        }

        private void LoadLoadingScreen()
        {
            loadingScreenVideo = _gameManager.Content.Load<Video>("Videos/LoadingScreen");
            videoRectangle = new Rectangle(0, 0, GameManager.screenWidth, GameManager.screenHeight);
        }

        private void LoadLevelCompleteMenu()
        {
            Texture2D backgroundTexture = _gameManager.GetTexture("UserInterface/LevelCompleteMenu/LevelCompleteBackground");
            MenuComponent background = new MenuComponent();
            background.InitialiseMenuComponent(backgroundTexture, Vector2.Zero, new Vector2(GameManager.screenWidth, GameManager.screenHeight));

            completeMenuComponentList.Add(background);

            Texture2D replayButtonTexture = _gameManager.GetTexture("UserInterface/LevelCompleteMenu/ReplayButton");
            Button replayButton = new Button(GameManager.ButtonName.Replay, this);
            replayButton.InitialiseMenuComponent(replayButtonTexture, new Vector2((GameManager.screenWidth / 2) - (replayButtonTexture.Width / 2) - 160, 420), new Vector2(replayButtonTexture.Width, replayButtonTexture.Height));

            Texture2D continueButtonTexture = _gameManager.GetTexture("UserInterface/LevelCompleteMenu/ContinueButton");
            Button continueButton = new Button(GameManager.ButtonName.Continue, this);
            continueButton.InitialiseMenuComponent(continueButtonTexture, new Vector2((GameManager.screenWidth / 2) - (replayButtonTexture.Width / 2) + 160, 420), new Vector2(continueButtonTexture.Width, continueButtonTexture.Height));

            completeMenuButtonList.Add(replayButton);
            completeMenuButtonList.Add(continueButton);
        }

        private void LoadStartScreen()
        {
            _startScreen = new MenuComponent();
            _startScreen.InitialiseMenuComponent(_gameManager.GetTexture("UserInterface/StartScreen"), Vector2.Zero, new Vector2(GameManager.screenWidth, GameManager.screenHeight));
        }

        private void LoadOptionsMenu()
        {
            int viewportCentreX = (GameManager.screenWidth / 2); //Gets the centre of the screen on the X axis

            Texture2D OptionsMenuBackgroundTexture = _gameManager.GetTexture("UserInterface/OptionsMenu/OptionsMenuBackground");
            MenuComponent OptionsMenuBackground = new MenuComponent();
            OptionsMenuBackground.InitialiseMenuComponent(OptionsMenuBackgroundTexture, new Vector2(0, 0), new Vector2(GameManager.screenWidth, GameManager.screenHeight));
            optionsComponentList.Add(OptionsMenuBackground);

            Texture2D ResolutionButtonTexture = _gameManager.GetTexture("UserInterface/OptionsMenu/ResolutionButton");
            Button ResolutionButton = new Button(GameManager.ButtonName.ResolutionButton, this);
            ResolutionButton.InitialiseMenuComponent(ResolutionButtonTexture, new Vector2(viewportCentreX - (ResolutionButtonTexture.Width / 2), 110), new Vector2(ResolutionButtonTexture.Width, ResolutionButtonTexture.Height));
            optionsButtonList.Add(ResolutionButton);

            Texture2D MusicButtonTexture = _gameManager.GetTexture("UserInterface/OptionsMenu/MusicVolumeButton");
            Button MusicButton = new Button(GameManager.ButtonName.MusicButton, this);
            MusicButton.InitialiseMenuComponent(MusicButtonTexture, new Vector2(viewportCentreX - (MusicButtonTexture.Width / 2), 190), new Vector2(MusicButtonTexture.Width, MusicButtonTexture.Height));
            optionsButtonList.Add(MusicButton);

            Texture2D SFXVolButtonTexture = _gameManager.GetTexture("UserInterface/OptionsMenu/SFXVolumeButton");
            Button SFXVolButton = new Button(GameManager.ButtonName.OptionsButton, this);
            SFXVolButton.InitialiseMenuComponent(SFXVolButtonTexture, new Vector2(viewportCentreX - (SFXVolButtonTexture.Width / 2), 270), new Vector2(SFXVolButtonTexture.Width, SFXVolButtonTexture.Height));
            optionsButtonList.Add(SFXVolButton);

            Texture2D ControlsButtonTexture = _gameManager.GetTexture("UserInterface/OptionsMenu/ControlsButton");
            Button ControlsButton = new Button(GameManager.ButtonName.HelpScreenButton, this);
            ControlsButton.InitialiseMenuComponent(ControlsButtonTexture, new Vector2(viewportCentreX - (ControlsButtonTexture.Width / 2), 350), new Vector2(ControlsButtonTexture.Width, ControlsButtonTexture.Height));
            optionsButtonList.Add(ControlsButton);

            Texture2D ReturnButtonTexture = _gameManager.GetTexture("UserInterface/OptionsMenu/ReturnButton");
            Button ReturnButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            ReturnButton.InitialiseMenuComponent(ReturnButtonTexture, new Vector2(viewportCentreX - (ReturnButtonTexture.Width / 2), 430), new Vector2(ReturnButtonTexture.Width, ReturnButtonTexture.Height));
            optionsButtonList.Add(ReturnButton);

            Texture2D leftButtonTexture = _gameManager.GetTexture("UserInterface/OptionsMenu/LeftButton");
            Texture2D rightButtonTexture = _gameManager.GetTexture("UserInterface/OptionsMenu/RightButton");

            Button resDecButton = new Button(GameManager.ButtonName.ResolutionDown, this);
            resDecButton.InitialiseMenuComponent(leftButtonTexture, new Vector2(ResolutionButton.ComponentPosition.X - 59, ResolutionButton.ComponentPosition.Y + 5), new Vector2(leftButtonTexture.Width, leftButtonTexture.Height));
            optionsButtonList.Add(resDecButton);

            Button resIncButton = new Button(GameManager.ButtonName.ResolutionUp, this);
            resIncButton.InitialiseMenuComponent(rightButtonTexture, new Vector2(ResolutionButton.ComponentPosition.X + ResolutionButtonTexture.Width + 10, ResolutionButton.ComponentPosition.Y + 5), new Vector2(rightButtonTexture.Width, rightButtonTexture.Height));
            optionsButtonList.Add(resIncButton);

            Button musicVolDecButton = new Button(GameManager.ButtonName.MusicVolumeDown, this);
            musicVolDecButton.InitialiseMenuComponent(leftButtonTexture, new Vector2(MusicButton.ComponentPosition.X - 59, MusicButton.ComponentPosition.Y + 5), new Vector2(leftButtonTexture.Width, leftButtonTexture.Height));
            optionsButtonList.Add(musicVolDecButton);

            Button musicVolIncButton = new Button(GameManager.ButtonName.MusicVolumeUp, this);
            musicVolIncButton.InitialiseMenuComponent(rightButtonTexture, new Vector2(MusicButton.ComponentPosition.X + MusicButtonTexture.Width + 10, MusicButton.ComponentPosition.Y + 5), new Vector2(rightButtonTexture.Width, rightButtonTexture.Height));
            optionsButtonList.Add(musicVolIncButton);

            Button sfxVolDecButton = new Button(GameManager.ButtonName.SFXVolumeDown, this);
            sfxVolDecButton.InitialiseMenuComponent(leftButtonTexture, new Vector2(SFXVolButton.ComponentPosition.X - 59, SFXVolButton.ComponentPosition.Y + 5), new Vector2(leftButtonTexture.Width, leftButtonTexture.Height));
            optionsButtonList.Add(sfxVolDecButton);

            Button sfxVolIncButton = new Button(GameManager.ButtonName.SFXVolumeUp, this);
            sfxVolIncButton.InitialiseMenuComponent(rightButtonTexture, new Vector2(SFXVolButton.ComponentPosition.X + SFXVolButtonTexture.Width + 10, SFXVolButton.ComponentPosition.Y + 5), new Vector2(rightButtonTexture.Width, rightButtonTexture.Height));
            optionsButtonList.Add(sfxVolIncButton);

            _gameManager.IsMouseVisible = true;
        }

        private void LoadHelpScreen()
        {
            Texture2D helpScreenTexture = _gameManager.GetTexture("UserInterface/HelpScreen/HelpMenuScreen");
            _helpScreen = new MenuComponent();
            _helpScreen.InitialiseMenuComponent(helpScreenTexture, Vector2.Zero, new Vector2(GameManager.screenWidth, GameManager.screenHeight));

            Texture2D returnButtonTexture = _gameManager.GetTexture("UserInterface/OptionsMenu/ReturnButton");
            _helpScreenReturnButton = new Button(GameManager.ButtonName.ReturnMainMenu, this);
            _helpScreenReturnButton.InitialiseMenuComponent(returnButtonTexture, new Vector2(20, GameManager.screenHeight - 150), new Vector2(returnButtonTexture.Width, returnButtonTexture.Height));

            Texture2D dashButtonTexture = _gameManager.GetTexture("UserInterface/HelpScreen/DoubleTap");
            dashControlButton = new Button(GameManager.ButtonName.DashButton, this);
            dashControlButton.InitialiseMenuComponent(dashButtonTexture, new Vector2(750, 80), new Vector2(75, 75));

        }

        #endregion
    }
}
