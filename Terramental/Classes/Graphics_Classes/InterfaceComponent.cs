using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Terramental
{
    public class InterfaceComponent
    {
        private Vector2 _componentPosition;
        private Vector2 _componentOffset;
        private Vector2 _componentScale;
        private Texture2D _componentTexture;
        private Rectangle _componentRectangle;
        private Color _componentColor = Color.White;

        public InterfaceComponent(Vector2 position, Vector2 scale, Texture2D texture)
        {
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

        public Vector2 ComponentScale
        {
            get { return _componentScale; }
            set { _componentScale = value; }
        }

        public Color ComponentColor
        {
            get { return _componentColor; }
            set { _componentColor = value; }
        }

        public void SetProperties(Texture2D texture, Vector2 position, Vector2 scale)
        {
            _componentTexture = texture;
            _componentPosition = position;
            _componentScale = scale;
        }

        public void DrawComponent(SpriteBatch spriteBatch)
        {
            _componentPosition = CameraController.cameraTopLeftAnchor + _componentOffset;
            _componentRectangle = new Rectangle((int)_componentPosition.X, (int)_componentPosition.Y, (int)_componentScale.X, (int)_componentScale.Y);
            spriteBatch.Draw(_componentTexture, _componentRectangle, _componentColor);
        }

    }
}
