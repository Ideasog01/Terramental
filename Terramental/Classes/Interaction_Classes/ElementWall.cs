using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class ElementWall : Sprite
    {
        private PlayerCharacter _playerCharacter;

        private int _elementIndex;

        private bool _checkCollision;

        private MapManager _mapManager;

        public ElementWall(PlayerCharacter playerCharacter, MapManager mapManager, int elementIndex)
        {
            _playerCharacter = playerCharacter;
            _mapManager = mapManager;
            _elementIndex = elementIndex;
        }

        public int ElementIndex
        {
            set { _elementIndex = value; }
        }

        public void ElementWallCollisions()
        {
            if (_playerCharacter != null)
            {
                // 0 = Fire (Fire goest through snow)
                // 1 = Water (Water goes through fire)
                // 2 = Snow (Snow goes through water)

                if (_playerCharacter.ElementIndex == 0)
                {
                    if (_elementIndex == 0)
                    {
                        _checkCollision = true;
                    }
                    else if (_elementIndex == 1)
                    {
                        _checkCollision = true;
                    }
                    else if (_elementIndex == 2)
                    {
                        _checkCollision = false;
                    }
                }

                if (_playerCharacter.ElementIndex == 1)
                {
                    if (_elementIndex == 0)
                    {
                        _checkCollision = false;
                    }
                    else if (_elementIndex == 1)
                    {
                        _checkCollision = true;
                    }
                    else if (_elementIndex == 2)
                    {
                        _checkCollision = true;
                    }
                }

                if (_playerCharacter.ElementIndex == 2)
                {
                    if (_elementIndex == 0)
                    {
                        _checkCollision = true;
                    }
                    else if (_elementIndex == 1)
                    {
                        _checkCollision = false;
                    }
                    else if (_elementIndex == 2)
                    {
                        _checkCollision = true;
                    }
                }

                if (_checkCollision)
                {
                    if (LeftCollision(_playerCharacter))
                    {
                        _playerCharacter.SpriteVelocity = new Vector2(0, _playerCharacter.SpriteVelocity.Y);
                        _playerCharacter.DisableLeft = true;
                        _playerCharacter.ElementWall = this;
                    }

                    if (RightCollision(_playerCharacter))
                    {
                        _playerCharacter.SpriteVelocity = new Vector2(0, _playerCharacter.SpriteVelocity.Y);
                        _playerCharacter.DisableRight = true;
                        _playerCharacter.ElementWall = this;
                    }

                    if (TopCollision(_playerCharacter))
                    {
                        Tile tile = _mapManager.GetTile(SpritePosition);
                        if (tile != null)
                        {
                            _playerCharacter.SpriteVelocity = new Vector2(_playerCharacter.SpriteVelocity.X, 0);
                            _playerCharacter.IsGrounded = true;
                            _playerCharacter.GroundTile = tile;
                        }
                    }

                    if (BottomCollision(_playerCharacter))
                    {
                        _playerCharacter.SpriteVelocity = new Vector2(_playerCharacter.SpriteVelocity.X, 0);
                        if (_playerCharacter.IsJumping)
                        {
                            _playerCharacter.JumpHeight = _playerCharacter.SpritePosition.Y;
                        }
                    }


                }
            }


        }
        public bool LeftCollision(Sprite otherSprite)
        {
            return (this.SpriteRectangle.Right <= otherSprite.SpriteRectangle.Right && this.SpriteRectangle.Right >= otherSprite.SpriteRectangle.Left - 5 && this.SpriteRectangle.Top <= otherSprite.SpriteRectangle.Bottom - (otherSprite.SpriteRectangle.Width / 4) && this.SpriteRectangle.Bottom >= otherSprite.SpriteRectangle.Top + (otherSprite.SpriteRectangle.Width / 4));
        }

        public bool RightCollision(Sprite otherSprite)
        {
            return (this.SpriteRectangle.Left >= otherSprite.SpriteRectangle.Left && this.SpriteRectangle.Left <= otherSprite.SpriteRectangle.Right + 5 && this.SpriteRectangle.Top <= otherSprite.SpriteRectangle.Bottom - (otherSprite.SpriteRectangle.Width / 4) && this.SpriteRectangle.Bottom >= otherSprite.SpriteRectangle.Top + (otherSprite.SpriteRectangle.Width / 4));
        }

        public bool TopCollision(Sprite otherSprite)
        {
            return (this.SpriteRectangle.Top + this.SpriteVelocity.Y < otherSprite.SpriteRectangle.Bottom && this.SpriteRectangle.Bottom > otherSprite.SpriteRectangle.Bottom && this.SpriteRectangle.Right > otherSprite.SpriteRectangle.Left + (otherSprite.SpriteRectangle.Width / 4) && this.SpriteRectangle.Left < otherSprite.SpriteRectangle.Right - (otherSprite.SpriteRectangle.Width / 4));
        }

        public bool BottomCollision(Sprite otherSprite)
        {
            return (this.SpriteRectangle.Bottom + this.SpriteVelocity.Y > otherSprite.SpriteRectangle.Top && this.SpriteRectangle.Top < otherSprite.SpriteRectangle.Top && this.SpriteRectangle.Right > otherSprite.SpriteRectangle.Left + (otherSprite.SpriteRectangle.Width / 4) && this.SpriteRectangle.Left < otherSprite.SpriteRectangle.Right - (otherSprite.SpriteRectangle.Width / 4));
        }
    }
}
