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
        public static List<HealthPickup> healthPickups = new List<HealthPickup>();
        public static List<Sprite> effects = new List<Sprite>();

        public static void UpdateEntities(GameTime gameTime)
        {
            //Updates all characters in the enemy characters list

            foreach(BaseCharacter character in enemyCharacters)
            {
                character.UpdateCharacter(gameTime);
            }

            foreach(HealthPickup pickup in healthPickups)
            {
                pickup.CheckCollision();
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

        public static void SpawnEnemy(int index, Vector2 position)
        {
            if(index == 0) //Knight Enemy Character
            {
                BaseCharacter enemyCharacter = new BaseCharacter();
                enemyCharacter.Initialise(position, _gameManager.GetTexture("Sprites/Enemies/Knight/KnightCharacter_Sprite_Default"), new Vector2(96, 96));

                enemyCharacters.Add(enemyCharacter);
            }
        }

        public static void SpawnHealthPickup(Vector2 position)
        {
            HealthPickup healthPickup = new HealthPickup(_gameManager.playerCharacter, 20);
            healthPickup.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Health_Pickup"), new Vector2(64, 64));

            healthPickups.Add(healthPickup);
        }

        public static void SpawnScorePickup()
        {

        }

        public static void SpawnElementPickup(int index)
        {

        }

        public static void SetGameManager(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

    }
}
