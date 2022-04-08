using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Terramental
{
    class InputManager
    {
        /// <summary>
        /// InputManager controls all essential player input
        /// </summary>

        private MenuManager _menuManager;
        private CameraController _playerCam;
        private GameManager _gameManager;

        private KeyboardState _currentKeyboardState = Keyboard.GetState();
        private MouseState _currentMouseState = Mouse.GetState();

        public InputManager(CameraController playerCam, MenuManager menuManager, GameManager gameManager)
        {
            _playerCam = playerCam;
            _menuManager = menuManager;
            _gameManager = gameManager;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardMouseInput(gameTime);
        }

        private void KeyboardMouseInput(GameTime gameTime)
        {
            KeyboardState oldKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            MouseState oldMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if(_currentMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                _menuManager.MouseClick(_playerCam.ScreenToWorldSpace(new Vector2(oldMouseState.Position.X, oldMouseState.Position.Y)));

                if(DialogueManager.dialogueActive)
                {
                    if(_gameManager.dialogueManager != null)
                    {
                        _gameManager.dialogueManager.CheckButtonInteraction(_playerCam.ScreenToWorldSpace(new Vector2(oldMouseState.Position.X, oldMouseState.Position.Y)));
                    }
                }
            }

            if(oldKeyboardState.IsKeyDown(Keys.F) && _currentKeyboardState.IsKeyUp(Keys.F))
            {
                _gameManager.playerCharacter.CharacterHealth = 0;

                _menuManager.DisplayRespawnScreen(true);
            }

            if(_gameManager.playerCharacter != null)
            {
                if (_currentKeyboardState.IsKeyDown(Keys.D))
                {
                    _gameManager.playerCharacter.PlayerMovement(1, gameTime);
                }
                else if (_currentKeyboardState.IsKeyDown(Keys.A))
                {
                    _gameManager.playerCharacter.PlayerMovement(-1, gameTime);
                }
                else if (_currentKeyboardState.IsKeyUp(Keys.D) && _currentKeyboardState.IsKeyUp(Keys.A))
                {
                    _gameManager.playerCharacter.PlayerMovement(0, gameTime);
                }

                if (_currentKeyboardState.IsKeyUp(Keys.Q) && oldKeyboardState.IsKeyDown(Keys.Q))
                {
                    _gameManager.playerCharacter.ActivateUltimate();
                }

                if (_currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    _gameManager.playerCharacter.PrimaryAttack();
                }

                if (_currentKeyboardState.IsKeyUp(Keys.Space) && oldKeyboardState.IsKeyDown(Keys.Space))
                {
                    _gameManager.playerCharacter.PlayerJump();
                }
            }
        }

    }
}
