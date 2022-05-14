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

        public float dashCooldown = 0;
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
            _gameManager.playerInterface.UpdatePlayerLives(3);
            ultimateActiveTimer = 0;
            ultimateActive = false;
            ultimateCooldown = 0;
            disableMovement = false;
            _isJumping = false;
            _playerScore = 0;
            _isGrounded = false;
            _enemiesDefeated = 0;
            ElementIndex = 0;
            SpritePosition = SpawnManager.levelStartPosition;
            SpriteVelocity = Vector2.Zero;
        }

        public void UpdatePlayerCharacter(GameTime gameTime)
        {
            if(GameManager.currentGameState == GameManager.GameState.Level) // Checks to see if the game is in the level state
            {
                UpdateUltimateStatus(gameTime);
                UpdateShiftDashCooldown(gameTime);

                DashDamage();
                DashCheck();
                Dash(gameTime);

                MoveIfValid(gameTime);

                if(IsGrounded()) // Checks to see if the player is touching the ground
                {
                    SimulateFriction(); //Simulates extra friction for when the player is grounded
                }

                SimulateFriction();
                StopMovingIfBlocked();

                if(!IsGrounded()) // Checks to see if the player is not touching the ground
                {
                    ApplyGravity();
                }

                if (_attackTimer <= 0) // Checks to see if there is no time left on attack timer, meaning animations should go back to movement animations
                {
                    MovementAnimations();
                }

                if(_attackTimer > 0) // Checks to see if there is time left on attack timer, meaning animations should be attack animations
                {
                    switch (_elementIndex) // Switch statement to handle the 3 different element types
                    {
                        case 0:
                            SetAnimation((int)AnimationIndexEnum.FireUltimate); // Fire ultimate animations
                            break;
                        case 1:
                            SetAnimation((int)AnimationIndexEnum.WaterUltimate); // Water ultimate animations
                            break;
                        case 2:
                            SetAnimation((int)AnimationIndexEnum.SnowUltimate); // Snow ultimate animations
                            break;
                    }
                }           
            }
        }
        
        public void TeleportPlayer(Vector2 position) // Moves the player to a given position
        {
            SpritePosition = position;
            ElementIndex = 0;
        }

        public void DashStateMachine() // Used to keep track of direction of dashes using double tap input method
        {
            switch (dashDir)
            {
                case DashDirections.Up:
                    upDashCheck++; // Increments the up dash to show that the player has performed an input for dashing vertically
                    DoubleTapToDashCooldown();
                    break;
                case DashDirections.Left:
                    leftDashCheck++; // Increments the up dash to show that the player has performed an input for dashing left
                    DoubleTapToDashCooldown();
                    break;
                case DashDirections.Right:
                    rightDashCheck++; // Increments the up dash to show that the player has performed an input for dashing right
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

                if (_gameManager.mapManager.HasRoomForRectangle(rectangle))
                {
                    SpriteVelocity += new Vector2(dashDirX * dashVelocity, dashDirY * dashVelocity);
                }
                else
                {
                    SpriteVelocity = new Vector2(0, 0);
                }
                _isDashing = false;
            }
            else if (_canDash && _isDashing && !useDoubleTapDash)
            {
                for (int i = 0; i < 4; i++)
                {
                    Rectangle rectangle = SpriteRectangle;
                    rectangle.Offset(dashDirX, dashDirY);

                    if (_gameManager.mapManager.HasRoomForRectangle(rectangle))
                    {
                        SpriteVelocity += new Vector2(dashDirX * dashVelocity, dashDirY * dashVelocity);
                    }
                    else
                    {
                        SpriteVelocity = new Vector2(0, 0);
                    }
                    _isDashing = false;
                }
                AudioManager.PlaySound("Dash_SFX");
                dashCooldown = 2;
                _canDash = false;
            }
        }

        public void UpdateShiftDashCooldown(GameTime gameTime) // Cooldown method for shift input method of dash
        {
            if (dashCooldown >= 0) // Checks to see if the dash cooldown timer is greater or equal to 0
            {
                dashCooldown -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds; // Decreases dash cooldown timer
                _canDash = false; // Boolean used to prevent the player from dashing
            }

            if (dashCooldown <= 0) // Checks to see if the dash cooldown timer is less than or equal to 0
            {
                _canDash = true; // Boolean used to allow the player to dash
            }
        }

        public async void DoubleTapToDashCooldown() // Cooldown method for double tap input method of dash
        {
            await Task.Delay(300); // Performs a 300ms delay before executing any following code 

            // Resets direction of dash counters to 0
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
            if(ultimateActive && _elementIndex == 1) // Checks to see if the players ultimate is active and that they are using the water element
            {
                SpriteVelocity += new Vector2(amount*1.34f, 0); // Applies a speed boost to the player

            }
            else
            {
                SpriteVelocity += new Vector2(amount, 0); // Applies the standard speed to the player

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
            if (IsGrounded()) // Checks to see if the player is touching the ground
            {
                if (ultimateActive && _elementIndex == 1) // Checks to see if the players ultimate is active and that they are using the water element
                {
                    SpriteVelocity = -Vector2.UnitY * 22.25f * 1.25f; // Applies a vertical component of velocity to the player

                }
                else
                {
                    SpriteVelocity = -Vector2.UnitY * 22.25f;
                }
                _isDoubleJumpUsed = false;
                AudioManager.PlaySound("Jump_SFX"); // Plays the jump sound effect
            }
            else if (!_isDoubleJumpUsed) // Checks to see if the player has not used double jump
            {
                if (ultimateActive && _elementIndex == 1) // Checks to see if the players ultimate is active and that they are using the water element
                {
                    SpriteVelocity = -Vector2.UnitY * 22.25f * 1.25f; // Applies a vertical component of velocity to the player

                }
                else
                {
                    SpriteVelocity = -Vector2.UnitY * 22.25f;
                }
                _isDoubleJumpUsed = true;
                AudioManager.PlaySound("Jump_SFX"); // Plays the jump sound effect
            }
        }

        private void SimulateFriction() // Applies a counter force to the players velocity to act as friciton
        {
            SpriteVelocity -= SpriteVelocity * Vector2.One * 0.075f;
        }

        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            SpritePosition += SpriteVelocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15; // Moves the player based on sprite velocity
            SpritePosition = new Vector2(MathHelper.Clamp(SpritePosition.X, 0, (MapManager.mapWidth * 64) - 64), MathHelper.Clamp(SpritePosition.Y, -32, (MapManager.mapHeight * 64) + 300)); //Restricts the player's position within the level boundaries
        }

        private void ApplyGravity() // Applies a downward force on the player to act as gravity
        {
            SpriteVelocity += Vector2.UnitY * 0.5f; 
        }

        #endregion

        #region Collisions

        public bool IsGrounded() // Checks to see if the player is grounded
        {
            Rectangle onePixelLower = SpriteRectangle;
            onePixelLower.Offset(0, 1); // Adds a pixel offset
            return !_gameManager.mapManager.HasRoomForRectangle(onePixelLower) || !_gameManager.mapManager.HasRoomForRectangleMP(onePixelLower);
        }

        private void MoveIfValid(GameTime gameTime) // Checks to see if a movement is valid
        {
            _oldPosition = base.SpritePosition;
            UpdatePositionBasedOnMovement(gameTime);

            base.SpritePosition = _gameManager.mapManager.FindValidLoaction(_oldPosition, SpritePosition, SpriteRectangle);
        }

        private void StopMovingIfBlocked() // Checks if the movement is blocked and stops the movement accordingly
        {
            Vector2 lastMovement = SpritePosition - _oldPosition; // Stores the previous position

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
            if (ultimateCooldown <= 0 && ultimateActiveTimer <= 0) // Checks to see if the player can use their ultimate ability
            {
                ultimateActiveTimer = 8; // Resets the ultimate cooldown
                ultimateActive = true; // Sets the ultimate to active
                AudioManager.PlaySound("UltimateActivation_SFX"); // Plays the sound effect for ultimate activation
            }
        }

        public void PrimaryUltimateAttack()
        {
            if (ultimateActive && _attackTimer <= 0) // Checks if ultimate is active and if the player can attack (not on cooldown)
            {
                switch (_elementIndex) // Switch statement to handle the 3 possibly elements
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
            if (ultimateActiveTimer > 0) // Decreases ultimate timer
            {
                ultimateActiveTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (ultimateActive)
            {
                ultimateCooldown = 10; // Resets ultimate cooldown
                ultimateActive = false;
            }

            if (_attackTimer > 0) // Decreases attack timer
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
            if (SpriteVelocity.X != 0) // Checks to see if the player has a horizontal velocity (for walk animations)
            {
                if(_elementIndex == 0) // Checks to see if the player is currently using the fire element
                {
                    if(Animations[AnimationIndex] != Animations[(int)AnimationIndexEnum.FireWalk])
                    {
                        SetAnimation((int)AnimationIndexEnum.FireWalk);
                    }
                }
                else if(_elementIndex == 1) // Checks to see if the player is currently using the water element
                {
                    if (Animations[AnimationIndex] != Animations[(int)AnimationIndexEnum.WaterWalk])
                    {
                        SetAnimation((int)AnimationIndexEnum.WaterWalk);
                    }
                }
                else if(_elementIndex == 2) // Checks to see if the player is currently using the snow element
                {
                    if (Animations[AnimationIndex] != Animations[(int)AnimationIndexEnum.SnowWalk])
                    {
                        SetAnimation((int)AnimationIndexEnum.SnowWalk);
                    }
                }
            }
            else // For idle animations
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
