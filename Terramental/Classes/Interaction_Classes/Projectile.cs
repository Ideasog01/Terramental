﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class Projectile : Sprite
    {
        private bool _isEnemyProjectile;
        private PlayerCharacter _playerCharacter;
        private int _projectileTrigger;

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
                        ProjectileTrigger(_playerCharacter);
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
                            ProjectileTrigger(enemy);
                            DestroyProjectile();
                        }
                    }
                }
                
            }
        }

        public void ResetProjectile(Texture2D texture, Vector2 position, Vector2 scale, Vector2 velocity, bool isEnemyProjectile, int projectileTrigger)
        {
            SpriteTexture = texture;
            SpritePosition = position;
            SpriteScale = scale;
            SpriteVelocity = velocity;
            _isEnemyProjectile = isEnemyProjectile;
            IsActive = true;
            _projectileTrigger = projectileTrigger;
            _playerCharacter = SpawnManager._gameManager.playerCharacter;
        }

        public void DestroyProjectile()
        {
            IsActive = false;
            SpawnManager.inactiveProjectileList.Add(this);
        }

        public void ProjectileTrigger(BaseCharacter character)
        {
            if(_projectileTrigger == 1)
            {
                character.SetStatus(0, 5, 1);
            }
        }
    }
}
