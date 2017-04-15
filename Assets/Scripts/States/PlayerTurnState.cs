using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerTurnState : State
    {
        private State _nextState;
        private Text _stateButtonText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<EnemyTurnState>();
            this._stateButtonText = GameObject.Find("StateButton/Text").GetComponent<Text>();
        }

        public override void Enter()
        {
            this._stateButtonText.text = this.Id();
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
            return "PlayerTurnState";
        }
    }
}