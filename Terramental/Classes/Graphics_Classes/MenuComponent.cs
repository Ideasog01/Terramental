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
        private Color _componentColor;

        public void InitialiseMenuComponent(Texture2D texture, Vector2 position, Vector2 scale)
        {
            _componentTexture = texture;
            _componentPosition = position;
            _componentScale = scale;

            _componentRectangle = new Rectangle((int)_componentPosition.X, (int)_componentPosition.Y, (int)_componentScale.X, (int)_componentScale.Y);
            _componentColor = Color.White;
        }
        
        public Texture2D ComponentTexture
        {
            get { return _componentTexture; }
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

        public void DrawMenuComponent(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_componentTexture, _componentRectangle, _componentColor);
        }
    }
}
