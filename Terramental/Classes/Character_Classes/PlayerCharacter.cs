using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using System.Diagnostics;

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

        public bool _isGrounded;
        private float _playerMovementSpeed = 0.5f;
        private int _horizontalAxisRaw;
        private int _verticalAxisRaw;
        private int _lastNonZeroVAR;
        private int _lastNonZeroHAR;

        private bool _disableRight;
        private bool _disableLeft;

        // Dash Variables
        private float dashVelocity = 3.0f;
        public bool _isDashing = false;
        private bool _canDash = true;
        private bool _isHovering;
        private bool _dashActive;

        private int upDashCheck = 0;
        private int leftDashCheck = 0;
        private int rightDashCheck = 0;

        public int dashDirX;
        public int dashDirY;

        public bool useDoubleTapDash = false;

        private float _dashDistX;
        private float _dashDistY;

        private bool _isFacingRight;

        private Vector2 _oldPosition;

        private int _oldElementIndex;

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

        private List<Tile> _tileList;
        private GameManager _gameManager;

        #region Properties

        public bool IsJumping
        {
            get { return _isJumping; }
            set { _isJumping = value; }
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
            set 
            { 
                _elementIndex = value;
                _gameManager.playerInterface.UpdateElementDisplay(); 

                foreach(ElementWall elementWall in SpawnManager.elementWallList)
                {
                    elementWall.ElementWallCollision();
                }
            }
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
                UpdateUltimateStatus(gameTime);
                UpdateShiftDashCooldown(gameTime);


                DashDamage();
                DashCheck();
                Dash(gameTime);


                MoveIfValid(gameTime);

                if(IsGrounded())
                {
                    SimulateFriction();
                }

                SimulateFriction();
                StopMovingIfBlocked();

                if(!IsGrounded())
                {
                    ApplyGravity();
                }

                if (_attackTimer <= 0)
                {
                    MovementAnimations();
                }

                if(_attackTimer > 0)
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
            }
        }

        public void TeleportPlayer(Vector2 position, bool setCheckpoint)
        {
            SpritePosition = position;

            if(setCheckpoint)
            {
                GameManager.playerCheckpoint = position;
                ElementIndex = 0;
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
            if (!_isDashing && dashCooldown <= 0)
            {
                if (upDashCheck >= 2)
                {
                    dashDirY = -1;
                    dashDirX = 0;
                    _dashDistY = SpritePosition.Y - 500;
                    //Dash();
                    _dashActive = true;
                    _isDashing = true;
                    // AudioManager.PlaySound("Dash_SFX");
                }
                if (leftDashCheck >= 2)
                {
                    dashDirX = -1;
                    dashDirY = 0;
                    _dashDistX = SpritePosition.X - 500;
                    //Dash();
                    _dashActive = true;
                    _isDashing = true;
                    // AudioManager.PlaySound("Dash_SFX");
                }
                if (rightDashCheck >= 2)
                {
                    dashDirX = 1;
                    dashDirY = 0;
                    _dashDistX = SpritePosition.X + 500;
                    //Dash();
                    _dashActive = true;
                    _isDashing = true;
                    // AudioManager.PlaySound("Dash_SFX");
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
                AudioManager.PlaySound("Defeat_SFX");
            }
        }

        public void DisplayPlayerLives()
        {
            _gameManager.playerInterface.UpdatePlayerLives(CharacterHealth);
        }

        #endregion

        #region Dash

        public void Dash(GameTime gameTime)
        {
            if (_isDashing && useDoubleTapDash)
            {
                Rectangle rectangle = SpriteRectangle;
                rectangle.Offset(dashDirX, dashDirY);

                if (_gameManager.mapManager.HasRoomForRectangle(rectangle) && _gameManager.mapManager.HasRoomForRectangleMP(rectangle))
                {
                    SpriteVelocity += new Vector2(dashDirX * dashVelocity, dashDirY * dashVelocity);
                }
                else
                {
                    SpriteVelocity = new Vector2(0, 0);
                }
                _isDashing = false;
            }
            else if (_isDashing && !useDoubleTapDash && _canDash)
            {
                for(int i=0; i < 4; i++)
                {
                    Rectangle rectangle = SpriteRectangle;
                    rectangle.Offset(dashDirX, dashDirY);

                    if (_gameManager.mapManager.HasRoomForRectangle(rectangle) && _gameManager.mapManager.HasRoomForRectangleMP(rectangle))
                    {
                        SpriteVelocity += new Vector2(dashDirX * dashVelocity, dashDirY * dashVelocity);
                    }
                    else
                    {
                        SpriteVelocity = new Vector2(0, 0);
                    }
                }
                _isDashing = false;
                _canDash = false;
                dashCooldown = 2;
                Debug.WriteLine("HERE");
            }
            _isDashing = false;
        }

        public void UpdateShiftDashCooldown(GameTime gameTime)
        {
            //Debug.WriteLine("is dash: " + _isDashing);
            //Debug.WriteLine("can dash: " + _canDash);
            // Debug.WriteLine(dashCooldown);
            if (dashCooldown >= 0)
            {
                dashCooldown -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _canDash = false;
            }

            if (dashCooldown <= 0)
            {
                _canDash = true;
            }
        }

        public async void DoubleTapToDashCooldown()
        {
            await Task.Delay(300);

            upDashCheck = 0;
            leftDashCheck = 0;
            rightDashCheck = 0;
            _dashActive = false;
        }

        private void DashDamage()
        {
            if (_dashActive)
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

                        if (!checkCollision)
                        {
                            if (knight.OnCollision(this.SpriteRectangle))
                            {
                                knight.TakeDamage(25);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Movement

        public void PlayerMovement(int amount, GameTime gameTime)
        {
            if(ultimateActive && _elementIndex == 1)
            {
                SpriteVelocity += new Vector2(amount*2, 0);

            }
            else
            {
                SpriteVelocity += new Vector2(amount, 0);

            }

            if (amount > 0)
            {
                Animations[AnimationIndex].MirrorTexture = false;
            }
            else
            {
                Animations[AnimationIndex].MirrorTexture = true;
            }
        }

        public void PlayerJump()
        {
            if(IsGrounded())
            {
                SpriteVelocity = -Vector2.UnitY * 22.25f;
                _isDoubleJumpUsed = false;
                AudioManager.PlaySound("Jump_SFX");
            }
            else if(!_isDoubleJumpUsed)
            {
                SpriteVelocity = -Vector2.UnitY * 22.25f;
                _isDoubleJumpUsed = true;
                AudioManager.PlaySound("Jump_SFX");
            }
        }

        private void SimulateFriction()
        {
            SpriteVelocity -= SpriteVelocity * Vector2.One * 0.075f;
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            SpritePosition += SpriteVelocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        private void ApplyGravity()
        {
            SpriteVelocity += Vector2.UnitY * 0.5f;
        }

        #endregion

        #region Collisions

        public bool IsGrounded()
        {
            Rectangle onePixelLower = SpriteRectangle;
            onePixelLower.Offset(0, 1);
            return !_gameManager.mapManager.HasRoomForRectangle(onePixelLower) || !_gameManager.mapManager.HasRoomForRectangleMP(onePixelLower);
        }

        private void MoveIfValid(GameTime gameTime)
        {
            _oldPosition = base.SpritePosition;
            UpdatePositionBasedOnMovement(gameTime);

            base.SpritePosition = _gameManager.mapManager.FindValidLoaction(_oldPosition, SpritePosition, SpriteRectangle);
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = SpritePosition - _oldPosition;

            if(lastMovement.X == 0)
            {
                SpriteVelocity *= Vector2.UnitY;
            }

            if(lastMovement.Y == 0)
            {
                SpriteVelocity *= Vector2.UnitX;
            }
        }

        #endregion

        #region Ultimate

        public void ActivateUltimate()
        {
            if (ultimateCooldown <= 0 && ultimateActiveTimer <= 0)
            {
                ultimateActiveTimer = 10;
                ultimateActive = true;
                AudioManager.PlaySound("UltimateActivation_SFX");
            }
        }

        public void PrimaryUltimateAttack()
        {
            if (ultimateActive && _attackTimer <= 0)
            {
                switch (_elementIndex)
                {
                    case 0:

                        if(!Animations[AnimationIndex].MirrorTexture)
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Fire_Projectile"), SpritePosition + new Vector2(40, 0), new Vector2(32, 32), new Vector2(32, 32), new Vector2(4, 0), false, true, 1, 6, 30);
                        }
                        else
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Fire_Projectile"), SpritePosition - new Vector2(40, 0), new Vector2(32, 32), new Vector2(32, 32), new Vector2(-4, 0), false, true, 1, 6, 30);
                        }

                        AudioManager.PlaySound("FireProjectile_SFX");
                       
                        break;
                    case 1:

                        if(!Animations[AnimationIndex].MirrorTexture)
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Water_Projectile"), SpritePosition + new Vector2(40, 0), new Vector2(32, 32), new Vector2(32, 32), new Vector2(4, 0), false, true, 2, 6, 40);
                        }
                        else
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Water_Projectile"), SpritePosition - new Vector2(40, 0), new Vector2(32, 32), new Vector2(32, 32), new Vector2(-4, 0), false, true, 2, 6, 40);
                        }

                        AudioManager.PlaySound("WaterProjectile_SFX");

                        break;
                    case 2:

                        if (!Animations[AnimationIndex].MirrorTexture)
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Snow_Projectile"), SpritePosition + new Vector2(40, 0), new Vector2(32, 32), new Vector2(32, 32), new Vector2(4, 0), false, true, 3, 6, 20);
                        }
                        else
                        {
                            SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Snow_Projectile"), SpritePosition - new Vector2(40, 0), new Vector2(32, 32), new Vector2(32, 32), new Vector2(-4, 0), false, true, 3, 6, 20);
                        }

                        AudioManager.PlaySound("SnowProjectile_SFX");

                        break;
                    default:
                        Console.WriteLine("ERROR: Element index is invalid during ultimate attack");
                        break;
                }

                _attackTimer = 1f;
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

            Animations.Add(new Animation(walkFire, 4, 200f, true, new Vector2(64, 64))); //3
            Animations.Add(new Animation(walkWater, 4, 200f, true, new Vector2(64, 64))); //4 //Walking Animations
            Animations.Add(new Animation(walkSnow, 4, 200f, true, new Vector2(64, 64))); //5

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
                if(_elementIndex == 0)
                {
                    if(Animations[AnimationIndex] != Animations[(int)AnimationIndexEnum.FireWalk])
                    {
                        SetAnimation((int)AnimationIndexEnum.FireWalk);
                    }
                }
                else if(_elementIndex == 1)
                {
                    if (Animations[AnimationIndex] != Animations[(int)AnimationIndexEnum.WaterWalk])
                    {
                        SetAnimation((int)AnimationIndexEnum.WaterWalk);
                    }
                }
                else if(_elementIndex == 2)
                {
                    if (Animations[AnimationIndex] != Animations[(int)AnimationIndexEnum.SnowWalk])
                    {
                        SetAnimation((int)AnimationIndexEnum.SnowWalk);
                    }
                }
            }
            else
            {
                if (_elementIndex == 0)
                {
                    if (Animations[AnimationIndex] != Animations[(int)AnimationIndexEnum.IdleFire])
                    {
                        SetAnimation((int)AnimationIndexEnum.IdleFire);
                    }
                }
                else if (_elementIndex == 1)
                {
                    if (Animations[AnimationIndex] != Animations[(int)AnimationIndexEnum.IdleWater])
                    {
                        SetAnimation((int)AnimationIndexEnum.IdleWater);
                    }
                }
                else if (_elementIndex == 2)
                {
                    if (Animations[AnimationIndex] != Animations[(int)AnimationIndexEnum.IdleSnow])
                    {
                        SetAnimation((int)AnimationIndexEnum.IdleSnow);
                    }
                }
            }
        }

        #endregion
    }
}
