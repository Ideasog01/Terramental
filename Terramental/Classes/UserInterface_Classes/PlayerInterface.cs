using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Terramental
{
    class PlayerInterface
    {
        public static List<InterfaceComponent> playerInterfaceElements = new List<InterfaceComponent>();

        private bool _isActive;

        private GameManager _gameManager;

        public PlayerInterface(GameManager gameManager)
        {
            _gameManager = gameManager;

            LoadPlayerInterface();
        }

        public void HidePlayerInterface(bool isActive)
        {
            if(isActive)
            {
                //Display
            }
            else
            {
                //Hide
            }
        }

        public void UpdatePlayerInterface()
        {
            foreach(InterfaceComponent component in playerInterfaceElements)
            {
                component.FollowCamera();
            }
        }

        public void DrawInterface(SpriteBatch spriteBatch)
        {
            foreach(InterfaceComponent component in playerInterfaceElements)
            {
                component.DrawComponent(spriteBatch);
            }
        }

        private void LoadPlayerInterface()
        {
            InterfaceComponent currentElement = new InterfaceComponent(_gameManager.playerCharacter, new Vector2(400, -200), new Vector2(64, 64), _gameManager.GetTexture("UserInterface/MainMenu/NewGame_MainMenu_Button"));
            playerInterfaceElements.Add(currentElement);
        }
    }
}
