using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    class BaseCharacter : Sprite
    {
        private int _characterMaxHealth;

        private int _characterHealth;

        public void TakeDamage(int amount)
        {
            _characterHealth -= amount;
        }
    }
}
