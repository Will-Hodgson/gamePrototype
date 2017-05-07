using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class GameStateController : MonoBehaviour
    {
        private Battlefield _battlefield;
        private HandController _playerHandController;
        private HandController _enemyHandController;
        private DeckController _playerDeckController;
        private DeckController _enemyDeckController;
        private Text _playerHealthText;
        private Text _enemyHealthText;
        private Text _playerManaText;
        private Text _enemyManaText;
        private int _playerHealth;
        private int _enemyHealth;
        private int _playerMana;
        private int _playerManaMax;
        private int _enemyMana;
        private int _enemyManaMax;

        public State turnState { get; set; }
        public State phaseState { get; set; }
        public Transform selectedCard { get; set; }

        public int playerHealth
        {
            get { return this._playerHealth; }
            set { this._playerHealth = value; this.UpdatePlayerHealthText(); }
        }

        public int enemyHealth
        {
            get { return this._enemyHealth; }
            set { this._enemyHealth = value; this.UpdateEnemyHealthText(); }
        }

        public int playerMana
        {
            get { return this._playerMana; }
            set { this._playerMana = value; this.UpdatePlayerManaText(); }
        }

        public int playerManaMax
        {
            get { return this._playerManaMax; }
            set {  this._playerManaMax = value; this.UpdatePlayerManaText(); }
        }

        public int enemyMana
        {
            get { return this._enemyMana; }
            set {  this._enemyMana = value; this.UpdateEnemyManaText(); }
        }

        public int enemyManaMax
        {
            get { return this._enemyManaMax; }
            set {  this._enemyManaMax = value; this.UpdateEnemyManaText(); }
        }

        void Awake()
        {
            this.turnState = GameObject.Find("Camera").GetComponent<PlayerTurnState>();
            this.phaseState = GameObject.Find("Camera").GetComponent<MulliganPhase>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._playerHandController = GameObject.Find("PlayerHand").GetComponent<HandController>();
            this._enemyHandController = GameObject.Find("EnemyHand").GetComponent<HandController>();
            this._playerDeckController = GameObject.Find("PlayerDeckPanel/PlayerDeck").GetComponent<DeckController>();
            this._enemyDeckController = GameObject.Find("EnemyDeckPanel/EnemyDeck").GetComponent<DeckController>();
            this.selectedCard = null;
            this._playerHealthText = GameObject.Find("PlayerHealth").GetComponent<Text>();
            this._enemyHealthText = GameObject.Find("EnemyHealth").GetComponent<Text>();
            this._playerManaText = GameObject.Find("PlayerMana").GetComponent<Text>();
            this._enemyManaText = GameObject.Find("EnemyMana").GetComponent<Text>();
        }

        void Start()
        {
            this._playerHealth = this._enemyHealth = 25;
            this._playerMana = this._enemyMana = this._playerManaMax = this._enemyManaMax = 0;
            this._playerDeckController.Init(Enumerable.Repeat("TestUnit", 30).ToList());
            this._enemyDeckController.Init(Enumerable.Repeat("TestUnit", 30).ToList());
            this.phaseState.Enter();
            this.phaseState.Execute();
        }

        public void ChangeState()
        {
            this.selectedCard = null;
            this._battlefield.ResetSquareBorders();
            this.phaseState.Exit();
            if (this.phaseState.Id() == "MulliganPhase")
            {
                this.turnState.Enter();
                this.turnState.Execute();
            }
            else if (this.phaseState.Id() == "MainPhase2")
            {
                this.turnState.Exit();
                this.turnState = this.turnState.NextState();
                this.turnState.Enter();
                this.turnState.Exit();
            }
            this.phaseState = this.phaseState.NextState();
            this.phaseState.Enter();
            this.phaseState.Execute();
        }

        private void UpdatePlayerHealthText()
        {
            this._playerHealthText.text = "Health: " + this._playerHealth.ToString();
        }

        private void UpdateEnemyHealthText()
        {
            this._enemyHealthText.text = "Health: " + this._enemyHealth.ToString();
        }

        private void UpdatePlayerManaText()
        {
            this._playerManaText.text = "Mana: " + this._playerMana.ToString() + "/" + this._playerManaMax.ToString();
        }

        private void UpdateEnemyManaText()
        {
            this._enemyManaText.text = "Mana: " + this._enemyMana.ToString() + "/" + this._enemyManaMax.ToString();
        }

        public void ColorPlayableAndMovableCards()
        {
            if (this.turnState.Id() == "PlayerTurnState")
            {
                foreach (Transform card in this._playerHandController.cards)
                {
                    if (card.GetComponent<Card>().manaCost <= this._playerMana)
                    {
                        card.GetComponent<CardController>().ColorGreen();
                    }
                }
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cont = card.GetComponent<CardController>();
                    if (cont.ownedBy == Owner.PLAYER && card.GetComponent<UnitController>().canMove)
                    {
                        cont.ColorGreen();
                    }
                }
            }
            else
            {
                foreach (Transform card in this._enemyHandController.cards)
                {
                    if (card.GetComponent<Card>().manaCost <= this._enemyMana)
                    {
                        card.GetComponent<CardController>().ColorGreen();
                    }
                }
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cont = card.GetComponent<CardController>();
                    if (cont.ownedBy == Owner.ENEMY && card.GetComponent<UnitController>().canMove)
                    {
                        cont.ColorGreen();
                    }
                }
            }
        }

        public void ColorAttackableCards()
        {
            if (this.turnState.Id() == "PlayerTurnState")
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cardController = card.GetComponent<CardController>();
                    UnitController unitController = card.GetComponent<UnitController>();
                    if (cardController.ownedBy == Owner.PLAYER && unitController.canAttack && unitController.SquaresInAttackDistance().Count > 0)
                    {
                        cardController.ColorGreen();
                    }
                }
            }
            else
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cardController = card.GetComponent<CardController>();
                    UnitController unitController = card.GetComponent<UnitController>();
                    if (cardController.ownedBy == Owner.ENEMY && unitController.canAttack && unitController.SquaresInAttackDistance().Count > 0)
                    {
                        cardController.ColorGreen();
                    }
                }
            }
        }

        public void ResetCardColors()
        {
            foreach (Transform card in this._playerHandController.cards)
            {
                card.GetComponent<CardController>().ResetColor();
            }
            foreach (Transform card in this._enemyHandController.cards)
            {
                card.GetComponent<CardController>().ResetColor();
            }
            foreach (Transform card in this._battlefield.cards)
            {
                card.GetComponent<CardController>().ResetColor();
            }
        }
    }
}
