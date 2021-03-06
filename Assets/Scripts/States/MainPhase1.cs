﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MainPhase1 : State
    {
        private State _nextState;
        private GameStateController _gameState;
        private Battlefield _battlefield;
        private Text _phasePanelText;

        void Awake()
        {
            this._nextState = GameObject.Find("Camera").GetComponent<AttackPhase>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._phasePanelText = GameObject.Find("PhasePanel/Text").GetComponent<Text>();
        }

        public override void Enter()
        {
            this._phasePanelText.text = "MainPhase1";
            foreach (Transform card in this._battlefield.cards)
            {
                card.GetComponent<UnitController>().ResetStats();
            }
        }

        public override void Execute()
        {
            if (this._gameState.turnState.Id() == "PlayerTurnState")
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    UnitController unitController = card.gameObject.GetComponent<UnitController>();
                    if (unitController.ownedBy == Owner.PLAYER)
                    {
                        unitController.canMove = true;
                        unitController.isExhausted = false;
                    }
                }
            }
            else
            {
                foreach (Transform card in this._battlefield.cards)
                {
                    UnitController unitController = card.gameObject.GetComponent<UnitController>();
                    if (unitController.ownedBy == Owner.ENEMY)
                    {
                        unitController.canMove = true;
                        unitController.isExhausted = false;
                    }
                }
            }
            this._gameState.ColorPlayableAndMovableCards();
        }

        public override void Exit()
        {
            this._gameState.ResetCardColors();
        }

        public override State NextState()
        {
            return this._nextState;
        }

        public override string Id()
        {
            return "MainPhase1";
        }
    }
}