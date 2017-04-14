using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyTurnState1 : State
    {
        private State _nextState;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<EnemyAttackState>();
        }

        public override void Enter()
        {

        }

        public override void Execute()
        {
			
        }

        public override void Exit()
        {

        }

        public override State NextState()
        {
            return this._nextState;
        }

        public override string Id()
        {
            return "EnemyTurnState1";
        }
    }
}
