using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SquareController : MonoBehaviour
    {
        private int[] _battlefieldLocation;
        private CardController _cardController;

        public int[] battlefieldLocation
        {
            get { return this._battlefieldLocation; } 
            set { this._battlefieldLocation = value; }
        }

        public CardController card
        {
            get { return this._cardController; }
            set { this._cardController = value; }
        }

        public void Init(int[] location)
        {
            this.battlefieldLocation = location;
            this.card = null;
        }
    }
}