using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    class SnowBeam : Sprite
    {

        public void CheckBeamCollisions()
        {
            foreach (BaseCharacter enemy in SpawnManager.knightEnemies)
            {
                if (OnCollision(enemy.SpriteRectangle))
                {
                    enemy.SetStatus(1, 4, 2);
                }
            }
        }
    }
}
