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
        private float _playerMovementSpeed = 0.5f;

        private bool _disableRight;
        private bool _disableLeft;
        private bool _disableMovement;

        //Ability Variables

        private bool _ultimateActive;
        private float _ultimateAbilityCooldown = 0;
        private float _ultimateActiveTimer = 0;
        private float _attackTimer;
        private int _elementIndex = 0;
        private bool _isJumping;
        private bool _isDoubleJumpUsed;
        private float _jumpHeight;
        private float _jumpSpeed;
        private Tile _groundTile;
        private Tile _leftTile;
        private Tile _rightTile;

        private List<Tile> _tileList;

        #region Properties

        public bool IsJumping
        {
            get { return _isJumping; }
            set { _isJumping = value; }
        }

        public bool IsGrounded
        {
            get { return _isGrounded; }
            set { _isGrounded = value; }
        }

        public bool IsDoubleJumpUsed
        {
            get { return _isDoubleJumpUsed; }
            set { _isDoubleJumpUsed = value; }
        }

        #endregion

        #region Player Core

        public void UpdatePlayerCharacter(GameTime gameTime)
        {
            UpdateUltimateStatus(gameTime);

            ApplyGravity();

            PlayerJumpBehavior(gameTime);

            CheckGroundCollision();

            CheckMovementCollision();

            MovementAnimations();

            if (_groundTile != null)
            {
                if (!_groundTile.TopCollision(this) || _groundTile.RightCollision(this) || _groundTile.LeftCollision(this))
                {
                    _isGrounded = false;
                    _groundTile = null;
                }
            }

            SpritePosition += SpriteVelocity;

        }

        public void PlayerMovement(float vertical, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (vertical > 0)
            {
                // IsFacingRight = true;

                if (!_disableRight)
                {
                    SpriteVelocity = new Vector2(_playerMovementSpeed * deltaTime, 0);
                }
                else
                {
                    SpriteVelocity = new Vector2(0, 0);
                }

            }
            else if (vertical < 0)
            {
                // IsFacingRight = false;

                if (!_disableLeft)
                {
                    SpriteVelocity = new Vector2(-_playerMovementSpeed * deltaTime, 0);
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

        public void PlayerJump()
        {
            if (!_isJumping && _isGrounded)
            {
                _isGrounded = false;
                _isJumping = true;
                _jumpHeight = SpritePosition.Y - 150;
                _jumpSpeed = -5;
                _isDoubleJumpUsed = false;
                return;
            }

            if (!_isDoubleJumpUsed)
            {
                _jumpHeight = SpritePosition.Y - 150;
                _jumpSpeed = -5;
                _isDoubleJumpUsed = true;
            }
        }

        private void PlayerJumpBehavior(GameTime gameTime)
        {
            if (_isJumping)
            {
                SpriteVelocity += new Vector2(0, _jumpSpeed);

                float distance = (SpritePosition.Y * SpritePosition.Y) - (_jumpHeight * _jumpHeight);

                CheckJumpCollision();

                if (SpritePosition.Y == _jumpHeight)
                {
                    _isJumping = false;
                    _isGrounded = false;
                }
            }
        }

        public void ApplyGravity()
        {
            if (!_isGrounded)
            {
                if (!_isJumping)
                {
                    SpriteVelocity = new Vector2(SpriteVelocity.X, 4);
                }
            }
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

        #region Collisions

        private void CheckGroundCollision()
        {
            _tileList = MapManager.tileList;

            foreach(Tile tile in _tileList)
            {
                if(tile.GroundTile)
                {
                    if(tile.TopCollision(this))
                    {
                        _isGrounded = true;
                        _groundTile = tile;
                    }
                }
            }
        }

        private void CheckJumpCollision()
        {
            _tileList = MapManager.tileList;

            foreach (Tile tile in _tileList)
            {
                if (tile.GroundTile)
                {
                    if (tile.BottomCollision(this))
                    {
                        _jumpHeight = SpritePosition.Y;
                    }
                }
            }
        }

        private void CheckMovementCollision()
        {
            _tileList = MapManager.tileList;

            foreach (Tile tile in _tileList)
            {
                if (tile.WallTile)
                {
                    if (tile.RightCollision(this))
                    {
                        _disableRight = true;
                        _rightTile = tile;
                    }

                    if (tile.LeftCollision(this))
                    {
                        _disableLeft = true;
                        _leftTile = tile;
                    }
                }
            }

            if(_disableRight && _rightTile != null)
            {
                if(!_rightTile.RightCollision(this))
                {
                    _disableRight = false;
                    _rightTile = null;
                }
            }

            if (_disableLeft && _leftTile != null)
            {
                if (!_leftTile.LeftCollision(this))
                {
                    _disableLeft = false;
                    _leftTile = null;
                }
            }


        }


        #endregion

        #region Animations

        public void InitialisePlayerAnimations(GameManager gameManager)
        {
                                                                                                                            //Index
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_Fire_SpriteSheet"), 5, 120f, true)); //0
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_LeftFire_SpriteSheet"), 5, 120f, true)); //1
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_Water_SpriteSheet"), 5, 120f, true)); //2
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_LeftWater_SpriteSheet"), 5, 120f, true)); //3
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_Snow_SpriteSheet"), 5, 120f, true)); //4
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_LeftSnow_SpriteSheet"), 5, 120f, true)); //5

            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Fire_Walk_SpriteSheet"), 4, 120f, true)); //6
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Fire_LeftWalk_SpriteSheet"), 4, 120f, true)); //7
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Water_Walk_SpriteSheet"), 4, 120f, true)); //8
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Water_LeftWalk_SpirteSheet"), 4, 120f, true)); //9
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Snow_Walk_SpriteSheet"), 4, 120f, true)); //10
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Snow_LeftWalk_SpriteSheet"), 4, 120f, true)); //11
        }

        private void MovementAnimations()
        {
            if(SpriteVelocity.X > 0)
            {
                switch(_elementIndex)
                {
                    case 0: SetAnimation(6);
                        break;
                    case 1: SetAnimation(8);
                        break;
                    case 2: SetAnimation(10);
                        break;
                    default: _elementIndex = 0;
                        break;
                }
            }
            else if(SpriteVelocity.X < 0)
            {
                switch (_elementIndex)
                {
                    case 0:
                        SetAnimation(7);
                        break;
                    case 1:
                        SetAnimation(9);
                        break;
                    case 2:
                        SetAnimation(11);
                        break;
                    default:
                        _elementIndex = 0;
                        break;
                }
            }
            else if(SpriteVelocity.X == 0)
            {
                if(AnimationIndex % 2 == 0)
                {
                    switch (_elementIndex)
                    {
                        case 0:
                            SetAnimation(0);
                            break;
                        case 1:
                            SetAnimation(2);
                            break;
                        case 2:
                            SetAnimation(4);
                            break;
                        default:
                            _elementIndex = 0;
                            break;
                    }
                }
                else
                {
                    switch (_elementIndex)
                    {
                        case 0:
                            SetAnimation(1);
                            break;
                        case 1:
                            SetAnimation(3);
                            break;
                        case 2:
                            SetAnimation(5);
                            break;
                        default:
                            _elementIndex = 0;
                            break;
                    }
                }
            }
        }

        #endregion

    }
}
