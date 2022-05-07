namespace Terramental
{
    public class HealthPickup : Pickup
    {
        private int _amount; // Amount of health the pickup should heal for

        public HealthPickup(PlayerCharacter playerCharacter, int amount) // Health pickup constructor
        {
            Player = playerCharacter;
            _amount = amount;
        }


        public void CheckHealthPickupCollision()
        {
            if(Player.SpriteRectangle != null) // Checks to see if the player has a rectangle 
            {
                if (OnCollision(Player.SpriteRectangle) && IsActive && Player.CharacterHealth < 3) // Checks to see if the health pickup has collided with the player and that the player does not already have max health (3)
                {
                    Player.Heal(_amount); // Heals the player 
                    Player.DisplayPlayerLives(); // Updates the player health UI
                    AudioManager.PlaySound("HealthPickup_SFX"); // Plays the health pickup sound effect
                    IsActive = false; // Used to hide the health pickup by setting IsActive to false 
                }
            }
        }
    }
}
