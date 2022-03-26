using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    class MenuManager
    {
        public static List<Button> buttonList = new List<Button>();
        public static List<Sprite> interfaceList = new List<Sprite>();

        private bool _mainMenuLoaded;
        private GameManager _gameManager;
        private GraphicsDeviceManager _graphics;


        public MenuManager(GameManager gameManager, GraphicsDeviceManager graphics)
        {
            _gameManager = gameManager;
            _graphics = graphics;

            LoadMainMenu();
        }

        public void MouseClick(Vector2 mousePos)
        {
            foreach(Button button in buttonList)
            {
                button.CheckInteraction(mousePos);
            }
        }

        public void ButtonInteraction(int buttonIndex)
        {
            switch(buttonIndex)
            {
                case 0: _gameManager.LoadNewGame();
                    break;
                case 5: _gameManager.ExitGame();
                    break;
            }

            DestroyMainMenu();
        }

        public void DestroyMainMenu()
        {
            _gameManager.IsMouseVisible = false;

            foreach(Button button in buttonList)
            {
                button.IsActive = false;
            }

            foreach(Sprite sprite in interfaceList)
            {
                sprite.IsActive = false;
            }
        }

        private void LoadMainMenu()
        {
            Sprite mainMenuBackground = new Sprite();
            mainMenuBackground.Initialise(new Vector2(0, 0), _gameManager.GetTexture("UserInterface/MainMenu/MainMenu_FireBackground"), new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
            interfaceList.Add(mainMenuBackground);

            Sprite titleText = new Sprite();
            titleText.Initialise(new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_gameManager.GetTexture("UserInterface/MainMenu/GameTitleText").Width / 2), -5), _gameManager.GetTexture("UserInterface/MainMenu/GameTitleText"), new Vector2(400, 84));
            interfaceList.Add(titleText);

            Button newGameButton = new Button(0, this);
            newGameButton.Initialise(new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_gameManager.GetTexture("UserInterface/MainMenu/NewGame_MainMenu_Button").Width / 2), 70), _gameManager.GetTexture("UserInterface/MainMenu/NewGame_MainMenu_Button"), new Vector2(256, 64));
            buttonList.Add(newGameButton);

            Button loadGameButton = new Button(1, this);
            loadGameButton.Initialise(new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_gameManager.GetTexture("UserInterface/MainMenu/LoadGame_MainMenu_Button").Width / 2), 150), _gameManager.GetTexture("UserInterface/MainMenu/LoadGame_MainMenu_Button"), new Vector2(256, 64));
            buttonList.Add(loadGameButton);

            Button optionsButton = new Button(2, this);
            optionsButton.Initialise(new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_gameManager.GetTexture("UserInterface/MainMenu/Options_MainMenu_Button").Width / 2), 230), _gameManager.GetTexture("UserInterface/MainMenu/Options_MainMenu_Button"), new Vector2(256, 64));
            buttonList.Add(optionsButton);

            Button achievementsButton = new Button(3, this);
            achievementsButton.Initialise(new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_gameManager.GetTexture("UserInterface/MainMenu/Achievements_MainMenu_Button").Width / 2), 310), _gameManager.GetTexture("UserInterface/MainMenu/Achievements_MainMenu_Button"), new Vector2(256, 64));
            buttonList.Add(achievementsButton);

            Button creditsButton = new Button(4, this);
            creditsButton.Initialise(new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_gameManager.GetTexture("UserInterface/MainMenu/Credits_MainMenu_Button").Width / 2), 390), _gameManager.GetTexture("UserInterface/MainMenu/Credits_MainMenu_Button"), new Vector2(256, 64));
            buttonList.Add(creditsButton);

            Button exitButton = new Button(5, this);
            exitButton.Initialise(new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_gameManager.GetTexture("UserInterface/MainMenu/ExitGame_MainMenu_Button").Width / 2), 470), _gameManager.GetTexture("UserInterface/MainMenu/ExitGame_MainMenu_Button"), new Vector2(256, 64));
            buttonList.Add(exitButton);

            _mainMenuLoaded = true;
            _gameManager.IsMouseVisible = true;
        }
    }
}
