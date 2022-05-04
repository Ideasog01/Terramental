using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class ElementWall : Sprite
    {
        private PlayerCharacter _playerCharacter;

        private int _elementIndex;

        private bool _checkCollision;

        private int wallHealth;

        private Tile tile;

        public void InitialiseElementWall(PlayerCharacter playerCharacter, MapManager mapManager, int elementIndex)
        {
            _playerCharacter = playerCharacter;
            _elementIndex = elementIndex;
            tile = mapManager.FindTile(SpriteRectangle);
            tile.IsBlocking = true;
            tile.SpriteColor = Color.Red;
            wallHealth = 60;
        }

        public void DamageElementWall()
        {
            if (!_checkCollision)
            {
                wallHealth -= 20;
            }

            if (wallHealth <= 0)
            {
                AudioManager.PlaySound("WallBreak_SFX");
                tile.IsBlocking = false;
                IsActive = false;
            }
        }

        public void ElementWallCollision()
        {
            if (_playerCharacter != null && tile != null)
            {
                // 0 = Fire (Fire goest through snow)
                // 1 = Water (Water goes through fire)
                // 2 = Snow (Snow goes through water)

                if (_playerCharacter.ElementIndex == 0)
                {
                    if (_elementIndex == 0)
                    {
                        _checkCollision = true;
                    }
                    else if (_elementIndex == 1)
                    {
                        _checkCollision = true;
                    }
                    else if (_elementIndex == 2)
                    {
                        _checkCollision = false;
                    }
                }

                if (_playerCharacter.ElementIndex == 1)
                {
                    if (_elementIndex == 0)
                    {
                        _checkCollision = false;
                    }
                    else if (_elementIndex == 1)
                    {
                        _checkCollision = true;
                    }
                    else if (_elementIndex == 2)
                    {
                        _checkCollision = true;
                    }
                }

                if (_playerCharacter.ElementIndex == 2)
                {
                    if (_elementIndex == 0)
                    {
                        _checkCollision = true;
                    }
                    else if (_elementIndex == 1)
                    {
                        _checkCollision = false;
                    }
                    else if (_elementIndex == 2)
                    {
                        _checkCollision = true;
                    }
                }
            }
        }
    }
}
