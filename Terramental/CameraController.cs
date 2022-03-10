using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Terramental
{
    class CameraController
    {
        public Matrix Transform { get; private set; }

        public void MoveCamera(Sprite target)
        {
            var offset = Matrix.CreateTranslation(Game1.screenWidth / 2, Game1.screenHeight / 2, 0);
            var position = Matrix.CreateTranslation(new Vector3(-target.SpritePosition.X - (target.SpriteRectangle.Width / 2), -target.SpritePosition.Y - (target.SpriteRectangle.Height / 2), 0));
            
            Transform = position * offset;
        }
    }
}
