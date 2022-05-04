using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class Projectile : Sprite
    {
        private bool _isEnemyProjectile;
        private PlayerCharacter _playerCharacter;
        private int _projectileTrigger;
        private int _projectileDamage;
        private float _destroyTimer;

        public bool IsEnemyProjectile
        {
            get { return _isEnemyProjectile; }
        }

        public void UpdateProjectile(GameTime gameTime)
        {
            SpritePosition += SpriteVelocity;

            if (_isEnemyProjectile)
            {
                if (OnCollision(_playerCharacter.SpriteRectangle))
                {
                    _playerCharacter.PlayerTakeDamage(_projectileDamage);
                    ProjectileTrigger(_playerCharacter);
                    DestroyProjectile();
                }
            }
            else
            {
                foreach (EnemyCharacter enemy in SpawnManager.enemyList)
                {
                    if(enemy.IsActive)
                    {
                        if (OnCollision(enemy.SpriteRectangle))
                        {
                            enemy.TakeDamage(_projectileDamage);
                            ProjectileTrigger(enemy);
                            DestroyProjectile();
                            break;
                        }
                    }
                }

                foreach(ElementWall elementWall in SpawnManager.elementWallList)
                {
                    if(elementWall.IsActive)
                    {
                        if (OnCollision(elementWall.SpriteRectangle))
                        {
                            elementWall.DamageElementWall();
                            DestroyProjectile();
                            break;
                        }
                    }
                }
            }

            if (_destroyTimer > 0)
            {
                _destroyTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                DestroyProjectile();
            }
        }

        public void ResetProjectile(Texture2D texture, Vector2 position, Vector2 scale, Vector2 velocity, bool isEnemyProjectile, int projectileTrigger, float projectileDuration, int projectileDamage)
        {
            SpriteTexture = texture;
            SpritePosition = position;
            SpawnPosition = position;
            SpriteScale = scale;
            SpriteVelocity = velocity;
            _isEnemyProjectile = isEnemyProjectile;
            _projectileTrigger = projectileTrigger;
            _destroyTimer = projectileDuration;
            _projectileDamage = projectileDamage;

            if(_playerCharacter == null)
            {
                _playerCharacter = SpawnManager._gameManager.playerCharacter;
            }

            
            IsActive = true;
        }

        public void DestroyProjectile()
        {
            IsActive = false;
        }

        public void ProjectileTrigger(BaseCharacter character)
        {
            if(_projectileTrigger == 1)
            {
                character.SetStatus(BaseCharacter.CharacterStatus.Burning, 5, 2, 20);
            }

            if(_projectileTrigger == 3)
            {
                character.SetStatus(BaseCharacter.CharacterStatus.Frozen, 2, 0, 0);
            }
        }
    }
}
