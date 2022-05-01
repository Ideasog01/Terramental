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

        public int CannonDir
        {
            get { return _cannonDir; }
            set { _cannonDir = value; }
        }

        public void UpdateCannon(GameTime gameTime)
        {
            _cannonCooldownLeft -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_cannonCooldownLeft <= 0)
            {
                if(_cannonDir == 1)
                {
                    SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Bullet_RightExample"), SpritePosition + new Vector2(140 * _cannonDir, 20), new Vector2(50, 50), new Vector2(4.2f, -1), true, false, 0, 2, 1);
                }
                else
                {
                    SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Bullet_RightExample"), SpritePosition + new Vector2(140 * _cannonDir, 20), new Vector2(50, 50), new Vector2(-4.2f, -1), true, false, 0, 2, 1);
                }
                
                _cannonCooldownLeft = _cannonCooldown;
            }
        }
    }
}
