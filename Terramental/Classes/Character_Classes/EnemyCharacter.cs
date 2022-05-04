using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Terramental
{
    public class EnemyCharacter : BaseCharacter
    {
        #region Variables

        public PlayerCharacter playerCharacter;

        public enum AIState { Patrol, Chase, Attack, Dead, Idle };

        private AIState _currentState;

        private Sprite enemyHealthBar;
        private Sprite enemyHealthBarFill;
        private Sprite enemyElement;

        private Texture2D enemyHealthBarTexture;
        private Texture2D enemyHealthBarFillTexture;

        private bool _enemyAttacked;

        private float _attackTimer;

        private bool _isGrounded;

        private float _attackThreshold;
        private float _chaseThreshold;
        private float _attackCooldown;

        private float _enemyMovementSpeed;
        private float _enemyGravity;
        private int _enemyIndex;
        private int _elementIndex;

        private bool _rightBlocked;
        private bool _leftBlocked;

        private Tile _blockingTile;
        private Tile _groundTile;

        private bool _jumpActive;
        private float _jumpHeight;
        private float _jumpCooldownTimer;

        private float _groundCheckTimer;

        #endregion

        #region Properties

        public AIState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        public float AttackCooldown
        {
            set { _attackCooldown = value; }
        }

        public float AttackThreshold
        {
            set { _attackThreshold = value; }
        }

        public float ChaseThreshold
        {
            set { _chaseThreshold = value; }
        }

        public int ElementIndex
        {
            get
            { return _elementIndex; }
            set
            {
                if (value == 0)
                {
                    enemyElement.SpriteTexture = SpawnManager._gameManager.GetTexture("UserInterface/PlayerInterface/Fire_Element");
                }
                else if (value == 1)
                {
                    enemyElement.SpriteTexture = SpawnManager._gameManager.GetTexture("UserInterface/PlayerInterface/Water_Element");
                }
                else if (value == 2)
                {
                    enemyElement.SpriteTexture = SpawnManager._gameManager.GetTexture("UserInterface/PlayerInterface/Snow_Element");
                }

                _elementIndex = value;
            }
        }

        public int EnemyIndex
        {
            get { return _enemyIndex; }
            set { _enemyIndex = value; }
        }

        #endregion

        #region WorldCanvas

        public void LoadWorldCanvas(GameManager gameManager)
        {
            enemyHealthBarTexture = gameManager.GetTexture("UserInterface/Sliders/HealthBarBorder");
            enemyHealthBarFillTexture = gameManager.GetTexture("UserInterface/Sliders/HealthBarFill");

            enemyHealthBar = new Sprite();
            enemyHealthBar.Initialise(SpritePosition + new Vector2(0, -50), enemyHealthBarTexture, new Vector2(CharacterHealth / 2, enemyHealthBarTexture.Height));

            enemyHealthBarFill = new Sprite();
            enemyHealthBarFill.Initialise(enemyHealthBar.SpritePosition, enemyHealthBarFillTexture, new Vector2(enemyHealthBar.SpriteScale.X, enemyHealthBarFillTexture.Height));

            enemyHealthBar.LayerOrder = -1;
            enemyHealthBarFill.LayerOrder = -1;

            enemyElement = new Sprite();
            enemyElement.Initialise(SpritePosition + new Vector2(0, -70), gameManager.GetTexture("UserInterface/PlayerInterface/Fire_Element"), new Vector2(20, 20));
            enemyElement.LayerOrder = -1;
        }

        public void UpdateWorldCanvas()
        {
            if (!Animations[AnimationIndex].MirrorTexture)
            {
                enemyHealthBar.SpritePosition = SpritePosition + new Vector2(0, -30);
                enemyElement.SpritePosition = enemyHealthBar.SpritePosition + new Vector2(60, -5);
            }
            else
            {
                enemyHealthBar.SpritePosition = SpritePosition + new Vector2(48, -30);
                enemyElement.SpritePosition = enemyHealthBar.SpritePosition + new Vector2(60, -5);
            }

            enemyHealthBarFill.SpritePosition = enemyHealthBar.SpritePosition;


            enemyHealthBarFill.SpriteScale = new Vector2(CharacterHealth / 2, enemyHealthBarFill.SpriteScale.Y);
        }

        public void EnableWorldCanvas(bool active)
        {
            enemyHealthBar.IsActive = active;
            enemyHealthBarFill.IsActive = active;
            enemyElement.IsActive = active;
        }

        #endregion

        #region EnemyCore

        public void ResetEnemy(Texture2D texture, Vector2 position, Vector2 scale, int enemyMaxHealth, int enemyHealth, float enemyMovementSpeed, float enemyGravity)
        {
            SpriteTexture = texture;
            SpritePosition = position;
            SpawnPosition = position;
            SpriteScale = scale;
            CharacterMaxHealth = enemyMaxHealth;
            CharacterHealth = enemyHealth;
            _enemyMovementSpeed = enemyMovementSpeed;
            _enemyGravity = enemyGravity;
            IsActive = true;
        }

        public void UpdateEnemy(GameTime gameTime)
        {
            GroundCheck(gameTime);
            EnemyStateMachine();
            UpdateJump(gameTime);
            UpdateMovement();

            if (CurrentState == AIState.Attack)
            {
                Attack(gameTime);
            }

            if (CurrentState == AIState.Idle)
            {
                Idle();
            }

            if (CurrentState == AIState.Chase)
            {
                if (!DisableMovement)
                {
                    Chase();
                }
            }

            if (!_leftBlocked || !_rightBlocked)
            {
                CheckPath();
            }

            MirrorEnemy();

            SpritePosition += new Vector2(0, SpriteVelocity.Y);
        }

        #endregion

        #region Checks

        private void GroundCheck(GameTime gameTime)
        {
            if(!_isGrounded)
            {
                foreach (Tile tile in MapManager.activeTiles)
                {
                    if (tile.TopCollision(SpriteRectangle))
                    {
                        _isGrounded = true;
                        _groundTile = tile;
                        _groundCheckTimer = 3;
                        break;
                    }
                }
            }
            else
            {
                if(_groundCheckTimer <= 0)
                {
                    if (_groundTile == null)
                    {
                        _isGrounded = false;
                    }
                    else
                    {
                        if (!_groundTile.TopCollision(SpriteRectangle) || _groundTile.RightCollision(SpriteRectangle) || _groundTile.LeftCollision(SpriteRectangle))
                        {
                            _groundTile = null;
                        }
                    }
                }
            }

            if(_groundCheckTimer > 0)
            {
                _groundCheckTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void CheckPath()
        {
            foreach (Tile tile in MapManager.activeTiles)
            {
                if (tile.LeftCollision(new Rectangle((int)SpritePosition.X + 48, (int)SpritePosition.Y, (int)SpriteScale.X, (int)SpriteScale.Y)))
                {
                    _rightBlocked = true;
                    _blockingTile = tile;
                }

                if (tile.RightCollision(new Rectangle((int)SpritePosition.X - 48, (int)SpritePosition.Y, (int)SpriteScale.X, (int)SpriteScale.Y)))
                {
                    _leftBlocked = true;
                    _blockingTile = tile;
                }
            }
        }

        #endregion

        #region AI

        private float DistanceToPlayer()
        {
            float distance = MathF.Sqrt(MathF.Pow(playerCharacter.SpritePosition.X - SpritePosition.X, 2) + MathF.Pow(playerCharacter.SpritePosition.Y - SpritePosition.Y, 2));
            return distance;
        }

        private void EnemyStateMachine()
        {
            if (DistanceToPlayer() < _attackThreshold)
            {
                CurrentState = AIState.Attack;
            }

            if (DistanceToPlayer() >= _attackThreshold && DistanceToPlayer() < _chaseThreshold)
            {
                CurrentState = AIState.Chase;
            }

            if (DistanceToPlayer() >= _chaseThreshold)
            {
                CurrentState = AIState.Idle;
            }
        }

        private void Attack(GameTime gameTime)
        {
            if (_attackTimer <= 0)
            {
                _attackTimer = _attackCooldown;
                _enemyAttacked = false;
            }
            else
            {
                _attackTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (OnCollision(playerCharacter.SpriteRectangle) && !_enemyAttacked && _enemyIndex == 0)
                {
                    playerCharacter.PlayerTakeDamage(1);
                    _enemyAttacked = true;
                }

                if (_enemyIndex == 1 && !_enemyAttacked)
                {
                    if(!Animations[AnimationIndex].MirrorTexture)
                    {
                        SpawnManager.SpawnProjectile(SpawnManager._gameManager.GetTexture("Sprites/Projectiles/Fireball_Projectile"), SpritePosition + new Vector2(40, 20), new Vector2(40, 40), new Vector2(4, 0), true, false, 0, 3, 1);
                        _enemyAttacked = true;
                    }
                    else
                    {
                        SpawnManager.SpawnProjectile(SpawnManager._gameManager.GetTexture("Sprites/Projectiles/Fireball_Projectile"), SpritePosition + new Vector2(-40, 20), new Vector2(40, 40), new Vector2(-4, 0), true, false, 0, 3, 1);
                        _enemyAttacked = true;
                    }


                }
            }

            SpriteVelocity = new Vector2(0, SpriteVelocity.Y);

            SetAnimation(2);
        }

        private void Idle()
        {
            SetAnimation(0);
        }

        private void Chase()
        {
            Vector2 dir = playerCharacter.SpritePosition - SpritePosition;
            dir.Normalize();
            SpriteVelocity = new Vector2(dir.X, 0) * _enemyMovementSpeed;

            if(SpriteVelocity.X != 0)
            {
                SetAnimation(1);
            }
        }

        private void UpdateJump(GameTime gameTime)
        {
            if(_jumpActive)
            {
                if (SpritePosition.Y > _jumpHeight)
                {
                    SpritePosition = new Vector2(SpriteVelocity.X, -3);

                    if (!_rightBlocked && !_leftBlocked)
                    {
                        _jumpActive = false;
                        _jumpCooldownTimer = 3;
                    }
                }
                else
                {
                    _jumpActive = false;
                    _jumpCooldownTimer = 3;
                }
            }

            if (!_isGrounded)
            {
                if (!_jumpActive)
                {
                    SpriteVelocity = new Vector2(SpriteVelocity.X, _enemyGravity);
                }
            }

            if (_jumpCooldownTimer > 0)
            {
                _jumpCooldownTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void MirrorEnemy()
        {
            if (_currentState != AIState.Attack)
            {
                if (SpriteVelocity.X >= 0)
                {
                    Animations[AnimationIndex].MirrorTexture = false;
                }
                else
                {
                    Animations[AnimationIndex].MirrorTexture = true;
                }
            }
            else
            {
                if (playerCharacter.SpritePosition.X >= SpritePosition.X)
                {
                    Animations[AnimationIndex].MirrorTexture = false;
                }
                else
                {
                    Animations[AnimationIndex].MirrorTexture = true;
                }
            }
        }

        private void UpdateMovement()
        {
            if (SpriteVelocity.X > 0)
            {
                if (_rightBlocked)
                {
                    if (_blockingTile != null)
                    {
                        if (!_blockingTile.LeftCollision(new Rectangle((int)SpritePosition.X + 48, (int)SpritePosition.Y, (int)SpriteScale.X, (int)SpriteScale.Y)))
                        {
                            _rightBlocked = false;
                            _blockingTile = null;
                        }
                    }
                    else
                    {
                        _rightBlocked = false;
                    }
                }
                else
                {
                    SpritePosition += new Vector2(SpriteVelocity.X, 0);
                }
            }
            else if (SpriteVelocity.X < 0)
            {
                if (_leftBlocked)
                {
                    if (_blockingTile != null)
                    {
                        if (!_blockingTile.RightCollision(new Rectangle((int)SpritePosition.X - 48, (int)SpritePosition.Y, (int)SpriteScale.X, (int)SpriteScale.Y)))
                        {
                            _rightBlocked = false;
                            _blockingTile = null;
                        }
                    }
                    else
                    {
                        _leftBlocked = false;
                    }
                }
                else
                {
                    SpritePosition += new Vector2(SpriteVelocity.X, 0);
                }
            }
        }

        #endregion
    }
}
