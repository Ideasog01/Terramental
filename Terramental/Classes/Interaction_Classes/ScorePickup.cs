using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    public class ScorePickup : Pickup
    {
        private PlayerCharacter _playerCharacter; // Reference to player character

        public ScorePickup(PlayerCharacter character)
        {
            _playerCharacter = character;
        }

        public void UpdateScorePickup()
        {
            if (OnCollision(_playerCharacter.SpriteRectangle) && IsActive) // Checks to see if the score pickup has collided with the player character
            {
                _playerCharacter.PlayerScore++; // Increments player score value by 1
                AudioManager.PlaySound("ScorePickup_SFX"); // Plays the score pickup sound effect
                SpawnManager.gameManager.objectiveManager.UpdateObjective(ObjectiveManager.Objective.CollectGems); //Increase the objective progress by one
                IsActive = false; // Used to hide the score pickup by setting IsActive to false 
            }
        }
    }
}
