using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Terramental
{
    public class InterfaceComponent
    {
        private PlayerCharacter _playerCharacter;
        private Vector2 _componentPosition;
        private Vector2 _componentOffset;
        private Vector2 _componentScale;
        private Texture2D _componentTexture;
        private Rectangle _componentRectangle;
        private Color _componentColor = Color.White;

        public InterfaceComponent(PlayerCharacter playerCharacter, Vector2 position, Vector2 scale, Texture2D texture)
        {
            _playerCharacter = playerCharacter;
            _componentOffset = position;
            _componentPosition = position;
            _componentScale = scale;
            _componentTexture = texture;

            _componentRectangle = new Rectangle((int)_componentPosition.X, (int)_componentPosition.Y, (int)_componentScale.X, (int)_componentScale.Y);
        }

        public Vector2 ComponentPosition
        {
            get { return _componentPosition; }
            set { _componentPosition = value; }
        }

        public Color ComponentColor
        {
            get { return _componentColor; }
            set { _componentColor = value; }
        }

        public void FollowCamera()
        {
            _componentPosition = _playerCharacter.SpritePosition + _componentOffset;
            _componentRectangle = new Rectangle((int)_componentPosition.X, (int)_componentPosition.Y, (int)_componentScale.X, (int)_componentScale.Y);
        }

        public void DrawComponent(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_componentTexture, _componentRectangle, _componentColor);
        }

    }
}
