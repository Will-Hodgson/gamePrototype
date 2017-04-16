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
                        cardController.MoveCard(this.transform);
                        cardController.transform.SetParent(this.transform);
                        cardController.canMove = false;
                    }
                }

                // reset all the squares to clear
                this._battlefield.ResetSquareBorders();
                this._gameState.selectedCard = null;
            }
        }
    }
}