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

        private GameManager _gameManager;
        private GraphicsDeviceManager _graphics;


        public MenuManager(GameManager gameManager, GraphicsDeviceManager graphics)
        {
            _gameManager = gameManager;
            _graphics = graphics;

            LoadMainMenu();
            LoadRespawnScreen();
            LoadLevelSelect();
        }

        public void DrawMenus(SpriteBatch spriteBatch)
        {
            switch(GameManager.currentGameState)
            {
                case GameManager.GameState.MainMenu:
                     foreach(MenuComponent component in mainMenuComponentList)
                     {
                         component.DrawMenuComponent(spriteBatch);
                     }

                     foreach (Button button in mainMenuButtonList)
                     {
                            button.DrawMenuComponent(spriteBatch);
                     }
                 break;

                case GameManager.GameState.Respawn:
                    foreach (MenuComponent component in respawnMenuComponentList)
                    {
                        component.DrawMenuComponent(spriteBatch);
                    }

                    foreach (Button button in respawnMenuButtonList)
                    {
                        button.DrawMenuComponent(spriteBatch);
                    }
                 break;

                case GameManager.GameState.LevelSelect:
                    foreach(MenuComponent component in levelSelectComponentList)
                    {
                        component.DrawMenuComponent(spriteBatch);
                    }

                    foreach(Button button in levelSelectButtonList)
                    {
                        button.DrawMenuComponent(spriteBatch);
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
        }

        public void ButtonInteraction(GameManager.ButtonName buttonName)
        {
            switch(buttonName)
            {
                case GameManager.ButtonName.NewGameButton: _gameManager.LoadNewGame();
                    break;
                case GameManager.ButtonName.ExitGameButton: _gameManager.ExitGame();
                    break;
                case GameManager.ButtonName.RespawnButton: DisplayRespawnScreen(false);
                    _gameManager.playerCharacter.ResetPlayer();
                    break;
            }

            DisplayMainMenu(false);
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

        private void LoadMainMenu()
        {
            int viewportCentreX = _graphics.PreferredBackBufferWidth / 2;

            Texture2D mainMenuBackgroundTexture = _gameManager.GetTexture("UserInterface/MainMenu/MainMenu_FireBackground");
            MenuComponent mainMenuBackground = new MenuComponent();
            mainMenuBackground.InitialiseMenuComponent(mainMenuBackgroundTexture, new Vector2(0, 0), new Vector2(mainMenuBackgroundTexture.Width, mainMenuBackgroundTexture.Height));
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
        }
    }
}
