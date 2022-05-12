using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Terramental
{
    public class TutorialManager
    {
        /// <summary>
        /// TutorialManager is used to display information to the player in the form of text/images that explain mechanics and how to play the game.
        /// </summary>

        public enum TutorialMessage { ElementPickup, ElementWall }; //The tutorial message to display

        private float _displayMessageTimer; //The duration of the message

        private string _displayMessage; //The content of the message
        private float _displayMessageWidth; //The width of message in pixels

        private bool _elementPickupActivated; //Used to check whether the elementPickup message has been displayed
        private bool _elementWallActivated; //Used to check whether the elementWall message has been displayed

        private SpriteFont _messageFont; //The font to use for the tutorial message text

        private PlayerCharacter _playerCharacter; //Reference to the player character

        private float _elementNotificationTimer; //Used to display the 'Invalid Element' image when the player uses the wrong element against an entity

        private Sprite elementNotificationSprite; //The object of the 'Invalid Element' image

        public TutorialManager(SpriteFont spriteFont, PlayerCharacter playerCharacter) //The TutorialManager constructor that assigns required references and loads the invalid element notification sprite
        {
            _messageFont = spriteFont;
            _playerCharacter = playerCharacter;

            elementNotificationSprite = new Sprite();
            Texture2D notificationTexture = SpawnManager.gameManager.GetTexture("UserInterface/InvalidElementText");
            elementNotificationSprite.Initialise(new Vector2(0, 0), notificationTexture, new Vector2(notificationTexture.Width, notificationTexture.Height));
            elementNotificationSprite.IsActive = false;
            elementNotificationSprite.LayerOrder = -2;
        }

        public void UpdateDisplayMessageTimer(GameTime gameTime)
        {
            if (_displayMessageTimer > 0)
            {
                _displayMessageTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                CheckDistance(); //If a message is not displaying, check the distance between the player and entities
            }

            if (_elementNotificationTimer > 0) //Update the invalid element notification's position for it display within the moving camera's view
            {
                _elementNotificationTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                elementNotificationSprite.SpritePosition = CameraController.cameraTopLeftAnchor + new Vector2(-110, -140) + new Vector2((GameManager.screenWidth / 2) - (_playerCharacter.SpriteRectangle.Width / 2), GameManager.screenHeight / 2 - (_playerCharacter.SpriteRectangle.Height / 2));
            }
            else if (elementNotificationSprite.IsActive)
            {
                elementNotificationSprite.IsActive = false;
            }
        }

        public void DisplayIncorrectElementNotification() //This function is called when the player uses the wrong element against an entity
        {
            _elementNotificationTimer = 2;
            elementNotificationSprite.IsActive = true;
            AudioManager.PlaySound("NegativeElement_SFX");
        }

        public void DrawMessage(SpriteBatch spriteBatch) //Displays the tutorial message if it has been activated via distance check between a valid entity and the player
        {
            if (GameManager.currentGameState == GameManager.GameState.Level)
            {
                if (_displayMessageTimer > 0) //Subtracts and divides the _displayMessageWidth value to display message near the centre of the screen
                {
                    spriteBatch.DrawString(_messageFont, _displayMessage, new Vector2(CameraController.cameraTopLeftAnchor.X + (GameManager.screenWidth / 2) - (_displayMessageWidth / 2), CameraController.cameraTopLeftAnchor.Y + (GameManager.screenHeight / 2) - 80), Color.White);
                }
            }
            else
            {
                _displayMessageTimer = 0;
            }
        }

        private void CheckDistance()
        {
            if (!_elementPickupActivated) //If the element pickup tutorial message has not been displayed, check the distance between the player and each element pickup entity. If the player is near, display the message.
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
                                _displayMessageTimer = 12;
                                _elementPickupActivated = true;
                                break;
                            }
                        }
                    }
                }

                if (!_elementWallActivated) //If the element wall tutorial message has not been displayed, check the distance between the player and each element wall entity. If the player is near, display the message.
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

                                    if (gamePadState.IsConnected) //Changes the message depending on whether a controller is connected to display the appropriate input.
                                    {
                                        _displayMessage = "YOU CAN USE YOUR ULTIMATE ABILITY BY PRESSING THE Y BUTTON.\nCAST AN ELEMENT BOLT BY PRESSING RIGHT TRIGGER.\nUSE YOUR ULTIMATE TO BREAK THROUGH THE WALL.";
                                    }
                                    else
                                    {
                                        _displayMessage = "YOU CAN USE YOUR ULTIMATE ABILITY BY USING THE Q KEY.\nCAST AN ELEMENT BOLT BY PRESSING LEFT MOUSE BUTTON.\nUSE YOUR ULTIMATE TO BREAK THROUGH THE WALL.";
                                    }

                                    _displayMessageWidth = _messageFont.MeasureString(_displayMessage).X;
                                    _displayMessageTimer = 12;
                                    _elementWallActivated = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}