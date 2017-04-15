using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MainPhase1 : State
    {
        private State _nextState;
        private GameStateController _gameState;
        private Battlefield _battlefield;
        private DeckController _playerDeckController;
        private HandController _playerHandController;
        private DeckController _enemyDeckController;
        private HandController _enemyHandController;
        private Text _stateButtonText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<AttackPhase>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._playerDeckController = GameObject.Find("PlayerDeckPanel/PlayerDeck").GetComponent<DeckController>();
            this._playerHandController = GameObject.Find("PlayerHand").GetComponent<HandController>();
            this._enemyDeckController = GameObject.Find("EnemyDeckPanel/EnemyDeck").GetComponent<DeckController>();
            this._enemyHandController = GameObject.Find("EnemyHand").GetComponent<HandController>();
            this._stateButtonText = GameObject.Find("StateButton/Text").GetComponent<Text>();
        }

        public override void Enter()
        {
            this._stateButtonText.text = this.Id();
            if (this._gameState.turnState.Id() == "PlayerTurnState")
            {
                this._gameState.playerMana = this._gameState.playerManaMax;
                this._playerHandController.AddCard(this._playerDeckController.DrawCard());
            }
            else
            {
                this._gameState.enemyMana = this._gameState.enemyManaMax;
                this._enemyHandController.AddCard(this._enemyDeckController.DrawCard());
            }
        }

        public override void Execute()
        {
            if (this._gameState.turnState.Id() == "PlayerTurnState")
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cardController = card.gameObject.GetComponent<CardController>();
                    if (cardController.ownedBy == Owner.PLAYER)
                    {
                        cardController.canMove = true;
                    }
                }
                foreach (Transform card in this._playerHandController.cards)
                {
                    CardController cardController = card.gameObject.GetComponent<CardController>();
                    cardController.canMove = true;
                }
            }
            else
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cardController = card.gameObject.GetComponent<CardController>();
                    if (cardController.ownedBy == Owner.ENEMY)
                    {
                        cardController.canMove = true;
                    }
                }
                foreach (Transform card in this._enemyHandController.cards)
                {
                    CardController cardController = card.gameObject.GetComponent<CardController>();
                    cardController.canMove = true;
                }
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
            return "MainPhase1";
        }
    }
}