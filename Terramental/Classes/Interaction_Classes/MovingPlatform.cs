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

        public void UpdateMovingPlatform(GameTime gameTime)
        {
            Rectangle leftRect;
            Rectangle rightRect;
            Rectangle topRect;
            leftRect = _playerCharacter.SpriteRectangle;
            rightRect = _playerCharacter.SpriteRectangle;
            topRect = _playerCharacter.SpriteRectangle;
            leftRect.Offset(-5, 0);
            rightRect.Offset(5, 0);
            topRect.Offset(0, 5);
            if (LeftCollision(leftRect) || RightCollision(rightRect) || TopCollision(topRect))
            {
                _playerCharacter.SpriteVelocity += new Vector2(SpriteVelocity.X, _playerCharacter.SpriteVelocity.Y);
                _playerCharacter.SpriteVelocity -= SpriteVelocity * Vector2.One * 0.075f;
            }

            //Debug.WriteLine("sprite pos x: " + SpritePosition.X);
            //Debug.WriteLine("move dist: " + _moveDist);
            if (moveTowardsPos)
            {
                SpriteVelocity = new Vector2(3, 0);
                if (SpritePosition.X == _moveDistPos)
                {
                    moveTowardsPos = false;
                    moveTowardsNeg = true;
                }

            }

            if (moveTowardsNeg)
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
        public bool LeftCollision(Rectangle otherSprite)
        {
            return (this.SpriteRectangle.Right <= otherSprite.Right && this.SpriteRectangle.Right >= otherSprite.Left - 5 && this.SpriteRectangle.Top <= otherSprite.Bottom - (otherSprite.Width / 4) && this.SpriteRectangle.Bottom >= otherSprite.Top + (otherSprite.Width / 4));
        }

        public bool RightCollision(Rectangle otherSprite)
        {
            return (this.SpriteRectangle.Left >= otherSprite.Left && this.SpriteRectangle.Left <= otherSprite.Right + 5 && this.SpriteRectangle.Top <= otherSprite.Bottom - (otherSprite.Width / 4) && this.SpriteRectangle.Bottom >= otherSprite.Top + (otherSprite.Width / 4));
        }

        public bool TopCollision(Rectangle otherSprite)
        {
            return (this.SpriteRectangle.Top + this.SpriteVelocity.Y < otherSprite.Bottom && this.SpriteRectangle.Bottom > otherSprite.Bottom && this.SpriteRectangle.Right > otherSprite.Left + (otherSprite.Width / 4) && this.SpriteRectangle.Left < otherSprite.Right - (otherSprite.Width / 4));
        }
    }
}
