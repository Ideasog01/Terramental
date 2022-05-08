using Microsoft.Xna.Framework;

namespace Terramental
{
    public class SpikeObstacle : Pickup
    {
        private float _collisionTimer = 0;

        public void CheckCollision(GameTime gameTime)
        {
            if(_collisionTimer <= 0) // Checks to see if the value for the collision timer is less than or equal to zero (to check for a new collision)
            {
                if (OnCollision(Player.SpriteRectangle)) // Checks to see if the spike has collided with the player
                {
                    Player.PlayerTakeDamage(1);
                    _collisionTimer = 0.75f; // Resets the collision timer
                }
            } 

            if(_collisionTimer > 0) // Collision timer is used to prevent the player from losing all their health too quickly
            {
                _collisionTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds; // Decreases collision timer value
            }
        }
    }
}
