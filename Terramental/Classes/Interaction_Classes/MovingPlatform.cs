using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Terramental
{
    public class MovingPlatform : Sprite

    {
        private PlayerCharacter _playerCharacter;

        private MapManager _mapManager;

        public MovingPlatform(PlayerCharacter playerCharacter, MapManager mapManager)
        {
            _playerCharacter = playerCharacter;
            _mapManager = mapManager;
        }

        public void UpdateMovingPlatform()
        {
            if (_playerCharacter != null)
            {
                if (LeftCollision(_playerCharacter))
                {
                    _playerCharacter.SpriteVelocity = new Vector2(0, _playerCharacter.SpriteVelocity.Y);
                    _playerCharacter.DisableLeft = true;
                }

                if (RightCollision(_playerCharacter))
                {
                    _playerCharacter.SpriteVelocity = new Vector2(0, _playerCharacter.SpriteVelocity.Y);
                    _playerCharacter.DisableRight = true;
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
            SpriteVelocity = new Vector2(1, 0);
            SpritePosition += SpriteVelocity;


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
