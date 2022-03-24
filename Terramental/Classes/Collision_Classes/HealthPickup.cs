using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
<<<<<<< Updated upstream
    class HealthPickup : Sprite
=======
    public class HealthPickup : Pickup
>>>>>>> Stashed changes
    {
        private int _amount;


        public HealthPickup(PlayerCharacter playerCharacter, int amount)
        {
            Player = playerCharacter;
            _amount = amount;
        }


        public void CheckHealthPickupCollision()
        {
            if(Player.SpriteRectangle != null)
            {
                if (OnCollision(Player.SpriteRectangle) && IsActive)
                {
                    Player.Heal(_amount);
                    IsActive = false;
                }
            }
        }

        

    }
}
