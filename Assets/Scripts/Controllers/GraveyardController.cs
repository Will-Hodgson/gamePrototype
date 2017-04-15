using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GraveyardController : MonoBehaviour {

        private List<Transform> _cards;

        public List<Transform> cards
        {
            get { return this._cards; }
        }

        void Awake()
        {
            this._cards = new List<Transform>();
        }

        public void AddCard(Transform card)
        {
            var cardController = card.gameObject.GetComponent<CardController>();
            cardController.boardLocation = Location.GRAVEYARD;
            Destroy(cardController.gameObject.GetComponent<CardMouseController>());
            cardController.gameObject.transform.SetParent(this.gameObject.transform);
            cardController.transform.localScale = (new Vector3(1, 1, 1));
            cardController.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            cardController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            this._cards.Add(card);
        }

        public void RemoveCard(Transform card)
        {
            CardController cardController = card.gameObject.GetComponent<CardController>();
            cardController.gameObject.AddComponent<CardMouseController>();
            cardController.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            cardController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            this._cards.Remove(card);
        }
    }
}