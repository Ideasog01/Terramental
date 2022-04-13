using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    public class Checkpoint : Pickup
    {
        public void CheckCollision()
        {
            if(OnCollision(Player.SpriteRectangle))
            {
                GameManager.playerCheckpoint = Player.SpritePosition;
                IsActive = false;
            }
        }
    }
}
