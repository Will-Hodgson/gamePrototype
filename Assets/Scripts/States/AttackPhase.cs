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
        }

        public override void Execute()
        {
            this.UpdateAttackable(true);
            this._gameState.ColorAttackableCards();
        }

        public override void Exit()
        {
            this.UpdateAttackable(false);
            this._gameState.ResetCardColors();
        }

        public override State NextState()
        {
            return this._nextState;
        }

        private void UpdateAttackable(bool newValue)
        {
            Owner currentPlayer = Owner.ENEMY;
            if (this._gameState.turnState.Id() == "PlayerTurnState")
            {
                currentPlayer = Owner.PLAYER;
            }
            foreach (Transform card in this._battlefield.cards)
            {
                UnitController unitController = card.gameObject.GetComponent<UnitController>();
                if (unitController.ownedBy == currentPlayer)
                {
                    unitController.canAttack = newValue;
                }
            }
        }

        public override string Id()
        {
            return "AttackPhase";
        }
    }
}