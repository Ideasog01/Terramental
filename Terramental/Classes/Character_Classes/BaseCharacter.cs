using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TerraEngine.Graphics;

namespace Terramental
{
    public class BaseCharacter : Sprite
    {
        /// <summary>
        /// All characters inherit from BaseCharacter class
        /// </summary>
        /// 
        public enum CharacterStatus { Default, Burning, Empowered, Frozen }

        private CharacterStatus _currentStatus = CharacterStatus.Default;
        private float _statusDuration;
        private float _statusDamageDuration;
        private float _statusDamageTimer;
        private int _statusDamageAmount;

        private bool _disableMovement;

        private int _characterMaxHealth = 3;
        private int _characterHealth = 2;

        private float _takeDamageCooldown;
        private VisualEffect _characterVFX;

        private Vector2 _startPosition;

        public int CharacterHealth
        {
            get { return _characterHealth; }
            set { _characterHealth = value; }
        }

        public int CharacterMaxHealth
        {
            get { return _characterMaxHealth; }
            set { _characterMaxHealth = value; }
        }

        public bool DisableMovement
        {
            get { return  _disableMovement; }
            set { _disableMovement = value; }
        }

        public VisualEffect CharacterVFX
        {
            get { return _characterVFX; }
            set { _characterVFX = value; }
        }

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

        public void UpdateCharacter(GameTime gameTime)
        {
            if (SpriteVelocity.X > 0) // Facing right
            {
                SetAnimation(0);
            }
            else if (SpriteVelocity.X < 0)  // Facing left 
            {
                SetAnimation(1);
            }

            if (_takeDamageCooldown > 0)
            {
                _takeDamageCooldown -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(_currentStatus != CharacterStatus.Default)
            {
                UpdateStatus(gameTime);
            }
        }

        public void SetStatus(CharacterStatus status, float statusDuration, float statusDamageDuration, int statusDamageAmount)
        {
            _currentStatus = status;
            _statusDuration = statusDuration;

            _statusDamageDuration = statusDamageDuration;
            _statusDamageTimer = _statusDamageDuration;
            _statusDamageAmount = statusDamageAmount;

            if(_currentStatus == CharacterStatus.Frozen)
            {
                _disableMovement = true;
            }
        }

        public void TakeDamage(int amount)
        {
            if(_takeDamageCooldown <= 0 && IsActive)
            {
                _characterHealth -= amount;
                AudioManager.PlaySound("Hit_SFX");
                _takeDamageCooldown = 2f;

                if(_characterHealth <= 0)
                {
                    if(_characterVFX != null)
                    {
                        _characterVFX.IsActive = false;
                        _characterVFX = null;
                    }

                    IsActive = false;
                    SpawnManager._gameManager.playerCharacter.EnemiesDefeated++;
                    AudioManager.PlaySound("Hit_SFX");
                }
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

        private void UpdateStatus(GameTime gameTime)
        {
            if(_statusDuration > 0)
            {
                _statusDuration -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if(_currentStatus == CharacterStatus.Frozen)
                {
                    _disableMovement = false;
                }

                _currentStatus = CharacterStatus.Default;
            }

            if(_statusDamageAmount > 0)
            {
                if (_statusDamageTimer > 0)
                {
                    _statusDamageTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    if (_currentStatus != CharacterStatus.Default)
                    {
                        TakeDamage(_statusDamageAmount);
                        _statusDamageTimer = _statusDamageDuration;
                    }
                }
            }
        }
    }
}
