using Microsoft.Xna.Framework;

namespace Terramental
{
    public class SpikeObstacle : Pickup
    {
        private float _collisionTimer = 0;

        public void CheckCollision(GameTime gameTime)
        {
            if(_collisionTimer <= 0)
            {
                if (OnCollision(Player.SpriteRectangle))
                {
                    Player.PlayerTakeDamage(1);
                    _collisionTimer = 0.75f;
                }
            } 

            if(_collisionTimer > 0)
            {
                _collisionTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
