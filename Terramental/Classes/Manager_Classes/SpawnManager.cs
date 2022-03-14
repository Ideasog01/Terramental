using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class SpawnManager
    {
        public GameManager _gameManager;

        public List<BaseCharacter> enemyCharacters = new List<BaseCharacter>();
        public List<Sprite> effects = new List<Sprite>();

        public SpawnManager(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void Update(GameTime gameTime)
        {
            foreach(BaseCharacter character in enemyCharacters)
            {
                character.UpdateCharacter(gameTime);
            }
        }

        public virtual void SpawnEffect(string texturePath, Vector2 position, Vector2 scale, Sprite attachSprite, float duration)
        {
            Sprite effectSprite = new Sprite();
            Texture2D spriteTexture = _gameManager.GetTexture(texturePath);
            effectSprite.Initialise(position, spriteTexture, scale, this);
            effectSprite.AttachSprite = attachSprite;

            Animation effectAnimation = new Animation(spriteTexture, 4, 120f, true);

            effectSprite.Animations.Add(effectAnimation);

            effectSprite.Destroy(duration);
        }

        public virtual void SpawnEffect(string texturePath, Vector2 position, Vector2 scale, float duration)
        {
            Sprite effectSprite = new Sprite();
            Texture2D spriteTexture = _gameManager.GetTexture(texturePath);
            effectSprite.Initialise(position, spriteTexture, scale, this);

            Animation effectAnimation = new Animation(spriteTexture, 4, 120f, true);
            effectSprite.Animations.Add(effectAnimation);

            effectSprite.Destroy(duration);
        }

    }
}
