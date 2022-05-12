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

        public static GameManager gameManager; //Static reference to GameManager, can be used in other classes if a reference is not necessary

        public static Vector2 levelStartPosition; //The start location in the level for the player

        //Stores each entity in the appropriate list
        public static List<Projectile> projectileList = new List<Projectile>();
        public static List<EnemyCharacter> enemyList = new List<EnemyCharacter>();
        public static List<HealthPickup> healthPickupList = new List<HealthPickup>();
        public static List<ScorePickup> scorePickupList = new List<ScorePickup>();
        public static List<ElementWall> elementWallList = new List<ElementWall>();
        public static List<ElementPickup> elementPickupList = new List<ElementPickup>();
        public static List<SpikeObstacle> spikeObstacleList = new List<SpikeObstacle>();
        public static List<Cannon> cannonObstacleList = new List<Cannon>();
        public static List<VisualEffect> vfxList = new List<VisualEffect>();
        public static List<MovingPlatform> movingPlatformList = new List<MovingPlatform>();

        public static Fragment levelFragment; //The pickup that ends the level when collision occurs. This entity will reset if it has previously been instantiated

        public static int enemyCount; //The number of enemies active at the start of the level
        public static int gemCount; //The number of gems active at the start of the level

        public static void Update(GameTime gameTime)
        {
            if (GameManager.currentGameState == GameManager.GameState.Level) // Checks to see if the game is in the level state (in game)
            {
                foreach (EnemyCharacter enemy in enemyList) // Loops through list of enemies
                {
                    if(enemy.IsLoaded)
                    {
                        if (enemy.IsVisible) // Checks to see if the enemy is on screen
                        {
                            if (enemy.CharacterHealth > 0) // Checks to see if the enemy has health. If this condition is true, update the enemy accordingly
                            {
                                enemy.UpdateCharacter(gameTime);
                                enemy.UpdateEnemy(gameTime);
                                enemy.UpdateWorldCanvas();
                                enemy.EnableWorldCanvas(true);
                            }
                            else
                            {
                                enemy.EnableWorldCanvas(false); //If the enemy is not active, disable the world canvas
                            }
                        }
                    }
                }

                foreach(VisualEffect vfx in vfxList) // Loops through every VFX is the VFX list
                {
                    if(vfx.IsLoaded)
                    {
                        if (vfx.IsVisible) // Checks to see if the effect is visible
                        {
                            if (vfx.IsActive) // Checks to see if the effect is active
                            {
                                vfx.UpdateVisualEffect(gameTime);
                            }
                        }
                    }
                }

                foreach (SpikeObstacle spikeObstacle in spikeObstacleList) // Loops through every spike obstacle in the spike lsit
                {
                    if(spikeObstacle.IsLoaded)
                    {
                        if (spikeObstacle.IsVisible && spikeObstacle.IsActive) // Checks to see if the spike is visible and active
                        {
                            spikeObstacle.CheckCollision(gameTime);
                        }
                    }
                }

                foreach (Projectile projectile in projectileList) // Loops through every projectile in the projectile list
                {
                    if(projectile.IsLoaded)
                    {
                        if (projectile.IsVisible) // Checks if the projectile is visible
                        {
                            if (projectile.IsActive) // Checks if the projectile is active
                            {
                                projectile.UpdateProjectile(gameTime);
                            }
                        }
                    }

                }

                foreach (HealthPickup healthPickup in healthPickupList) // Loops through every health pickup in the health pickup list
                {
                    if(healthPickup.IsLoaded)
                    {
                        if (healthPickup.IsVisible) // Checks if the health pickup is visible
                        {
                            if (healthPickup.IsActive) // Checks if the health pickup is active
                            {
                                healthPickup.CheckHealthPickupCollision();
                            }
                        }
                    }
                }

                foreach (ElementPickup elementPickup in elementPickupList) // Loops through every element pickup in the list
                {
                    if(elementPickup.IsLoaded)
                    {
                        if (elementPickup.IsVisible) // Checks if the element pickup is visible
                        {
                            if (elementPickup.IsActive) // Checks if the element pickup is active
                            {
                                elementPickup.CheckElementPickupCollision(gameTime);
                            }
                        }
                    }
                }

                foreach (ScorePickup scorePickup in scorePickupList) // Loops through each score pickup in the list
                {
                    if(scorePickup.IsLoaded)
                    {
                        if (scorePickup.IsVisible) // Checks if the score pickup is visible
                        {
                            if (scorePickup.IsActive) // Checks to see if the score pickup is active
                            {
                                scorePickup.UpdateScorePickup();
                            }
                        }
                    }
                }

                if (levelFragment != null) // Checks to see that there is an end to the level
                {
                    if (levelFragment.IsVisible) // Checks if the fragment is visible
                    {
                        if (levelFragment.IsActive) // Checks if the fragment is active
                        {
                            levelFragment.CheckFragmentCollision(gameTime);
                        }
                    }
                }

                foreach (Cannon cannon in cannonObstacleList) // Loops through every cannon in the cannon list
                {
                    if(cannon.IsLoaded)
                    {
                        if (cannon.IsVisible) // Checks if the cannon is visible
                        {
                            if (cannon.IsActive) // Checks if the cannon is active
                            {
                                cannon.UpdateCannon(gameTime);
                            }
                        }
                    }
                }

                foreach (MovingPlatform movingPlatform in movingPlatformList) // Loops through every moving platform in the moving platform list
                {
                    if(movingPlatform.IsLoaded)
                    {
                        if (movingPlatform.IsVisible) //Checks if the moving platform is visible
                        {
                            if (movingPlatform.IsActive) // Checks if the moving platform is active
                            {
                                movingPlatform.UpdateMovingPlatform(gameTime);
                            }
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
                if(!vfx.IsActive) // Checks that the effect is not active
                {
                    if(vfx.Animations.Count > 0) //Checks that the number of animations is greater than 0
                    {
                        vfx.Animations[0].SpriteSheet = texture;
                        vfx.Animations[0].FrameDimensions = scale;
                        vfx.Animations[0].FrameCount = frameCount;
                        vfx.Animations[0].FrameDuration = frameDuration;
                        vfx.Animations[0].AnimationActive = true;
                        vfx.IsActive = true;
                    }
                    else
                    {
                        Animation newAnimation = new Animation(texture, frameCount, frameDuration, true, scale); // Creates a new animation
                        vfx.Animations.Add(newAnimation); // Adds the animation to the animation list
                    }

                    vfx.InitialiseVFX(attachSprite, positionOffset, vfxDuration); // Initialises the effect

                    vfx.SpritePosition = attachSprite.AttachSpriteOffset;
                    vfx.SpriteScale = scale;
                    vfxFound = true;
                    vfx.IsLoaded = true;
                    return vfx;
                }
            }

            if(!vfxFound) // Checks to see that no effect has been found
            {
                VisualEffect visualEffect = new VisualEffect(); // Creates a new effect
                visualEffect.Initialise(attachSprite.SpritePosition + positionOffset, texture, scale); // Initialises the effect
                visualEffect.InitialiseVFX(vfxDuration); // Initialises the effect for the specified amount of time
                visualEffect.LayerOrder = -2; // Changes the layer
                Animation newAnimation = new Animation(texture, frameCount, frameDuration, true, scale); // Creates a new animation
                visualEffect.Animations.Add(newAnimation); // Adds the animation to the animation list

                vfxList.Add(visualEffect); // Adds the effect to the list of visual effects
                visualEffect.IsLoaded = true;
                return visualEffect;
            }

            return null;
        }

        public static VisualEffect SpawnStaticVFX(Texture2D texture, Vector2 positionOffset, Vector2 scale, float vfxDuration, Sprite attachSprite) // Same as SpawnAnimatedVFX but for image effects instead of animated effects
        {
            bool vfxFound = false;

            foreach (VisualEffect vfx in vfxList)
            {
                if (!vfx.IsActive)
                {
                    if (vfx.Animations.Count > 0)
                    {
                        vfx.Animations[0].SpriteSheet = texture;
                        vfx.Animations[0].FrameDimensions = scale;
                        vfx.Animations[0].AnimationActive = true;
                    }

                    vfx.IsLoaded = true;
                    vfxFound = true;
                    return vfx;
                }
            }

            if (!vfxFound)
            {
                VisualEffect visualEffect = new VisualEffect();
                visualEffect.Initialise(attachSprite.SpritePosition + positionOffset, texture, scale);
                visualEffect.LayerOrder = -3;
                visualEffect.IsLoaded = true;
                vfxList.Add(visualEffect);
                return visualEffect;
            }

            return null;
        }

        public static void SpawnVisualEffectAtPosition(Texture2D texture, Vector2 position, Vector2 scale, float vfxDuration, int frameCount, float frameDuration) //Spawns the visual effect at a given position instead of attaching it to an existing sprite
        {
            bool vfxFound = false;

            foreach (VisualEffect vfx in vfxList)
            {
                if (!vfx.IsActive)
                {
                    if (vfx.Animations.Count > 0)
                    {
                        vfx.Animations[0].SpriteSheet = texture;
                        vfx.Animations[0].FrameDimensions = scale;
                        vfx.Animations[0].FrameCount = frameCount;
                        vfx.Animations[0].FrameDuration = frameDuration;
                        vfx.Animations[0].AnimationActive = true;
                        vfx.IsActive = true;
                    }
                    else
                    {
                        Animation newAnimation = new Animation(texture, frameCount, frameDuration, true, scale);
                        vfx.Animations.Add(newAnimation);
                    }

                    vfx.SpritePosition = position;
                    vfx.SpriteScale = scale;
                    vfx.IsLoaded = true;
                    vfx.InitialiseVFX(vfxDuration);

                    vfxFound = true;
                }
            }

            if (!vfxFound)
            {
                VisualEffect visualEffect = new VisualEffect();
                visualEffect.Initialise(position, texture, scale);
                visualEffect.InitialiseVFX(vfxDuration);
                visualEffect.LayerOrder = -3;
                Animation newAnimation = new Animation(texture, frameCount, frameDuration, true, scale);
                visualEffect.Animations.Add(newAnimation);
                visualEffect.IsLoaded = true;
                vfxList.Add(visualEffect);
            }
        }

        #endregion

        public static void SpawnEnemy(int index, Vector2 position, int elementIndex)
        {
            bool enemyFound = false;

            foreach (EnemyCharacter enemy in enemyList) // Loops through every enemy in the enemy list
            {
                if (!enemy.IsActive)
                {
                    if (index == 0) // Type of enemy
                    {
                        enemy.ResetEnemy(gameManager.GetTexture("Sprites/Enemies/Knight/KnightCharacter_Sprite_Default"), position, new Vector2(96, 96), 100, 100, 4, 2, gameManager);

                        if (index != enemy.EnemyIndex)
                        {
                            enemy.Animations[0].SpriteSheet = gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Idle_SpriteSheet"); // First animation (Idle)
                            enemy.Animations[0].FrameCount = 4;
                            enemy.Animations[0].FrameDuration = 250f;
                            enemy.Animations[0].LoopActive = true;
                            enemy.Animations[0].FrameDimensions = new Vector2(96, 96);

                            enemy.Animations[1].SpriteSheet = gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Walk_SpriteSheet"); // Second animation (Walk)
                            enemy.Animations[1].FrameCount = 7;
                            enemy.Animations[1].FrameDuration = 100f;
                            enemy.Animations[1].LoopActive = true;
                            enemy.Animations[1].FrameDimensions = new Vector2(96, 96);

                            enemy.Animations[2].SpriteSheet = gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Attack_SpriteSheet"); // Third animation (Attack)
                            enemy.Animations[2].FrameCount = 7;
                            enemy.Animations[2].FrameDuration = 150f;
                            enemy.Animations[2].LoopActive = true;
                            enemy.Animations[2].FrameDimensions = new Vector2(96, 96);

                            enemy.EnemyIndex = 0;
                            enemy.AttackThreshold = 60;
                            enemy.ChaseThreshold = 900;
                            enemy.AttackCooldown = 1;
                            enemy.ElementIndex = elementIndex;
                        }

                        enemy.IsLoaded = true;
                    }
                    else if (index == 1)
                    {
                        enemy.ResetEnemy(gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), position, new Vector2(96, 96), 100, 100, 2, 2, gameManager);

                        if (index != enemy.EnemyIndex)
                        {
                            enemy.Animations[0].SpriteSheet = gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet");
                            enemy.Animations[0].FrameCount = 4;
                            enemy.Animations[0].FrameDuration = 250f;
                            enemy.Animations[0].LoopActive = true;
                            enemy.Animations[0].FrameDimensions = new Vector2(96, 96);

                            enemy.Animations[1].SpriteSheet = gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Walk_SpriteSheet");
                            enemy.Animations[1].FrameCount = 4;
                            enemy.Animations[1].FrameDuration = 250f;
                            enemy.Animations[1].LoopActive = true;
                            enemy.Animations[1].FrameDimensions = new Vector2(96, 96);

                            enemy.Animations[2].SpriteSheet = gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Attack_SpriteSheet");
                            enemy.Animations[2].FrameCount = 12;
                            enemy.Animations[2].FrameDuration = 150f;
                            enemy.Animations[2].LoopActive = true;
                            enemy.Animations[2].FrameDimensions = new Vector2(96, 96);

                            enemy.EnemyIndex = 1;
                            enemy.AttackThreshold = 300;
                            enemy.ChaseThreshold = 900;
                            enemy.AttackCooldown = 3;
                            enemy.ElementIndex = elementIndex;

                        }

                        enemy.IsLoaded = true;
                    }

                    enemyFound = true;
                    break;
                }
            }

            if (!enemyFound)
            {
                if (index == 0) //Knight Enemy Character
                {
                    Animation knightIdle = new Animation(gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Idle_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                    Animation knightWalk = new Animation(gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Walk_SpriteSheet"), 7, 120f, true, new Vector2(96, 96));
                    Animation knightAttack = new Animation(gameManager.GetTexture("Sprites/Enemies/Knight/Knight_Character_Attack_SpriteSheet"), 7, 150f, true, new Vector2(96, 96));

                    EnemyCharacter knightEnemy = new EnemyCharacter();
                    knightEnemy.Initialise(position + new Vector2(0, -32), gameManager.GetTexture("Sprites/Enemies/Knight/KnightCharacter_Sprite_Default"), new Vector2(96, 96));
                    knightEnemy.SetProperties(position, 100, 100);
                    knightEnemy.ResetEnemy(gameManager.GetTexture("Sprites/Enemies/Knight/KnightCharacter_Sprite_Default"), position, new Vector2(96, 96), 100, 100, 4, 2, gameManager);

                    knightEnemy.AddAnimation(knightIdle);
                    knightEnemy.AddAnimation(knightWalk);
                    knightEnemy.AddAnimation(knightAttack);
                    knightEnemy.SetAnimation(0);
                    
                    knightEnemy.LoadWorldCanvas(gameManager);
                    knightEnemy.LayerOrder = -1;
                    knightEnemy.playerCharacter = gameManager.playerCharacter;
                    knightEnemy.AttackThreshold = 60; // Distance to attack the player
                    knightEnemy.ChaseThreshold = 900; // Distance to chase the player
                    knightEnemy.ElementIndex = elementIndex;
                    knightEnemy.AttackCooldown = 1; // Cooldown between attacks
                    knightEnemy.EnemyIndex = 0;
                    knightEnemy.IsLoaded = true;
                    enemyList.Add(knightEnemy);
                }
                else if (index == 1) //Dark Mage Character
                {
                    Animation mageIdle = new Animation(gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                    Animation mageWalk = new Animation(gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Walk_SpriteSheet"), 4, 250f, true, new Vector2(96, 96));
                    Animation mageAttack = new Animation(gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Attack_SpriteSheet"), 12, 100f, true, new Vector2(96, 96));

                    EnemyCharacter darkMageCharacter = new EnemyCharacter();
                    darkMageCharacter.Initialise(position, gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), new Vector2(96, 96));
                    darkMageCharacter.SetProperties(position, 100, 100);
                    darkMageCharacter.ResetEnemy(gameManager.GetTexture("Sprites/Enemies/DarkMage/DarkMage_Idle_SpriteSheet"), position, new Vector2(96, 96), 100, 100, 2, 2, gameManager);

                    darkMageCharacter.AddAnimation(mageIdle);
                    darkMageCharacter.AddAnimation(mageWalk);
                    darkMageCharacter.AddAnimation(mageAttack);
                    darkMageCharacter.SetAnimation(0);

                    darkMageCharacter.AttackThreshold = 300; // Distance to attack the player
                    darkMageCharacter.ChaseThreshold = 900; // Distance to chase the player

                    darkMageCharacter.LoadWorldCanvas(gameManager);
                    darkMageCharacter.LayerOrder = -1;
                    darkMageCharacter.playerCharacter = gameManager.playerCharacter;
                    darkMageCharacter.EnemyIndex = 1;
                    darkMageCharacter.AttackCooldown = 3; // Cooldown between attacks

                    darkMageCharacter.ElementIndex = elementIndex;
                    darkMageCharacter.IsLoaded = true;
                    darkMageCharacter.EnemyIndex = 1;
                    enemyList.Add(darkMageCharacter);
                }
            }

            enemyCount++;
        }

        public static void SpawnHealthPickup(Vector2 position) // Code that runs when spawning a health pickup 
        {
            bool healthPickupFound = false;

            foreach(HealthPickup healthPickup in healthPickupList)
            {
                if(!healthPickup.IsActive) // Checks to see that the health pick up is not active
                {
                    healthPickup.SpritePosition = position; // Sets the position of the health pickup
                    healthPickup.SpawnPosition = position; // Sets the spawn position of the position
                    healthPickup.ResetPickup(position);
                    healthPickup.IsActive = true; // Enables the health pickup
                    healthPickupFound = true;
                    healthPickup.IsLoaded = true;
                    break;
                }
            }
            
            if(!healthPickupFound) // Checks to see if no health pickup has been found
            {
                HealthPickup healthPickup = new HealthPickup(gameManager.playerCharacter, 1); // Creates a new health pickup
                healthPickup.Initialise(position, gameManager.GetTexture("Sprites/Pickups/Health_Pickup"), new Vector2(64, 64)); // Initialises
                healthPickup.LayerOrder = -2; // Sets the layer
                healthPickup.IsLoaded = true;
                healthPickupList.Add(healthPickup); // Adds the health pickup to the list of health pickups
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
                    scorePickup.IsLoaded = true;
                    break;
                }
            }
            
            if(!scorePickupFound)
            {
                ScorePickup scorePickup = new ScorePickup(gameManager.playerCharacter);
                scorePickup.Initialise(position, gameManager.GetTexture("Sprites/Pickups/Collectible"), new Vector2(64, 64));
                scorePickup.LayerOrder = -2;
                scorePickup.IsLoaded = true;
                scorePickupList.Add(scorePickup);
            }

            gemCount++;
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
                    elementPickup.IsLoaded = true;
                    break;
                }
            }

            if(!elementPickupFound)
            {
                ElementPickup elementPickup = new ElementPickup(elementIndex, gameManager.GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), gameManager.GetTexture("Sprites/Pickups/WaterPickup_SpriteSheet"), gameManager.GetTexture("Sprites/Pickups/SnowPickup_SpriteSheet"), gameManager.playerCharacter);
                elementPickup.Initialise(new Vector2(position.X, position.Y), gameManager.GetTexture("Sprites/Pickups/FirePickup_SpriteSheet"), new Vector2(64, 64));
                elementPickup.LayerOrder = -1;
                elementPickup.IsLoaded = true;
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
                    switch (elementIndex) // Checks to see what element the wall is set to
                    {
                        case 0: // Fire wall
                            elementWall.LoadWallTextures(gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame1"), gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame2"), gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame3"));
                            break;
                        case 1: // Water wall
                            elementWall.LoadWallTextures(gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame1"), gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame2"), gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame3"));
                            break;
                        case 2: // Snow wall
                            elementWall.LoadWallTextures(gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame1"), gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame2"), gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame3"));
                            break;
                    }

                    elementWall.SpritePosition = position;
                    elementWall.SpawnPosition = position;
                    elementWall.IsActive = true;
                    elementWall.IsLoaded = true;
                    elementWallFound = true;

                    elementWall.InitialiseElementWall(gameManager.playerCharacter, gameManager.mapManager, elementIndex);
                    elementWall.ElementWallCollision();
                    break;
                }
            }

            if(!elementWallFound)
            {
                ElementWall elementWall = new ElementWall();

                switch (elementIndex)
                {
                    case 0: // Fire wall
                        elementWall.Initialise(position, gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame1"), new Vector2(64, 64));
                        elementWall.LoadWallTextures(gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame1"), gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame2"), gameManager.GetTexture("Sprites/Obstacles/FireWall_Frame3"));
                        break;
                    case 1: // Water wall
                        elementWall.Initialise(position, gameManager.GetTexture("Sprites/Obstacles/WaterTile"), new Vector2(64, 64));
                        elementWall.LoadWallTextures(gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame1"), gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame2"), gameManager.GetTexture("Sprites/Obstacles/WaterWall_Frame3"));
                        break;
                    case 2: // Snow wall
                        elementWall.Initialise(position, gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame1"), new Vector2(64, 64));
                        elementWall.LoadWallTextures(gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame1"), gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame2"), gameManager.GetTexture("Sprites/Obstacles/SnowWall_Frame3"));
                        break;
                }

                elementWall.InitialiseElementWall(gameManager.playerCharacter, mapManager, elementIndex);
                elementWall.ElementWallCollision();
                elementWall.IsLoaded = true;
                elementWall.LayerOrder = -2;
                elementWallList.Add(elementWall); // Adds the elemental wall to the list of elemental walls
            }
        }

        public static void SpawnLevelEnd(Vector2 position)
        {
            if(levelFragment == null) // Checks to see that there is no level end
            {
                levelFragment = new Fragment(gameManager.menuManager, gameManager.playerCharacter); // Creates a level end object
                levelFragment.Initialise(position, gameManager.GetTexture("Sprites/Pickups/Camp_Fire"), new Vector2(64, 64)); // Initialises the object
                Animation levelEndAnim = new Animation(gameManager.GetTexture("Sprites/Pickups/Camp_Fire"), 4, 120f, true, new Vector2(64, 64)); // Adds animation to the level end object
                levelFragment.AddAnimation(levelEndAnim);
                levelFragment.LayerOrder = -2;
                levelFragment.IsLoaded = true;
                levelFragment.IsActive = false;
            }
            else
            {
                levelFragment.SpritePosition = position;
                levelFragment.SpawnPosition = position;
                levelFragment.IsActive = false;
                levelFragment.IsLoaded = true;
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

                    if(projectile.Animations.Count > 0) // Checks that the projectile has more than one animation
                    {
                        projectile.Animations[0].FrameDimensions = frameDimensions;
                        projectile.Animations[0].AnimationActive = false;
                    }

                    if(hasAnimation)
                    {
                        if(projectile.Animations.Count > 0) // Checks that the projectile has more than one animation
                        {
                            // Sets initial values for projectile animations
                            projectile.Animations[0].SpriteSheet = texture;
                            projectile.Animations[0].FrameCount = 4;
                            projectile.Animations[0].FrameDuration = 120f;
                            projectile.Animations[0].LoopActive = true;
                            projectile.Animations[0].FrameDimensions = frameDimensions;
                            projectile.Animations[0].AnimationActive = true;
                        }
                        else
                        {
                            Animation projectileAnimation = new Animation(texture, 4, 120f, true, scale); // Creates a new animation
                            projectile.Animations.Add(projectileAnimation); // Adds the animation to the projectile object
                            projectile.SetAnimation(0);
                        }
                    }

                    projectile.IsLoaded = true;
                    projectileFound = true;
                    break;
                }
            }

            if(!projectileFound) // Checks whether there is no projectile
            {
                Projectile projectile = new Projectile(); // Creates a new projectile
                projectile.ResetProjectile(texture, position, scale, velocity, isEnemyProjectile, projectileTrigger, projectileDuration, projectileDamage);
                projectile.Initialise(position, texture, scale);
                projectile.LayerOrder = -1;

                if (hasAnimation)
                {
                    Animation projectileAnimation = new Animation(texture, 4, 120f, true, frameDimensions);
                    projectile.Animations.Add(projectileAnimation);
                    projectile.SetAnimation(0);
                }

                projectile.IsLoaded = true;
                projectileList.Add(projectile);
            }
        }

        public static void SpawnSpikeObstacle(Vector2 position)
        {
            bool spikeObstacleFound = false;

            foreach(SpikeObstacle spikeObstacle in spikeObstacleList)
            {
                if(!spikeObstacle.IsActive) // Checks to see if the spike obstacle is not active
                {
                    spikeObstacle.SpawnPosition = position;
                    spikeObstacle.SpritePosition = position;
                    spikeObstacle.IsActive = true;
                    spikeObstacle.IsLoaded = true;
                    spikeObstacle.ResetPickup(position);
                    spikeObstacleFound = true;
                    break;
                }
            }

            if(!spikeObstacleFound)
            {
                SpikeObstacle spikeObstacle = new SpikeObstacle();
                spikeObstacle.Player = gameManager.playerCharacter;
                spikeObstacle.SpawnPosition = position;
                spikeObstacle.LayerOrder = -2;
                spikeObstacle.Initialise(position, gameManager.GetTexture("Sprites/Obstacles/Spikes"), new Vector2(64, 64));
                spikeObstacle.IsLoaded = true;
                spikeObstacleList.Add(spikeObstacle); // Adds the spike obstacle to the list of spikes
            }
        }

        public static void SpawnCannonObstacle(Vector2 position, bool faceRight)
        {
            bool cannonObstacleFound = false;

            foreach(Cannon cannon in cannonObstacleList)
            {
                if(!cannon.IsLoaded)
                {
                    Texture2D cannonTexture; // Gets the cannon texture

                    if (faceRight) // Checks to see if it is a right or left facing cannon
                    {
                        cannonTexture = gameManager.GetTexture("Sprites/Obstacles/Cannon_Right"); // Right facing cannon
                    }
                    else
                    {
                        cannonTexture = gameManager.GetTexture("Sprites/Obstacles/Cannon_Left"); // Left facing cannon
                    }

                    cannon.SpriteTexture = cannonTexture;

                    cannon.SpritePosition = position - new Vector2(0, 50);
                    cannon.SpawnPosition = position - new Vector2(0, 50);
                    cannon.FaceRight = faceRight;
                    cannon.IsActive = true;
                    cannon.IsLoaded = true;
                    cannonObstacleFound = true;
                    break;
                }
            }

            if(!cannonObstacleFound)
            {
                Cannon cannon = new Cannon(gameManager, gameManager.playerCharacter, faceRight);
                Texture2D cannonTexture; // Gets the cannon texture

                if(faceRight) // Checks to see if it is a right or left facing cannon
                {
                    cannonTexture = gameManager.GetTexture("Sprites/Obstacles/Cannon_Right"); // Right facing cannon
                }
                else
                {
                    cannonTexture = gameManager.GetTexture("Sprites/Obstacles/Cannon_Left"); // Left facing cannon
                }

                cannon.Initialise(position - new Vector2(0, 50), cannonTexture, new Vector2(cannonTexture.Width, cannonTexture.Height));
                cannon.LayerOrder = -1;
                cannon.IsLoaded = true;
                cannonObstacleList.Add(cannon);
            }
        }

        public static void SpawnMovingPlatform(Vector2 position, MapManager mapManager)
        {
            bool movingPlatformFound = false;

            foreach(MovingPlatform movingPlatform in movingPlatformList)
            {
                if(!movingPlatform.IsLoaded)
                {
                    movingPlatform.InitialiseMovingPlatform(position, 0);
                    movingPlatform.SpritePosition = position;
                    movingPlatform.SpawnPosition = position;
                    movingPlatform.IsActive = true;
                    movingPlatformFound = true;
                    movingPlatform.IsLoaded = true;
                    break;
                }
            }

            if(!movingPlatformFound)
            {
                MovingPlatform movingPlatform = new MovingPlatform();
                movingPlatform.Initialise(position, gameManager.GetTexture("Sprites/Obstacles/SnowTile"), new Vector2(64, 64));
                movingPlatform.InitialiseMovingPlatform(gameManager.playerCharacter, mapManager, position, 0);
                movingPlatform.LayerOrder = -2;
                movingPlatform.IsLoaded = true;
                movingPlatformList.Add(movingPlatform);
            }
        }

        public static void ResetEntities() // Used to set entities back to their starting positions
        {
            foreach(Projectile projectile in projectileList)
            {
                if(projectile.IsLoaded)
                {
                    projectile.IsActive = false;
                }
            }

            foreach (EnemyCharacter enemy in enemyList)
            {
                if(enemy.IsLoaded)
                {
                    enemy.CharacterHealth = enemy.CharacterMaxHealth;
                    enemy.IsActive = true;
                    enemy.SpritePosition = enemy.SpawnPosition;
                }
            }

            foreach (ElementPickup elementPickup in elementPickupList)
            {
                if(elementPickup.IsLoaded)
                {
                    elementPickup.IsActive = true;
                    elementPickup.SpritePosition = elementPickup.SpawnPosition;
                    elementPickup.ResetElementPickup();
                }
            }

            foreach (ElementWall elementWall in elementWallList)
            {
                if(elementWall.IsLoaded)
                {
                    elementWall.ResetWall(gameManager.mapManager);
                }
            }

            foreach (HealthPickup healthPickup in healthPickupList)
            {
                if(healthPickup.IsLoaded)
                {
                    healthPickup.IsActive = true;
                    healthPickup.SpritePosition = healthPickup.SpawnPosition;
                }
            }

            foreach (ScorePickup scorePickup in scorePickupList)
            {
                if(scorePickup.IsLoaded)
                {
                    scorePickup.IsActive = true;
                    scorePickup.SpritePosition = scorePickup.SpawnPosition;
                }
            }

            foreach (SpikeObstacle spikeObstacle in spikeObstacleList)
            {
                if(spikeObstacle.IsLoaded)
                {
                    spikeObstacle.IsActive = true;
                    spikeObstacle.SpritePosition = spikeObstacle.SpawnPosition;
                }
            }

            foreach (Cannon cannon in cannonObstacleList)
            {
                if (cannon.IsLoaded)
                {
                    cannon.IsActive = true;
                    cannon.SpritePosition = cannon.SpawnPosition;
                }
            }

            foreach(MovingPlatform movingPlatform in movingPlatformList)
            {
                if(movingPlatform.IsLoaded)
                {
                    movingPlatform.IsActive = true;
                    movingPlatform.InitialiseMovingPlatform(movingPlatform.SpawnPosition, 0);
                }
            }

            levelFragment.IsActive = false;
        }

        public static void UnloadEntities() // Used to deactivate entities
        {
            foreach (EnemyCharacter enemy in enemyList)
            {
                enemy.IsActive = false;
                enemy.IsLoaded = false;
            }

            foreach (ElementPickup elementPickup in elementPickupList)
            {
                elementPickup.IsActive = false;
                elementPickup.IsLoaded = false;
            }

            foreach(ElementWall elementWall in elementWallList)
            {
                elementWall.IsActive = false;
                elementWall.IsLoaded = false;
            }

            foreach (HealthPickup healthPickup in healthPickupList)
            {
                healthPickup.IsActive = false;
                healthPickup.IsLoaded = false;
            }

            foreach (ScorePickup scorePickup in scorePickupList)
            {
                scorePickup.IsActive = false;
                scorePickup.IsLoaded = false;
            }

            foreach (SpikeObstacle spikeObstacle in spikeObstacleList)
            {
                spikeObstacle.IsActive = false;
                spikeObstacle.IsLoaded = false;
            }

            foreach (Cannon cannon in cannonObstacleList)
            {
                cannon.IsActive = false;
                cannon.IsLoaded = false;
            }

            foreach (Projectile projectile in projectileList)
            {
                projectile.IsActive = false;
                projectile.IsLoaded = false;
            }

            foreach(MovingPlatform movingPlatform in movingPlatformList)
            {
                movingPlatform.IsActive = false;
                movingPlatform.IsLoaded = false;
            }

            levelFragment.IsActive = false;
            GameManager.levelLoaded = false;
            gemCount = 0;
            enemyCount = 0;
        }
    }
}
