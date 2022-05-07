using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Terramental
{
    public class TutorialManager
    {
        public enum TutorialMessage { ElementPickup, ElementWall };

        private float _displayMessageTimer;

        private string _displayMessage = "HELLO!";
        private float _displayMessageWidth;

        private bool _elementPickupActivated;
        private bool _elementWallActivated;

        public bool displayingElementWallMessage;

        private SpriteFont _messageFont;

        private PlayerCharacter _playerCharacter;

        private float _elementNotificationTimer;

        private Sprite elementNotificationSprite;

        public TutorialManager(SpriteFont spriteFont, PlayerCharacter playerCharacter)
        {
            _messageFont = spriteFont;
            _playerCharacter = playerCharacter;

            elementNotificationSprite = new Sprite();
            Texture2D notificationTexture = SpawnManager._gameManager.GetTexture("UserInterface/InvalidElementText");
            elementNotificationSprite.Initialise(new Vector2(0, 0), notificationTexture, new Vector2(notificationTexture.Width, notificationTexture.Height));
            elementNotificationSprite.IsActive = false;
            elementNotificationSprite.LayerOrder = -2;
        }

        public float DisplayMessageTimer
        {
            get { return _displayMessageTimer; }
        }

        public void UpdateDisplayMessageTimer(GameTime gameTime)
        {
            if (_displayMessageTimer > 0)
            {
                _displayMessageTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                CheckDistance();
            }

            if(_elementNotificationTimer > 0)
            {
                _elementNotificationTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                elementNotificationSprite.SpritePosition = CameraController.cameraCentre + new Vector2(-110, -140) + new Vector2((GameManager.screenWidth / 2) - (_playerCharacter.SpriteRectangle.Width / 2), GameManager.screenHeight / 2 - (_playerCharacter.SpriteRectangle.Height / 2));
            }
            else if(elementNotificationSprite.IsActive)
            {
                elementNotificationSprite.IsActive = false;
            }
        }

        public void RemoveMessage()
        {
            _displayMessageTimer = 0;
            displayingElementWallMessage = false;
            _displayMessage = "";
        }

        public void DisplayIncorrectElementNotification()
        {
            _elementNotificationTimer = 2;
            elementNotificationSprite.IsActive = true;
            AudioManager.PlaySound("NegativeElement_SFX");
        }

        public void DrawMessage(SpriteBatch spriteBatch)
        {
            if (GameManager.currentGameState == GameManager.GameState.Level)
            {
                if(_displayMessageTimer > 0)
                {
                    spriteBatch.DrawString(_messageFont, _displayMessage, new Vector2(CameraController.cameraCentre.X + (GameManager.screenWidth / 2) - (_displayMessageWidth / 2), CameraController.cameraCentre.Y + (GameManager.screenHeight / 2) - 200), Color.White);
                }
            }
        }

        private void CheckDistance()
        {
            if (!_elementPickupActivated)
            {
                foreach (ElementPickup elementPickup in SpawnManager.elementPickupList)
                {
                    if (elementPickup.IsVisible)
                    {
                        if (elementPickup.IsActive)
                        {
                            float distanceToElementPickup = MathF.Sqrt(MathF.Pow(_playerCharacter.SpritePosition.X - elementPickup.SpritePosition.X, 2) + MathF.Pow(_playerCharacter.SpritePosition.Y - elementPickup.SpritePosition.Y, 2));

                            if (distanceToElementPickup < 300)
                            {
                                _displayMessage = "ELEMENTS CAN BE FOUND THROUGHOUT THE LANDS OF TERRA.\nUSE THEM TO OVERCOME OBSTACLES AND DEFEAT ENEMIES.";
                                _displayMessageWidth = _messageFont.MeasureString(_displayMessage).X;
                                _displayMessageTimer = 40;
                                _elementPickupActivated = true;
                                break;
                            }
                        }
                    }

                }
            }

            if (!_elementWallActivated)
            {
                foreach (ElementWall elementWall in SpawnManager.elementWallList)
                {
                    if (elementWall.IsVisible)
                    {
                        if (elementWall.IsActive)
                        {
                            float distanceToElementWall = MathF.Sqrt(MathF.Pow(_playerCharacter.SpritePosition.X - elementWall.SpritePosition.X, 2) + MathF.Pow(_playerCharacter.SpritePosition.Y - elementWall.SpritePosition.Y, 2));

                            if (distanceToElementWall < 300)
                            {
                                GamePadState gamePadState = GamePad.GetState(0);

                                if (gamePadState.IsConnected)
                                {
                                    _displayMessage = "YOU CAN USE YOUR ULTIMATE ABILITY BY PRESSING THE Y BUTTON.\nCAST AN ELEMENT BOLT BY PRESSING RIGHT TRIGGER.\nUSE YOUR ULTIMATE TO BREAK THROUGH THE WALL.";
                                    _displayMessageWidth = _messageFont.MeasureString(_displayMessage).X;
                                }
                                else
                                {
                                    _displayMessage = "YOU CAN USE YOUR ULTIMATE ABILITY BY USING THE Q KEY.\nCAST AN ELEMENT BOLT BY PRESSING LEFT MOUSE BUTTON.\nUSE YOUR ULTIMATE TO BREAK THROUGH THE WALL.";
                                    _displayMessageWidth = _messageFont.MeasureString(_displayMessage).X;
                                }

                                _displayMessageTimer = 40;

                                _elementWallActivated = true;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
