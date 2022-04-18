using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Terramental
{
    public class Cannon : Sprite
    {
        private PlayerCharacter _playerCharacter;

        private int _cannonDir;
        private int _cannonCooldownLeft;
        private int _cannonCooldown = 5000;

        private GameManager _gameManager;

        public Cannon(GameManager gameManager, PlayerCharacter playerCharacter, int cannonDir)
        {
            _gameManager = gameManager;
            _playerCharacter = playerCharacter;
            _cannonDir = cannonDir;
            _cannonCooldownLeft = 5000;

        }

        public void UpdateCannon(GameTime gameTime)
        {
            _cannonCooldownLeft -= 1 * (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            Debug.WriteLine(_cannonCooldownLeft);
            if (_cannonCooldownLeft <= 0)
            {
                SpawnManager.SpawnCannonProjectile(_cannonDir, SpritePosition);
                _cannonCooldownLeft = _cannonCooldown;
            }
        }
    }
}