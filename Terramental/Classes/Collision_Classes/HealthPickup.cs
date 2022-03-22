using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    public class HealthPickup : Sprite
    {
        private PlayerCharacter _playerCharacter;
        private int _amount;

        public HealthPickup(PlayerCharacter playerCharacter, int amount)
        {
            _playerCharacter = playerCharacter;
            _amount = amount;
        }


        public void CheckCollision()
        {
            if (OnCollision(_playerCharacter.SpriteRectangle) && IsActive)
            {
                _playerCharacter.Heal(_amount);
                IsActive = false;
            }
        }
    }
}
