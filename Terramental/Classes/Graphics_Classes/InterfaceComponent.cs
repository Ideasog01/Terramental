using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Terramental
{
    class InterfaceComponent
    {
        private PlayerCharacter _playerCharacter;
        private Vector2 _componentPosition;
        private Vector2 _componentOffset;
        private Vector2 _componentScale;
        private Texture2D _componentTexture;
        private Rectangle _componentRectangle;

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
        }

        public void FollowCamera()
        {
            _componentPosition = _playerCharacter.SpritePosition + _componentOffset;
            _componentRectangle = new Rectangle((int)_componentPosition.X, (int)_componentPosition.Y, (int)_componentScale.X, (int)_componentScale.Y);
        }

        public void DrawComponent(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_componentTexture, _componentRectangle, Color.White);
        }

    }
}
