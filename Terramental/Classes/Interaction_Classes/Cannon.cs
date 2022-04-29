using Microsoft.Xna.Framework;


namespace Terramental
{
    public class Cannon : Sprite
    {
        private PlayerCharacter _playerCharacter;

        private int _cannonDir;
        private float _cannonCooldownLeft;
        private float _cannonCooldown;

        private GameManager _gameManager;

        public Cannon(GameManager gameManager, PlayerCharacter playerCharacter, int cannonDir)
        {
            _gameManager = gameManager;
            _playerCharacter = playerCharacter;
            _cannonDir = cannonDir;
            _cannonCooldown = 5;
            _cannonCooldownLeft = _cannonCooldown;
        }

        public void UpdateCannon(GameTime gameTime)
        {
            _cannonCooldownLeft -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_cannonCooldownLeft <= 0)
            {
                SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Bullet_RightExample"), SpritePosition + new Vector2(140 * _cannonDir, 20), new Vector2(40, 40), new Vector2(15 * _cannonDir, -1), true, false, 0, 2);
                _cannonCooldownLeft = _cannonCooldown;
            }
        }
    }
}
