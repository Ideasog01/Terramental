using Microsoft.Xna.Framework;

namespace Terramental
{
    public class EnemyCharacter : BaseCharacter
    {
        public PlayerCharacter playerCharacter;

        public enum AIState { Patrol, Chase, Attack, Dead, Idle};

        private AIState _currentState;

        private Sprite _healthBar;
        private Sprite _healthBarFill;

        public AIState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        public void UpdateEnemyHealthBar()
        {

        }

        public void LoadEnemyHealthBar(GameManager gameManager)
        {
            _healthBar = new Sprite();
            _healthBar.Initialise(SpritePosition + new Vector2(SpriteRectangle.X / 2, 20), gameManager.GetTexture("UserInterface/Slider/HealthBarBorder"), new Vector2(100, 10));

            _healthBarFill = new Sprite();
            _healthBarFill.Initialise(SpritePosition + new Vector2(SpriteRectangle.X / 2, 20), gameManager.GetTexture("UserInterface/Slider/HealthBarFill"), new Vector2(100, 10));
        }
    }
}
