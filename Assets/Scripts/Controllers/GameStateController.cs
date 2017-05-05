using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class GameStateController : MonoBehaviour
    {
        private State _turnState;
        private State _phaseState;
        private Battlefield _battlefield;
        private HandController _playerHandController;
        private HandController _enemyHandController;
        private DeckController _playerDeckController;
        private DeckController _enemyDeckController;
        private Transform _selectedCard;
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

        public Transform selectedCard
        {
            get { return this._selectedCard; }
            set { this._selectedCard = value; }
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
            this._turnState = GameObject.Find("Camera").GetComponent<PlayerTurnState>();
            this._phaseState = GameObject.Find("Camera").GetComponent<MulliganPhase>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._playerHandController = GameObject.Find("PlayerHand").GetComponent<HandController>();
            this._enemyHandController = GameObject.Find("EnemyHand").GetComponent<HandController>();
            this._playerDeckController = GameObject.Find("PlayerDeckPanel/PlayerDeck").GetComponent<DeckController>();
            this._enemyDeckController = GameObject.Find("EnemyDeckPanel/EnemyDeck").GetComponent<DeckController>();
            this._selectedCard = null;
            this._playerManaText = GameObject.Find("PlayerMana").GetComponent<Text>();
            this._enemyManaText = GameObject.Find("EnemyMana").GetComponent<Text>();
        }

        void Start()
        {
            this._playerDeckController.Init(Enumerable.Repeat("TestUnit", 30).ToList());
            this._enemyDeckController.Init(Enumerable.Repeat("TestUnit", 30).ToList());
            this._phaseState.Enter();
            this._phaseState.Execute();
        }

        public void ChangeState()
        {
            this._selectedCard = null;
            this._battlefield.ResetSquareBorders();
            this._phaseState.Exit();
            if (this._phaseState.Id() == "MulliganPhase")
            {
                this._turnState.Enter();
                this._turnState.Execute();
            }
            else if (this._phaseState.Id() == "MainPhase2")
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

        public void ColorPlayableAndMovableCards()
        {
            if (this._turnState.Id() == "PlayerTurnState")
            {
                foreach (Transform card in this._playerHandController.cards)
                {
                    CardController cont = card.GetComponent<CardController>();
                    if (cont.canMove && card.GetComponent<CardData>().manaCost <= this._playerMana)
                    {
                        cont.ColorGreen();
                    }
                }
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cont = card.GetComponent<CardController>();
                    if (cont.ownedBy == Owner.PLAYER && cont.canMove)
                    {
                        cont.ColorGreen();
                    }
                }
            }
            else
            {
                foreach (Transform card in this._enemyHandController.cards)
                {
                    CardController cont = card.GetComponent<CardController>();
                    if (cont.canMove && card.GetComponent<CardData>().manaCost <= this._enemyMana)
                    {
                        cont.ColorGreen();
                    }
                }
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cont = card.GetComponent<CardController>();
                    if (cont.ownedBy == Owner.ENEMY && cont.canMove)
                    {
                        cont.ColorGreen();
                    }
                }
            }
        }

        public void ColorAttackableCards()
        {
            if (this._turnState.Id() == "PlayerTurnState")
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cont = card.GetComponent<CardController>();
                    if (cont.ownedBy == Owner.PLAYER && cont.canAttack && cont.SquaresInAttackDistance().Count > 0)
                    {
                        cont.ColorGreen();
                    }
                }
            }
            else
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cont = card.GetComponent<CardController>();
                    if (cont.ownedBy == Owner.ENEMY && cont.canAttack && cont.SquaresInAttackDistance().Count > 0)
                    {
                        cont.ColorGreen();
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
