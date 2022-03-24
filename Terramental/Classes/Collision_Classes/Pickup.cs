using System;
using System.Collections.Generic;
using System.Text;

namespace Terramental
{
    public class Pickup : Sprite
    {
        private PlayerCharacter _playerCharacter;

        public PlayerCharacter Player
        {
            get { return _playerCharacter; }
            set { _playerCharacter = value; }
        }
    }
}
