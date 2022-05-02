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
                    if (LeftCollision(new Rectangle(_playerCharacter.SpriteRectangle.X - 5, _playerCharacter.SpriteRectangle.Y, _playerCharacter.SpriteRectangle.Width, _playerCharacter.SpriteRectangle.Height)))
                    {
                        _playerCharacter.DisableLeft = true;
                        _playerCharacter.ElementWall = this;
                    }

                    if (RightCollision(new Rectangle(_playerCharacter.SpriteRectangle.X + 5, _playerCharacter.SpriteRectangle.Y, _playerCharacter.SpriteRectangle.Width, _playerCharacter.SpriteRectangle.Height)))
                    {
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
        public bool LeftCollision(Rectangle otherRectangle)
        {
            return (this.SpriteRectangle.Right <= otherRectangle.Right && this.SpriteRectangle.Right >= otherRectangle.Left - 5 && this.SpriteRectangle.Top <= otherRectangle.Bottom - (otherRectangle.Width / 4) && this.SpriteRectangle.Bottom >= otherRectangle.Top + (otherRectangle.Width / 4));
        }

        public bool RightCollision(Rectangle otherRectangle)
        {
            return (this.SpriteRectangle.Left >= otherRectangle.Left && this.SpriteRectangle.Left <= otherRectangle.Right + 5 && this.SpriteRectangle.Top <= otherRectangle.Bottom - (otherRectangle.Width / 4) && this.SpriteRectangle.Bottom >= otherRectangle.Top + (otherRectangle.Width / 4));
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
