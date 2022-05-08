using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Terramental
{
    public class MovingPlatform : Sprite

    {
        private PlayerCharacter _playerCharacter; // Reference to the player character

        private MapManager _mapManager; // Reference to the map manager

        private Vector2 _position; // Starting position to spawn the moving platform

        private float _moveDistPos; // Float value responsible for holding the final distance that the platform will end at in the positive direction
        private float _moveDistNeg; // Float value responsible for holding the final distance that the platform will end at in the negative direction

        private bool moveTowardsPos = true; // Boolean check to see whether the platform is moving in the positive direction (right)
        private bool moveTowardsNeg = false; // Boolean check to see whether the platform is moving in the negative direction (left)

        private int _moveDir; // Stores the type of moving platform, 1 = vertical moving platfrom 0 = horizontal moving platform

        public MovingPlatform(PlayerCharacter playerCharacter, MapManager mapManager, Vector2 position, int moveDir) // Moving platfrom constructor
        {
            _playerCharacter = playerCharacter;
            _mapManager = mapManager;
            _position = position;
            _moveDir = moveDir;

            if (_moveDir == 1) // 1 = Vertical moving platform
            {
                _moveDistPos = _position.Y + 300; // Adds 300 to the current Y position of the platform in order to calculate the final distance in the positive direction
                _moveDistNeg = _position.Y - 300; // Subtracts 300 to the current Y position of the platform in order to calculate the final distance in the negative direction
            }
            else if (_moveDir == 0) // 0 = Horizontal moving platform
            {
                _moveDistPos = _position.X + 300; // Adds 300 to the current X position of the platform in order to calculate the final distance in the positive direction
                _moveDistNeg = _position.X - 300; // Subtracts 300 to the current Y position of the platform in order to calculate the final distance in the negative direction
            }
        }

        public void UpdateMovingPlatform(GameTime gameTime) // Main code block of MovingPlatform.cs holding all logic that must be run every tick.
        {
            Rectangle leftRect; // Left rectangle used to add an offset to the player for collisions
            Rectangle rightRect; // Right rectangle offset used to add an offset to the player for collisions
            Rectangle topRect; // Top rectangle offset used to add an offset to the player for collisions

            leftRect = _playerCharacter.SpriteRectangle; // Sets leftRect to the player rectangle
            rightRect = _playerCharacter.SpriteRectangle; // Sets rightRect to the player rectangle
            topRect = _playerCharacter.SpriteRectangle; // Sets topRect to the player Rectangle

            leftRect.Offset(-5, 0);
            rightRect.Offset(5, 0);
            topRect.Offset(0, 5);

            if (LeftCollision(leftRect) || RightCollision(rightRect) || TopCollision(topRect)) // Checks to see if there is a collision between any of the offset player rectangles and the moving platform
            {
                _playerCharacter.SpriteVelocity = new Vector2(SpriteVelocity.X, _playerCharacter.SpriteVelocity.Y); // Sets the player velocity to a new vector2 made up of the x velocity of the moving platform, but keeps the y velocity of the player the same
                _playerCharacter.SpriteVelocity -= SpriteVelocity * Vector2.One * 0.075f; // Subtracts the frictional velocity added to the player so that the player stays on the platform
            }

            // Debug.WriteLine("sprite pos x: " + SpritePosition.X);
            // Debug.WriteLine("move dist: " + _moveDist);

            if (moveTowardsPos) // Checks to see if the moving platform is moving in the positive direction (right)
            {
                SpriteVelocity = new Vector2(3, 0); // Right movement
                if (SpritePosition.X == _moveDistPos) // Checks to see whether the X position of the moving platform is equal to the final distance value in the positive direction
                {
                    // Booleans used to reverse the movement direction of the platform 
                    moveTowardsPos = false;
                    moveTowardsNeg = true;
                }

            }

            if (moveTowardsNeg) // Checks to see if the moving platform is moving in the negative direction (left)
            {
                SpriteVelocity = new Vector2(-3, 0); // Left movement
                if (SpritePosition.X == _moveDistNeg) // Checks to see whether the X position of the moving platform is equal to the final distance value in the negative direction
                {
                    // Booleans used to reverse the movement direction of the platform 
                    moveTowardsNeg = false;
                    moveTowardsPos = true;
                }
            }
            SpritePosition += SpriteVelocity; // Applies the velocity to change the position of the player
        }
        public bool LeftCollision(Rectangle otherSprite) // Compares co ordinates of each edge for overlap, signifying there is a left collision
        {
            return (this.SpriteRectangle.Right <= otherSprite.Right && this.SpriteRectangle.Right >= otherSprite.Left - 5 && this.SpriteRectangle.Top <= otherSprite.Bottom - (otherSprite.Width / 4) && this.SpriteRectangle.Bottom >= otherSprite.Top + (otherSprite.Width / 4));
        }

        public bool RightCollision(Rectangle otherSprite) // Compares co ordinates of each edge for overlap, signifying there is a right collision
        {
            return (this.SpriteRectangle.Left >= otherSprite.Left && this.SpriteRectangle.Left <= otherSprite.Right + 5 && this.SpriteRectangle.Top <= otherSprite.Bottom - (otherSprite.Width / 4) && this.SpriteRectangle.Bottom >= otherSprite.Top + (otherSprite.Width / 4));
        }

        public bool TopCollision(Rectangle otherSprite) // Compares co ordinates of each edge for overlap, signifying there is a top collision
        {
            return (this.SpriteRectangle.Top + this.SpriteVelocity.Y < otherSprite.Bottom && this.SpriteRectangle.Bottom > otherSprite.Bottom && this.SpriteRectangle.Right > otherSprite.Left + (otherSprite.Width / 4) && this.SpriteRectangle.Left < otherSprite.Right - (otherSprite.Width / 4));
        }
    }
}
