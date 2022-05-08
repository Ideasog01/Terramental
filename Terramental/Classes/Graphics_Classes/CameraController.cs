using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Terramental
{
    public class CameraController
    {
        /// <summary>
        /// The class responsible for controlling the player camera
        /// </summary>
        /// 


        public Matrix cameraTransform;
        public Viewport viewPort;
        public static Vector2 cameraWorldPos;
        public static Vector2 cameraCentre;
        public static PlayerCharacter playerCharacter;

        public CameraController(Viewport newView)
        {
            viewPort = newView;
        }
        
        public Vector2 CameraCentre
        {
            get { return cameraCentre; }
        }

        public void UpdateCamera(GameTime gameTime)
        {
            var scaleX = (float)GameManager.screenWidth / 960;
            var scaleY = (float)GameManager.screenHeight / 540;
            var matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            if (GameManager.currentGameState != GameManager.GameState.Level && GameManager.currentGameState != GameManager.GameState.LevelPause)
            {
                var cameraPosition = matrix * Matrix.CreateTranslation(new Vector3(0, 0, 0));
                cameraTransform = cameraPosition;
                cameraCentre = new Vector2(GameManager.screenWidth / 2, GameManager.screenHeight / 2);
            }
            else
            {
                if (playerCharacter != null)
                {
                    cameraWorldPos = new Vector2(playerCharacter.SpritePosition.X - (viewPort.Width / 2) + (playerCharacter.SpriteRectangle.Width / 2), playerCharacter.SpritePosition.Y - (viewPort.Height / 2) + (playerCharacter.SpriteRectangle.Height / 2));

                    float cameraCentreX = MathHelper.Clamp(cameraWorldPos.X, 0, (MapManager.mapWidth * 64) - 960);
                    float cameraCentreY = MathHelper.Clamp(cameraWorldPos.Y, 0, (MapManager.mapHeight * 64) - 540);

                    cameraWorldPos.X = cameraCentreX;
                    cameraWorldPos.Y = cameraCentreY;

                    cameraCentre = cameraWorldPos;

                    cameraTransform = matrix * Matrix.CreateTranslation(new Vector3(-cameraWorldPos.X, -cameraWorldPos.Y, 0));
                }
            }    
        }

        public Vector2 ScreenToWorldSpace(Vector2 point)
        {
            Matrix invertedMatrix = Matrix.Invert(cameraTransform);
            return Vector2.Transform(point, invertedMatrix);
        }

        public static bool ObjectIsVisible(Vector2 position)
        {
            float distance = (float)Math.Sqrt(Math.Pow(position.X - playerCharacter.SpritePosition.X, 2) + MathF.Pow(position.Y - playerCharacter.SpritePosition.Y, 2));

            if (distance < 1200)
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
