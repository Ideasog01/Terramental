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

        public static List<Projectile> activeProjectileList = new List<Projectile>();
        public static List<Projectile> inactiveProjectileList = new List<Projectile>();

        public static List<EnemyCharacter> enemyList = new List<EnemyCharacter>();
        public static List<HealthPickup> healthPickupList = new List<HealthPickup>();
        public static List<ScorePickup> scorePickupList = new List<ScorePickup>();
        public static List<ElementWall> elementWallList = new List<ElementWall>();
        public static List<ElementPickup> elementPickupList = new List<ElementPickup>();
        public static List<Checkpoint> checkpointList = new List<Checkpoint>();
        public static Fragment levelFragment;

        public static void Update(GameTime gameTime)
        {
            if(GameManager.currentGameState == GameManager.GameState.Level)
            {
                foreach (EnemyCharacter knightEnemy in enemyList)
                {
                    if (knightEnemy.IsActive)
                    {
                        knightEnemy.UpdateCharacter(gameTime);
                        knightEnemy.UpdateKnightEnemy(gameTime);
                        knightEnemy.UpdateHealthBar();
                        knightEnemy.EnableHealthBar(true);
                    }
                    else
                    {
                        knightEnemy.EnableHealthBar(false);
                    }
                }

                foreach(Projectile projectile in activeProjectileList)
                {
                    if(projectile != null)
                    {
                        if (projectile.IsActive)
                        {
                            projectile.UpdateProjectile(gameTime);
                        }
                    }
                    else
                    {
                        break;
                    }
                     
                }

                foreach (HealthPickup healthPickup in healthPickupList)
                {
                    if (healthPickup.IsActive)
                    {
                        healthPickup.CheckHealthPickupCollision();
                    }
                }

                foreach (ElementPickup elementPickup in elementPickupList)
                {
                    if (elementPickup.IsActive)
                    {
                        elementPickup.CheckElementPickupCollision(gameTime);
                    }
                }

                foreach (ScorePickup scorePickup in scorePickupList)
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

                foreach (Checkpoint checkpoint in checkpointList)
                {
                    if (checkpoint.IsActive)
                    {
                        checkpoint.CheckCollision();
                    }
                }

                if(levelFragment != null)
                {
                    levelFragment.CheckFragmentCollision(gameTime);
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
                knightEnemy.SetProperties(position + new Vector2(0, -32), 100, 100);

                knightEnemy.AddAnimation(knightIdle);
                knightEnemy.AddAnimation(knightWalk);
                knightEnemy.AddAnimation(knightAttack);

                knightEnemy.LoadHealthBar(_gameManager);

                knightEnemy.LayerOrder = -1;

                knightEnemy.playerCharacter = _gameManager.playerCharacter;
                knightEnemy.AttackThreshold = 40;

                knightEnemy.ElementIndex = elementIndex;
                enemyList.Add(knightEnemy);
                knightEnemy.AttackCooldown = 1;
                knightEnemy.EnemyIndex = 0;
            }
            else if(index == 1)
            {
                Random rand = new Random();
                int elementIndex = rand.Next(0, 2);

                Animation mageIdle = new Animation(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                Animation mageWalk = new Animation(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Walk_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                Animation mageAttack = new Animation(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Attack_SpriteSheet"), 8, 140f, true, new Vector2(64, 64));

                DarkMageCharacter darkMageCharacter = new DarkMageCharacter();
                darkMageCharacter.Initialise(position + new Vector2(0, -86), _gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), new Vector2(96, 96));
                darkMageCharacter.SetProperties(position + new Vector2(0, -86), 100, 100);

                darkMageCharacter.AddAnimation(mageIdle);
                darkMageCharacter.AddAnimation(mageWalk);
                darkMageCharacter.AddAnimation(mageAttack);

                darkMageCharacter.AttackThreshold = 300;

                darkMageCharacter.LoadHealthBar(_gameManager);
                darkMageCharacter.LayerOrder = -1;
                darkMageCharacter.playerCharacter = _gameManager.playerCharacter;

                darkMageCharacter.EnemyIndex = 1;
                darkMageCharacter.AttackCooldown = 3;

                enemyList.Add(darkMageCharacter);
            }
        }

        public static void SpawnHealthPickup(Vector2 position)
        {
            HealthPickup healthPickup = new HealthPickup(_gameManager.playerCharacter, 1);
            healthPickup.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Health_Pickup"), new Vector2(64, 64));

            healthPickupList.Add(healthPickup);
        }

        public static void SpawnScorePickup(Vector2 position)
        {
            ScorePickup scorePickup = new ScorePickup(_gameManager.playerCharacter);
            scorePickup.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Collectible"), new Vector2(64, 64));

            scorePickupList.Add(scorePickup);
        }

        public static void SpawnElementPickup(int elementIndex, Vector2 position)
        {
            ElementPickup elementPickup = new ElementPickup(elementIndex, _gameManager.GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), _gameManager.GetTexture("Sprites/Pickups/WaterPickup_SpriteSheet"), _gameManager.GetTexture("Sprites/Pickups/SnowPickup_SpriteSheet"), _gameManager.playerCharacter);
            elementPickup.Initialise(new Vector2(position.X, position.Y), _gameManager.GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), new Vector2(64, 64));
            elementPickupList.Add(elementPickup);
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

        public static void SpawnCheckpoint(Vector2 position)
        {
            Checkpoint checkpoint = new Checkpoint(_gameManager.playerCharacter);
            checkpoint.Initialise(position, _gameManager.GetTexture("UserInterface/PlayerInterface/Collectible"), new Vector2(64, 64));
            checkpointList.Add(checkpoint);
        }

        public static void SpawnFragment(Vector2 position)
        {
            levelFragment = new Fragment(_gameManager.menuManager, _gameManager.playerCharacter);
            levelFragment.Initialise(position, _gameManager.GetTexture("UserInterface/PlayerInterface/Collectible"), new Vector2(64, 64));
        }

        public static void ResetEntities()
        {
            foreach(EnemyCharacter character in enemyList)
            {
                character.ResetCharacter();
            }

            foreach(ElementPickup elementPickup in elementPickupList)
            {
                elementPickup.ResetPickup();
            }

            foreach(HealthPickup healthPickup in healthPickupList)
            {
                healthPickup.IsActive = true;
            }

            foreach(ScorePickup scorePickup in scorePickupList)
            {
                scorePickup.IsActive = true;
            }

            levelFragment.IsActive = true;
        }

        public static void SpawnProjectile(Texture2D texture, Vector2 position, Vector2 scale, Vector2 velocity, bool isEnemyProjectile)
        {
            if(inactiveProjectileList.Count > 0)
            {
                inactiveProjectileList[0].ResetProjectile(texture, position, scale, velocity, isEnemyProjectile);
                activeProjectileList.Add(inactiveProjectileList[0]);
                inactiveProjectileList.RemoveAt(0);
            }
            else
            {
                Projectile projectile = new Projectile();
                projectile.ResetProjectile(texture, position, scale, velocity, isEnemyProjectile);
                projectile.Initialise(position, texture, scale);

                activeProjectileList.Add(projectile);
            }
        }
    }
}
