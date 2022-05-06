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
        private float _activeDelay;

        public bool IsEnemyProjectile
        {
            get { return _isEnemyProjectile; }
        }

        public void UpdateProjectile(GameTime gameTime)
        {
            SpritePosition += SpriteVelocity;

            if (_isEnemyProjectile)
            {
                if(_activeDelay <= 0)
                {
                    if (OnCollision(_playerCharacter.SpriteRectangle))
                    {
                        _playerCharacter.PlayerTakeDamage(_projectileDamage);
                        ProjectileTrigger(_playerCharacter);
                        DestroyProjectile();
                    }

                    foreach (Tile tile in MapManager.activeTiles)
                    {
                        if (tile.IsVisible)
                        {
                            if (tile.IsActive)
                            {
                                if (OnCollision(tile.SpriteRectangle))
                                {
                                    DestroyProjectile();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {

                if(_activeDelay <= 0)
                {
                    foreach (EnemyCharacter enemy in SpawnManager.enemyList)
                    {
                        if (enemy.IsActive)
                        {
                            if (_activeDelay <= 0)
                            {
                                if (OnCollision(enemy.SpriteRectangle))
                                {
                                    bool damageEnemy = true;

                                    if (_playerCharacter.ElementIndex == 0)
                                    {
                                        if (enemy.ElementIndex != 2)
                                        {
                                            damageEnemy = false;
                                        }
                                    }

                                    if (_playerCharacter.ElementIndex == 1)
                                    {
                                        if (enemy.ElementIndex != 0)
                                        {
                                            damageEnemy = false;
                                        }
                                    }

                                    if (_playerCharacter.ElementIndex == 2)
                                    {
                                        if (enemy.ElementIndex != 1)
                                        {
                                            damageEnemy = false;
                                        }
                                    }


                                    if(damageEnemy)
                                    {
                                        enemy.TakeDamage(_projectileDamage);

                                        if (enemy.CharacterHealth > 0)
                                        {
                                            ProjectileTrigger(enemy);
                                        }
                                    }

                                    DestroyProjectile();
                                    break;
                                }
                            }
                        }
                    }

                    foreach (ElementWall elementWall in SpawnManager.elementWallList)
                    {
                        if (elementWall.IsActive)
                        {
                            if (_activeDelay <= 0)
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

                    foreach (Tile tile in MapManager.activeTiles)
                    {
                        if (tile.IsVisible)
                        {
                            if (tile.IsActive)
                            {
                                if (OnCollision(tile.SpriteRectangle))
                                {
                                    DestroyProjectile();
                                    break;
                                }
                            }
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

            if(_activeDelay > 0)
            {
                _activeDelay -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
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

            _activeDelay = 0.1f;
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

                if(character.CharacterVFX == null)
                {
                    character.CharacterVFX = SpawnManager.SpawnAnimatedVFX(SpawnManager._gameManager.GetTexture("Sprites/Effects/Flame_SpriteSheet"), new Vector2(12, 12), new Vector2(64, 64), 5, 4, 120f, character);
                }

            }

            if (_projectileTrigger == 2)
            {
                character.SetStatus(BaseCharacter.CharacterStatus.Empowered, 10, 0, 0);
            }

            if(_projectileTrigger == 3)
            {
                character.SetStatus(BaseCharacter.CharacterStatus.Frozen, 2, 0, 0);

                if(character.CharacterVFX == null)
                {
                    character.CharacterVFX = SpawnManager.SpawnStaticVFX(SpawnManager._gameManager.GetTexture("Sprites/Effects/FrozenEffect"), new Vector2(0, 0), new Vector2(96, 96), 2, character);
                }
            }
        }
    }
}
