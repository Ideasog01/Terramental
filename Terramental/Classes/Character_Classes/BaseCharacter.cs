using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terramental
{
    public class BaseCharacter : Sprite
    {
        /// <summary>
        /// All characters inherit from BaseCharacter class
        /// </summary>
        /// 
        public enum CharacterStatus { Default, Burning, Empowered, Frozen } // Enum holding the different possible states of base character

        private CharacterStatus _currentStatus = CharacterStatus.Default; // Sets the initial state to the default state
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
                SetAnimation(0); // Right animation
            }
            else if (SpriteVelocity.X < 0)  // Facing left 
            {
                SetAnimation(1); // Left animation
            }

            if (_takeDamageCooldown > 0)
            {
                _takeDamageCooldown -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds; // Decreases the timer for taking damage
            }

            if(_currentStatus != CharacterStatus.Default) // Checks to see if base character is not in the default state
            {
                UpdateStatus(gameTime);
            }
        }

        public void SetStatus(CharacterStatus status, float statusDuration, float statusDamageDuration, int statusDamageAmount)
        {
            _currentStatus = status; // Updates the state of basecharacter to the given CharacterStatus parameter
            _statusDuration = statusDuration; // Amount of time to apply the status

            _statusDamageDuration = statusDamageDuration;
            _statusDamageTimer = _statusDamageDuration;
            _statusDamageAmount = statusDamageAmount; // Amount of damage the status will apply

            if(_currentStatus == CharacterStatus.Burning)
            {
                if(_characterVFX == null)
                {
                    _characterVFX = SpawnManager.SpawnAnimatedVisualEffectAtSprite(SpawnManager.gameManager.GetTexture("Sprites/Effects/Flame_SpriteSheet"), this, new Vector2(46, 46), new Vector2(64, 64), statusDuration, 4, 120f);
                }
            }

            if(_currentStatus == CharacterStatus.Frozen)
            {
                if(_characterVFX == null)
                {
                    _characterVFX = SpawnManager.SpawnStaticVisualEffectAtSprite(SpawnManager.gameManager.GetTexture("Sprites/Effects/FrozenEffect"), this, Vector2.Zero, SpriteScale, statusDuration);
                }

                _disableMovement = true; // Disables movement if the character is frozen
            }
        }

        public void TakeDamage(int amount)
        {
            //TakeDamage is only called for Enemy Characters. For player, use PlayerTakeDamage().

            if(_takeDamageCooldown <= 0 && IsActive) // Checks if there is no time remaining in damage cooldown
            {
                _characterHealth -= amount; // Decreases health
                AudioManager.PlaySound("Hit_SFX"); // Plays hit sound effect
                _takeDamageCooldown = 0.2f; // Resets the damage cooldown

                if(_characterHealth <= 0) // Checks if the character has no health left
                {
                    if(_characterVFX != null)
                    {
                        _characterVFX.IsActive = false; // Removes any VFX related to the character
                        _characterVFX = null;
                    }

                    IsActive = false; // Removes the character
                    SpawnManager.gameManager.playerCharacter.EnemiesDefeated++; // Increments the count storing the number of enemies defeated
                    SpawnManager.gameManager.objectiveManager.UpdateObjective(ObjectiveManager.Objective.DefeatEnemies);
                    AudioManager.PlaySound("Hit_SFX");
                }
            }
        }

        public void Heal(int amount) { 
        
            if(_characterHealth + amount > _characterMaxHealth) // Checks to see if the amount of health that would be applied would exceed the max possible health
            {
                _characterHealth = _characterMaxHealth; // Sets the character health to maximum health
            }
            else
            {
                _characterHealth += amount; // Adds a given amount of health to the character
            }
        }

        private void UpdateStatus(GameTime gameTime)
        {
            if(_statusDuration > 0) // Checks to see if the status has not finished
            {
                _statusDuration -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds; // Decreases time left using statuss
            }
            else
            {
                if(_currentStatus == CharacterStatus.Frozen) // Checks to see if the character has the frozen status
                {
                    _disableMovement = false; // Disables movement
                }

                _currentStatus = CharacterStatus.Default; // Sets the status to the default state
            }

            if(_statusDamageAmount > 0) // Checks to see if the damage amount is greater than 0
            {
                if (_statusDamageTimer > 0) // Checks to see if there is time on the damage timer
                {
                    _statusDamageTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds; // Decreases damage timer
                }
                else
                {
                    if (_currentStatus != CharacterStatus.Default) 
                    {
                        TakeDamage(_statusDamageAmount); // Applies damage
                        _statusDamageTimer = _statusDamageDuration; // Resets damage timer
                    }
                }
            }
        }
    }
}
