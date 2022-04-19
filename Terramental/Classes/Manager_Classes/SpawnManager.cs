using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Terramental
{
    public static class SpawnManager
    {
        /// <summary>
        /// Controls the majority of loaded objects
        /// </summary>

        public static GameManager _gameManager;

        public static int dialogueTriggersCount;

        public static void Update(GameTime gameTime)
        {
            if(GameManager.currentGameState == GameManager.GameState.Level)
            {
                foreach (KnightCharacter knightEnemy in _gameManager.currentLevelData.knightEnemies)
                {
                    if (knightEnemy.IsActive)
                    {
                        knightEnemy.UpdateCharacter(gameTime);
                        knightEnemy.UpdateKnightEnemy(gameTime);
                    }
                }

                foreach (HealthPickup healthPickup in _gameManager.currentLevelData._healthPickups)
                {
                    if (healthPickup.IsActive)
                    {
                        healthPickup.CheckHealthPickupCollision();
                    }
                }

                foreach (ElementPickup elementPickup in _gameManager.currentLevelData._elementPickups)
                {
                    if (elementPickup.IsActive)
                    {
                        elementPickup.CheckElementPickupCollision(gameTime);
                    }
                }

                foreach (ScorePickup scorePickup in _gameManager.currentLevelData._scorePickups)
                {
                    if (scorePickup.IsActive)
                    {
                        scorePickup.UpdateScorePickup();
                    }
                }

                foreach (ElementWall elementWall in _gameManager.currentLevelData.elementWallList)
                {
                    if (elementWall.IsActive)
                    {
                        elementWall.ElementWallCollisions();
                    }
                }

                foreach (DialogueController dialogueController in _gameManager.currentLevelData.dialogueControllerList)
                {
                    dialogueController.CheckDialogueCollision();
                }

                foreach (Checkpoint checkpoint in _gameManager.currentLevelData.levelCheckpointList)
                {
                    if (checkpoint.IsActive)
                    {
                        checkpoint.CheckCollision();
                    }
                }

                if(_gameManager.currentLevelData.levelFragment != null)
                {
                    _gameManager.currentLevelData.levelFragment.CheckFragmentCollision(gameTime);
                }
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
                Random rand = new Random();
                int elementIndex = rand.Next(0, 2);

                Animation knightIdle = new Animation(_gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Idle_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                Animation knightWalk = new Animation(_gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Walk_SpriteSheet"), 7, 120f, true, new Vector2(96, 96));
                Animation knightAttack = new Animation(_gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Attack_SpriteSheet"), 7, 150f, true, new Vector2(96, 96));

                KnightCharacter knightEnemy = new KnightCharacter();
                knightEnemy.Initialise(position + new Vector2(0, -32), _gameManager.GetTexture("Sprites/Enemies/Knight/KnightCharacter_Sprite_Default"), new Vector2(96, 96));
                knightEnemy.SetProperties(position + new Vector2(0, -32));

                knightEnemy.AddAnimation(knightIdle);
                knightEnemy.AddAnimation(knightWalk);
                knightEnemy.AddAnimation(knightAttack);

                knightEnemy.LayerOrder = -1;

                knightEnemy.playerCharacter = _gameManager.playerCharacter;

                knightEnemy.ElementIndex = elementIndex;
                _gameManager.currentLevelData.knightEnemies.Add(knightEnemy);
            }
        }

        public static void SpawnHealthPickup(Vector2 position)
        {
            HealthPickup healthPickup = new HealthPickup(_gameManager.playerCharacter, 1);
            healthPickup.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Health_Pickup"), new Vector2(64, 64));

            _gameManager.currentLevelData._healthPickups.Add(healthPickup);
        }

        public static void SpawnScorePickup(Vector2 position)
        {
            ScorePickup scorePickup = new ScorePickup(_gameManager.playerCharacter);
            scorePickup.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Collectible"), new Vector2(64, 64));

            _gameManager.currentLevelData._scorePickups.Add(scorePickup);
        }

        public static void SpawnElementPickup(int elementIndex, Vector2 position)
        {
            ElementPickup elementPickup = new ElementPickup(elementIndex, _gameManager.GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), _gameManager.GetTexture("Sprites/Pickups/WaterPickup_SpriteSheet"), _gameManager.GetTexture("Sprites/Pickups/SnowPickup_SpriteSheet"), _gameManager.playerCharacter);
            elementPickup.Initialise(new Vector2(position.X, position.Y), _gameManager.GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), new Vector2(64, 64));
            _gameManager.currentLevelData._elementPickups.Add(elementPickup);
        }

        public static void SpawnElementWall(int elementIndex, Vector2 position, MapManager mapManager)
        {
            ElementWall elementWall = new ElementWall(_gameManager.playerCharacter, mapManager, elementIndex);

            switch(elementIndex)
            {
                case 0:
                    elementWall.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/FireTile"), new Vector2(64, 64));
                    break;
                case 1:
                    elementWall.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/WaterTile"), new Vector2(64, 64));
                    break;
                case 2:
                    elementWall.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/SnowTile"), new Vector2(64, 64));
                    break;
            }

            _gameManager.currentLevelData.elementWallList.Add(elementWall);
        }

        public static void SpawnDialogueTrigger(Vector2 position)
        {
            DialogueController dialogueController = new DialogueController(_gameManager.playerCharacter, new Rectangle((int)position.X, (int)position.Y,
                    (int)_gameManager.currentLevelData.dialogueScaleList[dialogueTriggersCount].X, (int)_gameManager.currentLevelData.dialogueScaleList[dialogueTriggersCount].Y),
                    _gameManager.dialogueManager, _gameManager.currentLevelData.levelDialogueList[dialogueTriggersCount]);

            _gameManager.currentLevelData.dialogueControllerList.Add(dialogueController);

            dialogueTriggersCount++;
        }

        public static void GenerateDialogue(int levelIndex)
        {
            _gameManager.currentLevelData.levelDialogueList.Clear();
            _gameManager.currentLevelData.dialogueScaleList.Clear();

            if(levelIndex == 0)
            {
                string[] dialogue1Content = { "Hello, my name is Bob.", "How are you?", "It is nice weather.", "Goodbye!"};
                Dialogue dialogue1 = new Dialogue(dialogue1Content, "Bob");

                string[] dialogue2Content = { "Today is Saturday.", "Nice weather!", "Bye, have a good day!" };
                Dialogue dialogue2 = new Dialogue(dialogue2Content, "Sam");

                string[] dialogue3Content = { "Hello there!", "It is over Anakin", "I have the high ground!" };
                Dialogue dialogue3 = new Dialogue(dialogue3Content, "Obi-Wan Kenobi");

                _gameManager.currentLevelData.levelDialogueList.Add(dialogue1);
                _gameManager.currentLevelData.levelDialogueList.Add(dialogue2);
                _gameManager.currentLevelData.levelDialogueList.Add(dialogue3);

                _gameManager.currentLevelData.dialogueScaleList.Add(new Vector2(64, 64));
                _gameManager.currentLevelData.dialogueScaleList.Add(new Vector2(64, 64));
                _gameManager.currentLevelData.dialogueScaleList.Add(new Vector2(64, 64));
            }
        }

        public static void SpawnCheckpoint(Vector2 position)
        {
            Checkpoint checkpoint = new Checkpoint(_gameManager.playerCharacter);
            checkpoint.Initialise(position, _gameManager.GetTexture("UserInterface/PlayerInterface/Collectible"), new Vector2(64, 64));
            _gameManager.currentLevelData.levelCheckpointList.Add(checkpoint);
        }

        public static void SpawnFragment(Vector2 position)
        {
            _gameManager.currentLevelData.levelFragment = new Fragment(_gameManager.menuManager, _gameManager.playerCharacter);
            _gameManager.currentLevelData.levelFragment.Initialise(position, _gameManager.GetTexture("UserInterface/PlayerInterface/Collectible"), new Vector2(64, 64));
        }

        public static void ResetEntities()
        {
            foreach(KnightCharacter character in _gameManager.currentLevelData.knightEnemies)
            {
                character.ResetCharacter();
            }

            foreach(ElementPickup elementPickup in _gameManager.currentLevelData._elementPickups)
            {
                elementPickup.ResetPickup();
            }

            foreach(HealthPickup healthPickup in _gameManager.currentLevelData._healthPickups)
            {
                healthPickup.IsActive = true;
            }

            foreach(ScorePickup scorePickup in _gameManager.currentLevelData._scorePickups)
            {
                scorePickup.IsActive = true;
            }

            _gameManager.currentLevelData.levelFragment.IsActive = true;
        }
    }
}
