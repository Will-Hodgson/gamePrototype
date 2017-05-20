using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Unit : Card
    {
        public int health { get; set; }
        public int attack { get; set; }
        public int attackDistance { get; set; }
        public int diagonalAttackDistance { get; set; }
        public int moveDistance { get; set; }
        public int diagonalMoveDistance { get; set; }
        public int maxHealth { get; set; }
        public int maxAttack { get; set; }
        public int maxAttackDistance { get; set; }
        public int maxDiagonalAttackDistance { get; set; }
        public int maxMoveDistance { get; set; }
        public int maxDiagonalMoveDistance { get; set; }
        private List<Ability> _abilities;

        public List<Ability> abilities
        {
            get { return this._abilities; }
        }

        public void Init(string nm, int mc, int hl, int at, int ad, int dad, int md, int dmd, List<Ability> abilities)
        {
            name = nm;
            manaCost = mc;
            health = hl;
            attack = at;
            attackDistance = ad;
            diagonalAttackDistance = dad;
            moveDistance = md;
            diagonalMoveDistance = dmd;
            maxHealth = health;
            maxAttack = attack;
            maxAttackDistance = attackDistance;
            maxDiagonalAttackDistance = diagonalAttackDistance;
            maxMoveDistance = moveDistance;
            maxDiagonalMoveDistance = diagonalMoveDistance;
            this._abilities = abilities;
        }

        public bool Has(Ability ability)
        {
            return this._abilities.Contains(ability);
        }

        public void Add(Ability ability)
        {
            this._abilities.Add(ability);
        }
            
        public void Remove(Ability ability)
        {
            this._abilities.Remove(ability);
        }

        public void Reset()
        {
            this.health = this.maxHealth;
            this.attack = this.maxAttack;
            this.attackDistance = this.maxAttackDistance;
            this.diagonalAttackDistance = this.maxDiagonalAttackDistance;
            this.moveDistance = this.maxMoveDistance;
            this.diagonalMoveDistance = this.maxDiagonalMoveDistance;
        }
    }
}