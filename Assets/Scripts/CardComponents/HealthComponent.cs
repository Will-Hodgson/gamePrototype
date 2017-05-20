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
                this._text.text = this.health.ToString() + "/" + this.maxHealth.ToString();;
            }
        }
            
        public int maxHealth
        {
            get { return maxHealth; }
            set
            {
                maxHealth = value;
                this._text.text = this.health.ToString() + "/" + this.maxHealth.ToString();
            }
        }

        public void Init(UnitController unitController)
        {
            this._unitController = unitController;
            GameObject healthPrefab = Resources.Load("Prefabs/Health", typeof(GameObject)) as GameObject;
            Transform healthTransform = GameObject.Instantiate(healthPrefab).transform;
            this._text = healthTransform.GetComponent<Text>();
        }

        public bool IsDead()
        {
            return health <= 0;
        }
    }
}