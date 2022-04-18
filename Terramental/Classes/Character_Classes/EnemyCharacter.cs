using Microsoft.Xna.Framework;

namespace Terramental
{
    public class EnemyCharacter : BaseCharacter
    {
        public PlayerCharacter playerCharacter;

        public enum AIState { Patrol, Chase, Attack, Dead, Idle};

        private AIState _currentState;

        public AIState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }
    }
}
