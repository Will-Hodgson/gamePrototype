using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EnemyCardMouseController : MonoBehaviour, IPointerDownHandler
    {
        private CardController _cardController;
        private Transform _selectedCardPanel;
        private Battlefield _battlefield;
        private GameStateController _gameState;

        void Awake()
        {
            this._cardController = this.GetComponent<CardController>();
            this._selectedCardPanel = GameObject.Find("SelectedCardPanel").transform;
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
        }

        public void OnPointerDown(PointerEventData data)
        {
            if ((this._gameState.currentState.Id() == "EnemyTurnState1" || this._gameState.currentState.Id() == "EnemyTurnState2")
                && this._cardController.ownedBy == Owner.ENEMY)
            {
                this._gameState.selectedCard = null;
                // reset all the squares to clear
                foreach (Transform square in this._battlefield.GetSquares())
                {
                    square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
                }

                // reset the selected card display
                foreach (Transform child in this._selectedCardPanel)
                {
                    Destroy(child.gameObject);
                }

                var duplicate = Instantiate(this.gameObject);
                Destroy(duplicate.GetComponent<EnemyCardMouseController>());
                duplicate.transform.SetParent(this._selectedCardPanel);
                duplicate.gameObject.GetComponent<CardController>().transform.localScale = (new Vector3(2.5f, 2.5f, 2.5f));

                // Check if the card has already been moved
                if (this._cardController.canMove)
                {
                    // Card selected - show available moves

                    if (this._cardController.boardLocation == Location.BATTLEFIELD)
                    {
                        this._cardController.square.gameObject.GetComponent<Image>().color = UnityEngine.Color.gray;
                    }

                    var moveSquares = this.GetComponent<CardController>().SquaresInMoveDistance();
                    foreach (var square in moveSquares)
                    {
                        square.gameObject.GetComponent<Image>().color = UnityEngine.Color.green;
                    }
                    this._gameState.selectedCard = this.gameObject.transform;
                }
            }
            else if (this._gameState.currentState.Id() == "EnemyAttackState" && this._cardController.ownedBy == Owner.ENEMY)
            {
                this._gameState.selectedCard = null;
                // reset all the squares to clear
                foreach (Transform square in this._battlefield.GetSquares())
                {
                    square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
                }

                // reset the selected card display
                foreach (Transform child in this._selectedCardPanel)
                {
                    Destroy(child.gameObject);
                }

                var duplicate = Instantiate(this.gameObject);
                Destroy(duplicate.GetComponent<EnemyCardMouseController>());
                duplicate.transform.SetParent(this._selectedCardPanel);
                duplicate.gameObject.GetComponent<CardController>().transform.localScale = (new Vector3(2.5f, 2.5f, 2.5f));

                // Check if the card has already attacked
                if (this._cardController.canAttack)
                {
                    // Card selected - show available attacks

                    if (this._cardController.boardLocation == Location.BATTLEFIELD)
                    {
                        this._cardController.square.gameObject.GetComponent<Image>().color = UnityEngine.Color.gray;
                    }

                    var attackSquares = this.GetComponent<CardController>().SquaresInAttackDistance();
                    foreach (var square in attackSquares)
                    {
                        square.gameObject.GetComponent<Image>().color = UnityEngine.Color.red;
                    }
                    this._gameState.selectedCard = this.gameObject.transform;
                }
            }
            else if (this._gameState.currentState.Id() == "PlayerAttackState" && this._cardController.ownedBy == Owner.ENEMY)
            {
                if (this._gameState.selectedCard != null)
                {
                    // This card is being attacked
                    if (this._gameState.selectedCard.GetComponent<CardController>().SquaresInAttackDistance().Contains(this._cardController.square))
                    {
                        Debug.Log("ATTACK!!!!!");
                        this._gameState.selectedCard.GetComponent<CardController>().canAttack = false;
                    }

                    this._gameState.selectedCard = null;
                    // reset all the squares to clear
                    foreach (Transform square in this._battlefield.GetSquares())
                    {
                        square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
                    }

                    // reset the selected card display
                    foreach (Transform child in this._selectedCardPanel)
                    {
                        Destroy(child.gameObject);
                    }

                    var duplicate = Instantiate(this.gameObject);
                    Destroy(duplicate.GetComponent<EnemyCardMouseController>());
                    duplicate.transform.SetParent(this._selectedCardPanel);
                    duplicate.gameObject.GetComponent<CardController>().transform.localScale = (new Vector3(2.5f, 2.5f, 2.5f));
                }
            }
        }
    }
}