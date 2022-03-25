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

        public void DestroyMenu()
        {

        }

        private void LoadMainMenu()
        {
            Button newGameButton = new Button(0, this);
            newGameButton.Initialise(new Vector2(_graphics.PreferredBackBufferWidth / 2, 100), _gameManager.GetTexture("UserInterface/MenuButton"), new Vector2(128, 64));
            buttonList.Add(newGameButton);

            _gameManager.IsMouseVisible = true;
        }
    }
}
