using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AttackPhase: State
    {
        private State _nextState;
        private GameStateController _gameState;
        private Battlefield _battlefield;
        private Text _phasePanelText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<MainPhase2>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._phasePanelText = GameObject.Find("PhasePanel/Text").GetComponent<Text>();
        }

        public override void Enter()
        {
            this._phasePanelText.text = "AttackPhase";
            this._gameState.ColorAttackableCards();
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
                        cardController.canAttack = true;
                    }
                }
            }
            else
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cardController = card.gameObject.GetComponent<CardController>();
                    if (cardController.ownedBy == Owner.ENEMY)
                    {
                        cardController.canAttack = true;
                    }
                }
            }
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
                        cardController.canAttack = false;
                    }
                }
            }
            else
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    CardController cardController = card.gameObject.GetComponent<CardController>();
                    if (cardController.ownedBy == Owner.ENEMY)
                    {
                        cardController.canAttack = false;
                    }
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
            return "AttackPhase";
        }
    }
}