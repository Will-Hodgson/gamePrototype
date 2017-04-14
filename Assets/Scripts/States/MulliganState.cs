using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class MulliganState : State
    {
        private State _nextState;
        private GameStateController _gameState;
        private PlayerDeckController _playerDeckController;
        private EnemyDeckController _enemyDeckController;
        private Transform _playerHand;
        private Transform _enemyHand;
        private Text _stateButtonText;
        private Transform _playerMulliganButton;
        private Transform _playerKeepCardsButton;
        private Transform _enemyMulliganButton;
        private Transform _enemyKeepCardsButton;
        private bool _playerButtonClicked = false;
        private bool _enemyButtonClicked = false;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<PlayerTurnState1>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._playerDeckController = GameObject.Find("PlayerDeckPanel/PlayerDeck").GetComponent<PlayerDeckController>();
            this._enemyDeckController = GameObject.Find("EnemyDeckPanel/EnemyDeck").GetComponent<EnemyDeckController>();
            this._playerHand = GameObject.Find("PlayerHand").transform;
            this._enemyHand = GameObject.Find("EnemyHand").transform;
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
                this._playerDeckController.DrawCard();
                this._enemyDeckController.DrawCard();
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
            foreach (Transform card in this._playerHand)
            {
                temp.Add(card);
            }

            foreach (Transform card in temp)
            {
                this._playerDeckController.ReplaceCard(card);
            }

            // Draw 7 cards
            for (int i = 0; i < 7; i++)
            {
                this._playerDeckController.DrawCard();
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
            foreach (Transform card in this._enemyHand)
            {
                temp.Add(card);
            }

            foreach (Transform card in temp)
            {
                this._enemyDeckController.ReplaceCard(card);
            }

            // Draw 7 cards
            for (int i = 0; i < 7; i++)
            {
                this._enemyDeckController.DrawCard();
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
            return "MulliganState";
        }
    }
}
