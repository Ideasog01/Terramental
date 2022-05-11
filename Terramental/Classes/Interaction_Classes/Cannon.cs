using Microsoft.Xna.Framework;


namespace Terramental
{
    public class Cannon : Sprite
    {
        private PlayerCharacter _playerCharacter;

        private bool _faceRight;
        private float _cannonCooldownLeft;
        private float _cannonCooldown;

        private GameManager _gameManager;

        public Cannon(GameManager gameManager, PlayerCharacter playerCharacter, bool faceRight)
        {
            _gameManager = gameManager;
            _playerCharacter = playerCharacter;
            _faceRight = faceRight;
            _cannonCooldown = 5;
            _cannonCooldownLeft = _cannonCooldown;
        }

        public bool FaceRight
        {
            get { return _faceRight; }
            set { _faceRight = value; }
        }

        public void UpdateCannon(GameTime gameTime)
        {
            _cannonCooldownLeft -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds; // Decreases time left on cannon cooldown

            if (_cannonCooldownLeft <= 0) // Checks to see if there is no time left on cooldown
            {
                if (_faceRight)
                {
                    SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Bullet_RightExample"), SpritePosition + new Vector2(140, 20), new Vector2(50, 50), new Vector2(50, 50), new Vector2(4.2f, -1), true, false, 0, 2, 1); // Spawns a cannon projectile for the right facing cannon
                }
                else
                {
                    SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Bullet_RightExample"), SpritePosition + new Vector2(-40, 20), new Vector2(50, 50), new Vector2(50, 50), new Vector2(-4.2f, -1), true, false, 0, 2, 1); // Spawns a cannon projectile for the left facing cannon
                }

                AudioManager.PlaySound("CannonFire_SFX");
                _cannonCooldownLeft = _cannonCooldown; // Resets the amount of time left to the value stored in cannonCooldown
            }
        }
    }
}
