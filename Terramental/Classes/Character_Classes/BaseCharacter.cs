using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Terramental
{
    public class BaseCharacter : Sprite
    {
        /// <summary>
        /// All characters inherit from BaseCharacter class
        /// </summary>

        private int _characterMaxHealth = 100;

        private int _characterHealth = 80;

        private bool _isBurning;
        private float _burnTimer;

        private float _damageTimer;
        private float _damageWaitTime;

        public bool IsBurning
        {
            get { return _isBurning; }
            set { _isBurning = value; }
        }

        public void SetStatus(int index, float statusTime, float damageTime)
        {
            if(index == 0)
            {
                _isBurning = true;
                _burnTimer = statusTime;
                _damageTimer = damageTime;
                _damageWaitTime = damageTime;
            }
        }

        public void TakeDamage(int amount)
        {
            _characterHealth -= amount;

            if(_characterHealth <= 0)
            {
                IsActive = false;
            }
        }

        public void UpdateCharacter(GameTime gameTime)
        {
            if(_isBurning)
            {
                if(_burnTimer > 0)
                {
                    _burnTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if(_damageTimer > 0)
                    {
                        _damageTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        TakeDamage(10);
                        _damageTimer = _damageWaitTime;
                    }
                }
                else
                {
                    _isBurning = false;
                    _burnTimer = 0;
                }
            }

            if (SpriteVelocity.X > 0) // Facing right
            {
                SetAnimation(0);
            }
            else if (SpriteVelocity.X < 0)  // Facing left 
            {
                SetAnimation(1);
            }
        }

        public void Heal(int amount) { 
        
            if(_characterHealth + amount > _characterMaxHealth)
            {
                _characterHealth = _characterMaxHealth;
            }
            else
            {
                _characterHealth += amount;
                
            }



        }
    }
}
