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
        public static List<SpikeObstacle> spikeObstacleList = new List<SpikeObstacle>();
        public static List<Cannon> cannonObstacleList = new List<Cannon>();
        public static Fragment levelFragment;

        public static void Update(GameTime gameTime)
        {
            if(GameManager.currentGameState == GameManager.GameState.Level)
            {
                foreach(EnemyCharacter enemy in enemyList)
                {
                    if (enemy.IsVisible && enemy.CharacterHealth > 0)
                    {
                        enemy.UpdateCharacter(gameTime);
                        enemy.UpdateKnightEnemy(gameTime);
                        enemy.UpdateHealthBar();
                        enemy.EnableHealthBar(true);
                    }
                    else
                    {
                        enemy.EnableHealthBar(false);
                    }
                }

                foreach(SpikeObstacle spikeObstacle in spikeObstacleList)
                {
                    if(spikeObstacle.IsVisible)
                    {
                        spikeObstacle.CheckCollision(gameTime);
                    }
                }

                foreach(Projectile projectile in activeProjectileList)
                {
                    if(projectile != null)
                    {
                        if(projectile.IsVisible)
                        {
                            if (projectile.IsActive)
                            {
                                projectile.UpdateProjectile(gameTime);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                foreach (HealthPickup healthPickup in healthPickupList)
                {
                    if(healthPickup.IsVisible)
                    {
                        if (healthPickup.IsActive)
                        {
                            healthPickup.CheckHealthPickupCollision();
                        }
                    }
                }

                foreach (ElementPickup elementPickup in elementPickupList)
                {
                    if(elementPickup.IsVisible)
                    {
                        if (elementPickup.IsActive)
                        {
                            elementPickup.CheckElementPickupCollision(gameTime);
                        }
                    }
                }

                foreach (ScorePickup scorePickup in scorePickupList)
                {
                    if(scorePickup.IsVisible)
                    {
                        if (scorePickup.IsActive)
                        {
                            scorePickup.UpdateScorePickup();
                        }
                    }
                }

                foreach (ElementWall elementWall in elementWallList)
                {
                    if(elementWall.IsVisible)
                    {
                        if (elementWall.IsActive)
                        {
                            elementWall.ElementWallCollisions();
                        }
                    } 
                }

                foreach (Checkpoint checkpoint in checkpointList)
                {
                    if(checkpoint.IsVisible)
                    {
                        if (checkpoint.IsActive)
                        {
                            checkpoint.CheckCollision();
                        }
                    } 
                }

                if(levelFragment != null)
                {
                    if(levelFragment.IsVisible)
                    {
                        levelFragment.CheckFragmentCollision(gameTime);
                    }
                }

                foreach(Cannon cannon in cannonObstacleList)
                {
                    if(cannon.IsVisible)
                    {
                        if(cannon.IsActive)
                        {
                            cannon.UpdateCannon(gameTime);
                        }
                    }
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
                knightEnemy.SetProperties(position, 100, 100);

                knightEnemy.AddAnimation(knightIdle);
                knightEnemy.AddAnimation(knightWalk);
                knightEnemy.AddAnimation(knightAttack);
                knightEnemy.SetAnimation(0);

                knightEnemy.LoadHealthBar(_gameManager);

                knightEnemy.LayerOrder = -1;
                knightEnemy.playerCharacter = _gameManager.playerCharacter;
                knightEnemy.AttackThreshold = 60;
                knightEnemy.ChaseThreshold = 400;
                knightEnemy.ElementIndex = elementIndex;
                knightEnemy.LoadStatusEffects();
                enemyList.Add(knightEnemy);
                knightEnemy.AttackCooldown = 1;
                knightEnemy.EnemyIndex = 0;
            }
            else if(index == 1) //Dark Mage Character
            {
                Random rand = new Random();
                int elementIndex = rand.Next(0, 2);

                Animation mageIdle = new Animation(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                Animation mageWalk = new Animation(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Walk_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                Animation mageAttack = new Animation(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Attack_SpriteSheet"), 8, 140f, true, new Vector2(64, 64));

                DarkMageCharacter darkMageCharacter = new DarkMageCharacter();
                darkMageCharacter.Initialise(position, _gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), new Vector2(96, 96));
                darkMageCharacter.SetProperties(position, 100, 100);

                darkMageCharacter.AddAnimation(mageIdle);
                darkMageCharacter.AddAnimation(mageWalk);
                darkMageCharacter.AddAnimation(mageAttack);
                darkMageCharacter.SetAnimation(0);

                darkMageCharacter.AttackThreshold = 400;
                darkMageCharacter.ChaseThreshold = 600;

                darkMageCharacter.LoadHealthBar(_gameManager);
                darkMageCharacter.LayerOrder = -1;
                darkMageCharacter.playerCharacter = _gameManager.playerCharacter;
                darkMageCharacter.EnemyIndex = 1;
                darkMageCharacter.AttackCooldown = 3;

                darkMageCharacter.ElementIndex = elementIndex;
                darkMageCharacter.LoadStatusEffects();
                enemyList.Add(darkMageCharacter);
                darkMageCharacter.AttackCooldown = 1;
                darkMageCharacter.EnemyIndex = 0;
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

        public static void SpawnProjectile(Texture2D texture, Vector2 position, Vector2 scale, Vector2 velocity, bool isEnemyProjectile, bool hasAnimation, int projectileTrigger, float projectileDuration)
        {
            if(inactiveProjectileList.Count > 0)
            {
                inactiveProjectileList[0].ResetProjectile(texture, position, scale, velocity, isEnemyProjectile, projectileTrigger, projectileDuration);

                if(hasAnimation)
                {
                    Animation projectileAnimation = new Animation(texture, 4, 120f, true, scale);
                    inactiveProjectileList[0].Animations.Clear();
                    inactiveProjectileList[0].Animations.Add(projectileAnimation);
                    inactiveProjectileList[0].SetAnimation(0);
                }

                activeProjectileList.Add(inactiveProjectileList[0]);
                inactiveProjectileList.RemoveAt(0);
            }
            else
            {
                Projectile projectile = new Projectile();
                projectile.ResetProjectile(texture, position, scale, velocity, isEnemyProjectile, projectileTrigger, projectileDuration);
                projectile.Initialise(position, texture, scale);
                projectile.LayerOrder = -1;

                if (hasAnimation)
                {
                    Animation projectileAnimation = new Animation(texture, 4, 120f, true, scale);
                    projectile.Animations.Add(projectileAnimation);
                    projectile.SetAnimation(0);
                }

                activeProjectileList.Add(projectile);
            }
        }

        public static void SpawnSpikeObstacle(Vector2 position)
        {
            SpikeObstacle spikeObstacle = new SpikeObstacle();
            spikeObstacle.Player = _gameManager.playerCharacter;
            spikeObstacle.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/Spikes"), new Vector2(64, 64));
            spikeObstacleList.Add(spikeObstacle);
        }

        public static void SpawnCannonObstacle(Vector2 position, int cannonDir)
        {
            Cannon cannon = new Cannon(_gameManager, _gameManager.playerCharacter, cannonDir);
            Texture2D cannonTexture = _gameManager.GetTexture("Sprites/Obstacles/Cannon_Right");

            if (cannonDir == 1)
            {
                cannonTexture = _gameManager.GetTexture("Sprites/Obstacles/Cannon_Right");
            }
            else if(cannonDir == -1)
            {
                cannonTexture = _gameManager.GetTexture("Sprites/Obstacles/Cannon_Left");
            }
            

            cannon.Initialise(position - new Vector2(0, 50), cannonTexture, new Vector2(cannonTexture.Width, cannonTexture.Height));
            cannon.LayerOrder = -1;
            cannonObstacleList.Add(cannon);
        }
    }
}
