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

        private int _manaCost;
		private int _manaCostOffset;
		private int _maxHealth;
		private int _maxHealthOffset;
		private int _currentHealth;
		private int _attack;
		private int _attackOffset;
		private int _attackDistance;
		private int _attackDistanceOffset;
		private int _diagonalAttackDistance;
		private int _diagonalAttackDistanceOffset;
		private int _moveDistance;
		private int _moveDistanceOffset;
		private int _diagonalMoveDistance;
		private int _diagonalMoveDistanceOffset;

		private Owner _ownedBy;
		private Location _boardLocation;
		private Transform _square;

		// Getters and Setters for private variables
        public int manaCost
        {
            get { return this._manaCost + this.manaCostOffset; }
            set { this._manaCost = value; }
        }
        public int manaCostOffset { 
			get { return this._manaCostOffset; } 
			set { this._manaCostOffset = value; }
		}
        public int maxHealth
        {
            get { return this._maxHealth + this.maxHealthOffset; }
            set { this._maxHealth = value; }
        }
        public int maxHealthOffset { 
			get { return _maxHealthOffset; } 
			set { this._maxHealthOffset = value; }
		}
        public int currentHealth { 
			get { return this._currentHealth; }
			set { this._currentHealth = value; }
		}
        public int attack
        {
            get { return this._attack + this.attackOffset; }
            set { this._attack = value; }
        }
        public int attackOffset { 
			get { return this._attackOffset; }
			set { this._attackOffset = value; }
		}
        public int attackDistance
        {
            get { return this._attackDistance + this.attackDistanceOffset; }
            set { this._attackDistance = value; }
        }
        public int attackDistanceOffset { 
			get { return this._attackDistanceOffset; }
			set { this._attackDistanceOffset = value; }
		}
        public int diagonalAttackDistance
        {
            get { return this._diagonalAttackDistance + this.diagonalAttackDistanceOffset; }
            set { this._diagonalAttackDistance = value; }
        }
        public int diagonalAttackDistanceOffset { 
			get { return this._diagonalAttackDistanceOffset; }
			set { this._diagonalAttackDistanceOffset = value; }
		}
		public int moveDistance
        {
            get { return this._moveDistance + this.moveDistanceOffset; }
            set { this._moveDistance = value; }
        }
        public int moveDistanceOffset { 
			get { return this._moveDistanceOffset; }
			set { this._moveDistanceOffset = value; }
		}
        public int diagonalMoveDistance
        {
            get { return this._diagonalMoveDistance + this.diagonalMoveDistanceOffset; }
            set { this._diagonalMoveDistance = value; }
        }
        public int diagonalMoveDistanceOffset { 
			get { return this._diagonalMoveDistanceOffset; }
			set { this._diagonalMoveDistanceOffset = value; }
		}

		public Owner ownedBy { 
			get { return this._ownedBy; }
			set { this._ownedBy = value; }
		}
        public Location boardLocation { 
			get { return this._boardLocation; }
			set { this._boardLocation = value; }
		}
        public Transform square {
			get { return this._square; }
			set { this._square = value; }
		}

        public void Awake()
        {
            var cardData = this.GetComponent<CardData>();
            this.manaCost = cardData.manaCost;
            this.manaCostOffset = 0;
            this.maxHealth = cardData.health;
            this.maxHealthOffset = 0;
            this.currentHealth = this.maxHealth;
            this.attack = cardData.attack;
            this.attackOffset = 0;
            this.attackDistance = cardData.attackDistance;
            this.attackDistanceOffset = 0;
            this.diagonalAttackDistance = cardData.diagonalAttackDistance;
            this.diagonalAttackDistanceOffset = 0;
            this.moveDistance = cardData.moveDistance;
            this.moveDistanceOffset = 0;
            this.diagonalMoveDistance = cardData.diagonalMoveDistance;
            this.diagonalMoveDistanceOffset = 0;

            this.boardLocation = Location.DECK;
            this.square = null;
        }

		public void Start() 
		{
			this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
		}

		public void Init(Owner owner) {
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

        public void DiscardCard()
        {
            if (this.boardLocation == Location.GRAVEYARD)
            {
                Debug.LogWarning("Discarded a card that is already in the graveyard");
            }
            this.boardLocation = Location.GRAVEYARD;
            this.square = null;
			this._battlefield.DeleteCard(this.transform);

            // reset all the buffs(offsets)
            this.attackDistanceOffset = 0;
            this.attackOffset = 0;
            this.diagonalAttackDistanceOffset = 0;
            this.diagonalMoveDistanceOffset = 0;
            this.manaCostOffset = 0;
            this.maxHealthOffset = 0;
            this.moveDistanceOffset = 0;
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
            
            for (var i = 1; i <= this.moveDistance; i++)
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

            for (var i = 1; i <= this.diagonalMoveDistance; i++)
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

            for (var i = 1; i <= this.attackDistance; i++)
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

            for (var i = 1; i <= this.diagonalAttackDistance; i++)
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
