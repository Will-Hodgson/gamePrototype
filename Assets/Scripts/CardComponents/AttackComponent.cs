using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class AttackComponent : MonoBehaviour {

        private UnitController unitController;
        private Transform attackTransform;
        public int attack
        { 
            get { return attack; }
            set
            {
                attack = value;
                UpdateAttack();
            }
        }

        public void Init(UnitController unitController)
        {

        }

        private void UpdateAttack() 
        {

        }

        public bool HasAttack()
        {
            return attack >= 0;
        }
    }
}