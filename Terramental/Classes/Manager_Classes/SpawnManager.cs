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

        public static List<Projectile> projectileList = new List<Projectile>();

        public static List<EnemyCharacter> enemyList = new List<EnemyCharacter>();
        public static List<HealthPickup> healthPickupList = new List<HealthPickup>();
        public static List<ScorePickup> scorePickupList = new List<ScorePickup>();
        public static List<ElementWall> elementWallList = new List<ElementWall>();
        public static List<ElementPickup> elementPickupList = new List<ElementPickup>();
        public static List<Checkpoint> checkpointList = new List<Checkpoint>();
        public static List<SpikeObstacle> spikeObstacleList = new List<SpikeObstacle>();
        public static List<Cannon> cannonObstacleList = new List<Cannon>();
        public static List<MovingPlatform> movingPlatformList = new List<MovingPlatform>();
        public static List<VisualEffect> vfxList = new List<VisualEffect>();

        public static Fragment levelFragment;

        public static void Update(GameTime gameTime)
        {
            if (GameManager.currentGameState == GameManager.GameState.Level)
            {
                foreach (EnemyCharacter enemy in enemyList)
                {
                    if (enemy.IsVisible)
                    {
                        if (enemy.CharacterHealth > 0)
                        {
                            enemy.UpdateCharacter(gameTime);
                            enemy.UpdateEnemy(gameTime);
                            enemy.UpdateWorldCanvas();
                            enemy.EnableWorldCanvas(true);
                        }
                        else
                        {
                            enemy.EnableWorldCanvas(false);
                        }
                    }
                }

                foreach(VisualEffect vfx in vfxList)
                {
                    if(vfx.IsVisible)
                    {
                        if(vfx.IsActive)
                        {
                            vfx.UpdateVisualEffect(gameTime);
                        }
                    }
                }

                foreach (SpikeObstacle spikeObstacle in spikeObstacleList)
                {
                    if(spikeObstacle.IsVisible && spikeObstacle.IsActive)
                    {
                        spikeObstacle.CheckCollision(gameTime);
                    }
                }

                foreach (Projectile projectile in projectileList)
                {
                    if (projectile.IsVisible)
                    {
                        if (projectile.IsActive)
                        {
                            projectile.UpdateProjectile(gameTime);
                        }
                    }
                }

                foreach (HealthPickup healthPickup in healthPickupList)
                {
                    if (healthPickup.IsVisible)
                    {
                        if (healthPickup.IsActive)
                        {
                            healthPickup.CheckHealthPickupCollision();
                        }
                    }
                }

                foreach (ElementPickup elementPickup in elementPickupList)
                {
                    if (elementPickup.IsVisible)
                    {
                        if (elementPickup.IsActive)
                        {
                            elementPickup.CheckElementPickupCollision(gameTime);
                        }
                    }
                }

                foreach (ScorePickup scorePickup in scorePickupList)
                {
                    if (scorePickup.IsVisible)
                    {
                        if (scorePickup.IsActive)
                        {
                            scorePickup.UpdateScorePickup();
                        }
                    }
                }

                foreach (Checkpoint checkpoint in checkpointList)
                {
                    if (checkpoint.IsVisible)
                    {
                        if (checkpoint.IsActive)
                        {
                            checkpoint.CheckCollision();
                        }
                    }
                }

                if (levelFragment != null)
                {
                    if (levelFragment.IsVisible)
                    {
                        if (levelFragment.IsActive)
                        {
                            levelFragment.CheckFragmentCollision(gameTime);
                        }
                    }
                }

                foreach (Cannon cannon in cannonObstacleList)
                {
                    if (cannon.IsVisible)
                    {
                        if (cannon.IsActive)
                        {
                            cannon.UpdateCannon(gameTime);
                        }
                    }
                }

                foreach (MovingPlatform movingPlatform in movingPlatformList)
                {
                    if (movingPlatform.IsVisible)
                    {
                        if (movingPlatform.IsActive)
                        {
                            movingPlatform.UpdateMovingPlatform(gameTime);
                        }
                    }
                }
            }

        }

        #region Visual Effects

        public static VisualEffect SpawnAnimatedVFX(Texture2D texture, Vector2 positionOffset, Vector2 scale, float vfxDuration, int frameCount, float frameDuration, Sprite attachSprite)
        {
            bool vfxFound = false;

            foreach(VisualEffect vfx in vfxList)
            {
                if(!vfx.IsActive)
                {
                    if(vfx.Animations.Count > 0)
                    {
                        vfx.Animations[0].SpriteSheet = texture;
                        vfx.Animations[0].FrameDimensions = scale;
                        vfx.Animations[0].FrameCount = frameCount;
                        vfx.Animations[0].FrameDuration = frameDuration;
                        vfx.Animations[0].AnimationActive = true;
                    }
                    else
                    {
                        Animation newAnimation = new Animation(texture, frameCount, frameDuration, true, scale);
                        vfx.Animations.Add(newAnimation);
                    }

                    vfx.InitialiseVFX(attachSprite, positionOffset, vfxDuration);

                    vfxFound = true;
                    return vfx;
                }
            }

            if(!vfxFound)
            {
                VisualEffect visualEffect = new VisualEffect();
                visualEffect.Initialise(attachSprite.SpritePosition + positionOffset, texture, scale);
                visualEffect.InitialiseVFX(vfxDuration);
                visualEffect.LayerOrder = -2;
                Animation newAnimation = new Animation(texture, frameCount, frameDuration, true, scale);
                visualEffect.Animations.Add(newAnimation);

                vfxList.Add(visualEffect);
                return visualEffect;
            }

            return null;
        }

        public static VisualEffect SpawnStaticVFX(Texture2D texture, Vector2 positionOffset, Vector2 scale, float vfxDuration, Sprite attachSprite)
        {
            bool vfxFound = false;

            foreach (VisualEffect vfx in vfxList)
            {
                if (!vfx.IsActive)
                {
                    if (vfx.Animations.Count > 0)
                    {
                        vfx.Animations[0].FrameDimensions = scale;
                        vfx.Animations[0].AnimationActive = false;
                    }

                    vfxFound = true;
                    return vfx;
                }
            }

            if (!vfxFound)
            {
                VisualEffect visualEffect = new VisualEffect();
                visualEffect.Initialise(attachSprite.SpritePosition + positionOffset, texture, scale);
                visualEffect.LayerOrder = -2;

                vfxList.Add(visualEffect);
                return visualEffect;
            }

            return null;
        }

        #endregion

        public static void SpawnEnemy(int index, Vector2 position, int elementIndex)
        {
            bool enemyFound = false;

            foreach (EnemyCharacter enemy in enemyList)
            {
                if (!enemy.IsActive)
                {
                    if (index == 0)
                    {
                        enemy.ResetEnemy(_gameManager.GetTexture("Sprites/Enemies/Knight/KnightCharacter_Sprite_Default"), position, new Vector2(96, 96), 100, 100, 4, 2, _gameManager);

                        if (index != enemy.EnemyIndex)
                        {
                            enemy.Animations[0].SpriteSheet = _gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Idle_SpriteSheet");
                            enemy.Animations[0].FrameCount = 4;
                            enemy.Animations[0].FrameDuration = 250f;
                            enemy.Animations[0].LoopActive = true;
                            enemy.Animations[0].FrameDimensions = new Vector2(96, 96);

                            enemy.Animations[1].SpriteSheet = _gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Walk_SpriteSheet");
                            enemy.Animations[1].FrameCount = 7;
                            enemy.Animations[1].FrameDuration = 120f;
                            enemy.Animations[1].LoopActive = true;
                            enemy.Animations[1].FrameDimensions = new Vector2(96, 96);

                            enemy.Animations[2].SpriteSheet = _gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Attack_SpriteSheet");
                            enemy.Animations[2].FrameCount = 7;
                            enemy.Animations[2].FrameDuration = 150f;
                            enemy.Animations[2].LoopActive = true;
                            enemy.Animations[2].FrameDimensions = new Vector2(96, 96);

                            enemy.EnemyIndex = 0;
                            enemy.AttackThreshold = 60;
                            enemy.ChaseThreshold = 400;
                            enemy.AttackCooldown = 1;
                            enemy.ElementIndex = elementIndex;
                            enemy.AttackCooldown = 1;
                        }
                    }
                    else if (index == 1)
                    {
                        enemy.ResetEnemy(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), position, new Vector2(96, 96), 100, 100, 2, 2, _gameManager);

                        if (index != enemy.EnemyIndex)
                        {
                            enemy.Animations[0].SpriteSheet = _gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet");
                            enemy.Animations[0].FrameCount = 4;
                            enemy.Animations[0].FrameDuration = 250f;
                            enemy.Animations[0].LoopActive = true;
                            enemy.Animations[0].FrameDimensions = new Vector2(96, 96);

                            enemy.Animations[1].SpriteSheet = _gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Walk_SpriteSheet");
                            enemy.Animations[1].FrameCount = 4;
                            enemy.Animations[1].FrameDuration = 250f;
                            enemy.Animations[1].LoopActive = true;
                            enemy.Animations[1].FrameDimensions = new Vector2(96, 96);

                            enemy.Animations[2].SpriteSheet = _gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Attack_SpriteSheet");
                            enemy.Animations[2].FrameCount = 12;
                            enemy.Animations[2].FrameDuration = 80f;
                            enemy.Animations[2].LoopActive = true;
                            enemy.Animations[2].FrameDimensions = new Vector2(96, 96);

                            enemy.EnemyIndex = 1;
                            enemy.AttackThreshold = 400;
                            enemy.ChaseThreshold = 600;
                            enemy.AttackCooldown = 3;
                            enemy.ElementIndex = elementIndex;
                            enemy.AttackCooldown = 1;

                        }
                    }

                    enemyFound = true;
                    break;
                }
            }

            if (!enemyFound)
            {
                if (index == 0) //Knight Enemy Character
                {
                    Animation knightIdle = new Animation(_gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Idle_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                    Animation knightWalk = new Animation(_gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Walk_SpriteSheet"), 7, 120f, true, new Vector2(96, 96));
                    Animation knightAttack = new Animation(_gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Attack_SpriteSheet"), 7, 150f, true, new Vector2(96, 96));

                    EnemyCharacter knightEnemy = new EnemyCharacter();
                    knightEnemy.Initialise(position + new Vector2(0, -32), _gameManager.GetTexture("Sprites/Enemies/Knight/KnightCharacter_Sprite_Default"), new Vector2(96, 96));
                    knightEnemy.SetProperties(position, 100, 100);
                    knightEnemy.ResetEnemy(_gameManager.GetTexture("Sprites/Enemies/Knight/KnightCharacter_Sprite_Default"), position, new Vector2(96, 96), 100, 100, 4, 2, _gameManager);

                    knightEnemy.AddAnimation(knightIdle);
                    knightEnemy.AddAnimation(knightWalk);
                    knightEnemy.AddAnimation(knightAttack);
                    knightEnemy.SetAnimation(0);
                    
                    knightEnemy.LoadWorldCanvas(_gameManager);
                    knightEnemy.LayerOrder = -1;
                    knightEnemy.playerCharacter = _gameManager.playerCharacter;
                    knightEnemy.AttackThreshold = 60;
                    knightEnemy.ChaseThreshold = 400;
                    knightEnemy.ElementIndex = elementIndex;
                    enemyList.Add(knightEnemy);
                    knightEnemy.AttackCooldown = 1;
                    knightEnemy.EnemyIndex = 0;
                }
                else if (index == 1) //Dark Mage Character
                {
                    Animation mageIdle = new Animation(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                    Animation mageWalk = new Animation(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Walk_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                    Animation mageAttack = new Animation(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Attack_SpriteSheet"), 12, 80f, true, new Vector2(96, 96));

                    EnemyCharacter darkMageCharacter = new EnemyCharacter();
                    darkMageCharacter.Initialise(position, _gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), new Vector2(96, 96));
                    darkMageCharacter.SetProperties(position, 100, 100);
                    darkMageCharacter.ResetEnemy(_gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), position, new Vector2(96, 96), 100, 100, 2, 2, _gameManager);

                    darkMageCharacter.AddAnimation(mageIdle);
                    darkMageCharacter.AddAnimation(mageWalk);
                    darkMageCharacter.AddAnimation(mageAttack);
                    darkMageCharacter.SetAnimation(0);

                    darkMageCharacter.AttackThreshold = 400;
                    darkMageCharacter.ChaseThreshold = 600;

                    darkMageCharacter.LoadWorldCanvas(_gameManager);
                    darkMageCharacter.LayerOrder = -1;
                    darkMageCharacter.playerCharacter = _gameManager.playerCharacter;
                    darkMageCharacter.EnemyIndex = 1;
                    darkMageCharacter.AttackCooldown = 3;

                    darkMageCharacter.ElementIndex = elementIndex;
                    enemyList.Add(darkMageCharacter);
                    darkMageCharacter.AttackCooldown = 1;
                    darkMageCharacter.EnemyIndex = 1;
                }
            }
        }

        public static void SpawnHealthPickup(Vector2 position)
        {
            bool healthPickupFound = false;

            foreach(HealthPickup healthPickup in healthPickupList)
            {
                if(!healthPickup.IsActive)
                {
                    healthPickup.SpritePosition = position;
                    healthPickup.SpawnPosition = position;
                    healthPickup.ResetPickup(position);
                    healthPickup.IsActive = true;
                    healthPickupFound = true;
                    break;
                }
            }
            
            if(!healthPickupFound)
            {
                HealthPickup healthPickup = new HealthPickup(_gameManager.playerCharacter, 1);
                healthPickup.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Health_Pickup"), new Vector2(64, 64));
                healthPickup.LayerOrder = -2;
                healthPickupList.Add(healthPickup);
            }
        }

        public static void SpawnScorePickup(Vector2 position)
        {
            bool scorePickupFound = false;

            foreach(ScorePickup scorePickup in scorePickupList)
            {
                if(!scorePickup.IsActive)
                {
                    scorePickup.ResetPickup(position);
                    scorePickup.SpritePosition = position;
                    scorePickup.SpawnPosition = position;
                    scorePickup.IsActive = true;
                    scorePickupFound = true;
                    break;
                }
            }
            
            if(!scorePickupFound)
            {
                ScorePickup scorePickup = new ScorePickup(_gameManager.playerCharacter);
                scorePickup.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Collectible"), new Vector2(64, 64));
                scorePickup.LayerOrder = -2;
                scorePickupList.Add(scorePickup);
            } 
        }

        public static void SpawnElementPickup(int elementIndex, Vector2 position)
        {
            bool elementPickupFound = false;

            foreach(ElementPickup elementPickup in elementPickupList)
            {
                if(!elementPickup.IsActive)
                {
                    elementPickup.ElementIndex = elementIndex;
                    elementPickup.ResetElementPickup();
                    elementPickup.ResetPickup(position);
                    elementPickup.SpritePosition = position;
                    elementPickup.SpawnPosition = position;
                    elementPickup.IsActive = true;
                    elementPickupFound = true;
                    break;
                }
            }

            if(!elementPickupFound)
            {
                ElementPickup elementPickup = new ElementPickup(elementIndex, _gameManager.GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), _gameManager.GetTexture("Sprites/Pickups/WaterPickup_SpriteSheet"), _gameManager.GetTexture("Sprites/Pickups/SnowPickup_SpriteSheet"), _gameManager.playerCharacter);
                elementPickup.Initialise(new Vector2(position.X, position.Y), _gameManager.GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), new Vector2(64, 64));
                elementPickup.LayerOrder = -1;
                elementPickupList.Add(elementPickup);
            }
        }

        public static void SpawnElementWall(int elementIndex, Vector2 position, MapManager mapManager)
        {
            bool elementWallFound = false;

            foreach(ElementWall elementWall in elementWallList)
            {
                if(!elementWall.IsActive)
                {
                    switch (elementIndex)
                    {
                        case 0:
                            elementWall.LoadWallTextures(_gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame1"), _gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame2"), _gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame3"));
                            break;
                        case 1:
                            elementWall.LoadWallTextures(_gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame1"), _gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame2"), _gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame3"));
                            break;
                        case 2:
                            elementWall.LoadWallTextures(_gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame1"), _gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame2"), _gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame3"));
                            break;
                    }

                    elementWall.SpritePosition = position;
                    elementWall.SpawnPosition = position;
                    elementWall.IsActive = true;
                    elementWallFound = true;

                    elementWall.InitialiseElementWall(_gameManager.playerCharacter, _gameManager.mapManager, elementIndex);
                    elementWall.ElementWallCollision();
                    break;
                }
            }

            if(!elementWallFound)
            {
                ElementWall elementWall = new ElementWall();

                switch (elementIndex)
                {
                    case 0:
                        elementWall.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame1"), new Vector2(64, 64));
                        elementWall.LoadWallTextures(_gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame1"), _gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame2"), _gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame3"));
                        break;
                    case 1:
                        elementWall.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/WaterTile"), new Vector2(64, 64));
                        elementWall.LoadWallTextures(_gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame1"), _gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame2"), _gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame3"));
                        break;
                    case 2:
                        elementWall.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame1"), new Vector2(64, 64));
                        elementWall.LoadWallTextures(_gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame1"), _gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame2"), _gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame3"));
                        break;
                }

                elementWall.InitialiseElementWall(_gameManager.playerCharacter, mapManager, elementIndex);
                elementWall.ElementWallCollision();

                elementWall.LayerOrder = -2;
                elementWallList.Add(elementWall);
            }
        }

        public static void SpawnLevelEnd(Vector2 position)
        {
            if(levelFragment == null)
            {
                levelFragment = new Fragment(_gameManager.menuManager, _gameManager.playerCharacter);
                levelFragment.Initialise(position, _gameManager.GetTexture("Sprites/Pickups/Camp_Fire"), new Vector2(64, 64));
                Animation levelEndAnim = new Animation(_gameManager.GetTexture("Sprites/Pickups/Camp_Fire"), 4, 120f, true, new Vector2(64, 64));
                levelFragment.AddAnimation(levelEndAnim);

                levelFragment.LayerOrder = -2;
            }
            else
            {
                levelFragment.SpritePosition = position;
                levelFragment.SpawnPosition = position;
                levelFragment.IsActive = true;
            }
            
        }

        public static void SpawnProjectile(Texture2D texture, Vector2 position, Vector2 scale, Vector2 frameDimensions, Vector2 velocity, bool isEnemyProjectile, bool hasAnimation, int projectileTrigger, float projectileDuration, int projectileDamage)
        {
            bool projectileFound = false;

            foreach(Projectile projectile in projectileList)
            {
                if(!projectile.IsActive)
                {
                    projectile.ResetProjectile(texture, position, scale, velocity, isEnemyProjectile, projectileTrigger, projectileDuration, projectileDamage);

                    if(projectile.Animations.Count > 0)
                    {
                        projectile.Animations[0].FrameDimensions = frameDimensions;
                        projectile.Animations[0].AnimationActive = false;
                    }

                    if(hasAnimation)
                    {
                        if(projectile.Animations.Count > 0)
                        {
                            projectile.Animations[0].SpriteSheet = texture;
                            projectile.Animations[0].FrameCount = 4;
                            projectile.Animations[0].FrameDuration = 120f;
                            projectile.Animations[0].LoopActive = true;
                            projectile.Animations[0].FrameDimensions = frameDimensions;
                            projectile.Animations[0].AnimationActive = true;
                        }
                        else
                        {
                            Animation projectileAnimation = new Animation(texture, 4, 120f, true, scale);
                            projectile.Animations.Add(projectileAnimation);
                            projectile.SetAnimation(0);
                        }
                    }

                    projectileFound = true;
                    break;
                }
            }

            if(!projectileFound)
            {
                Projectile projectile = new Projectile();
                projectile.ResetProjectile(texture, position, scale, velocity, isEnemyProjectile, projectileTrigger, projectileDuration, projectileDamage);
                projectile.Initialise(position, texture, scale);
                projectile.LayerOrder = -1;

                if (hasAnimation)
                {
                    Animation projectileAnimation = new Animation(texture, 4, 120f, true, frameDimensions);
                    projectile.Animations.Add(projectileAnimation);
                    projectile.SetAnimation(0);
                }

                projectileList.Add(projectile);
            }
        }

        public static void SpawnSpikeObstacle(Vector2 position)
        {
            bool spikeObstacleFound = false;

            foreach(SpikeObstacle spikeObstacle in spikeObstacleList)
            {
                if(!spikeObstacle.IsActive)
                {
                    spikeObstacle.SpawnPosition = position;
                    spikeObstacle.SpritePosition = position;
                    spikeObstacle.IsActive = true;
                    spikeObstacle.ResetPickup(position);
                    spikeObstacleFound = true;
                    break;
                }
            }

            if(!spikeObstacleFound)
            {
                SpikeObstacle spikeObstacle = new SpikeObstacle();
                spikeObstacle.Player = _gameManager.playerCharacter;
                spikeObstacle.LayerOrder = -2;
                spikeObstacle.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/Spikes"), new Vector2(64, 64));
                spikeObstacleList.Add(spikeObstacle);
            }
        }

        public static void SpawnCannonObstacle(Vector2 position, bool faceRight)
        {
            bool cannonObstacleFound = false;

            foreach(Cannon cannon in cannonObstacleList)
            {
                if(!cannon.IsActive)
                {
                    cannon.SpritePosition = position;
                    cannon.SpawnPosition = position;
                    cannon.FaceRight = faceRight;
                    cannon.IsActive = true;
                    cannonObstacleFound = true;
                    break;
                }
            }

            if(!cannonObstacleFound)
            {
                Cannon cannon = new Cannon(_gameManager, _gameManager.playerCharacter, faceRight);
                Texture2D cannonTexture = _gameManager.GetTexture("Sprites/Obstacles/Cannon_Right");

                if(faceRight)
                {
                    cannonTexture = _gameManager.GetTexture("Sprites/Obstacles/Cannon_Right");
                }
                else
                {
                    cannonTexture = _gameManager.GetTexture("Sprites/Obstacles/Cannon_Left");
                }

                cannon.Initialise(position - new Vector2(0, 50), cannonTexture, new Vector2(cannonTexture.Width, cannonTexture.Height));
                cannon.LayerOrder = -1;
                cannonObstacleList.Add(cannon);
            }
        }

        public static void SpawnMovingPlatform(Vector2 position, MapManager mapManager)
        {
            MovingPlatform movingPlatform = new MovingPlatform(_gameManager.playerCharacter, mapManager, position, 0);
            movingPlatform.Initialise(position, _gameManager.GetTexture("Sprites/Obstacles/SnowTile"), new Vector2(64, 64));
            movingPlatform.LayerOrder = -2;

            movingPlatformList.Add(movingPlatform);
        }

        public static void ResetEntities()
        {
            foreach (EnemyCharacter enemy in enemyList)
            {
                enemy.IsActive = true;
                enemy.SpritePosition = enemy.SpawnPosition;
                break;
            }

            foreach (ElementPickup elementPickup in elementPickupList)
            {
                elementPickup.IsActive = true;
                elementPickup.SpritePosition = elementPickup.SpawnPosition;
                elementPickup.ResetElementPickup();
                break;
            }

            foreach (ElementWall elementWall in elementWallList)
            {
                elementWall.IsActive = true;
                elementWall.SpritePosition = elementWall.SpawnPosition;
                break;
            }

            foreach (HealthPickup healthPickup in healthPickupList)
            {
                healthPickup.IsActive = true;
                healthPickup.SpritePosition = healthPickup.SpawnPosition;
                break;
            }

            foreach (ScorePickup scorePickup in scorePickupList)
            {
                scorePickup.IsActive = true;
                scorePickup.SpritePosition = scorePickup.SpawnPosition;
                break;
            }

            foreach (Checkpoint checkpoint in checkpointList)
            {
                checkpoint.IsActive = true;
                checkpoint.SpritePosition = checkpoint.SpawnPosition;
                break;
            }

            foreach (SpikeObstacle spikeObstacle in spikeObstacleList)
            {
                spikeObstacle.IsActive = true;
                spikeObstacle.SpritePosition = spikeObstacle.SpawnPosition;
                break;
            }

            foreach (Cannon cannon in cannonObstacleList)
            {
                cannon.IsActive = true;
                cannon.SpritePosition = cannon.SpawnPosition;
                break;
            }

            levelFragment.IsActive = true;
        }

        public static void UnloadEntities()
        {
            foreach (EnemyCharacter enemy in enemyList)
            {
                enemy.IsActive = false;
            }

            foreach (ElementPickup elementPickup in elementPickupList)
            {
                elementPickup.IsActive = false;
            }

            foreach(ElementWall elementWall in elementWallList)
            {
                elementWall.IsActive = false;
            }

            foreach (HealthPickup healthPickup in healthPickupList)
            {
                healthPickup.IsActive = false;
            }

            foreach (ScorePickup scorePickup in scorePickupList)
            {
                scorePickup.IsActive = false;
            }

            foreach (Checkpoint checkpoint in checkpointList)
            {
                checkpoint.IsActive = false;
            }

            foreach (SpikeObstacle spikeObstacle in spikeObstacleList)
            {
                spikeObstacle.IsActive = false;
            }

            foreach (Cannon cannon in cannonObstacleList)
            {
                cannon.IsActive = false;
            }

            foreach (Projectile projectile in projectileList)
            {
                projectile.IsActive = false;
            }

            levelFragment.IsActive = false;
        }
    }
}
