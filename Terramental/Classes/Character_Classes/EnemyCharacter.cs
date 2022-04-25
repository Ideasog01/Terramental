using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terramental
{
    public class EnemyCharacter : BaseCharacter
    {
        public PlayerCharacter playerCharacter;

        public enum AIState { Patrol, Chase, Attack, Dead, Idle};

        private AIState _currentState;

        private Sprite enemyHealthBar;
        private Sprite enemyHealthBarFill;

        private Texture2D enemyHealthBarTexture;
        private Texture2D enemyHealthBarFillTexture;

        public AIState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        public void LoadHealthBar(GameManager gameManager)
        {
            enemyHealthBarTexture = gameManager.GetTexture("UserInterface/Sliders/HealthBarBorder");
            enemyHealthBarFillTexture = gameManager.GetTexture("UserInterface/Sliders/HealthBarFill");

            enemyHealthBar = new Sprite();
            enemyHealthBar.Initialise(SpritePosition + new Vector2(0, -50), enemyHealthBarTexture, new Vector2(CharacterHealth / 2, enemyHealthBarTexture.Height));

            enemyHealthBarFill = new Sprite();
            enemyHealthBarFill.Initialise(enemyHealthBar.SpritePosition, enemyHealthBarFillTexture, new Vector2(enemyHealthBar.SpriteScale.X, enemyHealthBarFillTexture.Height));

            enemyHealthBar.LayerOrder = -1;
            enemyHealthBarFill.LayerOrder = -1;
        }

        public void UpdateHealthBar()
        {
           
            if(!Animations[AnimationIndex].MirrorTexture)
            {
                enemyHealthBar.SpritePosition = SpritePosition + new Vector2(0, -30);
            }
            else
            {
                enemyHealthBar.SpritePosition = SpritePosition + new Vector2(48, -30);
            }

            enemyHealthBarFill.SpritePosition = enemyHealthBar.SpritePosition;


            enemyHealthBarFill.SpriteScale = new Vector2(CharacterHealth / 2, enemyHealthBarFill.SpriteScale.Y);
        }

        public void EnableHealthBar(bool active)
        {
            enemyHealthBar.IsActive = active;
            enemyHealthBarFill.IsActive = active;
        }
    }
}
