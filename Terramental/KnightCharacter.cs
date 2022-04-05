using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Terramental
{
    public class KnightCharacter : EnemyCharacter
    {
        private float attackTimer = 2;
        private bool _characterAttacked;

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
                Chase();
            }
        }

        private float DistanceToPlayer()
        {
            float distance = MathF.Sqrt(MathF.Pow(playerCharacter.SpritePosition.X - SpritePosition.X, 2) + MathF.Pow(playerCharacter.SpritePosition.Y - SpritePosition.Y, 2));
            return distance;
        }

        private void KnightStateMachine()
        {
            if (DistanceToPlayer() < 50)
            {
                CurrentState = AIState.Attack;
            }
            else if (DistanceToPlayer() < 200)
            {
                CurrentState = AIState.Chase;
            }
        }

        private void Chase()
        {
            if (CurrentState == AIState.Chase && DistanceToPlayer() > 50)
            {
                SpritePosition += SpriteVelocity;
                SetAnimation(1);
            }
        }

        private void Attack(GameTime gameTime)
        {
            if (attackTimer <= 0)
            {
                if(!_characterAttacked)
                {
                    if (OnCollision(playerCharacter.SpriteRectangle))
                    {
                        playerCharacter.PlayerTakeDamage(1);
                    }

                    attackTimer = 2;
                    _characterAttacked = true;

                    SetAnimation(2);
                }
                else
                {
                    CurrentState = AIState.Idle;
                    _characterAttacked = false;
                    attackTimer = 2;
                }
            }
            else
            {
                attackTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void Idle()
        {
            SetAnimation(0);
        }

        private void FacePlayer()
        {
            if (playerCharacter.SpritePosition.X > SpritePosition.X)
            {
                SpriteVelocity = new Vector2(3, 0);
                Animations[AnimationIndex].MirrorTexture = false;
            }
            else if (playerCharacter.SpritePosition.X < SpritePosition.X)
            {
                SpriteVelocity = new Vector2(-3, 0);
                Animations[AnimationIndex].MirrorTexture = true;
            }
        }
    }
}
