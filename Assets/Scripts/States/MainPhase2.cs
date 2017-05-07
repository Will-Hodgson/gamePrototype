using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MainPhase2 : State
    {
        private State _nextState;
        private GameStateController _gameState;
        private Battlefield _battlefield;
        private Text _phasePanelText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<MainPhase1>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._phasePanelText = GameObject.Find("PhasePanel/Text").GetComponent<Text>();
        }

        public override void Enter()
        {
            this._phasePanelText.text = "MainPhase2";
        }

        public override void Execute()
        {
            this._gameState.ColorPlayableAndMovableCards();
        }

        public override void Exit()
        {
            foreach (Transform card in this._battlefield.cards)
            {
                card.gameObject.GetComponent<UnitController>().canMove = false;
            }
            this._gameState.ResetCardColors();
        }

        public override State NextState()
        {
            return this._nextState;
        }

        public override string Id()
        {
            return "MainPhase2";
        }
    }
}