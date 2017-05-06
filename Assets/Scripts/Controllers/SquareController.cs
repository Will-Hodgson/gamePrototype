using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SquareController : MonoBehaviour
    {
        public int[] battlefieldLocation { get; set; }
        private CardController _cardController;
        private UnitController _unitController;

        public UnitController unit 
        { 
            get { return this._unitController; }
        }
        public CardController card
        { 
            get { return this._cardController; }
        }

        public void Init(int[] location)
        {
            this.battlefieldLocation = location;
        }

        public void SetNewCard(CardController card)
        {
            this._cardController = card;
            this._unitController = card.GetComponent<UnitController>();
        }

        public void SetNewUnit(UnitController unit)
        {
            this._unitController = unit;
            this._cardController = unit.GetComponent<CardController>();
        }

        public void RemoveCard()
        {
            this._unitController = null;
            this._cardController = null;
        }

        public void RemoveUnit()
        {
            this.RemoveCard();
        }

        public void ColorGray()
        {
            this.gameObject.GetComponent<Image>().color = UnityEngine.Color.gray;
        }

        public void ColorClear()
        {
            this.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
        }
    }
}