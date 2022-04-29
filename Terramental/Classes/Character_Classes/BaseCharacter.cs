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

        private int _characterMaxHealth = 3;
        private int _characterHealth = 2;

        private bool _isBurning;
        private float _burnTimer;

        private bool _isFrozen;
        private bool _snowBeamCollision;
        private float _frozenTimer;

        private float _damageTimer;
        private float _damageWaitTime;

        private float _takeDamageCooldown;

        private int _statusIndex;

        private Vector2 _startPosition;

        public void SetProperties(Vector2 position, int maxHealth, int currentHealth)
        {
            _startPosition = position;
            _characterMaxHealth = maxHealth;
            _characterHealth = currentHealth;
        }

        public void ResetCharacter()
        {
            _characterHealth = _characterMaxHealth;
            SpritePosition = _startPosition;
        }

        public bool IsBurning
        {
            get { return _isBurning; }
            set { _isBurning = value; }
        }

        public void SetStatus(int index, float statusTime, float damageTime)
        {
            if(index != _statusIndex)
            {
                if (index == 0)
                {
                    _isBurning = true;
                    _burnTimer = statusTime;
                    _damageTimer = damageTime;
                    _damageWaitTime = damageTime;
                }

                if (index == 1)
                {
                    _frozenTimer = statusTime;
                    _snowBeamCollision = true;
                }

                _statusIndex = index;
            }
            
        }

        public void TakeDamage(int amount)
        {
            if(_takeDamageCooldown <= 0)
            {
                _characterHealth -= amount;

                _takeDamageCooldown = 2f;

                if (_characterHealth <= 0)
                {
                    IsActive = false;
                }
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
                    _statusIndex = 0;
                }
            }

            if(_snowBeamCollision)
            {
                if (_frozenTimer > 0)
                {
                    _frozenTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    _isFrozen = true;
                    _snowBeamCollision = false;
                    _frozenTimer = 4;
                    SpawnManager.SpawnAttachEffect("Sprites/Effects/FrozenEffect", SpritePosition, SpriteScale, this, 4, false);
                }
            }

            if(_isFrozen)
            {
                if(_frozenTimer > 0)
                {
                    _frozenTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    _isFrozen = false;

                    _statusIndex = 0;
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

            if(_takeDamageCooldown > 0)
            {
                _takeDamageCooldown -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
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

        public int CharacterHealth
        {
            get { return _characterHealth; }
            set { _characterHealth = value; }
        }
    }
}
