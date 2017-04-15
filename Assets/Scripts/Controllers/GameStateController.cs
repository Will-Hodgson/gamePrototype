﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameStateController : MonoBehaviour
    {
        private State _turnState;
        private State _phaseState;
        private Battlefield _battlefield;
        private SelectedCardController _selectedCardController;
        private Text _playerManaText;
        private Text _enemyManaText;

        private int _playerMana = 0;
        private int _playerManaMax = 0;
        private int _enemyMana = 0;
        private int _enemyManaMax = 0;

        public State turnState
        {
            get { return this._turnState; }
            private set { this._turnState = value; }
        }

        public State phaseState
        {
            get { return this._phaseState; }
            private set { this._phaseState = value; }
        }

        public int playerMana
        {
            get { return this._playerMana; }
            set { this.UpdatePlayerManaText(); this._playerMana = value; }
        }

        public int playerManaMax
        {
            get { return this._playerManaMax; }
            set { this.UpdatePlayerManaText(); this._playerManaMax = value; }
        }

        public int enemyMana
        {
            get { return this._enemyMana; }
            set { this.UpdateEnemyManaText(); this._enemyMana = value; }
        }

        public int enemyManaMax
        {
            get { return this._enemyManaMax; }
            set { this.UpdateEnemyManaText(); this._enemyManaMax = value; }
        }

        void Awake()
        {
            this._turnState = GameObject.Find("Camera").GetComponent<PlayerTurnState>();
            this._phaseState = GameObject.Find("Camera").GetComponent<MulliganPhase>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._selectedCardController = GameObject.Find("SelectedCardPanel").GetComponent<SelectedCardController>();
            this._playerManaText = GameObject.Find("PlayerMana").GetComponent<Text>();
            this._enemyManaText = GameObject.Find("EnemyMana").GetComponent<Text>();
        }

        void Start()
        {
            this._phaseState.Enter();
            this._phaseState.Execute();
        }

        public void ChangeState()
        {
            this._selectedCardController.ResetSelectedCard();
            this._battlefield.ResetSquareBorders();
            this._phaseState.Exit();
            if (this._phaseState.Id() == "MainPhase2")
            {
                this._turnState.Exit();
                this._turnState = this._turnState.NextState();
                this._turnState.Enter();
                this._turnState.Exit();
            }
            this._phaseState = this._phaseState.NextState();
            this._phaseState.Enter();
            this._phaseState.Execute();
        }

        private void UpdatePlayerManaText()
        {
            this._playerManaText.text = "Mana:" + this._playerMana.ToString() + "/" + this._playerManaMax.ToString();
        }

        private void UpdateEnemyManaText()
        {
            this._enemyManaText.text = "Mana:" + this._enemyMana.ToString() + "/" + this._enemyManaMax.ToString();
        }
    }
}
