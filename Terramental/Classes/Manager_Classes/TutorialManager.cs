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

        private SpriteFont _messageFont;

        private PlayerCharacter _playerCharacter;

        public TutorialManager(SpriteFont spriteFont, PlayerCharacter playerCharacter)
        {
            _messageFont = spriteFont;
            _playerCharacter = playerCharacter;
        }

        public float DisplayMessageTimer
        {
            get { return _displayMessageTimer; }
        }

        public void DisplayMessage(TutorialMessage message)
        {
            GamePadState gamePadState = GamePad.GetState(0);

            switch(message)
            {
                case TutorialMessage.ElementPickup:

                    _displayMessage = "CHANGE YOUR ELEMENT BY HARNESSING THE NEARBY ELEMENT";

                    break;

                case TutorialMessage.ElementWall:

                    _displayMessage = "USE YOUR ULTIMATE WITH THE OPPOSITE ELEMENT TO BREAK THE WALL";

                    break;
            }

            _displayMessageTimer = 10;
            _displayMessageWidth = _displayMessage.Length * 6;
        }

        public void UpdateDisplayMessageTimer(GameTime gameTime)
        {
            if(_displayMessageTimer > 0)
            {
                _displayMessageTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                CheckDistance();
            }
        }

        public void DrawMessage(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_messageFont, _displayMessage, new Vector2(CameraController.cameraCentre.X + (GameManager.screenWidth / 2) - _displayMessageWidth, CameraController.cameraCentre.Y + (GameManager.screenHeight / 2) - 200), Color.White);
        }

        private void CheckDistance()
        {
            if(!_elementPickupActivated)
            {
                foreach(ElementPickup elementPickup in SpawnManager.elementPickupList)
                {
                    if(elementPickup.IsVisible)
                    {
                        if(elementPickup.IsActive)
                        {
                            float distanceToElementPickup = MathF.Sqrt(MathF.Pow(_playerCharacter.SpritePosition.X - elementPickup.SpritePosition.X, 2) + MathF.Pow(_playerCharacter.SpritePosition.Y - elementPickup.SpritePosition.Y, 2));

                            if(distanceToElementPickup < 300)
                            {
                                DisplayMessage(TutorialMessage.ElementPickup);
                                _elementPickupActivated = true;
                                break;
                            }
                        }
                    }
                    
                }
            }

            if(!_elementWallActivated)
            {
                foreach(ElementWall elementWall in SpawnManager.elementWallList)
                {
                    if(elementWall.IsVisible)
                    {
                        if(elementWall.IsActive)
                        {
                            float distanceToElementWall = MathF.Sqrt(MathF.Pow(_playerCharacter.SpritePosition.X - elementWall.SpritePosition.X, 2) + MathF.Pow(_playerCharacter.SpritePosition.Y - elementWall.SpritePosition.Y, 2));

                            if(distanceToElementWall < 300)
                            {
                                DisplayMessage(TutorialMessage.ElementWall);
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
