using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CardMouseController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        private CardController _cardController;
        private Battlefield _battlefield;
        private GameStateController _gameState;
        private Transform _cardPreviewPanel;

        void Awake()
        {
            this._cardController = this.GetComponent<CardController>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._cardPreviewPanel = GameObject.Find("CardPreviewPanel").transform;
        }

        public void OnPointerEnter(PointerEventData data)
        {
            var duplicate = Instantiate(this.gameObject);
            Destroy(duplicate.GetComponent<CardMouseController>());
            duplicate.transform.SetParent(this._cardPreviewPanel);
            duplicate.gameObject.GetComponent<CardController>().transform.localScale = (new Vector3(2.5f, 2.5f, 2.5f));
        }

        public void OnPointerExit(PointerEventData data)
        {
            foreach (Transform card in this._cardPreviewPanel)
            {
                Destroy(card.gameObject);
            }
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

                        // Check if this card has already been selected, if so deselect it
                        if (this._gameState.selectedCard != null && this._gameState.selectedCard.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
                        {
                            this._gameState.selectedCard = null;
                        }
                        else if (this._cardController.canMove)
                        {
                            this._gameState.selectedCard = this.transform;
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
                }
                else
                {
                    if (this._cardController.ownedBy == Owner.PLAYER)
                    {
                        // Clicked on player's card during player's attack phase
                        // reset all the squares to clear
                        this._battlefield.ResetSquareBorders();

                        // Check if this card has already been selected, if so deselect it
                        if (this._gameState.selectedCard != null && this._gameState.selectedCard.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
                        {
                            this._gameState.selectedCard = null;
                        }
                        else if (this._cardController.canAttack)
                        {
                            this._gameState.selectedCard = this.transform;
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
                        if (this._gameState.selectedCard != null)
                        {
                            // This card is being attacked
                            if (this._gameState.selectedCard.GetComponent<CardController>().SquaresInAttackDistance().Contains(this._cardController.square))
                            {
                                this._cardController.TakeDamage(this._gameState.selectedCard.GetComponent<CardData>().attack);
                                this._gameState.selectedCard.GetComponent<CardController>().canAttack = false;
                                this._gameState.selectedCard = null;
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

                        // Check if this card has already been selected, if so deselect it
                        if (this._gameState.selectedCard != null && this._gameState.selectedCard.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
                        {
                            this._gameState.selectedCard = null;
                        }
                        else if (this._cardController.canMove)
                        {
                            this._gameState.selectedCard = this.transform;
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
                }
                else
                {
                    if (this._cardController.ownedBy == Owner.ENEMY)
                    {
                        // Clicked on enemy's card during enemy's attack phase
                        // reset all the squares to clear
                        this._battlefield.ResetSquareBorders();

                        // Check if this card has already been selected, if so deselect it
                        if (this._gameState.selectedCard != null && this._gameState.selectedCard.gameObject.GetInstanceID() == this.gameObject.GetInstanceID())
                        {
                            this._gameState.selectedCard = null;
                        }
                        else if (this._cardController.canAttack)
                        {
                            this._gameState.selectedCard = this.transform;
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
                        if (this._gameState.selectedCard != null)
                        {
                            // This card is being attacked
                            if (this._gameState.selectedCard.GetComponent<CardController>().SquaresInAttackDistance().Contains(this._cardController.square))
                            {
                                this._cardController.TakeDamage(this._gameState.selectedCard.GetComponent<CardData>().attack);
                                this._gameState.selectedCard.GetComponent<CardController>().canAttack = false;
                                this._gameState.selectedCard = null;
                            }
                        }
                    }
                }
            }
        }
    }
}