namespace Terramental
{
    public class HealthPickup : Pickup
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
                if (OnCollision(Player.SpriteRectangle) && IsActive && Player.CharacterHealth < 3)
                {
                    Player.Heal(_amount);
                    IsActive = false;
                    Player.DisplayPlayerLives();
                }
            }
        }
    }
}
