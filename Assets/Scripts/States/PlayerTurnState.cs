using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerTurnState : State
    {
        private State _nextState;
        private PlayerController _playerController;
        private HandController _handController;
        private DeckController _deckController;
        private Text _turnPanelText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<EnemyTurnState>();
            this._playerController = GameObject.Find("PlayerHand").GetComponent<PlayerController>();
            this._handController = GameObject.Find("PlayerHand").GetComponent<HandController>();
            this._deckController = GameObject.Find("PlayerDeckPanel/PlayerDeck").GetComponent<DeckController>();
            this._turnPanelText = GameObject.Find("TurnPanel/Text").GetComponent<Text>();
        }

        public override void Enter()
        {
            this._turnPanelText.text = "PlayerTurn";
            this._playerController.maxMana += 1;
            this._playerController.mana = this._playerController.maxMana;
            this._handController.AddCard(this._deckController.DrawCard());
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