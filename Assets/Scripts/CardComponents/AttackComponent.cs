using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AttackComponent : MonoBehaviour {

        private UnitController _unitController;
        private Text _text;

        public int attack
        { 
            get { return attack; }
            set
            {
                attack = value;
                this._text.text = attack.ToString();
            }
        }

        public void Init(UnitController unitController)
        {
            this._unitController = unitController;
            GameObject attackPrefab = Resources.Load("Prefabs/Attack", typeof(GameObject)) as GameObject;
            Transform attackTransform = GameObject.Instantiate(attackPrefab).transform;
            this._text = attackTransform.GetComponent<Text>();
        }

        public bool HasAttack()
        {
            return attack >= 0;
        }
    }
}