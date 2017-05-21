using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ManaComponent : MonoBehaviour {

        private UnitController _unitController;
        private Text _text;

        public int mana
        { 
            get { return mana; }
            set
            {
                mana = value;
                _text.text = mana.ToString();
            }
        }

        public void Init(UnitController unitController)
        {
            _unitController = unitController;
            GameObject manaPrefab = Resources.Load("Prefabs/Mana", typeof(GameObject)) as GameObject;
            Transform manaTransform = GameObject.Instantiate(manaPrefab).transform;
            _text = manaTransform.GetComponent<Text>();
        }
    }
}