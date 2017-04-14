using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerTurnState1 : State
    {
        private State _nextState;
        private GameStateController _gameState;
        private PlayerDeckController _deckController;
        private Text _stateButtonText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<PlayerAttackState>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._deckController = GameObject.Find("PlayerDeckPanel/PlayerDeck").GetComponent<PlayerDeckController>();
            this._stateButtonText = GameObject.Find("StateButton/Text").GetComponent<Text>();
        }

        public override void Enter()
        {
            this._stateButtonText.text = this.Id();
            this._gameState.playerMana = this._gameState.playerManaMax;
        }

        public override void Execute()
        {
            //Draw a card then display playable/movable cards
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
            return "PlayerTurnState1";
        }
    }
}
