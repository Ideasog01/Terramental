using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraEngine.Graphics;

namespace Terramental
{
    public class ElementWall : Sprite
    {
        private PlayerCharacter _playerCharacter;

        private int _elementIndex;

        private bool _checkCollision;

        private int wallHealth;

        private Tile tile;

        private Texture2D _texture1;
        private Texture2D _texture2;
        private Texture2D _texture3;

        public void InitialiseElementWall(PlayerCharacter playerCharacter, MapManager mapManager, int elementIndex)
        {
            _playerCharacter = playerCharacter;
            _elementIndex = elementIndex;
            tile = mapManager.FindTile(SpriteRectangle);
            tile.IsBlocking = true;
            tile.SpriteColor = Color.Red;
            wallHealth = 60;
        }

        public void ResetWall(MapManager mapManager)
        {
            tile = mapManager.FindTile(SpriteRectangle);
            tile.IsBlocking = true;
            tile.SpriteColor = Color.Red;
            wallHealth = 60;
            SpriteTexture = _texture1;
            IsActive = true;
        }

        public void DamageElementWall()
        {
            if (!_checkCollision)
            {
                wallHealth -= 20;
            }
            else
            {
                SpawnManager._gameManager.tutorialManager.DisplayIncorrectElementNotification();
            }

            if(wallHealth == 40)
            {
                SpriteTexture = _texture2;
            }

            if(wallHealth == 20)
            {
                SpriteTexture = _texture3;
            }

            if (wallHealth <= 0)
            {
                if (SpawnManager._gameManager.tutorialManager.displayingElementWallMessage)
                {
                    SpawnManager._gameManager.tutorialManager.RemoveMessage();
                }

                AudioManager.PlaySound("WallBreak_SFX");
                tile.IsBlocking = false;
                IsActive = false;
            }
        }

        public void LoadWallTextures(Texture2D texture1, Texture2D texture2, Texture2D texture3)
        {
            _texture1 = texture1;
            _texture2 = texture2;
            _texture3 = texture3;

            SpriteTexture = texture1;
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
