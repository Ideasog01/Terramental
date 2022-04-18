using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Terramental
{
    public class CannonProjectile : Sprite
    {
        private PlayerCharacter _playerCharacter;
        private float _cannonSpeed = 5.0f;
        private int _cannonDamage = 1;
        private int _cannonRange;

        private int _cannonDir;

        public CannonProjectile(PlayerCharacter playerCharacter, int cannonDir)
        {
            _playerCharacter = playerCharacter;
            _cannonDir = cannonDir;
        }

        public void UpdateCannonProjectile()
        {
            if (_cannonDir == 0) // Left facing cannon
            {
                SpriteVelocity = new Vector2(-_cannonSpeed, 0);
            }
            else if (_cannonDir == 1) // Right facing cannon
            {
                SpriteVelocity = new Vector2(_cannonSpeed, 0);
            }

            SpritePosition += SpriteVelocity;
        }

        public void CheckCannonProjectileCollisions()
        {
            if (_playerCharacter.SpriteRectangle != null)
            {
                if (OnCollision(_playerCharacter.SpriteRectangle) && IsActive)
                {
                    _playerCharacter.PlayerTakeDamage(1);
                    IsActive = false;
                    _playerCharacter.DisplayPlayerLives();
                }
            }
        }
    }
}





