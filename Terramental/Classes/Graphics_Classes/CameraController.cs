using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Terramental
{
    class CameraController
    {
        /// <summary>
        /// The class responsible for controlling the player camera
        /// </summary>
        /// 


        public Matrix cameraTransform;
        public Viewport viewPort;
        public static Vector2 _cameraCentre;
        public static PlayerCharacter playerCharacter;

        public CameraController(Viewport newView)
        {
            viewPort = newView;
        }

        public void UpdateCamera(GameTime gameTime)
        {
            if (GameManager.currentGameState != GameManager.GameState.Level)
            {
                var _cameraPosition = Matrix.CreateTranslation(new Vector3(0, 0, 0));
                cameraTransform = _cameraPosition;
            }
            else
            {
                if (playerCharacter != null)
                {
                    _cameraCentre = new Vector2(playerCharacter.SpritePosition.X - (viewPort.Width / 2) + (playerCharacter.SpriteRectangle.Width / 2), playerCharacter.SpritePosition.Y - (viewPort.Height / 2) + (playerCharacter.SpriteRectangle.Height / 2));
                    
                    float cameraCentreX = MathHelper.Clamp(_cameraCentre.X, 0, (MapManager.mapWidth * 64) - 960);
                    float cameraCentreY = MathHelper.Clamp(_cameraCentre.Y, 0, (MapManager.mapHeight * 64) - 540);

                    _cameraCentre.X = cameraCentreX;
                    _cameraCentre.Y = cameraCentreY;

                    cameraTransform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-_cameraCentre.X, -_cameraCentre.Y, 0));
                }
            }    
        }

        //public static bool cameraActive = false;
        //public static Vector2 playerPosition;
        //public static Viewport viewPort;
        //public Matrix _transform = Matrix.CreateTranslation(new Vector3(0, 0, 0));

        //public Matrix Transform
        //{
        //    get { return _transform; }
        //    set { _transform = value; }
        //}

        //public static Rectangle ViewPortDimensions
        //{
        //    get { return new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 540, 960); }
        //}

        //public void MoveCamera(Sprite target)
        //{
        //    if(cameraActive && GameManager.currentGameState == GameManager.GameState.Level)
        //    {
        //        var offset = Matrix.CreateTranslation(GameManager.screenWidth / 2, GameManager.screenHeight / 2, 0);
        //        var _cameraPosition = Matrix.CreateTranslation(new Vector3(-target.SpritePosition.X - (target.SpriteRectangle.Width / 2), -target.SpritePosition.Y - (target.SpriteRectangle.Height / 2), 0));

        //        _transform = _cameraPosition * offset;
        //    }

        //    if(GameManager.currentGameState != GameManager.GameState.Level)
        //    {
        //        var _cameraPosition = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        //        _transform = _cameraPosition;
        //    }
        //}

        public Vector2 ScreenToWorldSpace(Vector2 point)
        {
            Matrix invertedMatrix = Matrix.Invert(cameraTransform);
            return Vector2.Transform(point, invertedMatrix);
        }

        public static bool ObjectIsVisible(Vector2 position)
        {
            float distance = (float)Math.Sqrt(Math.Pow(position.X - playerCharacter.SpritePosition.X, 2) + MathF.Pow(position.Y - playerCharacter.SpritePosition.Y, 2));

            if (distance < 1000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
