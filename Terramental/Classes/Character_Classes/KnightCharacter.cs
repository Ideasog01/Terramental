using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Terramental
{
    public class KnightCharacter : EnemyCharacter
    {
        private float attackTimer;
        private bool _knightAttacked;
        private bool _isGrounded;
        private bool _faceRight;
        private Tile _groundTile;
        private bool _rightPathBlocked;
        private bool _leftPathBlocked;
        private int _elementIndex;

        public int ElementIndex
        {
            get { return _elementIndex; }
            set { _elementIndex = value; }
        }

        public void UpdateKnightEnemy(GameTime gameTime)
        {
            KnightStateMachine();
            FacePlayer();

            if(CurrentState == AIState.Attack)
            {
                Attack(gameTime);
            }

            if(CurrentState == AIState.Idle)
            {
                Idle();
            }

            if(CurrentState == AIState.Chase)
            {
                CheckPath();
                Chase();
            }

            GroundCheck();

            SpritePosition += SpriteVelocity;
        }

        private void GroundCheck()
        {
            if (!_isGrounded)
            {
                foreach (Tile tile in MapManager.activeTiles)
                {
                    if (tile.IsActive && tile.GroundTile)
                    {
                        if (tile.TopCollision(this))
                        {
                            _isGrounded = true;
                            _groundTile = tile;
                        }
                    }
                }

                if(_groundTile == null)
                {
                    SpriteVelocity = new Vector2(SpriteVelocity.X, 3);
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

        private void KnightStateMachine()
        {
            if(DistanceToPlayer() < 40)
            {
                CurrentState = AIState.Attack;
            }
            
            if (DistanceToPlayer() < 200 && DistanceToPlayer() >= 50)
            {
                CurrentState = AIState.Chase;
            }

            if(DistanceToPlayer() > 200)
            {
                CurrentState = AIState.Idle;
            }
        }

        private void Chase()
        {
            if(_rightPathBlocked && _faceRight || _leftPathBlocked && !_faceRight)
            {
                CurrentState = AIState.Idle;
                return;
            }

            float yPositionDistance = SpritePosition.Y - playerCharacter.SpritePosition.Y;

            if (CurrentState == AIState.Chase && yPositionDistance < 4f && DistanceToPlayer() > 50f)
            {
                if (_faceRight)
                {
                    SpriteVelocity = new Vector2(3, 0);
                }
                else
                {
                    SpriteVelocity = new Vector2(-3, 0);
                }

                SetAnimation(1);
            }
        }

        private void Attack(GameTime gameTime)
        {
            if (attackTimer <= 0)
            {
                attackTimer = 1.2f;
                _knightAttacked = false;
            }
            else
            {
                attackTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (OnCollision(playerCharacter.SpriteRectangle) && !_knightAttacked)
                {
                     playerCharacter.PlayerTakeDamage(1);
                    _knightAttacked = true;
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

        private void FacePlayer()
        {
            if(DistanceToPlayer() > 60)
            {
                float yPositionDistance = SpritePosition.Y - playerCharacter.SpritePosition.Y;

                if(yPositionDistance < 4f)
                {
                    _faceRight = SpritePosition.X <= playerCharacter.SpritePosition.X;

                    if (_faceRight)
                    {
                        foreach (Animation anim in Animations)
                        {
                            anim.MirrorTexture = false;
                        }
                    }
                    else
                    {

                        foreach (Animation anim in Animations)
                        {
                            anim.MirrorTexture = true;
                        }
                    }
                }
            }
        }

        private void CheckPath()
        {
            if(_faceRight)
            {
                foreach (Tile tile in MapManager.activeTiles)
                {
                    if (tile.IsActive && tile.WallTile)
                    {
                        if (tile.LeftCollision(this))
                        {
                            _rightPathBlocked = true;
                        }
                    }
                }
            }
            else
            {
                foreach (Tile tile in MapManager.activeTiles)
                {
                    if (tile.IsActive && tile.WallTile)
                    {
                        if (tile.LeftCollision(this))
                        {
                            _leftPathBlocked = true;
                        }
                    }
                }
            }

            if(_leftPathBlocked && _faceRight)
            {
                _leftPathBlocked = false;
            }

            if(_rightPathBlocked && !_faceRight)
            {
                _rightPathBlocked = false;
            }
        }
    }
}
