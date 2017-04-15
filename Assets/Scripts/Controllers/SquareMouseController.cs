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
        private Transform _selectedCardPanel;

        void Awake()
        {
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._selectedCardPanel = GameObject.Find("SelectedCardPanel").transform;
        }

        public void OnPointerDown(PointerEventData data)
        {
            var selectedCard = this._gameState.selectedCard;
            var currentState = this._gameState.currentState.Id();
            if (selectedCard != null && (currentState == "PlayerTurnState1" || currentState == "PlayerTurnState2" || currentState == "EnemyTurnState1" || currentState == "EnemyTurnState2"))
            {
                var cardController = selectedCard.GetComponent<CardController>();
                if (cardController.boardLocation == Location.HAND)
                {
                    this._battlefield.AddCard(selectedCard);
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
                foreach (Transform square in this._battlefield.GetSquares())
                {
                    square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
                }
                foreach (Transform child in this._selectedCardPanel)
                {
                    Destroy(child.gameObject);
                }
                this._gameState.selectedCard = null;
            }
        }
    }
}

