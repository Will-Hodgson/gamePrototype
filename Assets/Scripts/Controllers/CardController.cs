using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Assets.Scripts
{
    public enum Location
    {
        DECK,
        HAND,
        BATTLEFIELD,
        GRAVEYARD
    };

    public enum Owner
    {
        PLAYER,
        ENEMY
    };

    public class CardController : MonoBehaviour
    {
        private Battlefield _battlefield;
        private GameStateController _gameState;
        private HandController _handController;
        private GraveyardController _graveyardController;
        private CardData _cardData;
        public Owner ownedBy { get; set; }
        public Location boardLocation { get; set; }
        public Transform square { get; set; }
        public bool canMove { get; set; }
        public bool canAttack { get; set; }

        void Awake()
        {
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._handController = null;
            this._graveyardController = null;
            this._cardData = this.GetComponent<CardData>();
            this.boardLocation = Location.DECK;
            this.square = null;
        }

        public void Init(Owner owner)
        {
            this.ownedBy = owner;
            if (this.ownedBy == Owner.PLAYER)
            {
                this._handController = GameObject.Find("PlayerHand").GetComponent<HandController>();
                this._graveyardController = GameObject.Find("PlayerGraveyardPanel/PlayerGraveyard").GetComponent<GraveyardController>();
            }
            else
            {
                this._handController = GameObject.Find("EnemyHand").GetComponent<HandController>();
                this._graveyardController = GameObject.Find("EnemyGraveyardPanel/EnemyGraveyard").GetComponent<GraveyardController>();
            }
        }

        public void DiscardCard()
        {
            if (this.boardLocation == Location.GRAVEYARD)
            {
                Debug.LogWarning("Discarded a card that is already in the graveyard");
            }
            if (this.boardLocation == Location.BATTLEFIELD)
            {
                this._battlefield.DeleteCard(this.transform);
            }
            else if (this.boardLocation == Location.HAND)
            {
                this._handController.RemoveCard(this.transform);
            }
            this.square.GetComponent<SquareController>().card = null;
            this.square = null;
            this._graveyardController.AddCard(this.transform);
        }

        public void MoveCard(Transform square)
        {
            if (this.canMove == false)
            {
                Debug.LogWarning("Trying to move a card that has already moved this turn");
                return;
            }
            if (this.boardLocation != Location.BATTLEFIELD && this.boardLocation != Location.HAND)
            {
                Debug.LogWarning("Card is not on the battlefield or in your hand!");
                return;
            }
            if (square.gameObject.GetComponent<SquareController>().card != null)
            {
                Debug.LogWarning("Square is already filled!");
                return;
            }
            if (this.square != null)
            {
                this.square.gameObject.GetComponent<SquareController>().card = null; // old square

            }
            this.square = square;
            this.square.gameObject.GetComponent<SquareController>().card = this; // new square
            if (this.boardLocation == Location.HAND)
            {
                if (this.ownedBy == Owner.PLAYER)
                {
                    GameObject.Find("PlayerHand").GetComponent<HandController>().RemoveCard(this.transform);
                }
                else
                {
                    GameObject.Find("EnemyHand").GetComponent<HandController>().RemoveCard(this.transform);
                }
                this.boardLocation = Location.BATTLEFIELD;
                this._battlefield.AddCard(this.transform);
            }
            this.ResetColor();
            this._gameState.ColorPlayableAndMovableCards();
        }

        public void TakeDamage(int damage)
        {
            this._cardData.health -= damage;
            if (this._cardData.health <= 0)
            {
                // Card has been killed
                this.DiscardCard();
            }
        }

        public void ResetStats()
        {
            this._cardData.Reset();
        }

        public List<Transform> SquaresInMoveDistance()
        {
            List<Transform> squares = new List<Transform>();
            List<Transform> possibleSquaresInMoveDistance = new List<Transform>();
            List<List<int>> coordinates = new List<List<int>>();

            if (this.boardLocation == Location.HAND)
            {
                // Card is in your hand
                int y_coor = 0;
                if (this.ownedBy == Owner.PLAYER)
                {
                    y_coor = this._battlefield.height - 1;
                }
                // return all the not filled squares in the row closest to the player/enemy
                for (var i = 0; i < this._battlefield.width; i++)
                {
                    if (this._battlefield.GetSquareAt(i, y_coor).GetComponent<SquareController>().card == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(i, y_coor));
                    }
                }
                return squares;
            }

            if (this.boardLocation != Location.BATTLEFIELD)
            {
                Debug.LogWarning("Card is not on the battlefield or in players hand");
                return null;
            }

            // Card is on the battlefield
            var currentLocation = this.square.GetComponent<SquareController>().battlefieldLocation;
            // Add the coordinates of squares in the move distance
            for (int i = 1; i <= this._cardData.moveDistance; i++)
            {
                coordinates.Add(new List<int> { currentLocation[0] - i, currentLocation[1] });
                coordinates.Add(new List<int> { currentLocation[0] + i, currentLocation[1] });
                coordinates.Add(new List<int> { currentLocation[0], currentLocation[1] - i });
                coordinates.Add(new List<int> { currentLocation[0], currentLocation[1] + i });
            }
            for (int i = 1; i <= this._cardData.diagonalMoveDistance; i++)
            {
                coordinates.Add(new List<int> { currentLocation[0] - i, currentLocation[1] - i});
                coordinates.Add(new List<int> { currentLocation[0] - i, currentLocation[1] + i});
                coordinates.Add(new List<int> { currentLocation[0] + i, currentLocation[1] - i });
                coordinates.Add(new List<int> { currentLocation[0] + i, currentLocation[1] + i });
            }
            // Get rid of the coordinates that are not on the game board
            foreach (List<int> coor in coordinates)
            {
                if (coor[0] >= 0 && coor[0] < this._battlefield.width && coor[1] >= 0 && coor[1] < this._battlefield.height)
                {
                    possibleSquaresInMoveDistance.Add(this._battlefield.GetSquareAt(coor[0], coor[1]));
                }
            }
            // Check that no other cards are on the square
            foreach (Transform sq in possibleSquaresInMoveDistance)
            {
                if (sq.GetComponent<SquareController>().card == null)
                {
                    squares.Add(sq);
                }
            }
            return squares;
        }

        public List<Transform> SquaresInAttackDistance()
        {			
            List<Transform> squares = new List<Transform>();
            List<Transform> possibleSquaresInAttackDistance = new List<Transform>();
            List<List<int>> coordinates = new List<List<int>>();

            if (this.boardLocation == Location.HAND)
            {
                return squares;
            }
            if (this.boardLocation != Location.BATTLEFIELD)
            {
                Debug.LogWarning("Card is not on the battlefield!");
                return null;
            }

            // Card is on the battlefield
            var currentLocation = this.square.GetComponent<SquareController>().battlefieldLocation;
            // Add the coordinates of squares in the attack distance
            for (int i = 1; i <= this._cardData.attackDistance; i++)
            {
                coordinates.Add(new List<int> { currentLocation[0] - i, currentLocation[1] });
                coordinates.Add(new List<int> { currentLocation[0] + i, currentLocation[1] });
                coordinates.Add(new List<int> { currentLocation[0], currentLocation[1] - i });
                coordinates.Add(new List<int> { currentLocation[0], currentLocation[1] + i });
            }
            for (int i = 1; i <= this._cardData.diagonalAttackDistance; i++)
            {
                coordinates.Add(new List<int> { currentLocation[0] - i, currentLocation[1] - i});
                coordinates.Add(new List<int> { currentLocation[0] - i, currentLocation[1] + i});
                coordinates.Add(new List<int> { currentLocation[0] + i, currentLocation[1] - i });
                coordinates.Add(new List<int> { currentLocation[0] + i, currentLocation[1] + i });
            }
            // Get rid of the coordinates that are not on the battlefield
            foreach (List<int> coor in coordinates)
            {
                if (coor[0] >= 0 && coor[0] < this._battlefield.width && coor[1] >= 0 && coor[1] < this._battlefield.height)
                {
                    possibleSquaresInAttackDistance.Add(this._battlefield.GetSquareAt(coor[0], coor[1]));
                }
            }
            // Get rid of the squares with no units or friendly units on them
            foreach (Transform sq in possibleSquaresInAttackDistance)
            {
                CardController card = sq.GetComponent<SquareController>().card;
                if (card != null && card.ownedBy != this.ownedBy)
                {
                    squares.Add(sq);
                }
            }
            return squares;
        }

        public void ColorRed()
        {
            this.GetComponent<Image>().color = UnityEngine.Color.red;
        }

        public void ColorGreen()
        {
            this.GetComponent<Image>().color = UnityEngine.Color.green;
        }

        public void ColorBlue()
        {
            this.GetComponent<Image>().color = UnityEngine.Color.blue;
        }

        public void ResetColor()
        {
            this.GetComponent<Image>().color = UnityEngine.Color.white;
        }
    }
}
