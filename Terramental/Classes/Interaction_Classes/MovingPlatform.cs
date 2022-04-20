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

        private Vector2 _position;

        private float _moveDistPos;
        private float _moveDistNeg;

        private bool moveTowardsPos = true;
        private bool moveTowardsNeg = false;

        private int _moveDir;

        public MovingPlatform(PlayerCharacter playerCharacter, MapManager mapManager, Vector2 position, int moveDir)
        {
            _playerCharacter = playerCharacter;
            _mapManager = mapManager;
            _position = position;
            _moveDir = moveDir;

            if (_moveDir == 1) // 1 = Vertical moving platform
            {
                _moveDistPos = _position.Y + 300;
                _moveDistNeg = _position.Y - 300;
            }
            else if (_moveDir == 0) // 0 = Horizontal moving platform
            {
                _moveDistPos = _position.X + 300;
                _moveDistNeg = _position.X - 300;
            }
        }

        public void UpdateMovingPlatform()
        {
            if (_playerCharacter != null)
            {
                if (LeftCollision(_playerCharacter))
                {
                    _playerCharacter.SpriteVelocity = new Vector2(0, _playerCharacter.SpriteVelocity.Y);
                    _playerCharacter.DisableLeft = true;
                    _playerCharacter.MovingPlatform = this;

                }

                if (RightCollision(_playerCharacter))
                {
                    _playerCharacter.SpriteVelocity = new Vector2(0, _playerCharacter.SpriteVelocity.Y);
                    _playerCharacter.DisableRight = true;
                    _playerCharacter.MovingPlatform = this;

                }

                if (TopCollision(_playerCharacter))
                {
                    /*
                    Tile tile = _mapManager.GetTile(SpritePosition);
                    if (tile != null)
                    {
                        Debug.WriteLine("here");
                        _playerCharacter.SpriteVelocity = new Vector2(_playerCharacter.SpriteVelocity.X, 0);
                        _playerCharacter.IsGrounded = true;
                        _playerCharacter.GroundTile = tile;
                    }
                    */
                    _playerCharacter.SpriteVelocity = new Vector2(SpriteVelocity.X, SpriteVelocity.Y);
                    _playerCharacter.IsGrounded = true;
                }
                else
                {

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
            //Debug.WriteLine("sprite pos x: " + SpritePosition.X);
            //Debug.WriteLine("move dist: " + _moveDist);

            if(moveTowardsPos)
            {
                SpriteVelocity = new Vector2(3, 0);
                if (SpritePosition.X == _moveDistPos)
                {
                    moveTowardsPos = false;
                    moveTowardsNeg = true;
                }

            }

            if(moveTowardsNeg)
            {
                SpriteVelocity = new Vector2(-3, 0);
                if (SpritePosition.X == _moveDistNeg)
                {
                    moveTowardsNeg = false;
                    moveTowardsPos = true;
                }
            }

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
