using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace Terramental
{
    public class PlayerCharacter : BaseCharacter
    {
        public static bool disableMovement;

        public float dashCooldown;
        public float ultimateCooldown;

        private int _playerScore;

        //Movement Variables

        private bool _isGrounded;
        private float _playerMovementSpeed = 0.5f;

        private bool _disableRight;
        private bool _disableLeft;
        private Vector2 _checkpointPosition;

        //Ability Variables

        public bool ultimateActive;
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
        private SnowBeam _snowBeam;

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

        #endregion

        #region Player Core

        public PlayerCharacter(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void ResetPlayer()
        {
            CharacterHealth = 3;
            SpritePosition = _checkpointPosition;
            _gameManager.playerInterface.UpdatePlayerLives(3);
        }

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

            if (_snowBeam != null)
            {
                _snowBeam.CheckBeamCollisions();
            }

            if (_snowBeam != null && _elementIndex == 2 && ultimateActive)
            {
                if (_snowBeam.AnimationIndex != 0 && _snowBeam.AnimationIndex != 2)
                {
                    if ((AnimationIndex % 2) == 0 || AnimationIndex == 0)
                    {
                        _snowBeam.SetAnimation(1);
                        _snowBeam.AttachSpriteOffset = new Vector2(40, 5);
                    }
                    else
                    {
                        _snowBeam.SetAnimation(3);
                        _snowBeam.AttachSpriteOffset = new Vector2(-310, 5);
                    }
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

                if(!disableMovement)
                {
                    if (!_disableRight)
                    {
                        SpriteVelocity = new Vector2(_playerMovementSpeed * deltaTime, 0);
                    }
                    else
                    {
                        SpriteVelocity = new Vector2(0, 0);
                    }
                }
            }
            else if (vertical < 0)
            {
                // IsFacingRight = false;

                if(!disableMovement)
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
            }

            if (vertical == 0)
            {
                SpriteVelocity = new Vector2(0, 0);
            }
        }

        public void TeleportPlayer(Vector2 position, bool setCheckpoint)
        {
            SpritePosition = position;

            if(setCheckpoint)
            {
                _checkpointPosition = position;
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

                CheckJumpCollision();

                if (SpritePosition.Y == _jumpHeight || disableMovement)
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
            if(ultimateCooldown <= 0 && _ultimateActiveTimer <= 0)
            {
                switch(_elementIndex)
                {
                    case 0 :
                        ActivateFireUltimate();
                        break;
                    case 1:
                        break;
                    case 2:
                        ActivateSnowUltimate();
                        break;
                    default:
                        Console.WriteLine("ERROR: Element index is invalid during ultimate activatation");
                        break;
                }
            }
        }

        public void PrimaryAttack()
        {
            if(ultimateActive)
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

        public void DisplayPlayerLives()
        {
            _gameManager.playerInterface.UpdatePlayerLives(CharacterHealth);
        }

        private void UpdateUltimateStatus(GameTime gameTime)
        {
            if (_ultimateActiveTimer > 0)
            {
                _ultimateActiveTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (ultimateActive)
            {
                ultimateCooldown = 10;
                ultimateActive = false;

                if(_elementIndex == 2)
                {
                    SnowUltimateEnd();
                }
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

        public void PlayerTakeDamage(int amount)
        {
            CharacterHealth -= amount;
            _gameManager.playerInterface.UpdatePlayerLives(CharacterHealth);

            if (CharacterHealth <= 0)
            {
                _gameManager.menuManager.DisplayRespawnScreen(true);
            }
        }

        #region Fire Ultimate

        private void ActivateFireUltimate()
        {
            ultimateActive = true;
            _ultimateActiveTimer = 10;
        }

        public void FireSwordAttack()
        {
            if (_attackTimer <= 0)
            {
                Rectangle rect;

                rect = new Rectangle((int)SpritePosition.X + 2, (int)SpritePosition.Y, 96, 96);

                foreach (BaseCharacter character in SpawnManager.knightEnemies)
                {
                    if (this.OnCollision(character.SpriteRectangle))
                    {
                        character.TakeDamage(20);
                        if (!character.IsBurning)
                        {
                            Vector2 scale = new Vector2(64, 128);
                            SpawnManager.SpawnAttachEffect("Sprites/SpriteSheets/Effects/Flame_SpriteSheet", character.SpritePosition, scale, character, 5, true);
                            character.SetStatus(0, 5, 1.5f);
                        }

                    }
                }

                _attackTimer = 2;
            }
        }

        #endregion

        #region Snow Ultimate

        private void ActivateSnowUltimate()
        {
            if(_snowBeam == null)
            {
                _snowBeam = new SnowBeam();
                _snowBeam.Initialise(SpritePosition + new Vector2(5, 5), _gameManager.GetTexture("Sprites/SpriteSheets/Ultimates/SnowBeam_Activation_SpriteSheet"), new Vector2(320, 64));


                Animation snowActivationAnim = new Animation(_gameManager.GetTexture("Sprites/SpriteSheets/Ultimates/SnowBeam_Activation_SpriteSheet"), 8, 100f, false, new Vector2(320, 64));
                Animation snowIdleAnim = new Animation(_gameManager.GetTexture("Sprites/SpriteSheets/Ultimates/SnowBeam_Idle_SpriteSheet"), 8, 100f, true, new Vector2(320, 64));
                snowActivationAnim.NextAnimation = true;

                Animation snowLeftActivationAnim = new Animation(_gameManager.GetTexture("Sprites/SpriteSheets/Ultimates/SnowBeam_Activation_SpriteSheet"), 8, 100f, false, new Vector2(320, 64));
                Animation snowLeftIdleAnim = new Animation(_gameManager.GetTexture("Sprites/SpriteSheets/Ultimates/SnowBeam_Idle_SpriteSheet"), 8, 100f, true, new Vector2(320, 64));
                snowLeftActivationAnim.NextAnimation = true;
                snowLeftActivationAnim.MirrorTexture = true;
                snowLeftIdleAnim.MirrorTexture = true;

                _snowBeam.AddAnimation(snowActivationAnim);
                _snowBeam.AddAnimation(snowIdleAnim);
                _snowBeam.AddAnimation(snowLeftActivationAnim);
                _snowBeam.AddAnimation(snowLeftIdleAnim);

                if((AnimationIndex % 2) == 0 || AnimationIndex == 0)
                {
                    _snowBeam.SetAnimation(0);
                    _snowBeam.AttachSpriteOffset = new Vector2(40, 5);
                }
                else
                {
                    _snowBeam.SetAnimation(2);
                    _snowBeam.AttachSpriteOffset = new Vector2(-310, 5);
                }
            }
            else
            {
                _snowBeam.SetAnimation(0);
                _snowBeam.IsActive = true;
                

                if (SpriteVelocity.X > 0)
                {
                    _snowBeam.SetAnimation(0);
                    _snowBeam.AttachSpriteOffset = new Vector2(40, 5);
                }
                else if(SpriteVelocity.X < 0)
                {
                    _snowBeam.SetAnimation(2);
                    _snowBeam.AttachSpriteOffset = new Vector2(-310, 5);
                }
            }

            _snowBeam.AttachSprite = this;


            ultimateActive = true;
            _ultimateActiveTimer = 10;
        }

        private void SnowUltimateEnd()
        {
            _snowBeam.IsActive = false;
        }

        #endregion

        #endregion

        #region Collisions

        private void CheckGroundCollision()
        {
            _tileList = MapManager.activeTiles;

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
            _tileList = MapManager.activeTiles;

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
            _tileList = MapManager.activeTiles;

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
        {                                                                                                          //Index
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_Fire_SpriteSheet"), 5, 120f, true, new Vector2(64, 64))); //0
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_LeftFire_SpriteSheet"), 5, 120f, true, new Vector2(64, 64))); //1
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_Water_SpriteSheet"), 5, 120f, true, new Vector2(64, 64))); //2
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_LeftWater_SpriteSheet"), 5, 120f, true, new Vector2(64, 64))); //3
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_Snow_SpriteSheet"), 5, 120f, true, new Vector2(64, 64))); //4
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Idle/Idle_LeftSnow_SpriteSheet"), 5, 120f, true, new Vector2(64, 64))); //5

            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Fire_Walk_SpriteSheet"), 4, 120f, true, new Vector2(64, 64))); //6
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Fire_LeftWalk_SpriteSheet"), 4, 120f, true, new Vector2(64, 64))); //7
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Water_Walk_SpriteSheet"), 4, 120f, true, new Vector2(64, 64))); //8
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Water_LeftWalk_SpirteSheet"), 4, 120f, true, new Vector2(64, 64))); //9
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Snow_Walk_SpriteSheet"), 4, 120f, true, new Vector2(64, 64))); //10
            Animations.Add(new Animation(gameManager.GetTexture("Sprites/Player/Walk/Snow_LeftWalk_SpriteSheet"), 4, 120f, true, new Vector2(64, 64))); //11
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
