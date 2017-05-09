using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class UnitController : MonoBehaviour
    {
        private Battlefield _battlefield;
        private GameStateController _gameState;
        private CardController _cardController;
        private HandController _handController;
        private GraveyardController _graveyardController;
        private Unit _unit;
        public Owner ownedBy { get; set; }
        public Transform square { get; set; }
        public bool canMove { get; set; }
        public bool canAttack { get; set; }
        public bool isExhausted { get; set; }

        public Unit unit
        {
            get { return this._unit; }
            set { this._unit = value; }
        }

        void Awake()
        {
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
            this._handController = null;
            this._graveyardController = null;
            this._unit = this.GetComponent<Unit>();
            this._cardController = this.GetComponent<CardController>();
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
            this.canMove = this.canAttack = this.isExhausted = false;
        }

        public void DiscardCard()
        {
            if (this._cardController.boardLocation == Location.GRAVEYARD)
            {
                Debug.LogWarning("Discarded a card that is already in the graveyard");
            }
            if (this._cardController.boardLocation == Location.BATTLEFIELD)
            {
                this._battlefield.DeleteCard(this.transform);
            }
            else if (this._cardController.boardLocation == Location.HAND)
            {
                this._handController.RemoveCard(this.transform);
            }
            this.square.GetComponent<SquareController>().RemoveUnit();
            this._battlefield.DeleteCard(this.transform);
            this.square = null;
            this._graveyardController.AddCard(this.transform);
            this._cardController.boardLocation = Location.GRAVEYARD;
            this.canMove = this.canAttack = this.isExhausted = false;
        }

        public void PlayCard(Transform square)
        {
            if (this._cardController.boardLocation != Location.HAND)
            {
                Debug.LogWarning("Trying to play a card that is not in a player's hand");
                return;
            }
            this.square = square;
            this.square.gameObject.GetComponent<SquareController>().SetNewUnit(this); // new square
            if (this.ownedBy == Owner.PLAYER)
            {
                GameObject.Find("PlayerHand").GetComponent<HandController>().RemoveCard(this.transform);
            }
            else
            {
                GameObject.Find("EnemyHand").GetComponent<HandController>().RemoveCard(this.transform);
            }
            this._cardController.boardLocation = Location.BATTLEFIELD;
            this._battlefield.AddCard(this.transform);
            this._cardController.ResetColor();
            this._gameState.ColorPlayableAndMovableCards();
        }

        public void MoveCard(Transform square)
        {
            if (this.canMove == false)
            {
                Debug.LogWarning("Trying to move a card that has already moved this turn");
                return;
            }
            if (this._cardController.boardLocation != Location.BATTLEFIELD)
            {
                Debug.LogWarning("Trying to move a card that is not on the battlefield");
                return;
            }
            if (square.gameObject.GetComponent<SquareController>().unit != null)
            {
                Debug.LogWarning("Square is already filled!");
                return;
            }
            if (this.square != null)
            {
                this.square.gameObject.GetComponent<SquareController>().RemoveUnit(); // old square
            }
            this.square = square;
            this.square.gameObject.GetComponent<SquareController>().SetNewUnit(this); // new square
            this.canMove = false;
            this._cardController.ResetColor();
            this._gameState.ColorPlayableAndMovableCards();
        }

        public void Attack(UnitController defender)
        {
            defender.TakeDamage(this._unit.attack);
            if (!defender.isExhausted)
            {
                this.TakeDamage(defender.unit.attack);
            }
            this.canAttack = false;
            this.isExhausted = true;
        }
        public void TakeDamage(int damage)
        {
            this._unit.health -= damage;
            if (this._unit.health <= 0)
            {
                // Card has been killed
                this.DiscardCard();
            }
            else 
            {
                this._cardController.UpdateText(this._unit.name + "\nMana: " + this._unit.manaCost.ToString() +
                    "\nAttack: " + this._unit.attack.ToString() + "\nHealth: " + this._unit.health.ToString());
            }
        }

        public void ResetStats()
        {
            this._unit.Reset();
        }

        public List<Transform> SquaresInMoveDistance()
        {
            List<Transform> squares = new List<Transform>();
            List<Transform> possibleSquaresInMoveDistance = new List<Transform>();
            List<List<int>> coordinates = new List<List<int>>();

            if (this._cardController.boardLocation == Location.HAND)
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
                    if (this._battlefield.GetSquareAt(i, y_coor).GetComponent<SquareController>().unit == null)
                    {
                        squares.Add(this._battlefield.GetSquareAt(i, y_coor));
                    }
                }
                return squares;
            }

            if (this._cardController.boardLocation != Location.BATTLEFIELD)
            {
                Debug.LogWarning("Card is not on the battlefield or in players hand");
                return null;
            }

            // Card is on the battlefield
            var currentLocation = this.square.GetComponent<SquareController>().battlefieldLocation;
            // Add the coordinates of squares in the move distance
            for (int i = 1; i <= this._unit.moveDistance; i++)
            {
                coordinates.Add(new List<int> { currentLocation[0] - i, currentLocation[1] });
                coordinates.Add(new List<int> { currentLocation[0] + i, currentLocation[1] });
                coordinates.Add(new List<int> { currentLocation[0], currentLocation[1] - i });
                coordinates.Add(new List<int> { currentLocation[0], currentLocation[1] + i });
            }
            for (int i = 1; i <= this._unit.diagonalMoveDistance; i++)
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
                if (sq.GetComponent<SquareController>().unit == null)
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

            if (this._cardController.boardLocation == Location.HAND)
            {
                return squares;
            }
            if (this._cardController.boardLocation != Location.BATTLEFIELD)
            {
                Debug.LogWarning("Card is not on the battlefield!");
                return null;
            }

            // Card is on the battlefield
            var currentLocation = this.square.GetComponent<SquareController>().battlefieldLocation;
            // Add the coordinates of squares in the attack distance
            for (int i = 1; i <= this._unit.attackDistance; i++)
            {
                coordinates.Add(new List<int> { currentLocation[0] - i, currentLocation[1] });
                coordinates.Add(new List<int> { currentLocation[0] + i, currentLocation[1] });
                coordinates.Add(new List<int> { currentLocation[0], currentLocation[1] - i });
                coordinates.Add(new List<int> { currentLocation[0], currentLocation[1] + i });
            }
            for (int i = 1; i <= this._unit.diagonalAttackDistance; i++)
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
                if (card != null && card.GetComponent<UnitController>().ownedBy != this.ownedBy)
                {
                    squares.Add(sq);
                }
            }
            return squares;
        }
    }
}