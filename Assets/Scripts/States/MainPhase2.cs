using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MainPhase2 : State
    {
        private State _nextState;
        private GameStateController _gameState;
        private Battlefield _battlefield;
        private HandController _playerHandController;
        private HandController _enemyHandController;
        private Text _phasePanelText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<MainPhase1>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._playerHandController = GameObject.Find("PlayerHand").GetComponent<HandController>();
            this._enemyHandController = GameObject.Find("EnemyHand").GetComponent<HandController>();
            this._phasePanelText = GameObject.Find("PhasePanel/Text").GetComponent<Text>();
        }

        public override void Enter()
        {
            this._phasePanelText.text = "MainPhase2";
            this._gameState.ColorPlayableAndMovableCards();
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            if (this._gameState.turnState.Id() == "PlayerTurnState")
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cardController = card.gameObject.GetComponent<CardController>();
                    if (cardController.ownedBy == Owner.PLAYER)
                    {
                        cardController.canMove = false;
                    }
                }
                foreach (Transform card in this._playerHandController.cards)
                {
                    CardController cardController = card.gameObject.GetComponent<CardController>();
                    cardController.canMove = false;
                }
            }
            else
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cardController = card.gameObject.GetComponent<CardController>();
                    if (cardController.ownedBy == Owner.ENEMY)
                    {
                        cardController.canMove = false;
                    }
                }
                foreach (Transform card in this._enemyHandController.cards)
                {
                    CardController cardController = card.gameObject.GetComponent<CardController>();
                    cardController.canMove = false;
                }
            }
            this._gameState.ResetCardColors();
        }

        public override State NextState()
        {
            return this._nextState;
        }

        public override string Id()
        {
            return "MainPhase2";
        }
    }
}