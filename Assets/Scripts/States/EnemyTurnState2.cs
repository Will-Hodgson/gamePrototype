using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EnemyTurnState2 : State
    {
        private State _nextState;
        private GameStateController _gameState;
        private Battlefield _battlefield;
        private Transform _enemyHand;
        private Text _stateButtonText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<PlayerTurnState1>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._enemyHand = GameObject.Find("EnemyHand").transform;
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
            foreach (Transform card in this._battlefield.cards)
            {
                CardController cardController = card.gameObject.GetComponent<CardController>();
                if (cardController.ownedBy == Owner.ENEMY)
                {
                    cardController.canMove = false;
                }
            }
            foreach (Transform card in this._enemyHand)
            {
                CardController cardController = card.gameObject.GetComponent<CardController>();
                cardController.canMove = false;
            }
        }

        public override State NextState()
        {
            return this._nextState;
        }

        public override string Id()
        {
            return "EnemyTurnState2";
        }
    }
}
