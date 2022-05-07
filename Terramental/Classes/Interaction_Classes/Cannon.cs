using Microsoft.Xna.Framework;
using TerraEngine.Graphics;

namespace Terramental
{
    public class Cannon : Sprite
    {
        private PlayerCharacter _playerCharacter; // Reference to the player ch

        private bool _faceRight;
        private float _cannonCooldownLeft; // Amount of time left between next cannon shot
        private float _cannonCooldown; // Float amount of time between cannon shotss

        private GameManager _gameManager; // Reference to game manager

        public Cannon(GameManager gameManager, PlayerCharacter playerCharacter, bool faceRight) // Cannon constructor
        {
            _gameManager = gameManager;
            _playerCharacter = playerCharacter;
            _faceRight = faceRight;
            _cannonCooldown = 5; // Sets the time between cannon shots to be 5 seconds
            _cannonCooldownLeft = _cannonCooldown;
        }

        public void UpdateCannon(GameTime gameTime)
        {
            _cannonCooldownLeft -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds; // Decreases time left on cannon cooldown

            if (_cannonCooldownLeft <= 0) // Checks to see if there is no time left on cooldown
            {
                SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Bullet_RightExample"), SpritePosition + new Vector2(140, 20), new Vector2(50, 50), new Vector2(50, 50), new Vector2(4.2f, -1), true, false, 0, 2, 1); // Spawns a cannon projectile
                _cannonCooldownLeft = _cannonCooldown; // Resets the amount of time left to the value stored in cannonCooldown
            }
        }
    }
}
