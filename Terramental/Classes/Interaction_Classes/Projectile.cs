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
        private bool _isTracking;

        public bool IsEnemyProjectile
        {
            get { return _isEnemyProjectile; }
        }

        public void UpdateProjectile(GameTime gameTime)
        {
            if(!_isTracking)
            {
                SpritePosition += SpriteVelocity; // Changes the projectile position based on the projectile velocity
            }
            else
            {
                Vector2 dir = _playerCharacter.SpritePosition - SpritePosition; //Get the direction of the player from origin of the projectile's location
                dir.Normalize();
                SpriteVelocity = new Vector2(dir.X * 5, dir.Y * 5); //Set velocity so the enemy moves towards the player's location
                SpritePosition += SpriteVelocity;
            }
            

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
                                            SpawnManager.gameManager.tutorialManager.DisplayIncorrectElementNotification();
                                            damageEnemy = false; // Should not apply damage as the element is invalid
                                        }
                                    }

                                    if (_playerCharacter.ElementIndex == 1) // Checks the player element to see if they are using the water element
                                    {
                                        if (enemy.ElementIndex != 0) // Check to see if the enemy is not of fire element type
                                        {
                                            SpawnManager.gameManager.tutorialManager.DisplayIncorrectElementNotification();
                                            damageEnemy = false; // Should not apply damage as the element is invalid
                                        }
                                    }

                                    if (_playerCharacter.ElementIndex == 2) // Checks the player element to see if they are using the snow element
                                    {
                                        if (enemy.ElementIndex != 1) // Check to see if the enemy is not of water element type
                                        {
                                            SpawnManager.gameManager.tutorialManager.DisplayIncorrectElementNotification();
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
                        if(elementWall.IsActive) // Checks if the elemental wall is active
                        {
                            if (_activeDelay <= 0)
                            {
                                if (OnCollision(elementWall.SpriteRectangle)) // Check to see if the projectile has collided with an elemental wall
                                {
                                    ProjectileTrigger();
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

        public void ResetProjectile(Texture2D texture, Vector2 position, Vector2 scale, Vector2 velocity, bool isEnemyProjectile, int projectileTrigger, float projectileDuration, int projectileDamage, bool trackPlayer) // Used to reset a projectile to given parameters
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
            _isTracking = trackPlayer;

            if(_playerCharacter == null)
            {
                _playerCharacter = SpawnManager.gameManager.playerCharacter;
            }

            _activeDelay = 0.1f;
            IsActive = true;
        }

        public void DestroyProjectile() // Deletes the projectile
        {
            IsActive = false;
            IsLoaded = false;
        }

        public void ProjectileTrigger(BaseCharacter character)
        {
            if(_projectileTrigger == 1) // Fire projectile
            {
                character.SetStatus(BaseCharacter.CharacterStatus.Burning, 5, 2, 20);
                SpawnManager.SpawnVisualEffectAtPosition(SpawnManager.gameManager.GetTexture("Sprites/Effects/FireExplosion_SpriteSheet"), SpritePosition, new Vector2(64, 64), 1, 8, 120f);
                AudioManager.PlaySound("FireExplosion_SFX");
            }

            if (_projectileTrigger == 2) // Water projectile
            {
                character.SetStatus(BaseCharacter.CharacterStatus.Empowered, 10, 0, 0);
                SpawnManager.SpawnVisualEffectAtPosition(SpawnManager.gameManager.GetTexture("Sprites/Effects/WaterExplosion_SpriteSheet"), SpritePosition, new Vector2(64, 64), 1, 8, 120f);
                AudioManager.PlaySound("WaterExplosion_SFX");
            }

            if(_projectileTrigger == 3) // Snow projectile
            {
                character.SetStatus(BaseCharacter.CharacterStatus.Frozen, 2, 0, 0);
                SpawnManager.SpawnVisualEffectAtPosition(SpawnManager.gameManager.GetTexture("Sprites/Effects/SnowExplosion_SpriteSheet"), SpritePosition, new Vector2(64, 64), 1, 8, 120f);
                AudioManager.PlaySound("SnowExplosion_SFX");
            }
        }

        public virtual void ProjectileTrigger()
        {
            if(_projectileTrigger == 1)
            {
                SpawnManager.SpawnVisualEffectAtPosition(SpawnManager.gameManager.GetTexture("Sprites/Effects/FireExplosion_SpriteSheet"), SpritePosition, new Vector2(64, 64), 1, 8, 120f);
                AudioManager.PlaySound("FireExplosion_SFX");
            }

            if(_projectileTrigger == 2)
            {
                SpawnManager.SpawnVisualEffectAtPosition(SpawnManager.gameManager.GetTexture("Sprites/Effects/WaterExplosion_SpriteSheet"), SpritePosition, new Vector2(64, 64), 1, 8, 120f);
                AudioManager.PlaySound("WaterExplosion_SFX");
            }

            if(_projectileTrigger == 3)
            {
                SpawnManager.SpawnVisualEffectAtPosition(SpawnManager.gameManager.GetTexture("Sprites/Effects/SnowExplosion_SpriteSheet"), SpritePosition, new Vector2(64, 64), 1, 8, 120f);
                AudioManager.PlaySound("SnowExplosion_SFX");
            }
        }
    }
}
