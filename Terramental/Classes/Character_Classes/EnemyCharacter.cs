using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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

        private Tile _groundTile;

        private bool _knightAttacked;

        private float _attackTimer;

        private bool _isGrounded;

        private float _attackThreshold;
        private float _chaseThreshold;
        private float _attackCooldown;

        private float _enemyMovementSpeed;
        private float _enemyGravity;
        private int _enemyIndex;
        private int _elementIndex;

        private List<Tile> pathList = new List<Tile>();

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

        #region HealthBar

        public void LoadHealthBar(GameManager gameManager)
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

        public void UpdateHealthBar()
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

        #region Utilities

        private void GroundCheck()
        {
            if (!_isGrounded)
            {
                foreach (Tile tile in MapManager.activeTiles)
                {
                    if (tile.GroundTile)
                    {
                        if (tile.TopCollision(this))
                        {
                            _isGrounded = true;
                            _groundTile = tile;
                        }
                    }
                }

                if (_groundTile == null)
                {
                    SpriteVelocity = new Vector2(SpriteVelocity.X, _enemyGravity);
                }
            }
            else
            {
                if (_groundTile != null)
                {
                    if (!_groundTile.TopCollision(this))
                    {
                        _groundTile = null;
                        _isGrounded = false;
                    }

                    SpriteVelocity = new Vector2(SpriteVelocity.X, 0);

                }
            }
        }

        private float DistanceToPlayer()
        {
            float distance = MathF.Sqrt(MathF.Pow(playerCharacter.SpritePosition.X - SpritePosition.X, 2) + MathF.Pow(playerCharacter.SpritePosition.Y - SpritePosition.Y, 2));
            return distance;
        }

        private void FacePlayer()
        {
            if (DistanceToPlayer() > 60)
            {
                Animations[AnimationIndex].MirrorTexture = SpritePosition.X > playerCharacter.SpritePosition.X;
            }
        }

        #endregion

        #region Enemy

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
            EnemyStateMachine();
            FacePlayer();

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

            GroundCheck();

            SpritePosition += SpriteVelocity;
        }

        #endregion

        #region AI

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

        private void Chase()
        {
            if(!Animations[AnimationIndex].MirrorTexture)
            {
                SpriteVelocity = new Vector2(_enemyMovementSpeed, 0);
            }
            else
            {
                SpriteVelocity = new Vector2(-_enemyMovementSpeed, 0);
            }

            SetAnimation(1);
        }

        private void Attack(GameTime gameTime)
        {
            if (_attackTimer <= 0)
            {
                _attackTimer = _attackCooldown;
                _knightAttacked = false;
            }
            else
            {
                _attackTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (OnCollision(playerCharacter.SpriteRectangle) && !_knightAttacked && _enemyIndex == 0)
                {
                    playerCharacter.PlayerTakeDamage(1);
                    _knightAttacked = true;
                }

                if (_enemyIndex == 1 && !_knightAttacked)
                {
                    Vector2 velocity = new Vector2(0, 0);

                    if(!Animations[AnimationIndex].MirrorTexture)
                    {
                        SpawnManager.SpawnProjectile(SpawnManager._gameManager.GetTexture("Sprites/Projectiles/Fireball_Projectile"), SpritePosition + new Vector2(40, 20), new Vector2(40, 40), new Vector2(6, 0), true, false, 0, 3, 1);
                        _knightAttacked = true;
                    }
                    else
                    {
                        SpawnManager.SpawnProjectile(SpawnManager._gameManager.GetTexture("Sprites/Projectiles/Fireball_Projectile"), SpritePosition + new Vector2(-40, 20), new Vector2(40, 40), new Vector2(-4, 0), true, false, 0, 3, 1);
                        _knightAttacked = true;
                    }


                }
            }

            SpriteVelocity = new Vector2(0, SpriteVelocity.Y);

            SetAnimation(2);
        }

        private void Idle()
        {
            SetAnimation(0);
            SpriteVelocity = new Vector2(0, SpriteVelocity.Y);
        }

        #endregion

        #region A* Pathfinding

        public void GeneratePath(Tile destinationTile)
        {
            float gCost;
            float hCost;

            Tile currentTile = null;

            foreach(Tile tile in MapManager.activeTiles)
            {
                float distance = MathF.Sqrt(MathF.Pow(tile.SpritePosition.X - SpritePosition.X, 2) + MathF.Pow(tile.SpritePosition.Y - SpritePosition.Y, 2));

                if(distance < 32)
                {
                    currentTile = tile;
                }
            }

        }

        #endregion
    }
}
