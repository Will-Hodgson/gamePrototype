using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerTurnState1 : State
    {
        private State _nextState;
        private GameStateController _gameState;
        private Battlefield _battlefield;
        private PlayerDeckController _deckController;
        private Transform _playerHand;
        private Text _stateButtonText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<PlayerAttackState>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._deckController = GameObject.Find("PlayerDeckPanel/PlayerDeck").GetComponent<PlayerDeckController>();
            this._playerHand = GameObject.Find("PlayerHand").transform;
            this._stateButtonText = GameObject.Find("StateButton/Text").GetComponent<Text>();
        }

        public override void Enter()
        {
            this._stateButtonText.text = this.Id();
            this._gameState.playerMana = this._gameState.playerManaMax;
            this._deckController.DrawCard();
        }

        public override void Execute()
        {
            foreach (Transform card in this._battlefield.cards)
            {
                CardController cardController = card.gameObject.GetComponent<CardController>();
                if (cardController.ownedBy == Owner.PLAYER)
                {
                    cardController.canMove = true;
                }
            }
            foreach (Transform card in this._playerHand)
            {
                CardController cardController = card.gameObject.GetComponent<CardController>();
                cardController.canMove = true;
            }
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
            return "PlayerTurnState1";
        }
    }
}
