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
        private Text _phasePanelText;
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
            this._phasePanelText = GameObject.Find("PhasePanel/Text").GetComponent<Text>();
            this._playerMulliganButton = GameObject.Find("PlayerMulliganButton").transform;
            this._playerKeepCardsButton = GameObject.Find("PlayerKeepCardsButton").transform;
            this._enemyMulliganButton = GameObject.Find("EnemyMulliganButton").transform;
            this._enemyKeepCardsButton = GameObject.Find("EnemyKeepCardsButton").transform;
        }

        public override void Enter()
        {
            this._phasePanelText.text = "MulliganPhase";
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
            this.Mulligan("Player");
        }

        public void EnemyMulligan()
        {
            this.Mulligan("Enemy");
        }

        private void Mulligan(string player)
        {
            List<Transform> temp = new List<Transform>();
            HandController handController = this._enemyHandController;
            DeckController deckController = this._enemyDeckController;
            if (player == "Player")
            {
                handController = this._playerHandController;
                deckController = this._playerDeckController;
            }
            foreach (Transform card in handController.cards)
            {
                temp.Add(card);
            }

            foreach (Transform card in temp)
            {
                handController.RemoveCard(card);
                deckController.ReplaceCard(card);
            }

            // Draw 7 cards
            for (int i = 0; i < 7; i++)
            {
                handController.AddCard(deckController.DrawCard());
            }

            this.ClickButton(player);
        }

        public void PlayerKeepCards()
        {
            this.ClickButton("Player");
        }

        public void EnemyKeepCards()
        {
            this.ClickButton("Enemy");
        }
            
        private void ClickButton(string player)
        {
            Transform mulliganButton = this._enemyMulliganButton;
            Transform keepCardsButton = this._enemyKeepCardsButton;
            bool buttonClicked = this._enemyButtonClicked;
            bool otherButtonClicked = this._playerButtonClicked;
            if (player == "Player")
            {
                mulliganButton = this._playerMulliganButton;
                keepCardsButton = this._playerKeepCardsButton;
                buttonClicked = this._playerButtonClicked;
                otherButtonClicked = this._enemyButtonClicked;
            }
            mulliganButton.gameObject.SetActive(false);
            keepCardsButton.gameObject.SetActive(false);

            buttonClicked = true;
            if (otherButtonClicked)
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
