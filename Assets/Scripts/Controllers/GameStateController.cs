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
            if (this.turnState.Id() == "PlayerTurnState")
            {
                foreach (Transform card in this.playerPlayerController.handController.cards)
                {
                    if (card.GetComponent<Card>().manaCost <= this.playerPlayerController.mana)
                    {
                        card.GetComponent<CardController>().ColorGreen();
                    }
                }
                foreach (Transform card in this.battlefield.cards)
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
                foreach (Transform card in this.enemyPlayerController.handController.cards)
                {
                    if (card.GetComponent<Card>().manaCost <= this.enemyPlayerController.mana)
                    {
                        card.GetComponent<CardController>().ColorGreen();
                    }
                }
                foreach (Transform card in this.battlefield.cards)
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
                foreach (Transform card in this.battlefield.cards)
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
                foreach (Transform card in this.battlefield.cards)
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
