using Microsoft.Xna.Framework;
using TerraEngine.Graphics;

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
            _cannonCooldownLeft -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_cannonCooldownLeft <= 0)
            {
                if(_faceRight)
                {
                    SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Bullet_RightExample"), SpritePosition + new Vector2(140, 20), new Vector2(50, 50), new Vector2(50, 50), new Vector2(4.2f, -1), true, false, 0, 2, 1);
                }
                else
                {
                    SpawnManager.SpawnProjectile(_gameManager.GetTexture("Sprites/Projectiles/Bullet_RightExample"), SpritePosition + new Vector2(-140, 20), new Vector2(50, 50), new Vector2(50, 50), new Vector2(-4.2f, -1), true, false, 0, 2, 1);
                }
                
                _cannonCooldownLeft = _cannonCooldown;
            }
        }
    }
}
