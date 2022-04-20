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

        public static List<KnightCharacter> knightEnemies = new List<KnightCharacter>();
        public static List<HealthPickup> _healthPickups = new List<HealthPickup>();
        public static List<Cannon> _cannons = new List<Cannon>();
        public static List<CannonProjectile> _cannonProjectiles = new List<CannonProjectile>();
        public static List<ElementPickup> _elementPickups = new List<ElementPickup>();
        public static List<ScorePickup> _scorePickups = new List<ScorePickup>();
        public static List<Sprite> effects = new List<Sprite>();
        public static List<ElementWall> elementWallList = new List<ElementWall>();
        public static List<MovingPlatform> _movingPlatforms = new List<MovingPlatform>();

        public static List<DialogueController> dialogueControllerList = new List<DialogueController>();
        public static List<Checkpoint> levelCheckpointList = new List<Checkpoint>();
        public static Fragment levelFragment;
        
        public static List<Dialogue> levelDialogueList = new List<Dialogue>();
        public static List<Vector2> dialogueScaleList = new List<Vector2>();

        public static int dialogueTriggersCount;

        public static void Update(GameTime gameTime)
        {
            if(GameManager.currentGameState == GameManager.GameState.Level)
            {
                foreach (KnightCharacter knightEnemy in knightEnemies)
                {
                    if (knightEnemy.IsActive)
                    {
                        knightEnemy.UpdateCharacter(gameTime);
                        knightEnemy.UpdateKnightEnemy(gameTime);
                    }
                }

                foreach (HealthPickup healthPickup in _healthPickups)
                {
                    if (healthPickup.IsActive)
                    {
                        healthPickup.CheckHealthPickupCollision();
                    }
                }

                foreach (Cannon cannon in _cannons)
                {
                    if (cannon.IsActive)
                    {
                        cannon.UpdateCannon(gameTime);

                    }
                }

                foreach (CannonProjectile cannonProjectile in _cannonProjectiles)
                {
                    if (cannonProjectile.IsActive)
                    {
                        cannonProjectile.UpdateCannonProjectile();
                        cannonProjectile.CheckCannonProjectileCollisions();
                    }
                }


                foreach (ElementPickup elementPickup in _elementPickups)
                {
                    if (elementPickup.IsActive)
                    {
                        elementPickup.CheckElementPickupCollision(gameTime);
                    }
                }

                foreach (ScorePickup scorePickup in _scorePickups)
                {
                    if (scorePickup.IsActive)
                    {
                        scorePickup.UpdateScorePickup();
                    }
                }

                foreach (ElementWall elementWall in elementWallList)
                {
                    if (elementWall.IsActive)
                    {
                        elementWall.ElementWallCollisions();
                    }
                }

                foreach (MovingPlatform movingPlatform in _movingPlatforms)
                {
                    if (movingPlatform.IsActive)
                    {
                        movingPlatform.UpdateMovingPlatform();
                    }
                }

                foreach (DialogueController dialogueController in dialogueControllerList)
                {
                    dialogueController.CheckDialogueCollision();
                }

                foreach (Checkpoint checkpoint in levelCheckpointList)
                {
                    if (checkpoint.IsActive)
                    {
                        checkpoint.CheckCollision();
                    }
                }

                if(levelFragment != null)
                {
                    levelFragment.CheckFragmentCollision();
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
                
                knightEnemy.AddAnimation(knightIdle);
                knightEnemy.AddAnimation(knightWalk);
                knightEnemy.AddAnimation(knightAttack);

                knightEnemy.LayerOrder = -1;

                knightEnemy.playerCharacter = _gameManager.playerCharacter;

                knightEnemy.ElementIndex = elementIndex;
                knightEnemies.Add(knightEnemy);
            }
        }

        public static void SpawnHealthPickup(Vector2 position)
        {
            HealthPickup healthPickup = new HealthPickup(_gameManager.playerCharacter, 1);
            healthPickup.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Health_Pickup"), new Vector2(64, 64));

            _healthPickups.Add(healthPickup);
        }

        public static void SpawnCannonObstacle(int cannonDir, Vector2 position)
        {
            Cannon cannonObstacle = new Cannon(_gameManager, _gameManager.playerCharacter, cannonDir);

            switch (cannonDir)
            {
                case 0: // Left facing cannon
                    cannonObstacle.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/Cannon/Cannon_LeftExample"), new Vector2(64, 64)); break;
                case 1: // Right facing cannon
                    cannonObstacle.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/Cannon/Cannon_RightExample"), new Vector2(64, 64)); break;
                default: break;
            }

            _cannons.Add(cannonObstacle);
        }

        public static void SpawnCannonProjectile(int cannonDir, Vector2 position)
        {
            CannonProjectile cannonProj = new CannonProjectile(_gameManager.playerCharacter, cannonDir);

            switch (cannonDir)
            {
                case 0: // Left facing cannon
                    cannonProj.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/Cannon/Bullet_LeftExample"), new Vector2(64, 64));
                    break;
                case 1: // Right facing cannon
                    cannonProj.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/Cannon/Bullet_RightExample"), new Vector2(64, 64));
                    break;

                default:
                    break;
            }

            _cannonProjectiles.Add(cannonProj);
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

            elementWallList.Add(elementWall);
        }

        public static void SpawnMovingPlatform(Vector2 position, MapManager mapManager)
        {
            MovingPlatform movingPlatform = new MovingPlatform(_gameManager.playerCharacter, mapManager);
            movingPlatform.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/SnowTile"), new Vector2(64, 64));
            movingPlatform.LayerOrder = -2;

            _movingPlatforms.Add(movingPlatform);
        }

        public static void SpawnDialogueTrigger(Vector2 position)
        {
            DialogueController dialogueController = new DialogueController(_gameManager.playerCharacter, new Rectangle((int)position.X, (int)position.Y,
                    (int)dialogueScaleList[dialogueTriggersCount].X, (int)dialogueScaleList[dialogueTriggersCount].Y),
                    _gameManager.dialogueManager, levelDialogueList[dialogueTriggersCount]);

            dialogueControllerList.Add(dialogueController);

            dialogueTriggersCount++;
        }

        public static void GenerateDialogue(int levelIndex)
        {
            levelDialogueList.Clear();
            dialogueScaleList.Clear();

            if(levelIndex == 0)
            {
                string[] dialogue1Content = { "Hello, my name is Bob.", "How are you?", "It is nice weather.", "Goodbye!"};
                Dialogue dialogue1 = new Dialogue(dialogue1Content, "Bob");

                string[] dialogue2Content = { "Today is Saturday.", "Nice weather!", "Bye, have a good day!" };
                Dialogue dialogue2 = new Dialogue(dialogue2Content, "Sam");

                string[] dialogue3Content = { "Hello there!", "It is over Anakin", "I have the high ground!" };
                Dialogue dialogue3 = new Dialogue(dialogue3Content, "Obi-Wan Kenobi");

                levelDialogueList.Add(dialogue1);
                levelDialogueList.Add(dialogue2);
                levelDialogueList.Add(dialogue3);

                dialogueScaleList.Add(new Vector2(64, 64));
                dialogueScaleList.Add(new Vector2(64, 64));
                dialogueScaleList.Add(new Vector2(64, 64));
            }
        }

        public static void SpawnCheckpoint(Vector2 position)
        {
            Checkpoint checkpoint = new Checkpoint(_gameManager.playerCharacter);
            checkpoint.Initialise(position, _gameManager.GetTexture("UserInterface/PlayerInterface/Collectible"), new Vector2(64, 64));
            levelCheckpointList.Add(checkpoint);
        }

        public static void SpawnFragment(Vector2 position)
        {
            levelFragment = new Fragment(_gameManager.menuManager, _gameManager.playerCharacter);
            levelFragment.Initialise(position, _gameManager.GetTexture("UserInterface/PlayerInterface/Collectible"), new Vector2(64, 64));
        }
    }
}
