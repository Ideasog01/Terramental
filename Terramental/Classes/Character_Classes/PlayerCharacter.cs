using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Terramental
{
    class PlayerCharacter : BaseCharacter
    {
        private int _playerMovementSpeed = 5;

        private int _elementIndex = 0;

        private float _ultimateAbilityCooldown;

        private float _ultimateActiveTimer;

        private bool _ultimateActive;

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
                //Play Animation
            }
        }

        public void PlayerMovement(int vertical)
        {
            SpritePosition = new Vector2(SpritePosition.X + (vertical * _playerMovementSpeed), SpritePosition.Y);
        }

        public override void Update(GameTime gameTime)
        {
            if(_ultimateActiveTimer > 0)
            {
                _ultimateActiveTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(_ultimateActive)
            {
                DeactivateUltimate();
            }

            if (_ultimateAbilityCooldown > 0)
            {
                _ultimateAbilityCooldown -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
        }

        private void FireUltimate()
        {
            //Play Animation
            
            _ultimateActive = true;
            _ultimateActiveTimer = 10;
        }

        private void FireSwordAttack()
        {
            //Play Animation
            //Check for enemies within range
        }

        private void DeactivateUltimate()
        {
            _ultimateActive = false;
        }
    }
}
