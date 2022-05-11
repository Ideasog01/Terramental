using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class MenuComponent
    {
        private Texture2D _componentTexture;
        private Vector2 _componentPosition;
        private Vector2 _componentScale;
        private Rectangle _componentRectangle;
        private Vector2 _componentOffset;
        private Color _componentColor;
        private float _componentAlpha;

        public void InitialiseMenuComponent(Texture2D texture, Vector2 position, Vector2 scale)
        {
            _componentTexture = texture;
            _componentPosition = position;
            _componentScale = scale;
            _componentAlpha = 1.0f;

            _componentRectangle = new Rectangle((int)_componentPosition.X, (int)_componentPosition.Y, (int)_componentScale.X, (int)_componentScale.Y);
            _componentColor = Color.White;
        }
        
        public Texture2D ComponentTexture
        {
            get { return _componentTexture; }
            set { _componentTexture = value; }
        }

        public Vector2 ComponentScale
        {
            get { return _componentScale; }
        }

        public Vector2 ComponentPosition
        {
            get { return _componentPosition; }
            set { _componentPosition = value; }
        }

        public Rectangle ComponentRectangle
        {
            get { return _componentRectangle; }
            set { _componentRectangle = value; }
        }

        public Color ComponentColor
        {
            get { return _componentColor; }
            set { _componentColor = value; }
        }

        public void FollowCamera()
        {
            if(_componentOffset == Vector2.Zero)
            {
                _componentOffset = _componentPosition;
            }

            int centreX = (int)CameraController.cameraTopLeftAnchor.X;
            int centreY = (int)CameraController.cameraTopLeftAnchor.Y;
            _componentRectangle = new Rectangle(centreX + (int)_componentOffset.X, centreY + (int)_componentOffset.Y, _componentTexture.Width, _componentTexture.Height);
        }

        public void DrawMenuComponent(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_componentTexture, _componentRectangle, _componentColor);
        }
    }
}
