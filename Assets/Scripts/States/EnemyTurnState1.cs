using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EnemyTurnState1 : State
    {
        private State _nextState;
        private GameStateController _gameState;
        private EnemyDeckController _deckController;
        private Text _stateButtonText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<EnemyAttackState>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._deckController = GameObject.Find("EnemyDeckPanel/EnemyDeck").GetComponent<EnemyDeckController>();
            this._stateButtonText = GameObject.Find("StateButton/Text").GetComponent<Text>();
        }

        public override void Enter()
        {
            this._stateButtonText.text = this.Id();
            this._gameState.enemyMana = this._gameState.enemyManaMax;
        }

        public override void Execute()
        {
            this._deckController.DrawCard();
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
