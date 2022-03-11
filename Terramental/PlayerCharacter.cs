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

        private int _elementIndex;

        private int _ultimateAbilityCooldown;

        private bool _ultimateActive;

        public void ActivateUltimate()
        {
            if(_ultimateAbilityCooldown <= 0)
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

        private void FireUltimate()
        {
            //Play Animation
            //Spawn Sword

        }

        private void FireSwordAttack()
        {
            //Play Animation
            //Check for enemies within range
        }
    }
}
