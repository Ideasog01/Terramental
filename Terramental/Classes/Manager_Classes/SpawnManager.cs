using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public static class SpawnManager
    {
        /// <summary>
        /// Controls the majority of loaded objects
        /// </summary>

        public static GameManager _gameManager;

        public static List<BaseCharacter> enemyCharacters = new List<BaseCharacter>();
        public static List<Sprite> effects = new List<Sprite>();

        public static void Update(GameTime gameTime)
        {
            //Updates all characters in the enemy characters list

            foreach(BaseCharacter character in enemyCharacters)
            {
                character.UpdateCharacter(gameTime);
            }
        }

        public static void SpawnAttachEffect(string texturePath, Vector2 position, Vector2 scale, Sprite attachSprite, float duration)
        {
            Sprite effectSprite = new Sprite();
            Texture2D spriteTexture = _gameManager.GetTexture(texturePath);
            effectSprite.Initialise(position, spriteTexture, scale);
            effectSprite.AttachSprite = attachSprite;

            Animation effectAnimation = new Animation(spriteTexture, 4, 120f, true);

            effectSprite.Animations.Add(effectAnimation);

            effectSprite.Destroy(duration);
        }

        public static void SpawnEffect(string texturePath, Vector2 position, Vector2 scale, float duration)
        {
            Sprite effectSprite = new Sprite();
            Texture2D spriteTexture = _gameManager.GetTexture(texturePath);
            effectSprite.Initialise(position, spriteTexture, scale);

            Animation effectAnimation = new Animation(spriteTexture, 4, 120f, true);
            effectSprite.Animations.Add(effectAnimation);

            effectSprite.Destroy(duration);
        }

    }
}
