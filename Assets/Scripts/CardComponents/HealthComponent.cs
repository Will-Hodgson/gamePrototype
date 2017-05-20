using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class HealthComponent : MonoBehaviour {

        private UnitController unitController;
        private Transform healthTransform;
        public int health
        { 
            get { return health; }
            set
            {
                health = value;
                UpdateHealth();
            }
        }
            
        public int maxHealth
        {
            get { return maxHealth; }
            set
            {
                maxHealth = value;
                UpdateHealth();
            }
        }

        public void Init(UnitController unitController)
        {
            
        }
    	
    	private void UpdateHealth() 
        {
    		
    	}

        public bool IsDead()
        {
            return health <= 0;
        }
    }
}