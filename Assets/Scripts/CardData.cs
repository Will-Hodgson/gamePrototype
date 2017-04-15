using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CardData : MonoBehaviour
    {
        private int _manaCost;
        private int _health;
        private int _attack;
        private int _attackDistance;
        private int _diagonalAttackDistance;
        private int _moveDistance;
        private int _diagonalMoveDistance;
        private int _maxHealth;
        private int _maxAttack;
        private int _maxAttackDistance;
        private int _maxDiagonalAttackDistance;
        private int _maxMoveDistance;
        private int _maxDiagonalMoveDistance;

        public int manaCost
        {
            get { return this._manaCost; }
            set { this._manaCost = value; }
        }

        public int health
        {
            get { return this._health; }
            set { this._health = value; }
        }

        public int attack
        {
            get { return this._attack; }
            set { this._attack = value; }
        }

        public int attackDistance
        {
            get { return this._attackDistance; }
            set { this._attackDistance = value; }
        }

        public int diagonalAttackDistance
        {
            get { return this._diagonalAttackDistance; }
            set { this._diagonalAttackDistance = value; }
        }

        public int moveDistance
        {
            get { return this._moveDistance; }
            set { this._moveDistance = value; }
        }

        public int diagonalMoveDistance
        {
            get { return this._diagonalMoveDistance; }
            set { this._diagonalMoveDistance = value; }
        }

        public int maxHealth
        {
            get { return this._maxHealth; }
            set { this._maxHealth = value; }
        }

        public int maxAttack
        {
            get { return this._maxAttack; }
            set { this._maxAttack = value; }
        }

        public int maxAttackDistance
        {
            get { return this._maxAttackDistance; }
            set { this._maxAttackDistance = value; }
        }

        public int maxDiagonalAttackDistance
        {
            get { return this._maxDiagonalAttackDistance; }
            set { this._maxDiagonalAttackDistance = value; }
        }

        public int maxMoveDistance
        {
            get { return this._maxMoveDistance; }
            set { this._maxMoveDistance = value; }
        }

        public int maxDiagonalMoveDistance
        {
            get { return this._maxDiagonalMoveDistance; }
            set { this._maxDiagonalMoveDistance = value; }
        }

        void Awake()
        {
            this._manaCost = 1;
            this._health = this._maxHealth = 1;
            this._attack = this._maxAttack = 1;
            this._attackDistance = this._maxAttackDistance = 1;
            this._diagonalAttackDistance = this._maxDiagonalAttackDistance = 1;
            this._moveDistance = this._maxMoveDistance = 2;
            this._diagonalMoveDistance = this._maxDiagonalMoveDistance = 1;
        }

        public void Reset()
        {
            this._health = this._maxHealth;
            this._attack = this._maxAttack;
            this._attackDistance = this._maxAttackDistance;
            this._diagonalAttackDistance = this._maxDiagonalAttackDistance;
            this._moveDistance = this._maxMoveDistance;
            this._diagonalMoveDistance = this._maxDiagonalMoveDistance;
        }
    }
}