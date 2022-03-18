using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace Terramental
{
    public class PlayerCharacter : BaseCharacter
    {

        //Movement Variables

        private bool _isGrounded;
        private bool _disableRight;
        private bool _disableLeft;
        private float _playerMovementSpeed = 0.5f;

        //Ability Variables

        private bool _ultimateActive;
        private float _ultimateAbilityCooldown = 0;
        private float _ultimateActiveTimer = 0;
        private float _attackTimer;
        private int _elementIndex = 0;
        private bool _isJumping;
        private float _jumpHeight;
        private float _jumpSpeed;

        #region Properties

        public bool IsGrounded
        {
            get { return _isGrounded; }
            set { _isGrounded = value; }
        }

        #endregion

        #region Ultimate Functions

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

        private void UpdateUltimateStatus(GameTime gameTime)
        {
            if (_ultimateActiveTimer > 0)
            {
                _ultimateActiveTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (_ultimateActive)
            {
                _ultimateActive = false;
            }

            if (_attackTimer > 0)
            {
                _attackTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (_ultimateAbilityCooldown > 0 && !_ultimateActive)
            {
                _ultimateAbilityCooldown -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        #region Fire Ultimate

        private void FireUltimate()
        {
            _ultimateActive = true;
            _ultimateActiveTimer = 10;
        }

        public void FireSwordAttack()
        {
            if (_attackTimer <= 0)
            {
                Rectangle rect;

                rect = new Rectangle((int)SpritePosition.X + 2, (int)SpritePosition.Y, 96, 96);

                foreach (BaseCharacter character in SpawnManager.enemyCharacters)
                {
                    if (this.OnCollision(character.SpriteRectangle))
                    {
                        character.TakeDamage(20);
                        if (!character.IsBurning)
                        {
                            Vector2 scale = new Vector2(64, 128);
                            SpawnManager.SpawnAttachEffect("Sprites/SpriteSheets/Effects/Flame_SpriteSheet", character.SpritePosition, scale, character, 5);
                            character.SetStatus(0, 5, 1.5f);
                        }

                    }
                }

                _attackTimer = 2;
            }
        }

        #endregion

        #endregion

        #region Player Core

        public void PlayerMovement(float vertical, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (vertical > 0)
            {
                if (!_disableRight)
                {
                    SpriteVelocity = new Vector2(_playerMovementSpeed * deltaTime, 0);
                    _disableLeft = false;
                }
                else
                {
                    SpriteVelocity = new Vector2(0, 0);
                }
            }
            else if (vertical < 0)
            {
                if (!_disableLeft)
                {
                    SpriteVelocity = new Vector2(-_playerMovementSpeed * deltaTime, 0);
                    _disableRight = false;
                }
                else
                {
                    SpriteVelocity = new Vector2(0, 0);
                }
            }

            if (vertical == 0)
            {
                SpriteVelocity = new Vector2(0, 0);
            }

        }

        public override void Update(GameTime gameTime)
        {
            UpdateUltimateStatus(gameTime);

            GroundCollision();

            PlayerJumpBehavior(gameTime);

            SpritePosition += SpriteVelocity;

        }

        public void PlayerJump()
        {
            
            if (!_isJumping && _isGrounded)
            {
                _isJumping = true;
                _jumpHeight = SpritePosition.Y - 150;
                _jumpSpeed = -5;
                
                
                
            }
            


        }

        private void PlayerJumpBehavior(GameTime gameTime)
        {

            if (_isJumping)
            {
               
                SpriteVelocity += new Vector2(0, _jumpSpeed);

                float distance = (SpritePosition.Y * SpritePosition.Y) - (_jumpHeight * _jumpHeight);

                if (_jumpSpeed < -1 && distance < 128)
                {
                    _jumpSpeed += 1f;
                }
             
                if (SpritePosition.Y == _jumpHeight)
                {
                    _isJumping = false;
                }



            }
        }
        #endregion

        #region Collisions

        public void GroundCollision()
        {
            if (!_isGrounded)
            {
                if (!_isJumping)
                {
                    SpriteVelocity = new Vector2(0, 4);
                    
                }
            }
            else
            {
                _isGrounded = false;
            }
        }
       

        public void WallCollision(bool left, bool right)
        {
            if (left && SpriteVelocity.X < 0)
            {
                _disableLeft = true;
            }

            if (right && SpriteVelocity.X > 0)
            {
                _disableRight = true;
            }
        }

        #endregion
    }
}
