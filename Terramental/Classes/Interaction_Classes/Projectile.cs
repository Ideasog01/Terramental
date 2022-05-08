using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraEngine.Graphics;

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
            SpritePosition += SpriteVelocity; // Changes the projectile position based on the projectile velocity

            if (_isEnemyProjectile) // Checks to see if there is an enemy projectile
            {
                if(_activeDelay <= 0)
                {
                    if (OnCollision(_playerCharacter.SpriteRectangle))// Checks to see wether the enemy projectile has collided with the player
                    {
                        _playerCharacter.PlayerTakeDamage(_projectileDamage);
                        ProjectileTrigger(_playerCharacter);
                        DestroyProjectile();
                    }

                    foreach (Tile tile in MapManager.activeTiles) // Loops through every tile in the list of active tiles
                    {
                        if (tile.IsVisible) // Checks if the tile is visible
                        {
                            if (tile.IsActive) // Checks if the tile is active
                            {
                                if (OnCollision(tile.SpriteRectangle)) // Checks if the projectile has collided with the tile
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
                    foreach (EnemyCharacter enemy in SpawnManager.enemyList) // Loops through every enemy in the list of enemies
                    {
                        if (enemy.IsActive) // Checks if the enemy is active
                        {
                            if (_activeDelay <= 0)
                            {
                                if (OnCollision(enemy.SpriteRectangle)) // Checks if the projectile has collided with an enemy
                                {
                                    bool damageEnemy = true;

                                    if (_playerCharacter.ElementIndex == 0) // Checks the player element to see if they are using the fire element
                                    {
                                        if (enemy.ElementIndex != 2) // Check to see if the enemy is not of snow element type
                                        {
                                            SpawnManager._gameManager.tutorialManager.DisplayIncorrectElementNotification();
                                            damageEnemy = false; // Should not apply damage as the element is invalid
                                        }
                                    }

                                    if (_playerCharacter.ElementIndex == 1) // Checks the player element to see if they are using the water element
                                    {
                                        if (enemy.ElementIndex != 0) // Check to see if the enemy is not of fire element type
                                        {
                                            SpawnManager._gameManager.tutorialManager.DisplayIncorrectElementNotification();
                                            damageEnemy = false; // Should not apply damage as the element is invalid
                                        }
                                    }

                                    if (_playerCharacter.ElementIndex == 2) // Checks the player element to see if they are using the snow element
                                    {
                                        if (enemy.ElementIndex != 1) // Check to see if the enemy is not of water element type
                                        {
                                            SpawnManager._gameManager.tutorialManager.DisplayIncorrectElementNotification();
                                            damageEnemy = false; // Should not apply damage as the element is invalid
                                        }
                                    }


                                    if(damageEnemy) // Check to see whether damage should be applied
                                    {
                                        enemy.TakeDamage(_projectileDamage);

                                        if (enemy.CharacterHealth > 0) // Check to see if the enemy has health remaining
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

                    foreach (ElementWall elementWall in SpawnManager.elementWallList) // Loops through every elemental wall in the list of elemental walls
                    {
                        if (elementWall.IsActive) // Checks if the elemental wall is active
                        {
                            if (_activeDelay <= 0)
                            {
                                if (OnCollision(elementWall.SpriteRectangle)) // Check to see if the projectile has collided with an elemental wall
                                {
                                    elementWall.DamageElementWall();
                                    DestroyProjectile();
                                    break;
                                }
                            }
                        }
                    }

                    foreach (Tile tile in MapManager.activeTiles) // Loops through every tile in the list of active tiles
                    {
                        if (tile.IsVisible) // Checks if the tile is visible
                        {
                            if (tile.IsActive) // Checks if the tile is active
                            {
                                if (OnCollision(tile.SpriteRectangle)) // Checks if the projectile has collided with the tile
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
                _destroyTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds; // Decreases destroy timer
            }
            else
            {
                DestroyProjectile();
            }

            if(_activeDelay > 0)
            {
                _activeDelay -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds; // Decreases delay timer
            }
        }

        public void ResetProjectile(Texture2D texture, Vector2 position, Vector2 scale, Vector2 velocity, bool isEnemyProjectile, int projectileTrigger, float projectileDuration, int projectileDamage) // Used to reset a projectile to given parameters
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

            _activeDelay = 0.2f;
            IsActive = true;
        }

        public void DestroyProjectile() // Deletes the projectile
        {
            IsActive = false;
        }

        public void ProjectileTrigger(BaseCharacter character)
        {
            if(_projectileTrigger == 1) // Fire projectile
            {
                character.SetStatus(BaseCharacter.CharacterStatus.Burning, 5, 2, 20);

                if(character.CharacterVFX == null)
                {
                    character.CharacterVFX = SpawnManager.SpawnAnimatedVFX(SpawnManager._gameManager.GetTexture("Sprites/Effects/Flame_SpriteSheet"), new Vector2(12, 12), new Vector2(64, 64), 5, 4, 120f, character);
                }

            }

            if (_projectileTrigger == 2) // Water projectile
            {
                character.SetStatus(BaseCharacter.CharacterStatus.Empowered, 10, 0, 0);
            }

            if(_projectileTrigger == 3) // Snow projectile
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
