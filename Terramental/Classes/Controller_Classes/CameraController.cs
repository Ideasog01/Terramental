using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Terramental
{
    class CameraController
    {
        /// <summary>
        /// The class responsible for controlling the player camera
        /// </summary>
        /// 

        public static bool cameraActive = false;

        public Matrix _transform = Matrix.CreateTranslation(new Vector3(0, 0, 0));

        public Matrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        public void MoveCamera(Sprite target)
        {
            if(cameraActive)
            {
                var offset = Matrix.CreateTranslation(GameManager.screenWidth / 2, GameManager.screenHeight / 2, 0);
                var position = Matrix.CreateTranslation(new Vector3(-target.SpritePosition.X - (target.SpriteRectangle.Width / 2), -target.SpritePosition.Y - (target.SpriteRectangle.Height / 2), 0));

                _transform = position * offset;
            }
        }

        public Vector2 ScreenToWorldSpace(Vector2 point)
        {
            Matrix invertedMatrix = Matrix.Invert(_transform);
            return Vector2.Transform(point, invertedMatrix);
        }
    }
}
