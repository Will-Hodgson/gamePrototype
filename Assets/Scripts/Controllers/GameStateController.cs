using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class GameStateController : MonoBehaviour
    {
        public Battlefield battlefield;
        public PlayerController playerPlayerController;
        public PlayerController enemyPlayerController;

        public State turnState { get; set; }
        public State phaseState { get; set; }
        public Transform selectedCard { get; set; }

        void Awake()
        {
            this.turnState = GameObject.Find("Camera").GetComponent<PlayerTurnState>();
            this.phaseState = GameObject.Find("Camera").GetComponent<MulliganPhase>();
            this.battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this.playerPlayerController = GameObject.Find("PlayerHand").GetComponent<PlayerController>();
            this.enemyPlayerController = GameObject.Find("EnemyHand").GetComponent<PlayerController>();
            this.selectedCard = null;
        }

        void Start()
        {
            this.playerPlayerController.Init("Player");
            this.enemyPlayerController.Init("Enemy");
            this.phaseState.Enter();
            this.phaseState.Execute();
        }

        public void ChangeState()
        {
            this.selectedCard = null;
            this.battlefield.ResetSquareBorders();
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

        public void ColorPlayableAndMovableCards()
        {
            PlayerController playerController = this.enemyPlayerController;
            int rowNum = 0;
            Owner owner = Owner.ENEMY;
            if (this.turnState.Id() == "PlayerTurnState")
            {
                playerController = this.playerPlayerController;
                rowNum = this.battlefield.height - 1;
                owner = Owner.PLAYER;
            }
            foreach (Transform card in playerController.handController.cards)
            {
                if (card.GetComponent<Card>().manaCost <= playerController.mana)
                {
                    UnitController unitController = card.GetComponent<UnitController>();
                    if (unitController == null || unitController.SquaresInMoveDistance().Count > 0)
                    {
                        card.GetComponent<CardController>().ColorGreen();
                    }
                }
            }
            foreach (Transform card in this.battlefield.cards)
            {
                CardController cardController = card.GetComponent<CardController>();
                UnitController unitController = card.GetComponent<UnitController>();
                if (cardController.ownedBy == owner && unitController.canMove &&
                    !unitController.isExhausted && unitController.SquaresInMoveDistance().Count > 0)
                {
                    cardController.ColorGreen();
                }
            }
        }

        public void ColorAttackableCards()
        {
            Owner owner = Owner.ENEMY;
            if (this.turnState.Id() == "PlayerTurnState")
            {
                owner = Owner.PLAYER;
            }
            foreach (Transform card in this.battlefield.cards)
            {
                CardController cardController = card.GetComponent<CardController>();
                UnitController unitController = card.GetComponent<UnitController>();
                if (cardController.ownedBy == owner && unitController.canAttack &&
                    !unitController.isExhausted && unitController.SquaresInAttackDistance().Count > 0)
                {
                    cardController.ColorGreen();
                }
            }
        }

        public void ResetCardColors()
        {
            foreach (Transform card in this.playerPlayerController.handController.cards)
            {
                card.GetComponent<CardController>().ResetColor();
            }
            foreach (Transform card in this.enemyPlayerController.handController.cards)
            {
                card.GetComponent<CardController>().ResetColor();
            }
            foreach (Transform card in this.battlefield.cards)
            {
                card.GetComponent<CardController>().ResetColor();
            }
        }
    }
}
