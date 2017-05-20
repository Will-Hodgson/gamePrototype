using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ManaComponent : MonoBehaviour {

        private UnitController unitController;
        private Transform manaTransform;
        public int mana
        { 
            get { return mana; }
            set
            {
                mana = value;
                UpdateMana();
            }
        }

        public void Init(UnitController unitController)
        {

        }

        private void UpdateMana() 
        {

        }
    }
}