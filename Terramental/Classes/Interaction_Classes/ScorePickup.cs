using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    public class ScorePickup : Sprite
    {
        private PlayerCharacter _playerCharacter;

        public ScorePickup(PlayerCharacter character)
        {
            _playerCharacter = character;
        }

        public void UpdateScorePickup()
        {
            if (OnCollision(_playerCharacter.SpriteRectangle) && IsActive)
            {
                _playerCharacter.PlayerScore++;
                IsActive = false;
            }
        }
    }
}
