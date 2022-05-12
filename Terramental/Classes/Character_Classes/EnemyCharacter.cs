using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Terramental
{
    public class EnemyCharacter : BaseCharacter
    {
        /// <summary>
        /// The EnemyCharacter class that handles enemy properties and AI logic for each enemy.
        /// </summary>

        #region Variables

        public enum AIState { Chase, Attack, Idle }; //AI States control the enemy's current actions

        private AIState _currentState; //The active AI state being used

        private Sprite enemyHealthBar; //The enemy's health bar border
        private Sprite enemyHealthBarFill; //The enemy's health bar fill
        private Sprite enemyElement; //Displays the enemy's current element via an icon

        private Texture2D enemyHealthBarTexture;
        private Texture2D enemyHealthBarFillTexture;

        private bool _enemyAttacked; //If the enemy recently attacked, this value is set to true. This value will be set to false when the potential for the enemy to attack is enabled
        private float _attackTimer; //When this value reaches zero, the enemy will attack

        private float _attackThreshold; //The distance required for the enemy to enter their attack state
        private float _chaseThreshold; //If the chase threshold is less than a value, and larger than the attack threshold, then the enemy will enter their chase state
        private float _attackCooldown; //The maximum attack cooldown. This value is assigned to the _attackTimer float variable when the enemy attacks

        private float _enemyMovementSpeed; //The movement speed assigned to the enemy
        private int _enemyIndex; //The current enemy index that changes the attack behaviour. 1 = Knight Enemy, 2 = Dark Mage Enemy
        private int _elementIndex; //The element that the enemy is assigned to. 0 = Fire, 1 = Water, 2 = Snow

        private bool _pathBlocked; //This value changes based on whether the enemy collides with a blocking tile that is to their right or left

        private PlayerCharacter playerCharacter; //Reference to the player character
        private GameManager _gameManager; //Reference to the game manager

        private Vector2 _oldPosition; //The recent position of the enemy in the last tick.

        #endregion

        #region Properties

        //Contains properties for use in other classes

        public AIState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        public float AttackCooldown
        {
            set { _attackCooldown = value; }
        }

        public float AttackThreshold
        {
            set { _attackThreshold = value; }
        }

        public float ChaseThreshold
        {
            set { _chaseThreshold = value; }
        }

        public int ElementIndex
        {
            get
            { return _elementIndex; }
            set
            {
                //Changes the enemy's current element icon texture depending on what element they are assigned to. (Used for resetting/spawning an enemy)
                if (value == 0)
                {
                    enemyElement.SpriteTexture = SpawnManager.gameManager.GetTexture("UserInterface/PlayerInterface/Fire_Element");
                }
                else if (value == 1)
                {
                    enemyElement.SpriteTexture = SpawnManager.gameManager.GetTexture("UserInterface/PlayerInterface/Water_Element");
                }
                else if (value == 2)
                {
                    enemyElement.SpriteTexture = SpawnManager.gameManager.GetTexture("UserInterface/PlayerInterface/Snow_Element");
                }

                _elementIndex = value;
            }
        }

        public int EnemyIndex
        {
            get { return _enemyIndex; }
            set { _enemyIndex = value; }
        }

        #endregion

        #region WorldCanvas

        public void LoadWorldCanvas(GameManager gameManager) //Loads the world canvas, including the enemy's health bar and element icon
        {
            enemyHealthBarTexture = gameManager.GetTexture("UserInterface/Sliders/HealthBarBorder");
            enemyHealthBarFillTexture = gameManager.GetTexture("UserInterface/Sliders/HealthBarFill");

            enemyHealthBar = new Sprite();
            enemyHealthBar.Initialise(SpritePosition + new Vector2(0, -50), enemyHealthBarTexture, new Vector2(CharacterHealth / 2, enemyHealthBarTexture.Height));

            enemyHealthBarFill = new Sprite();
            enemyHealthBarFill.Initialise(enemyHealthBar.SpritePosition, enemyHealthBarFillTexture, new Vector2(enemyHealthBar.SpriteScale.X, enemyHealthBarFillTexture.Height));

            enemyHealthBar.LayerOrder = -1;
            enemyHealthBarFill.LayerOrder = -1;

            enemyElement = new Sprite();
            enemyElement.Initialise(SpritePosition + new Vector2(0, -70), gameManager.GetTexture("UserInterface/PlayerInterface/Fire_Element"), new Vector2(20, 20));
            enemyElement.LayerOrder = -1;
        }

        public void UpdateWorldCanvas()
        {
            if (!Animations[AnimationIndex].MirrorTexture) //Changes the position of the components based on whether the enemy is moving left or right. (Ensures correct centring)
            {
                enemyHealthBar.SpritePosition = SpritePosition + new Vector2(0, -30); //Update the component's position to be above the enemy
                enemyElement.SpritePosition = enemyHealthBar.SpritePosition + new Vector2(60, -5);
            }
            else
            {
                enemyHealthBar.SpritePosition = SpritePosition + new Vector2(48, -30);
                enemyElement.SpritePosition = enemyHealthBar.SpritePosition + new Vector2(60, -5);
            }

            enemyHealthBarFill.SpritePosition = enemyHealthBar.SpritePosition;
            enemyHealthBarFill.SpriteScale = new Vector2(CharacterHealth / 2, enemyHealthBarFill.SpriteScale.Y); //Updates the scale of the health bar fill based on the enemy's current health
        }

        public void EnableWorldCanvas(bool active) //Enables/disables the world canvas components when the enemy is either active or inactive
        {
            enemyHealthBar.IsLoaded = active;
            enemyHealthBarFill.IsLoaded = active;
            enemyElement.IsLoaded = active;
            enemyHealthBar.IsActive = active;
            enemyHealthBarFill.IsActive = active;
            enemyElement.IsActive = active;
        }

        #endregion

        #region EnemyCore

        public void ResetEnemy(Texture2D texture, Vector2 position, Vector2 scale, int enemyMaxHealth, int enemyHealth, float enemyMovementSpeed, float enemyGravity, GameManager gameManager) //Resets the enemy object when a level is loaded or reset
        {
            SpriteTexture = texture;
            SpritePosition = position;
            SpawnPosition = position;
            SpriteScale = scale;
            CharacterMaxHealth = enemyMaxHealth;
            CharacterHealth = enemyHealth;
            _enemyMovementSpeed = enemyMovementSpeed;
            IsActive = true;

            if(enemyHealthBar != null)
            {
                enemyHealthBar.SpritePosition = SpritePosition + new Vector2(0, -50);

                if (enemyHealthBarFill != null)
                {
                    enemyHealthBarFill.SpritePosition = enemyHealthBar.SpritePosition;
                }
            }

            if(enemyElement != null)
            {
                enemyElement.SpritePosition = SpritePosition + new Vector2(0, -70);
            }

            if(_gameManager == null)
            {
                _gameManager = gameManager;
            }
        }

        public void UpdateEnemy(GameTime gameTime)
        {
            EnemyStateMachine();

            //Calls the appropriate function based on enemy's current AIState
            if (CurrentState == AIState.Attack)
            {
                Attack(gameTime);
            }

            if (CurrentState == AIState.Idle)
            {
                Idle();
            }

            if (CurrentState == AIState.Chase)
            {
                if (!DisableMovement)
                {
                    Chase();
                }
            }

            //Movement Function

            MoveIfValid(gameTime);

            SimulateFriction();
            StopMovingIfBlocked();

            if (_currentState == AIState.Idle)
            {
                if (!IsGrounded())
                {
                    ApplyGravity();
                }
            }
            else if (_currentState == AIState.Chase)
            {
                if (!IsGrounded() && !_pathBlocked && !DisableMovement)
                {
                    ApplyGravity();
                }

                if (_pathBlocked && !DisableMovement)
                {
                    Rectangle rectangleOffset = SpriteRectangle;

                    if(!Animations[AnimationIndex].MirrorTexture) //Sets the tile based on whether the enemy is moving left or right
                    {
                        rectangleOffset.Offset(64, -256); //Moving Right
                    }
                    else
                    {
                        rectangleOffset.Offset(-64, -256); //Moving Left
                    }
                    

                    Tile tile = _gameManager.mapManager.FindNearestTile(rectangleOffset); //Only jump if the tile four tiles up and to the enemy's right or left is not blocking

                    if (tile != null)
                    {
                        if(!tile.IsBlocking)
                        {
                            EnemyJump();
                        }
                    }
                }
            }

            MirrorEnemy();

            if(SpritePosition.Y < -(MapManager.mapHeight * 64)) //If the enemy is outside the level bounds due to gravity, disable the enemy
            {
                TakeDamage(CharacterMaxHealth);
            }
        }

        #endregion

        #region AI

        private float DistanceToPlayer() //Measures the distance between the player character's position and the enemy's position
        {
            float distance = MathF.Sqrt(MathF.Pow(playerCharacter.SpritePosition.X - SpritePosition.X, 2) + MathF.Pow(playerCharacter.SpritePosition.Y - SpritePosition.Y, 2));
            return distance;
        }

        private void EnemyStateMachine() //Changes the enemy's current AIState based on the distance between the enemy and the player and the assigned threshold values
        {
            if (DistanceToPlayer() < _attackThreshold)
            {
                CurrentState = AIState.Attack;
            }

            if (DistanceToPlayer() >= _attackThreshold && DistanceToPlayer() < _chaseThreshold)
            {
                CurrentState = AIState.Chase;
            }

            if (DistanceToPlayer() >= _chaseThreshold)
            {
                CurrentState = AIState.Idle;
            }
        }

        private void Attack(GameTime gameTime)
        {
            if (_attackTimer <= 0)
            {
                //Play attack sound based on what enemy is attacking
                if (_enemyIndex == 0)
                {
                    AudioManager.PlaySound("Sword_SFX");
                }
                else if(_enemyIndex == 1)
                {
                    AudioManager.PlaySound("FireProjectile_SFX");
                }

                _attackTimer = _attackCooldown;
                _enemyAttacked = false;
            }
            else
            {
                _attackTimer -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

                //If the attack timer is larger than zero, and the enemy has not attacked yet, perform attack behaviour based on enemy type (index)

                if (OnCollision(playerCharacter.SpriteRectangle) && !_enemyAttacked && _enemyIndex == 0) //Damages the player when collision occurs during attack
                {
                    playerCharacter.PlayerTakeDamage(1);
                    _enemyAttacked = true;
                }

                if (_enemyIndex == 1 && !_enemyAttacked)
                {
                    if(!Animations[AnimationIndex].MirrorTexture) //Fires projectile based on current facing direction
                    {
                        SpawnManager.SpawnProjectile(SpawnManager.gameManager.GetTexture("Sprites/Projectiles/Fireball_Projectile"), SpritePosition + new Vector2(40, 20), new Vector2(40, 40), new Vector2(40, 40), new Vector2(4, 0), true, false, 0, 3, 1);
                        _enemyAttacked = true;
                    }
                    else
                    {
                        SpawnManager.SpawnProjectile(SpawnManager.gameManager.GetTexture("Sprites/Projectiles/Fireball_Projectile"), SpritePosition + new Vector2(-40, 20), new Vector2(40, 40), new Vector2(40, 40), new Vector2(-4, 0), true, false, 0, 3, 1);
                        _enemyAttacked = true;
                    }
                }
            }

            SpriteVelocity = new Vector2(0, SpriteVelocity.Y); //Disable movement on the X axis when the enemy is attacking

            SetAnimation(2); //Set the current animation to attack
        }

        private void Idle()
        {
            SetAnimation(0); //Sets the current animation to idle


            //If the enemy has fallen into a recently activated tile, reset their position (Can be caused by camera occlusion)
            foreach(Tile tile in MapManager.tileList)
            {
                float distance = MathF.Sqrt((MathF.Pow(tile.SpritePosition.X - SpritePosition.X, 2)) + (MathF.Pow(tile.SpritePosition.Y - SpritePosition.Y, 2)));

                if(distance < 64)
                {
                    if (SpritePosition.Y > tile.SpritePosition.Y)
                    {
                        SpritePosition = SpawnPosition;
                    }
                }
            }
        }

        private void Chase()
        {
            Vector2 dir = playerCharacter.SpritePosition - SpritePosition; //Get the direction of the player from origin of the enemy's location
            dir.Normalize();
            SpriteVelocity = new Vector2(dir.X * _enemyMovementSpeed, SpriteVelocity.Y); //Set velocity so the enemy moves towards the player's location

            Vector2 lastMovement = SpritePosition - _oldPosition; //Gets difference between the player's current position and old position

            if (lastMovement.X == 0)
            {
                SetAnimation(0); //If the enemy is not moving, set the current animation to idle
            }
            else
            {
                SetAnimation(1); //If the enemy is moving, set the current animation to walk
            }
        }

        private void MirrorEnemy() //Mirrors the enemy's texture
        {
            if (_currentState != AIState.Attack) //Mirror based on velocity
            {
                if (SpriteVelocity.X >= 0)
                {
                    Animations[AnimationIndex].MirrorTexture = false;
                }
                else
                {
                    Animations[AnimationIndex].MirrorTexture = true;
                }
            }
            else
            {
                if (playerCharacter.SpritePosition.X >= SpritePosition.X) //Mirror based on difference between player X coordinate and enemy X coordinate as the velocity will be zero when the enemy attacks
                {
                    Animations[AnimationIndex].MirrorTexture = false;
                }
                else
                {
                    Animations[AnimationIndex].MirrorTexture = true;
                }
            }
        }

        #endregion

        #region EnemyMovement

        private void UpdateEnemyMovement(GameTime gameTime) //Updates the enemy's position based on velocity overtime for smooth movement
        {
            SpritePosition += SpriteVelocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 15;
        }

        private void ApplyGravity() //Applies a downward force to the player when they are not grounded
        {
            SpriteVelocity += Vector2.UnitY * 0.5f;
        }

        private void SimulateFriction() //Subtracts a small value to simulate friction
        {
            SpriteVelocity -= SpriteVelocity * Vector2.One * 0.075f;
        }

        private bool IsGrounded() //Checks if the player is grounded.
        {
            Rectangle onePixelLower = SpriteRectangle;
            onePixelLower.Offset(0, 1);
            return !_gameManager.mapManager.HasRoomForRectangle(onePixelLower);
        }

        private void MoveIfValid(GameTime gameTime) //Moves the player if the desired location is valid
        {
            _oldPosition = base.SpritePosition;
            UpdateEnemyMovement(gameTime);

            base.SpritePosition = _gameManager.mapManager.FindValidLoaction(_oldPosition, SpritePosition, SpriteRectangle);
        }

        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = SpritePosition - _oldPosition; //Gets difference between the player's current position and old position

            if (lastMovement.X == 0)
            {
                SpriteVelocity *= Vector2.UnitY;

                if (!_pathBlocked && IsGrounded() && _currentState == AIState.Chase) //If the enemy is supposed to be chasing, and is not moving on the X axis and grounded, cause the enemy to jump
                {
                    AudioManager.PlaySound("Jump_SFX");
                    _pathBlocked = true;
                }
            }
            else //If the enemy is moving on the X axis, then the path is not blocked
            {
                _pathBlocked = false;
            }

            if (lastMovement.Y == 0)
            {
                SpriteVelocity *= Vector2.UnitX;
            }
        }

        private void EnemyJump() //Applies a upwards force to simulate jumping
        {
            SpriteVelocity = -Vector2.UnitY * 25.25f;
        }

        #endregion
    }
}
