using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class MulliganPhase : State
    {
        private State _nextState;
        private GameStateController _gameState;
        private DeckController _playerDeckController;
        private DeckController _enemyDeckController;
        private HandController _playerHandController;
        private HandController _enemyHandController;
        private Text _stateButtonText;
        private Transform _playerMulliganButton;
        private Transform _playerKeepCardsButton;
        private Transform _enemyMulliganButton;
        private Transform _enemyKeepCardsButton;
        private bool _playerButtonClicked = false;
        private bool _enemyButtonClicked = false;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<MainPhase1>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._playerDeckController = GameObject.Find("PlayerDeckPanel/PlayerDeck").GetComponent<DeckController>();
            this._enemyDeckController = GameObject.Find("EnemyDeckPanel/EnemyDeck").GetComponent<DeckController>();
            this._playerHandController = GameObject.Find("PlayerHand").GetComponent<HandController>();
            this._enemyHandController = GameObject.Find("EnemyHand").GetComponent<HandController>();
            this._stateButtonText = GameObject.Find("StateButton/Text").GetComponent<Text>();
            this._playerMulliganButton = GameObject.Find("PlayerMulliganButton").transform;
            this._playerKeepCardsButton = GameObject.Find("PlayerKeepCardsButton").transform;
            this._enemyMulliganButton = GameObject.Find("EnemyMulliganButton").transform;
            this._enemyKeepCardsButton = GameObject.Find("EnemyKeepCardsButton").transform;
        }

        public override void Enter()
        {
            this._stateButtonText.text = this.Id();
        }

        public override void Execute()
        {
            // Draw 7 cards
            for (int i = 0; i < 7; i++)
            {
                this._playerHandController.AddCard(this._playerDeckController.DrawCard());
                this._enemyHandController.AddCard(this._enemyDeckController.DrawCard());
            }
                
        }

        public override void Exit()
        {

        }

        public override State NextState()
        {
            return this._nextState;
        }

        public void PlayerMulligan()
        {
            List<Transform> temp = new List<Transform>();
            foreach (Transform card in this._playerHandController.cards)
            {
                temp.Add(card);
            }

            foreach (Transform card in temp)
            {
                this._playerHandController.RemoveCard(card);
                this._playerDeckController.ReplaceCard(card);
            }

            // Draw 7 cards
            for (int i = 0; i < 7; i++)
            {
                this._playerHandController.AddCard(this._playerDeckController.DrawCard());
            }

            this._playerMulliganButton.gameObject.SetActive(false);
            this._playerKeepCardsButton.gameObject.SetActive(false);

            this._playerButtonClicked = true;
            if (this._enemyButtonClicked)
            {
                this._gameState.ChangeState();
            }
        }

        public void EnemyMulligan()
        {
            List<Transform> temp = new List<Transform>();
            foreach (Transform card in this._enemyHandController.cards)
            {
                temp.Add(card);
            }

            foreach (Transform card in temp)
            {
                this._enemyHandController.RemoveCard(card);
                this._enemyDeckController.ReplaceCard(card);
            }

            // Draw 7 cards
            for (int i = 0; i < 7; i++)
            {
                this._enemyHandController.AddCard(this._enemyDeckController.DrawCard());
            }

            this._enemyMulliganButton.gameObject.SetActive(false);
            this._enemyKeepCardsButton.gameObject.SetActive(false);

            this._enemyButtonClicked = true;
            if (this._playerButtonClicked)
            {
                this._gameState.ChangeState();
            }
        }

        public void PlayerKeepCards()
        {
            this._playerMulliganButton.gameObject.SetActive(false);
            this._playerKeepCardsButton.gameObject.SetActive(false);

            this._playerButtonClicked = true;
            if (this._enemyButtonClicked)
            {
                this._gameState.ChangeState();
            }
        }

        public void EnemyKeepCards()
        {
            this._enemyMulliganButton.gameObject.SetActive(false);
            this._enemyKeepCardsButton.gameObject.SetActive(false);

            this._enemyButtonClicked = true;
            if (this._playerButtonClicked)
            {
                this._gameState.ChangeState();
            }
        }

        public override string Id()
        {
            return "MulliganPhase";
        }
    }
}
