using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    class SnowBeam : Sprite
    {
        public void CheckBeamCollisions()
        {
            foreach(BaseCharacter enemy in SpawnManager.enemyCharacters)
            {
                if(OnCollision(enemy.SpriteRectangle))
                {
                    enemy.TakeDamage(5);
                }
            }


        }
    }
}
