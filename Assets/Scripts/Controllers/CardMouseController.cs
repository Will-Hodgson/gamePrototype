using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CardMouseController : MonoBehaviour, IPointerDownHandler
    {
        private CardController _cardController;
        private SelectedCardController _selectedCardController;
        private Battlefield _battlefield;
        private GameStateController _gameState;

        void Awake()
        {
            this._cardController = this.GetComponent<CardController>();
            this._selectedCardController = GameObject.Find("SelectedCardPanel").GetComponent<SelectedCardController>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (this._gameState.turnState.Id() == "PlayerTurnState")
            {
                if (this._gameState.phaseState.Id() == "MainPhase1" || this._gameState.phaseState.Id() == "MainPhase2")
                {
                    if (this._cardController.ownedBy == Owner.PLAYER)
                    {
                        // Clicked on player's card during player's main phase
                        // reset all the squares to clear
                        this._battlefield.ResetSquareBorders();

                        // reset the selected card display
                        this._selectedCardController.ResetSelectedCard();
                        this._selectedCardController.SetSelectedCardPanel(this.transform);

                        // Check if the card has already been moved
                        if (this._cardController.canMove)
                        {
                            this._selectedCardController.selectedCard = this.transform;
                            // Card selected - show available moves
                            if (this._cardController.boardLocation == Location.BATTLEFIELD)
                            {
                                this._cardController.square.GetComponent<SquareController>().ColorBoarderGray();
                            }
                            var moveSquares = this.GetComponent<CardController>().SquaresInMoveDistance();
                            foreach (var square in moveSquares)
                            {
                                square.GetComponent<SquareController>().ColorBoarderGreen();
                            }
                        }
                    }
                    else
                    {
                        // Clicked on enemy's card during player's main phase
                        // reset all the squares to clear
                        this._battlefield.ResetSquareBorders();

                        // reset the selected card display
                        this._selectedCardController.ResetSelectedCard();
                        this._selectedCardController.SetSelectedCardPanel(this.transform);
                    }
                }
                else
                {
                    if (this._cardController.ownedBy == Owner.PLAYER)
                    {
                        // Clicked on player's card during player's attack phase
                        // reset all the squares to clear
                        this._battlefield.ResetSquareBorders();

                        // reset the selected card display to have this card
                        this._selectedCardController.ResetSelectedCard();
                        this._selectedCardController.SetSelectedCardPanel(this.transform);

                        // Check if the card has already attacked
                        if (this._cardController.canAttack)
                        {
                            this._selectedCardController.selectedCard = this.transform;
                            // Card selected - show available attacks
                            if (this._cardController.boardLocation == Location.BATTLEFIELD)
                            {
                                this._cardController.square.GetComponent<SquareController>().ColorBoarderGray();
                            }

                            var attackSquares = this.GetComponent<CardController>().SquaresInAttackDistance();
                            foreach (var square in attackSquares)
                            {
                                square.GetComponent<SquareController>().ColorBoarderRed();
                            }
                        }
                    }
                    else
                    {
                        // Clicked on enemy's card during player's attack phase
                        if (this._selectedCardController.selectedCard != null)
                        {
                            // This card is being attacked
                            if (this._selectedCardController.selectedCard.GetComponent<CardController>().SquaresInAttackDistance().Contains(this._cardController.square))
                            {
                                this._cardController.TakeDamage(this._selectedCardController.selectedCard.GetComponent<CardData>().attack);
                                this._selectedCardController.selectedCard.GetComponent<CardController>().canAttack = false;
                                this._selectedCardController.ResetSelectedCard();
                                this._selectedCardController.SetSelectedCardPanel(this.transform);
                            }
                        }
                    }
                }
            }
            else
            {
                if (this._gameState.phaseState.Id() == "MainPhase1" || this._gameState.phaseState.Id() == "MainPhase2")
                {
                    if (this._cardController.ownedBy == Owner.ENEMY)
                    {
                        // Clicked on enemy's card during enemy's main phase
                        // reset all the squares to clear
                        this._battlefield.ResetSquareBorders();

                        // reset the selected card display to have this card
                        this._selectedCardController.ResetSelectedCard();
                        this._selectedCardController.SetSelectedCardPanel(this.transform);

                        // Check if the card has already been moved
                        if (this._cardController.canMove)
                        {
                            this._selectedCardController.selectedCard = this.transform;
                            // Card selected - show available moves
                            if (this._cardController.boardLocation == Location.BATTLEFIELD)
                            {
                                this._cardController.square.GetComponent<SquareController>().ColorBoarderGray();
                            }
                            var moveSquares = this.GetComponent<CardController>().SquaresInMoveDistance();
                            foreach (var square in moveSquares)
                            {
                                square.GetComponent<SquareController>().ColorBoarderGreen();
                            }
                        }
                    }
                    else
                    {
                        // Clicked on player's card during enemy's main phase
                        // reset all the squares to clear
                        this._battlefield.ResetSquareBorders();

                        // reset the selected card display
                        this._selectedCardController.ResetSelectedCard();
                        this._selectedCardController.SetSelectedCardPanel(this.transform);
                    }
                }
                else
                {
                    if (this._cardController.ownedBy == Owner.ENEMY)
                    {
                        // Clicked on enemy's card during enemy's attack phase
                        // reset all the squares to clear
                        this._battlefield.ResetSquareBorders();

                        // reset the selected card display to have this card
                        this._selectedCardController.ResetSelectedCard();
                        this._selectedCardController.SetSelectedCardPanel(this.transform);

                        // Check if the card has already attacked
                        if (this._cardController.canAttack)
                        {
                            this._selectedCardController.selectedCard = this.transform;
                            // Card selected - show available attacks
                            if (this._cardController.boardLocation == Location.BATTLEFIELD)
                            {
                                this._cardController.square.GetComponent<SquareController>().ColorBoarderGray();
                            }

                            var attackSquares = this.GetComponent<CardController>().SquaresInAttackDistance();
                            foreach (var square in attackSquares)
                            {
                                square.GetComponent<SquareController>().ColorBoarderRed();
                            }
                        }
                    }
                    else
                    {
                        // Clicked on player's card during enemy's attack phase
                        if (this._selectedCardController.selectedCard != null)
                        {
                            // This card is being attacked
                            if (this._selectedCardController.selectedCard.GetComponent<CardController>().SquaresInAttackDistance().Contains(this._cardController.square))
                            {
                                this._cardController.TakeDamage(this._selectedCardController.selectedCard.GetComponent<CardData>().attack);
                                this._selectedCardController.selectedCard.GetComponent<CardController>().canAttack = false;
                                this._selectedCardController.ResetSelectedCard();
                                this._selectedCardController.SetSelectedCardPanel(this.transform);
                            }
                        }
                    }
                }
            }
        }
    }
}