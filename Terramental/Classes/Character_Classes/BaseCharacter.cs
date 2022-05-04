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
        public enum CharacterStatus { Default, Burning, Frozen }

        private CharacterStatus _currentStatus = CharacterStatus.Default;
        private float _statusDuration;
        private float _statusDamageDuration;
        private float _statusDamageTimer;
        private int _statusDamageAmount;

        private bool _disableMovement;

        private List<Sprite> _statusEffects = new List<Sprite>();

        private int _characterMaxHealth = 3;
        private int _characterHealth = 2;

        private float _takeDamageCooldown;

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

        public void LoadStatusEffects()
        {
            Sprite flameEffect = new Sprite();
            flameEffect.Initialise(SpritePosition, SpawnManager._gameManager.GetTexture("Sprites/Effects/Flame_SpriteSheet"), new Vector2(64, 64));
            flameEffect.Animations.Add(new Animation(SpawnManager._gameManager.GetTexture("Sprites/Effects/Flame_SpriteSheet"), 4, 120f, true, new Vector2(64, 64)));
            flameEffect.SetAnimation(0);
            flameEffect.AttachSpriteOffset = new Vector2(40, 24);
            flameEffect.AttachSprite = this;
            flameEffect.LayerOrder = -2;
            flameEffect.IsActive = false;
            _statusEffects.Add(flameEffect);

            Sprite frozenEffect = new Sprite();
            frozenEffect.Initialise(SpritePosition, SpawnManager._gameManager.GetTexture("Sprites/Effects/FrozenEffect"), new Vector2(96, 96));
            frozenEffect.AttachSpriteOffset = new Vector2(10, 10);
            frozenEffect.AttachSprite = this;
            frozenEffect.LayerOrder = -2;
            frozenEffect.IsActive = false;
            _statusEffects.Add(frozenEffect);
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

            if(_currentStatus == CharacterStatus.Burning)
            {
                _statusEffects[0].IsActive = true;
            }

            if(_currentStatus == CharacterStatus.Frozen)
            {
                _statusEffects[1].IsActive = true;
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
                    IsActive = false;
                    SpawnManager._gameManager.playerCharacter.EnemiesDefeated++;
                    AudioManager.PlaySound("Hit_SFX");

                    foreach(Sprite effect in _statusEffects)
                    {
                        effect.IsActive = false;
                    }
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
                if(_currentStatus == CharacterStatus.Burning)
                {
                    _statusEffects[0].IsActive = false;
                }

                if(_currentStatus == CharacterStatus.Frozen)
                {
                    _statusEffects[1].IsActive = false;
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
