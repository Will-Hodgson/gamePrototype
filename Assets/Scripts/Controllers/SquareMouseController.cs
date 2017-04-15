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
        private SelectedCardController _selectedCardController;

        void Awake()
        {
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._selectedCardController = GameObject.Find("SelectedCardPanel").GetComponent<SelectedCardController>();
        }

        public void OnPointerDown(PointerEventData data)
        {
            var currentState = this._gameState.currentState.Id();
            if (this._selectedCardController.selectedCard != null && (currentState == "PlayerTurnState1" || currentState == "PlayerTurnState2" || currentState == "EnemyTurnState1" || currentState == "EnemyTurnState2"))
            {
                var cardController = this._selectedCardController.selectedCard.GetComponent<CardController>();
                if (cardController.boardLocation == Location.HAND)
                {
                    this._battlefield.AddCard(this._selectedCardController.selectedCard);
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
                this._selectedCardController.ResetSelectedCard();
            }
        }
    }
}

