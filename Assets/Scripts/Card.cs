using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Card : MonoBehaviour
    {
        private int _manaCost;
        private string _name;

        public int manaCost
        {
            get { return this._manaCost; }
            set { this._manaCost = value; }
        }

        public string name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public abstract void OnPlayCard();
    }
}