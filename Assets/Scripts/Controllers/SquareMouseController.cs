using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SquareMouseController : MonoBehaviour, IPointerDownHandler
    {
        private Battlefield _battlefield;
        private GameStateController _gameState;

        void Awake()
        {
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (this._gameState.selectedCard != null && this._gameState.phaseState.Id() != "AttackPhase")
            {
                var cardController = this._gameState.selectedCard.GetComponent<CardController>();
                if (cardController.boardLocation == Location.HAND)
                {
                    this._battlefield.AddCard(this._gameState.selectedCard);
                }
                var moveSquares = cardController.SquaresInMoveDistance();

                if (this.GetComponent<SquareController>().card == null)
                {
                    // Move action
                    if (moveSquares.Contains(this.transform))
                    {
                        if (cardController.ownedBy == Owner.PLAYER && cardController.boardLocation == Location.HAND && 
                            cardController.GetComponent<CardData>().manaCost <= this._gameState.playerMana)
                        {
                            this._gameState.playerMana = this._gameState.playerMana - cardController.GetComponent<CardData>().manaCost;
                        }
                        else if (cardController.ownedBy == Owner.ENEMY && cardController.boardLocation == Location.HAND &&
                            cardController.GetComponent<CardData>().manaCost <= this._gameState.enemyMana)
                        {
                            this._gameState.enemyMana = this._gameState.enemyMana - cardController.GetComponent<CardData>().manaCost;
                        }

                        cardController.MoveCard(this.transform);
                        cardController.transform.SetParent(this.transform);
                        cardController.canMove = false;
                    }
                }

                // reset all the squares to clear
                this._battlefield.ResetSquareBorders();
                this._gameState.ResetCardColors();
                this._gameState.ColorPlayableAndMovableCards();
                this._gameState.selectedCard = null;
            }
        }
    }
}