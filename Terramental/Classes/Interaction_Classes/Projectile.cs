using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class Projectile : Sprite
    {
        private bool _isEnemyProjectile;
        private PlayerCharacter _playerCharacter;

        public void UpdateProjectile(GameTime gameTime)
        {
            if(IsActive)
            {
                SpritePosition += SpriteVelocity;

                if(_isEnemyProjectile)
                {
                    if (OnCollision(_playerCharacter.SpriteRectangle))
                    {
                        _playerCharacter.PlayerTakeDamage(1);
                        DestroyProjectile();
                    }
                }
                else
                {
                    foreach(EnemyCharacter enemy in SpawnManager.enemyList)
                    {
                        if(OnCollision(enemy.SpriteRectangle))
                        {
                            enemy.TakeDamage(20);
                            DestroyProjectile();
                        }
                    }
                }
                
            }
        }

        public void ResetProjectile(Texture2D texture, Vector2 position, Vector2 scale, Vector2 velocity, bool isEnemyProjectile)
        {
            SpriteTexture = texture;
            SpritePosition = position;
            SpriteScale = scale;
            SpriteVelocity = velocity;
            _isEnemyProjectile = isEnemyProjectile;

            IsActive = true;

            _playerCharacter = SpawnManager._gameManager.playerCharacter;
        }

        public void DestroyProjectile()
        {
            IsActive = false;
            SpawnManager.inactiveProjectileList.Add(this);
        }
    }
}
