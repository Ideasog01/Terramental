using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;

namespace Terramental
{
    public class PlayerCharacter : BaseCharacter
    {
        public static bool disableMovement;

        public float dashCooldown;
        public float ultimateCooldown;

        private int _playerScore;
        private int _enemiesDefeated;

        public float deltaTime;
        //Movement Variables

        private bool _isGrounded;
        private float _playerMovementSpeed = 0.5f;
        private int _horizontalAxisRaw;
        private int _verticalAxisRaw;
        private int _lastNonZeroVAR;
        private int _lastNonZeroHAR;

        private bool _disableRight;
        private bool _disableLeft;

        // Dash Variables
        private float dashVelocity = 10.0f;
        private bool _isDashing;
        private bool _canDash = true;
        private bool _isHovering;
        private bool _dashActive;

        private int upDashCheck = 0;
        private int leftDashCheck = 0;
        private int rightDashCheck = 0;

        private int dashDirX;
        private int dashDirY;

        private float _dashDistX;
        private float _dashDistY;

        private bool _isFacingRight;

        public enum DashDirections
        {
            Up,
            Left,
            Right
        }
        public DashDirections dashDir = DashDirections.Right;

        //Ability Variables

        public bool ultimateActive;
        public float ultimateActiveTimer = 0;
        private float _attackTimer;
        private int _elementIndex = 0;
        private bool _isJumping;
        private bool _isDoubleJumpUsed;
        private float _jumpHeight;
        private float _jumpSpeed;
        private Tile _groundTile;
        private Tile _leftTile;
        private Tile _rightTile;
        private ElementWall _elementWall;
        private float _ultimateUsedTimer;

        private List<Tile> _tileList;
        private GameManager _gameManager;

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

        public bool DisableRight
        {
            get { return _disableRight; }
            set { _disableRight = value; }
        }

        public bool DisableLeft
        {
            get { return _disableLeft; }
            set { _disableLeft = value; }
        }

        public Tile LeftTile
        {
            get { return _leftTile; }
            set { _leftTile = value; }
        }

        public ElementWall ElementWall
        {
            get { return _elementWall; }
            set { _elementWall = value; }
        }

        public Tile RightTile
        {
            get { return _rightTile; }
            set { _rightTile = value; }
        }

        public int ElementIndex
        {
            get { return _elementIndex; }
            set { _elementIndex = value; }
        }

        public int PlayerScore
        {
            get { return _playerScore; }
            set { _playerScore = value; }
        }

        public int EnemiesDefeated
        {
            get { return _enemiesDefeated; }
            set { _enemiesDefeated = value; }
        }

        public float JumpHeight
        {
            get { return _jumpHeight; }
            set { _jumpHeight = value; }

        }

        public Tile GroundTile
        {
            get { return _groundTile; }
            set { _groundTile = value; }
        }

        public int HorizontalAxisRaw
        {
            get { return _horizontalAxisRaw; }
            set { _horizontalAxisRaw = value; }
        }

        public int VerticalAxisRaw
        {
            get { return _verticalAxisRaw; }
            set { _verticalAxisRaw = value; }
        }

        public int LastNonZeroVAR
        {
            get { return _lastNonZeroVAR; }
            set { _lastNonZeroVAR = value; }
        }

        public int LastNonZeroHAR
        {
            get { return _lastNonZeroHAR; }
            set { _lastNonZeroHAR = value; }
        }

        public bool CanDash
        {
            get { return _canDash; }
            set { _canDash = value; }
        }

        #endregion

        #region Player Core

        public PlayerCharacter(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void ResetPlayer()
        {
            CharacterHealth = 3;
            SpritePosition = GameManager.playerCheckpoint;
            _gameManager.playerInterface.UpdatePlayerLives(3);
            ultimateActiveTimer = 0;
            ultimateActive = false;
            ultimateCooldown = 0;
            disableMovement = false;
            _isJumping = false;
            _playerScore = 0;
            _isGrounded = false;
            _enemiesDefeated = 0;
            _elementIndex = 0;
        }

        public void UpdatePlayerCharacter(GameTime gameTime)
        {
            if(GameManager.currentGameState == GameManager.GameState.Level)
            {
                float currentTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                UpdateUltimateStatus(gameTime);

                if(_dashActive)
                {
                    foreach (EnemyCharacter knight in SpawnManager.enemyList)
                    {
                        if (knight.IsActive)
                        {
                            bool checkCollision = false;

                            if (_elementIndex == 0)
                            {
                                if (knight.ElementIndex == 0)
                                {
                                    checkCollision = true;
                                }
                                else if (knight.ElementIndex == 1)
                                {
                                    checkCollision = true;
                                }
                                else if (knight.ElementIndex == 2)
                                {
                                    checkCollision = false;
                                }
                            }

                            if (_elementIndex == 1)
                            {
                                if (knight.ElementIndex == 0)
                                {
                                    checkCollision = false;
                                }
                                else if (knight.ElementIndex == 1)
                                {
                                    checkCollision = true;
                                }
                                else if (knight.ElementIndex == 2)
                                {
                                    checkCollision = true;
                                }
                            }

                            if (_elementIndex == 2)
                            {
                                if (knight.ElementIndex == 0)
                                {
                                    checkCollision = true;
                                }
                                else if (knight.ElementIndex == 1)
                                {
                                    checkCollision = false;
                                }
                                else if (knight.ElementIndex == 2)
                                {
                                    checkCollision = true;
                                }
                            }

                            if(!checkCollision)
                            {
                                if (knight.OnCollision(this.SpriteRectangle))
                                {
                                    knight.TakeDamage(25);
                                }
                            }
                        }
                    }
                }

                DashCheck();

                ApplyGravity();

                PlayerJumpBehavior(gameTime);

                Dash();

                CheckCollisions();

                if(_ultimateUsedTimer <= 0)
                {
                    MovementAnimations();
                }

                if (_groundTile != null)
                {
                    if (!_groundTile.TopCollision(this) || _groundTile.RightCollision(new Rectangle((int)SpritePosition.X, (int)SpritePosition.Y, (int)SpriteScale.X, (int)SpriteScale.Y)) || _groundTile.LeftCollision(new Rectangle((int)SpritePosition.X, (int)SpritePosition.Y, (int)SpriteScale.X, (int)SpriteScale.Y)))
                    {
                        _isGrounded = false;
                        _groundTile = null;
                    }
                }

                if(_ultimateUsedTimer > 0)
                {
                    switch (_elementIndex)
                    {
                        case 0:
                            SetAnimation((int)AnimationIndexEnum.FireUltimate);

                            break;
                        case 1:

                            SetAnimation((int)AnimationIndexEnum.WaterUltimate);

                            break;
                        case 2:
                            SetAnimation((int)AnimationIndexEnum.SnowUltimate);

                            break;
                    }
                }

                SpritePosition += SpriteVelocity;

                float posX = MathHelper.Clamp(SpritePosition.X, 0, (MapManager.mapWidth - 1) * 64);

                SpritePosition = new Vector2(posX, SpritePosition.Y);

                Animations[AnimationIndex].MirrorTexture = !_isFacingRight;
            }
        }

        public void PlayerMovement(float horizontal, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (!_isHovering)
            {
                if(!disableMovement)
                {
                    if(horizontal > 0)
                    {
                        if(!_disableRight)
                        {
                            SpriteVelocity = new Vector2(_playerMovementSpeed * deltaTime, 0);
                        }
                        else
                        {
                            SpriteVelocity = new Vector2(0, 0);
                        }

                        _isFacingRight = true;
                    }
                    else if(horizontal < 0)
                    {
                        if (!disableMovement)
                        {
                            if (!_disableLeft)
                            {
                                SpriteVelocity = new Vector2(-_playerMovementSpeed * deltaTime, 0);
                            }
                            else
                            {
                                SpriteVelocity = new Vector2(0, 0);
                            }
                        }

                        _isFacingRight = false;
                    }
                }
            }

            if (horizontal == 0)
            {
                SpriteVelocity = new Vector2(0, 0);
            }
        }

        public void TeleportPlayer(Vector2 position, bool setCheckpoint)
        {
            SpritePosition = position;

            if(setCheckpoint)
            {
                GameManager.playerCheckpoint = position;
            }
        }

        public void PlayerJump()
        {
            if(!disableMovement)
            {
                if (!_isJumping && _isGrounded)
                {
                    AudioManager.PlaySound("PlayerJump_SFX");
                    _isGrounded = false;
                    _isJumping = true;
                    _jumpHeight = SpritePosition.Y - 150;
                    _jumpSpeed = -5;
                    _isDoubleJumpUsed = false;
                    return;
                }

                if (!_isDoubleJumpUsed)
                {
                    AudioManager.PlaySound("PlayerJump_SFX");
                    _jumpHeight = SpritePosition.Y - 150;
                    _jumpSpeed = -5;
                    _isDoubleJumpUsed = true;
                }
            }
        }

        private void PlayerJumpBehavior(GameTime gameTime)
        {
            if (_isJumping)
            {
                SpriteVelocity += new Vector2(0, _jumpSpeed);

                float distance = (SpritePosition.Y * SpritePosition.Y) - (_jumpHeight * _jumpHeight);

                if (SpritePosition.Y <= _jumpHeight || disableMovement)
                {
                    _isJumping = false;
                    _isGrounded = false;
                }
            }
        }
        public void DashStateMachine()
        {
            switch (dashDir)
            {
                case DashDirections.Up:
                    upDashCheck+=1;
                    DoubleTapToDashCooldown();
                    break;
                case DashDirections.Left:
                    leftDashCheck++;
                    DoubleTapToDashCooldown();
                    break;
                case DashDirections.Right:
                    rightDashCheck++;
                    DoubleTapToDashCooldown();
                    break;
            }
        }

        public void DashCheck()
        {
            if (!_isDashing)
            {
                if (upDashCheck >= 2)
                {
                    dashDirY = -1;
                    dashDirX = 0;
                    _dashDistY = SpritePosition.Y - 500;
                    //Dash();
                    _dashActive = true;
                    _isDashing = true;
                }
                if (leftDashCheck >= 2)
                {
                    dashDirX = -1;
                    dashDirY = 0;
                    _dashDistX = SpritePosition.X - 500;
                    //Dash();
                    _dashActive = true;
                    _isDashing = true;
                }
                if (rightDashCheck >= 2)
                {
                    dashDirX = 1;
                    dashDirY = 0;
                    _dashDistX = SpritePosition.X + 500;
                    //Dash();
                    _dashActive = true;
                    _isDashing = true;
                }
            }
        }

        public void Dash()
        {
            if (_isDashing)
            {
                if (!_disableRight && !_disableLeft)
                {
                    SpriteVelocity += new Vector2(dashDirX * dashVelocity, dashDirY * dashVelocity);
                }
                else
                {
                    SpriteVelocity = new Vector2(0, 0);
                }

                _isDashing = false;
            }
        }

        public async void DoubleTapToDashCooldown()
        {
            await Task.Delay(500);

            upDashCheck = 0;
            leftDashCheck = 0;
            rightDashCheck = 0;
            _dashActive = false;
        }

        public void ApplyGravity()
        {
            if (!_isGrounded)
            {
                if (!_isJumping)
                {
                    if (!_isDashing)
                    {
                        SpriteVelocity = new Vector2(SpriteVelocity.X, 4);
                    }

                }
            }
        }

        public void PlayerTakeDamage(int amount)
        {
            CharacterHealth -= amount;
            _gameManager.playerInterface.UpdatePlayerLives(CharacterHealth);

            if (CharacterHealth <= 0)
            {
                _gameManager.menuManager.DisplayRespawnScreen(true);
            }
        }

        public void DisplayPlayerLives()
        {
            _gameManager.playerInterface.UpdatePlayerLives(CharacterHealth);
        }

        #endregion

        #region Ultimate Functions

        public void ActivateUltimate()
        {
            if (ultimateCooldown <= 0 && ultimateActiveTimer <= 0)
            {
                ultimateActiveTimer = 10;
                ultimateActive = true;
            }
        }

        public void PrimaryUltimateAttack()
        {
            if (ultimateActive)
            {
                switch (_elementIndex)
                {
                    case 0:

                        if(!Animations[AnimationIndex].MirrorTexture)
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Fire_Projectile"), SpritePosition + new Vector2(40, 0), new Vector2(32, 32), new Vector2(4, 0), false, true, 1, 6, 30);
                        }
                        else
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Fire_Projectile"), SpritePosition - new Vector2(40, 0), new Vector2(32, 32), new Vector2(-4, 0), false, true, 1, 6, 30);
                        }
                       
                        break;
                    case 1:

                        if(!Animations[AnimationIndex].MirrorTexture)
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Fire_Projectile"), SpritePosition + new Vector2(40, 0), new Vector2(32, 32), new Vector2(4, 0), false, true, 2, 6, 40);
                        }
                        else
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Fire_Projectile"), SpritePosition - new Vector2(40, 0), new Vector2(32, 32), new Vector2(-4, 0), false, true, 2, 6, 40);
                        }

                        break;
                    case 2:

                        if (!Animations[AnimationIndex].MirrorTexture)
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Snow_Projectile"), SpritePosition + new Vector2(40, 0), new Vector2(32, 32), new Vector2(4, 0), false, true, 3, 6, 20);
                        }
                        else
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Snow_Projectile"), SpritePosition - new Vector2(40, 0), new Vector2(32, 32), new Vector2(-4, 0), false, true, 3, 6, 20);
                        }

                        break;
                    default:
                        Console.WriteLine("ERROR: Element index is invalid during ultimate attack");
                        break;
                }

                _ultimateUsedTimer = 0.5f;
                _attackTimer = 2;
            }
        }

        private void UpdateUltimateStatus(GameTime gameTime)
        {
            if (ultimateActiveTimer > 0)
            {
                ultimateActiveTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (ultimateActive)
            {
                ultimateCooldown = 10;
                _ultimateUsedTimer = 0;
                ultimateActive = false;
            }

            if (_attackTimer > 0)
            {
                _attackTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (ultimateCooldown > 0 && !ultimateActive)
            {
                ultimateCooldown -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(_ultimateUsedTimer > 0)
            {
                _ultimateUsedTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        #endregion

        #region Collisions

        private void CheckCollisions()
        {
            if (_elementWall != null)
            {
                if (!_elementWall.RightCollision(new Rectangle(SpriteRectangle.X, SpriteRectangle.Y, SpriteRectangle.Width, SpriteRectangle.Height)) && !_elementWall.LeftCollision(new Rectangle(SpriteRectangle.X, SpriteRectangle.Y, SpriteRectangle.Width, SpriteRectangle.Height)))
                {
                    _disableRight = false;
                    _elementWall = null;
                    return;
                }

                if (!_elementWall.LeftCollision(new Rectangle(SpriteRectangle.X, SpriteRectangle.Y, SpriteRectangle.Width, SpriteRectangle.Height)))
                {
                    _disableLeft = false;
                    _elementWall = null;
                }
            }
            else
            {
                _disableLeft = false;
                _disableRight = false;
            }

            foreach (Tile tile in MapManager.activeTiles)
            {
                if (tile.GroundTile)
                {
                    if (tile.TopCollision(this))
                    {
                        _isGrounded = true;
                        _groundTile = tile;
                      
                    }

                    if (tile.BottomCollision(this))
                    {
                        _isJumping = false;
                        _jumpHeight = SpritePosition.Y;
                    }
                }

                if (tile.WallTile)
                {
                    if (tile.RightCollision(new Rectangle((int)SpritePosition.X + 5, (int)SpritePosition.Y, (int)SpriteScale.X, (int)SpriteScale.Y)))
                    {
                        _disableRight = true;
                        _rightTile = tile;
                    }

                    if (tile.LeftCollision(new Rectangle((int)SpritePosition.X - 5, (int)SpritePosition.Y, (int)SpriteScale.X, (int)SpriteScale.Y)))
                    {
                        _disableLeft = true;
                        _leftTile = tile;
                    }
                }
            }



            if (_disableRight && _rightTile != null)
            {
                if (!_rightTile.RightCollision(new Rectangle((int)SpritePosition.X + 5, (int)SpritePosition.Y, (int)SpriteScale.X, (int)SpriteScale.Y)))
                {
                    _disableRight = false;
                    _rightTile = null;
                }
            }

            if (_disableLeft && _leftTile != null)
            {
                if (!_leftTile.LeftCollision(new Rectangle((int)SpritePosition.X - 5, (int)SpritePosition.Y, (int)SpriteScale.X, (int)SpriteScale.Y)))
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
            Texture2D idleFire = gameManager.GetTexture("Sprites/Player/Idle/Idle_Fire_SpriteSheet");
            Texture2D idleWater = gameManager.GetTexture("Sprites/Player/Idle/Idle_Water_SpriteSheet");
            Texture2D idleSnow = gameManager.GetTexture("Sprites/Player/Idle/Idle_Snow_SpriteSheet");

            Texture2D walkFire = gameManager.GetTexture("Sprites/Player/Walk/Fire_Walk_SpriteSheet");
            Texture2D walkWater = gameManager.GetTexture("Sprites/Player/Walk/Water_Walk_SpriteSheet");
            Texture2D walkSnow = gameManager.GetTexture("Sprites/Player/Walk/Snow_Walk_SpriteSheet");

            Texture2D fireUltimateActivation = gameManager.GetTexture("Sprites/Player/UltimateActivations/Fire_Activation");
            Texture2D waterUltimateActivation = gameManager.GetTexture("Sprites/Player/UltimateActivations/Water_Activation");
            Texture2D snowUltimateActivation = gameManager.GetTexture("Sprites/Player/UltimateActivations/Snow_Activation");

            //Index
            Animations.Add(new Animation(idleFire, 5, 120f, true, new Vector2(64, 64))); //0
            Animations.Add(new Animation(idleWater, 5, 120f, true, new Vector2(64, 64))); //1 //Idle Animations
            Animations.Add(new Animation(idleSnow, 5, 120f, true, new Vector2(64, 64))); //2

            Animations.Add(new Animation(walkFire, 4, 120f, true, new Vector2(64, 64))); //3
            Animations.Add(new Animation(walkWater, 4, 120f, true, new Vector2(64, 64))); //4 //Walking Animations
            Animations.Add(new Animation(walkSnow, 4, 120f, true, new Vector2(64, 64))); //5

            Animations.Add(new Animation(fireUltimateActivation, 4, 120f, true, new Vector2(64, 64))); //6
            Animations.Add(new Animation(waterUltimateActivation, 4, 120f, true, new Vector2(64, 64))); //7 //Ultimate Activation Animations
            Animations.Add(new Animation(snowUltimateActivation, 4, 120f, true, new Vector2(64, 64))); //8
        }

        enum AnimationIndexEnum
        {
            IdleFire, // 0
            IdleWater, // 1
            IdleSnow, // 2
            FireWalk, // 3
            WaterWalk, // 4
            SnowWalk, // 5
            FireUltimate, //6
            WaterUltimate, //7
            SnowUltimate, //8
        }

        private void MovementAnimations()
        {
            if (SpriteVelocity.X != 0)
            {
                switch (_elementIndex)
                {
                    case 0:
                        SetAnimation((int)AnimationIndexEnum.FireWalk);
                        break;
                    case 1:
                        SetAnimation((int)AnimationIndexEnum.WaterWalk);
                        break;
                    case 2:
                        SetAnimation((int)AnimationIndexEnum.SnowWalk);
                        break;
                    default:
                        _elementIndex = (int)AnimationIndexEnum.IdleFire;
                        break;
                }
            }
            else
            {
                switch (_elementIndex)
                {
                    case 0:
                        SetAnimation((int)AnimationIndexEnum.IdleFire);
                        break;
                    case 1:
                        SetAnimation((int)AnimationIndexEnum.IdleWater);
                        break;
                    case 2:
                        SetAnimation((int)AnimationIndexEnum.IdleSnow);
                        break;
                    default:
                        _elementIndex = (int)AnimationIndexEnum.IdleFire;
                        break;
                }
            }
        }

        #endregion
    }
}
