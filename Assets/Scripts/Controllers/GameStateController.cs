using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameStateController : MonoBehaviour
    {
        private State _currentState;
        private Battlefield _battlefield;
        private SelectedCardController _selectedCardController;
        private Text _playerManaText;
        private Text _enemyManaText;

        private int _playerMana = 0;
        private int _playerManaMax = 0;
        private int _enemyMana = 0;
        private int _enemyManaMax = 0;

        public State currentState
        {
            get { return this._currentState; }
            private set { this._currentState = value; }
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
            this._currentState = GameObject.Find("Camera").GetComponent<MulliganState>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._selectedCardController = GameObject.Find("SelectedCardPanel").GetComponent<SelectedCardController>();
            this._playerManaText = GameObject.Find("PlayerMana").GetComponent<Text>();
            this._enemyManaText = GameObject.Find("EnemyMana").GetComponent<Text>();
        }

        void Start()
        {
            this._currentState.Enter();
            this._currentState.Execute();
        }

        public void ChangeState()
        {
            this._selectedCardController.ResetSelectedCard();
            this._battlefield.ResetSquareBorders();
            this._currentState.Exit();
            this._currentState = this._currentState.NextState();
            this._currentState.Enter();
            this._currentState.Execute();
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
