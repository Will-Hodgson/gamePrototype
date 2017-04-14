using System;
using System.Collections.Generic;
using UnityEngine;
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
        private CardData _cardData;
        private Owner _ownedBy;
        private Location _boardLocation;
        private Transform _square;

        public Owner ownedBy
        { 
            get { return this._ownedBy; }
            set { this._ownedBy = value; }
        }

        public Location boardLocation
        { 
            get { return this._boardLocation; }
            set { this._boardLocation = value; }
        }

        public Transform square
        {
            get { return this._square; }
            set { this._square = value; }
        }

        void Awake()
        {
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._cardData = this.GetComponent<CardData>();
            this.boardLocation = Location.DECK;
            this.square = null;
        }

        public void Init(Owner owner)
        {
            this.ownedBy = owner;
        }

        public void DrawCard()
        {
            if (this.boardLocation != Location.DECK)
            {
                Debug.LogWarning("Drew a card that is not in the deck!");
            }
            this.boardLocation = Location.HAND;
        }

        public void ReplaceCard()
        {
            if (this.boardLocation != Location.HAND)
            {
                Debug.LogWarning("Replacing a card that is not in your hand");
            }
            this.boardLocation = Location.DECK;
        }

        public void DiscardCard()
        {
            if (this.boardLocation == Location.GRAVEYARD)
            {
                Debug.LogWarning("Discarded a card that is already in the graveyard");
            }
            this.boardLocation = Location.GRAVEYARD;
            this.square = null;
            this._battlefield.DeleteCard(this.transform);
        }

        public void MoveCard(Transform square)
        {
            if (this.boardLocation != Location.BATTLEFIELD && this.boardLocation != Location.HAND)
            {
                Debug.LogWarning("Card is not on the battlefield or in your hand!");
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
                this.boardLocation = Location.BATTLEFIELD;
                this._battlefield.AddCard(this.transform);
            }
        }

        public List<Transform> SquaresInMoveDistance()
        {
            var squares = new List<Transform>();
            if (this.boardLocation == Location.HAND)
            {
                // return all the not filled squares in the first row
                for (var i = 0; i < _battlefield.width; i++)
                {
                    if (this._battlefield.GetSquareAt(i, this._battlefield.height - 1).GetComponent<SquareController>().card == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(i, this._battlefield.height - 1));
                    }
                }
                return squares;
            }
            if (this.boardLocation != Location.BATTLEFIELD)
            {
                Debug.LogWarning("Card is not on the battlefield or in players hand");
                return null;
            }
            var currentLocation = this.square.GetComponent<SquareController>().battlefieldLocation;           
            
            for (var i = 1; i <= this._cardData.moveDistance; i++)
            {
                // left movement
                if (currentLocation[0] - i >= 0)
                {
                    if (this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1]).GetComponent<SquareController>().card == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1]));
                    }
                }
                // right movement
                if (currentLocation[0] + i < this._battlefield.width)
                {
                    if (this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1]).GetComponent<SquareController>().card == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1]));
                    }
                }
                // up movement
                if (currentLocation[1] - i >= 0)
                {
                    if (this._battlefield.GetSquareAt(currentLocation[0], currentLocation[1] - i).GetComponent<SquareController>().card == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(currentLocation[0], currentLocation[1] - i));
                    }
                }
                // down movement
                if (currentLocation[1] + i < this._battlefield.height)
                {
                    if (this._battlefield.GetSquareAt(currentLocation[0], currentLocation[1] + i).GetComponent<SquareController>().card == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(currentLocation[0], currentLocation[1] + i));
                    }
                }
            }

            for (var i = 1; i <= this._cardData.diagonalMoveDistance; i++)
            {
                // diagonal left up movement
                if ((currentLocation[0] - i >= 0) && (currentLocation[1] - i >= 0))
                {
                    if (this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1] - i).GetComponent<SquareController>().card == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1] - i));
                    }
                }
                // diagonal right up movement
                if ((currentLocation[0] + i < this._battlefield.width) && (currentLocation[1] - i >= 0))
                {
                    if (this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1] - i).GetComponent<SquareController>().card == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1] - i));
                    }
                }
                // diagonal left down movement
                if ((currentLocation[0] - i >= 0) && (currentLocation[1] + i < this._battlefield.height))
                {
                    if (this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1] + i).GetComponent<SquareController>().card == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1] + i));
                    }
                }
                // diagonal left down movement
                if ((currentLocation[0] + i < this._battlefield.width) && (currentLocation[1] + i < this._battlefield.height))
                {
                    if (this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1] + i).GetComponent<SquareController>().card == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1] + i));
                    }
                }
            }
            return squares;
        }

        public List<Transform> SquaresInAttackDistance()
        {			
            if (this.boardLocation == Location.HAND)
            {
                return new List<Transform>();
            }
            if (this.boardLocation != Location.BATTLEFIELD)
            {
                Debug.LogWarning("Card is not on the battlefield!");
                return null;
            }
            var currentLocation = this.square.GetComponent<SquareController>().battlefieldLocation;
            var transforms = new List<Transform>();

            for (var i = 1; i <= this._cardData.attackDistance; i++)
            {
                // left movement
                if (currentLocation[0] - i >= 0)
                {
                    var card = this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1]).GetComponent<SquareController>().card;
                    if (card != null && card.GetComponent<CardController>().ownedBy == Owner.ENEMY)
                    {
                        transforms.Add(this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1]));
                    }
                }
                // right movement
                if (currentLocation[0] + i < this._battlefield.width)
                {
                    var card = this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1]).GetComponent<SquareController>().card;
                    if (card != null && card.GetComponent<CardController>().ownedBy == Owner.ENEMY)
                    {
                        transforms.Add(this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1]));
                    }
                }
                // up movement
                if (currentLocation[1] - i >= 0)
                {
                    var card = this._battlefield.GetSquareAt(currentLocation[0], currentLocation[1] - i).GetComponent<SquareController>().card;
                    if (card != null && card.GetComponent<CardController>().ownedBy == Owner.ENEMY)
                    {
                        transforms.Add(_battlefield.GetSquareAt(currentLocation[0], currentLocation[1] - i));
                    }
                }
                // down movement
                if (currentLocation[1] + i < this._battlefield.height)
                {
                    var card = this._battlefield.GetSquareAt(currentLocation[0], currentLocation[1] + i).GetComponent<SquareController>().card;
                    if (card != null && card.GetComponent<CardController>().ownedBy == Owner.ENEMY)
                    {
                        transforms.Add(this._battlefield.GetSquareAt(currentLocation[0], currentLocation[1] + i));
                    }
                }
            }

            for (var i = 1; i <= this._cardData.diagonalAttackDistance; i++)
            {
                // diagonal left up movement
                if ((currentLocation[0] - i >= 0) && (currentLocation[1] - i >= 0))
                {
                    var card = this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1] - i).GetComponent<SquareController>().card;
                    if (card != null && card.GetComponent<CardController>().ownedBy == Owner.ENEMY)
                    {
                        transforms.Add(this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1] - i));
                    }
                }
                // diagonal right up movement
                if ((currentLocation[0] + i < this._battlefield.width) && (currentLocation[1] - i >= 0))
                {
                    var card = this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1] - i).GetComponent<SquareController>().card;
                    if (card != null && card.GetComponent<CardController>().ownedBy == Owner.ENEMY)
                    {
                        transforms.Add(this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1] - i));
                    }
                }
                // diagonal left down movement
                if ((currentLocation[0] - i >= 0) && (currentLocation[1] + i < this._battlefield.height))
                {
                    var card = this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1] + i).GetComponent<SquareController>().card;
                    if (card != null && card.GetComponent<CardController>().ownedBy == Owner.ENEMY)
                    {
                        transforms.Add(this._battlefield.GetSquareAt(currentLocation[0] - i, currentLocation[1] + i));
                    }
                }
                // diagonal left down movement
                if ((currentLocation[0] + i < this._battlefield.width) && (currentLocation[1] + i < this._battlefield.height))
                {
                    var card = this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1] + i).GetComponent<SquareController>().card;
                    if (card != null && card.GetComponent<CardController>().ownedBy == Owner.ENEMY)
                    {
                        transforms.Add(this._battlefield.GetSquareAt(currentLocation[0] + i, currentLocation[1] + i));
                    }
                }
            }
            return transforms;
        }
    }
}
