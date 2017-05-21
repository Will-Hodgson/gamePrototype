using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HealthComponent : MonoBehaviour {

        private UnitController _unitController;
        private Text _text;

        public int health
        { 
            get { return health; }
            set
            {
                health = value;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                _text.text = health.ToString();
            }
        }
            
        public int maxHealth
        {
            get { return maxHealth; }
            set
            {
                maxHealth = value;
                _text.text = health.ToString();
            }
        }

        public void Init(UnitController unitController)
        {
            _unitController = unitController;
            GameObject healthPrefab = Resources.Load("Prefabs/Health", typeof(GameObject)) as GameObject;
            Transform healthTransform = GameObject.Instantiate(healthPrefab).transform;
            _text = healthTransform.GetComponent<Text>();
        }

        public bool IsDead()
        {
            return health <= 0;
        }
    }
}