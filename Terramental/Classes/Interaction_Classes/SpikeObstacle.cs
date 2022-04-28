using Microsoft.Xna.Framework;

namespace Terramental
{
    public class SpikeObstacle : Pickup
    {
        private float _collisionTimer;

        public void CheckCollision(GameTime gameTime)
        {
            if(_collisionTimer <= 0)
            {
                if (OnCollision(Player.SpriteRectangle))
                {
                    Player.CharacterHealth -= 40;
                    _collisionTimer = 2;
                }
            } 

            if(_collisionTimer > 0)
            {
                _collisionTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
