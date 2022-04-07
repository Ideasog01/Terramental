﻿using System.Collections.Generic;
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

        public static List<KnightCharacter> knightEnemies = new List<KnightCharacter>();
        public static List<HealthPickup> _healthPickups = new List<HealthPickup>();
        public static List<ElementPickup> _elementPickups = new List<ElementPickup>();
        public static List<ScorePickup> _scorePickups = new List<ScorePickup>();
        public static List<Sprite> effects = new List<Sprite>();

        public static void Update(GameTime gameTime)
        {
            //Updates all characters in the enemy characters list

            foreach(KnightCharacter knightEnemy in knightEnemies)
            {
                knightEnemy.UpdateCharacter(gameTime);
                knightEnemy.UpdateKnightEnemy(gameTime);
            }

            foreach (HealthPickup healthPickup in _healthPickups)
            {
                healthPickup.CheckHealthPickupCollision();
            }

            foreach (ElementPickup elementPickup in _elementPickups)
            {
                elementPickup.CheckElementPickupCollision(gameTime);
            }

            foreach(ScorePickup scorePickup in _scorePickups)
            {
                scorePickup.UpdateScorePickup();
            }
        }

        public static void SpawnAttachEffect(string texturePath, Vector2 position, Vector2 scale, Sprite attachSprite, float duration, bool animation)
        {
            Sprite effectSprite = new Sprite();
            Texture2D spriteTexture = _gameManager.GetTexture(texturePath);
            effectSprite.Initialise(position, spriteTexture, scale);
            effectSprite.AttachSprite = attachSprite;

            if(animation)
            {
                Animation effectAnimation = new Animation(spriteTexture, 4, 120f, true, new Vector2(64, 64));
                effectSprite.Animations.Add(effectAnimation);
            }

            effectSprite.Destroy(duration);
        }

        public static void SpawnEffect(string texturePath, Vector2 position, Vector2 scale, float duration)
        {
            Sprite effectSprite = new Sprite();
            Texture2D spriteTexture = _gameManager.GetTexture(texturePath);
            effectSprite.Initialise(position, spriteTexture, scale);

            Animation effectAnimation = new Animation(spriteTexture, 4, 120f, true, new Vector2(64, 64));
            effectSprite.Animations.Add(effectAnimation);

            effectSprite.Destroy(duration);
        }
        public static void SpawnEnemy(int index, Vector2 position)
        {
            if(index == 0) //Knight Enemy Character
            {
                Animation knightIdle = new Animation(_gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Idle_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                Animation knightWalk = new Animation(_gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Walk_SpriteSheet"), 7, 120f, true, new Vector2(96, 96));
                Animation knightAttack = new Animation(_gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Attack_SpriteSheet"), 7, 150f, true, new Vector2(96, 96));

                KnightCharacter knightEnemy = new KnightCharacter();
                knightEnemy.Initialise(position + new Vector2(0, -32), _gameManager.GetTexture("Sprites/Enemies/Knight/KnightCharacter_Sprite_Default"), new Vector2(96, 96));
                
                knightEnemy.AddAnimation(knightIdle);
                knightEnemy.AddAnimation(knightWalk);
                knightEnemy.AddAnimation(knightAttack);

                knightEnemy.LayerOrder = -1;

                knightEnemy.playerCharacter = _gameManager.playerCharacter;

                knightEnemies.Add(knightEnemy);
            }
        }

        public static void SpawnHealthPickup(Vector2 position)
        {
            HealthPickup healthPickup = new HealthPickup(_gameManager.playerCharacter, 1);
            healthPickup.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Health_Pickup"), new Vector2(64, 64));

            _healthPickups.Add(healthPickup);
        }

        public static void SpawnScorePickup(Vector2 position)
        {
            ScorePickup scorePickup = new ScorePickup(_gameManager.playerCharacter);
            scorePickup.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Collectible"), new Vector2(64, 64));

            _scorePickups.Add(scorePickup);
        }

        public static void SpawnElementPickup(int elementIndex, Vector2 position)
        {
            ElementPickup elementPickup = new ElementPickup(elementIndex, _gameManager.GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), _gameManager.GetTexture("Sprites/Pickups/WaterPickup_SpriteSheet"), _gameManager.GetTexture("Sprites/Pickups/SnowPickup_SpriteSheet"), _gameManager.playerCharacter);
            elementPickup.Initialise(new Vector2(position.X, position.Y), _gameManager.GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), new Vector2(64, 64));   
            _elementPickups.Add(elementPickup);
        }
    }
}
