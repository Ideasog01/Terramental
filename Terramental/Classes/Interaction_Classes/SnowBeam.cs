using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    class SnowBeam : Sprite
    {
        public void CheckBeamCollisions()
        {
            foreach (BaseCharacter enemy in SpawnManager.enemyList)
            {
                if (OnCollision(enemy.SpriteRectangle))
                {
                    enemy.SetStatus(1, 4, 2);
                }
            }
        }
    }
}
