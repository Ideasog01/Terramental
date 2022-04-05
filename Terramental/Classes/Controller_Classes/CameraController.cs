using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    class CameraController
    {
        /// <summary>
        /// The class responsible for controlling the player camera
        /// </summary>
        /// 

        public static bool cameraActive = false;
        public static Vector2 playerPosition;
        public static Viewport viewPort;
        public Matrix _transform = Matrix.CreateTranslation(new Vector3(0, 0, 0));

        public Matrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        public static Rectangle ViewPortDimensions
        {
            get { return new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 540, 960); }
        }

        public void MoveCamera(Sprite target)
        {
            if(cameraActive && GameManager.currentGameState == GameManager.GameState.Level)
            {
                var offset = Matrix.CreateTranslation(GameManager.screenWidth / 2, GameManager.screenHeight / 2, 0);
                var _cameraPosition = Matrix.CreateTranslation(new Vector3(-target.SpritePosition.X - (target.SpriteRectangle.Width / 2), -target.SpritePosition.Y - (target.SpriteRectangle.Height / 2), 0));

                _transform = _cameraPosition * offset;
            }

            if(GameManager.currentGameState != GameManager.GameState.Level)
            {
                var _cameraPosition = Matrix.CreateTranslation(new Vector3(0, 0, 0));
                _transform = _cameraPosition;
            }
        }

        public Vector2 ScreenToWorldSpace(Vector2 point)
        {
            Matrix invertedMatrix = Matrix.Invert(_transform);
            return Vector2.Transform(point, invertedMatrix);
        }

        public static bool ObjectIsVisible(Rectangle bounds)
        {
            return true;
        }
    }
}
