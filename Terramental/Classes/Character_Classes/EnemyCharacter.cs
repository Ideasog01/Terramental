﻿using Microsoft.Xna.Framework;
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

        private float _attackThreshold;
        private float _chaseThreshold;
        private float _attackCooldown;

        private float _enemyMovementSpeed;
        private int _enemyIndex;
        private int _elementIndex;

        private bool _pathBlocked;

        private GameManager _gameManager;

        private Vector2 _oldPosition;

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

        public void ResetEnemy(Texture2D texture, Vector2 position, Vector2 scale, int enemyMaxHealth, int enemyHealth, float enemyMovementSpeed, float enemyGravity, GameManager gameManager)
        {
            SpriteTexture = texture;
            SpritePosition = position;
            SpawnPosition = position;
            SpriteScale = scale;
            CharacterMaxHealth = enemyMaxHealth;
            CharacterHealth = enemyHealth;
            _enemyMovementSpeed = enemyMovementSpeed;
            IsActive = true;

            if(enemyHealthBar != null)
            {
                enemyHealthBar.SpritePosition = SpritePosition + new Vector2(0, -50);

                if (enemyHealthBarFill != null)
                {
                    enemyHealthBarFill.SpritePosition = enemyHealthBar.SpritePosition;
                }
            }

            if(enemyElement != null)
            {
                enemyElement.SpritePosition = SpritePosition + new Vector2(0, -70);
            }

            if(_gameManager == null)
            {
                _gameManager = gameManager;
            }
        }

        public void UpdateEnemy(GameTime gameTime)
        {
            EnemyStateMachine();

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

            MoveIfValid(gameTime);

            SimulateFriction();
            StopMovingIfBlocked();

            if (_currentState == AIState.Idle)
            {
                if (!IsGrounded())
                {
                    ApplyGravity();
                }
            }
            else if (_currentState == AIState.Chase)
            {
                ApplyGravity();
                if (!IsGrounded() && !_pathBlocked && !DisableMovement)
                {
                    ApplyGravity();
                }

                

                if (_pathBlocked && !DisableMovement)
                {
                    Rectangle rectangleOffset = SpriteRectangle;
                    rectangleOffset.Offset(0, -256);

                    if(!_gameManager.mapManager.FindNearestTile(rectangleOffset).IsBlocking)
                    {
                        EnemyJump();
                    }
                }
            }

            MirrorEnemy();

            if(SpritePosition.Y < -(MapManager.mapHeight * 64))
            {
                TakeDamage(CharacterMaxHealth);
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
                if (_enemyIndex == 0)
                {
                    AudioManager.PlaySound("Sword_SFX");
                }
                else if(_enemyIndex == 1)
                {
                    AudioManager.PlaySound("FireProjectile_SFX");
                }

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
                        SpawnManager.SpawnProjectile(SpawnManager._gameManager.GetTexture("Sprites/Projectiles/Fireball_Projectile"), SpritePosition + new Vector2(40, 20), new Vector2(40, 40), new Vector2(40, 40), new Vector2(4, 0), true, false, 0, 3, 1);
                        _enemyAttacked = true;
                    }
                    else
                    {
                        SpawnManager.SpawnProjectile(SpawnManager._gameManager.GetTexture("Sprites/Projectiles/Fireball_Projectile"), SpritePosition + new Vector2(-40, 20), new Vector2(40, 40), new Vector2(40, 40), new Vector2(-4, 0), true, false, 0, 3, 1);
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

            foreach(Tile tile in MapManager.tileList)
            {
                float distance = MathF.Sqrt((MathF.Pow(tile.SpritePosition.X - SpritePosition.X, 2)) + (MathF.Pow(tile.SpritePosition.Y - SpritePosition.Y, 2)));

                if(distance < 64)
                {
                    if (SpritePosition.Y > tile.SpritePosition.Y)
                    {
                        SpritePosition = SpawnPosition;
                    }
                }
            }
        }

        private void Chase()
        {
            Vector2 dir = playerCharacter.SpritePosition - SpritePosition;
            dir.Normalize();
            SpriteVelocity = new Vector2(dir.X * _enemyMovementSpeed, SpriteVelocity.Y);

            Vector2 lastMovement = SpritePosition - _oldPosition; // Stores the previous position

            if(lastMovement.X == 0)
            {
                SetAnimation(0);
            }
            else
            {
                SetAnimation(1);
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

        #endregion

        #region EnemyMovement

        private void UpdateEnemyMovement(GameTime gameTime)
        {
            SpritePosition += SpriteVelocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        private void ApplyGravity()
        {
            SpriteVelocity += Vector2.UnitY * 0.5f;
        }

        private void SimulateFriction()
        {
            SpriteVelocity -= SpriteVelocity * Vector2.One * 0.075f;
        }

        private bool IsGrounded()
        {
            Rectangle onePixelLower = SpriteRectangle;
            onePixelLower.Offset(0, 1);
            return !_gameManager.mapManager.HasRoomForRectangle(onePixelLower);
        }

        private void MoveIfValid(GameTime gameTime)
        {
            _oldPosition = base.SpritePosition;
            UpdateEnemyMovement(gameTime);

            base.SpritePosition = _gameManager.mapManager.FindValidLoaction(_oldPosition, SpritePosition, SpriteRectangle);
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = SpritePosition - _oldPosition;

            if (lastMovement.X == 0)
            {
                SpriteVelocity *= Vector2.UnitY;

                if (!_pathBlocked && IsGrounded() && _currentState == AIState.Chase)
                {
                    AudioManager.PlaySound("Jump_SFX");
                    _pathBlocked = true;
                }
            }
            else
            {
                _pathBlocked = false;
            }

            if (lastMovement.Y == 0)
            {
                SpriteVelocity *= Vector2.UnitX;
            }
        }

        private void EnemyJump()
        {
            SpriteVelocity = -Vector2.UnitY * 25.25f;
        }

        #endregion
    }
}
