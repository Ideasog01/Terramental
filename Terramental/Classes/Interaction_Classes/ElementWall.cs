using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class ElementWall : Sprite
    {
        private PlayerCharacter _playerCharacter;

        private int _elementIndex;

        private bool _invalidElement;

        private int wallHealth;

        private Tile tile;

        private Texture2D _texture1;
        private Texture2D _texture2;
        private Texture2D _texture3;

        public void InitialiseElementWall(PlayerCharacter playerCharacter, int elementIndex, Vector2 position)
        {
            SpritePosition = position;
            _playerCharacter = playerCharacter;
            _elementIndex = elementIndex;
            IsActive = true;
            IsLoaded = true;
            SetElementWallProperties(SpawnManager.gameManager.mapManager);
        }

        public Tile AssignedTile
        {
            get { return tile; }
            set { tile = value; }
        }

        public void SetElementWallProperties(MapManager mapManager)
        {
            wallHealth = 60;

            if(tile != null)
            {
                tile.IsBlocking = false;
            }

            SpriteTexture = _texture1;
            tile = mapManager.FindTile(SpritePosition);
            tile.IsBlocking = true;
            IsActive = true;
        }

        public void DamageElementWall()
        {
            if(IsActive)
            {
                if (!_invalidElement)
                {
                    wallHealth -= 20;
                }
                else
                {
                    SpawnManager.gameManager.tutorialManager.DisplayIncorrectElementNotification();
                }

                if (wallHealth == 40)
                {
                    SpriteTexture = _texture2;
                }

                if (wallHealth == 20)
                {
                    SpriteTexture = _texture3;
                }

                if (wallHealth <= 0)
                {
                    AudioManager.PlaySound("WallBreak_SFX");
                    tile.IsBlocking = false;
                    IsActive = false;
                    tile = null;
                }
            }
        }

        public void LoadWallTextures(Texture2D texture1, Texture2D texture2, Texture2D texture3)
        {
            _texture1 = texture1;
            _texture2 = texture2;
            _texture3 = texture3;

            SpriteTexture = _texture1;
        }

        public void AssignElementLogic()
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
                        _invalidElement = true;
                    }
                    else if (_elementIndex == 1)
                    {
                        _invalidElement = true;
                    }
                    else if (_elementIndex == 2)
                    {
                        _invalidElement = false;
                    }
                }

                if (_playerCharacter.ElementIndex == 1)
                {
                    if (_elementIndex == 0)
                    {
                        _invalidElement = false;
                    }
                    else if (_elementIndex == 1)
                    {
                        _invalidElement = true;
                    }
                    else if (_elementIndex == 2)
                    {
                        _invalidElement = true;
                    } 
                }

                if (_playerCharacter.ElementIndex == 2)
                {
                    if (_elementIndex == 0)
                    {
                        _invalidElement = true;
                    }
                    else if (_elementIndex == 1)
                    {
                        _invalidElement = false;
                    }
                    else if (_elementIndex == 2)
                    {
                        _invalidElement = true;
                    }
                }
            }
        }
    }
}
