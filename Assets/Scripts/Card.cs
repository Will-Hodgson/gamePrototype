using UnityEngine;

namespace Assets.Scripts
{
    public class Card : MonoBehaviour
    {
        private int _manaCost;

        public int manaCost
        {
            get { return this._manaCost; }
            set { this._manaCost = value; }
        }

        public virtual void Awake() 
        {
        }

        public virtual void OnPlayCard()
        {
        }
    }
}