using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class SelectedCardController : MonoBehaviour
    {
        private Transform _selectedCard;
        private Battlefield _battlefield;

        public Transform selectedCard
        { 
            get { return this._selectedCard; }
            set { this._selectedCard = value; }
        }

        public void Awake()
        {
            this._selectedCard = null;
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
        }

        public void ResetSelectedCard()
        {
            this._selectedCard = null;
            // reset all the squares to clear
            this._battlefield.ResetSquareBorders();

            // reset the selected card display
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void SetSelectedCardPanel(Transform card)
        {
            var duplicate = Instantiate(card.gameObject);
            if (card.GetComponent<CardController>().ownedBy == Owner.PLAYER)
            {
                Destroy(duplicate.GetComponent<PlayerCardMouseController>());
            }
            else
            {
                Destroy(duplicate.GetComponent<EnemyCardMouseController>());
            }
            duplicate.transform.SetParent(this.transform);
            duplicate.gameObject.GetComponent<CardController>().transform.localScale = (new Vector3(2.5f, 2.5f, 2.5f));
        }
    }
}
