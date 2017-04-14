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

        void Awake()
        {
            this._manaCost = 1;
            this._health = 1;
            this._attack = 1;
            this._attackDistance = 1;
            this._diagonalAttackDistance = 1;
            this._moveDistance = 2;
            this._diagonalMoveDistance = 1;
        }
    }
}