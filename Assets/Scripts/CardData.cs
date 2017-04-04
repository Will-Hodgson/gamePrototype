using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CardData : MonoBehaviour
    {

        public int manaCost;
        public int health;
        public int attack;
        public int attackDistance;
        public int diagonalAttackDistance;
        public int moveDistance;
        public int diagonalMoveDistance;

        private void Awake()
        {
            this.manaCost = 1;
            this.health = 1;
            this.attack = 1;
            this.attackDistance = 1;
            this.diagonalAttackDistance = 1;
            this.moveDistance = 2;
            this.diagonalMoveDistance = 1;
        }

        public int ManaCost
        {
            get { return this.manaCost; }
            set { this.manaCost = value; }
        }

        public int Health
        {
            get { return this.health; }
            set { this.health = value; }
        }

        public int Attack
        {
            get { return this.attack; }
            set { this.attack = value; }
        }

        public int AttackDistance
        {
            get { return this.attackDistance; }
            set { this.attackDistance = value; }
        }

        public int DiagonalAttackDistance
        {
            get { return this.diagonalAttackDistance; }
            set { this.diagonalAttackDistance = value; }
        }

        public int MoveDistance
        {
            get { return this.moveDistance; }
            set { this.moveDistance = value; }
        }

        public int DiagonalMoveDistance
        {
            get { return this.diagonalMoveDistance; }
            set { this.diagonalMoveDistance = value; }
        }
    }
}