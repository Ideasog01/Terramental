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


        public Matrix cameraTransform; //The current location of the camera in Matrix form
        public Viewport viewPort; //The viewport/game window
        public static Vector2 cameraWorldPos; //The current location of the camera in world coordinates in the form of a Vector2
        public static Vector2 cameraTopLeftAnchor; //The current location of the camera justified to the topLeft of the screen
        public static PlayerCharacter playerCharacter; //A reference to the playerCharacter used for positioning of the camera

        public CameraController(Viewport newView) //The CameraController constructor used to assign the viewport
        {
            viewPort = newView;
        }

        public void UpdateCamera(GameTime gameTime)
        {
            var scaleX = (float)GameManager.screenWidth / 960; //Gets the current screen width and converts it to the virtual width
            var scaleY = (float)GameManager.screenHeight / 540; //Gets the current screen height and converts it to the virtual height
            var matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f); //Creates a scale for the matrix based on the given scaleX and scaleY

            if (GameManager.currentGameState != GameManager.GameState.Level && GameManager.currentGameState != GameManager.GameState.LevelPause) //If the game is not in a level state, set the camera position to the default location at Vector3.Zero (Ensures menus are displayed correctly)
            {
                var cameraPosition = matrix * Matrix.CreateTranslation(new Vector3(0, 0, 0)); //Sets the location of the camera
                cameraTransform = cameraPosition; //Applies the location
            }
            else
            {
                if (playerCharacter != null) //Checks if the playerCharacter is not null in case it hasn't been loaded yet
                {
                    cameraWorldPos = new Vector2(playerCharacter.SpritePosition.X - (viewPort.Width / 2) + (playerCharacter.SpriteRectangle.Width / 2), playerCharacter.SpritePosition.Y - (viewPort.Height / 2) + (playerCharacter.SpriteRectangle.Height / 2)); //Sets the camera position to the location of the player so the object is at the screen centre

                    float cameraCentreX = MathHelper.Clamp(cameraWorldPos.X, 0, (MapManager.mapWidth * 64) - 960); //Clamps the cameraCentre X coordinate to be within the level boundaries
                    float cameraCentreY = MathHelper.Clamp(cameraWorldPos.Y, 0, (MapManager.mapHeight * 64) - 540); //Clamps the cameraCentre Y coordinate to be within the level boundaries

                    cameraWorldPos.X = cameraCentreX; //Applies the clamped values
                    cameraWorldPos.Y = cameraCentreY;

                    cameraTopLeftAnchor = cameraWorldPos;

                    cameraTransform = matrix * Matrix.CreateTranslation(new Vector3(-cameraWorldPos.X, -cameraWorldPos.Y, 0)); //Applies the location
                }
            }    
        }

        public Vector2 ScreenToWorldSpace(Vector2 point) //Converts a location of the screen to world coordinates
        {
            Matrix invertedMatrix = Matrix.Invert(cameraTransform);
            return Vector2.Transform(point, invertedMatrix);
        }

        public static bool ObjectIsVisible(Vector2 position) //Checks if the object is currently visible by comparing the location of the object and the viewport bounds
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
