using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace Terramental
{
    class PlayerCharacter : BaseCharacter
    {
        private float _playerMovementSpeed = 0.5f;

        private int _elementIndex = 0;

        private float _ultimateAbilityCooldown = 0;

        private float _ultimateActiveTimer = 0;

        private bool _ultimateActive;

        private bool _rightDirection;

        private float _attackTimer;

        public void ActivateUltimate()
        {
            if(_ultimateAbilityCooldown <= 0 && _ultimateActiveTimer <= 0)
            {
                switch(_elementIndex)
                {
                    case 0 :
                        FireUltimate();
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    default:
                        Console.WriteLine("ERROR: Element index is invalid during ultimate activatation");
                        break;
                }
            }
        }

        public void PrimaryAttack()
        {
            if(_ultimateActive)
            {
                switch (_elementIndex)
                {
                    case 0:
                        FireSwordAttack();
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    default:
                        Console.WriteLine("ERROR: Element index is invalid during ultimate attack");
                        break;
                }
            }
        }

        public void PlayerMovement(float vertical , GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(vertical > 0)
            {
                _rightDirection = true;
                SpritePosition += new Vector2(_playerMovementSpeed * deltaTime, 0);
            }
            else
            {
                _rightDirection = false;
                SpritePosition += new Vector2(-_playerMovementSpeed * deltaTime, 0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(_ultimateActiveTimer > 0)
            {
                _ultimateActiveTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(_ultimateActive)
            {
                _ultimateActive = false;
            }

            if(_attackTimer > 0)
            {
                _attackTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (_ultimateAbilityCooldown > 0 && !_ultimateActive)
            {
                _ultimateAbilityCooldown -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void FireUltimate()
        {
            _ultimateActive = true;
            _ultimateActiveTimer = 10;
        }

        public void FireSwordAttack()
        {
            if(_attackTimer <= 0)
            {
                Rectangle rect;

                if (_rightDirection)
                {
                    rect = new Rectangle((int)SpritePosition.X + 2, (int)SpritePosition.Y, 96, 96);
                }
                else
                {
                    rect = new Rectangle((int)SpritePosition.X - 2, (int)SpritePosition.Y, 96, 96);
                }

                Collision enemyCheck = new Collision(rect);

                foreach (BaseCharacter character in SpawnManager.enemyCharacters)
                {
                    if (enemyCheck.OnCollision(character.SpriteRectangle))
                    {
                        character.TakeDamage(20);
                        if (!character.IsBurning)
                        {
                            Vector2 scale = new Vector2(64, 128);
                            SpawnManager.SpawnEffect("Sprites/SpriteSheets/Effects/Flame_SpriteSheet", character.SpritePosition, scale, character, 5);
                            character.SetStatus(0, 5, 1.5f);
                        }

                    }
                }

                _attackTimer = 2;
                enemyCheck.IsActive = false;
            }
        }
    }
}
