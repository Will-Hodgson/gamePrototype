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
        public GameObject attackPlayerButton;
        public GameObject attackEnemyButton;

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
            this.attackPlayerButton = GameObject.Find("AttackPlayerButton");
            this.attackEnemyButton = GameObject.Find("AttackEnemyButton");
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
            Owner owner = Owner.ENEMY;
            if (this.turnState.Id() == "PlayerTurnState")
            {
                playerController = this.playerPlayerController;
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
                if ((cardController.ownedBy == owner && unitController.canMove &&   // Cards that can move; Cards owned by the player whose turn it is
                    !unitController.isExhausted && unitController.SquaresInMoveDistance().Count > 0) && // Cards that are not exhausted; Free squares to move
                    (unitController.unit.moveDistance > 0 || unitController.unit.diagonalMoveDistance > 0)) // Cards whose move distance is at least 1
                {
                    cardController.ColorGreen();
                }
            }
        }

        public void ColorAttackableCards()
        {
            Owner owner = Owner.ENEMY;
            int rowNum = this.battlefield.height - 1;
            if (this.turnState.Id() == "PlayerTurnState")
            {
                owner = Owner.PLAYER;
                rowNum = 0;
            }
            foreach (Transform card in this.battlefield.cards)
            {
                CardController cardController = card.GetComponent<CardController>();
                UnitController unitController = card.GetComponent<UnitController>();
                if (cardController.ownedBy == owner && unitController.canAttack && !unitController.isExhausted)
                {
                    if (unitController.SquaresInAttackDistance().Count > 0 ||
                        unitController.square.GetComponent<SquareController>().battlefieldLocation[1] == rowNum) // Can attack the other player
                    {
                        cardController.ColorGreen();
                    }
                }
            }
        }

        public void ColorPlayerAttackable(string player)
        {
            if (player == "Player")
            {
                this.attackPlayerButton.SetActive(true);
            }
            else
            {
                this.attackEnemyButton.SetActive(true);
            }
        }

        public void ResetPlayerAttackableColor()
        {
            this.attackEnemyButton.SetActive(false);
            this.attackPlayerButton.SetActive(false);
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
            this.ResetPlayerAttackableColor();
        }

        public void AttackPlayer()
        {
            this.selectedCard.GetComponent<UnitController>().AttackPlayer();
            this.ResetPlayerAttackableColor();
            this.ResetCardColors();
            this.selectedCard = null;
        }
    }
}
