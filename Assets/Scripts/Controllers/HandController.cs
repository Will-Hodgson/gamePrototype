using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class HandController : MonoBehaviour {

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
            CardController cardController = card.GetComponent<CardController>();
            cardController.boardLocation = Location.HAND;
            cardController.gameObject.AddComponent<CardMouseController>();
            cardController.gameObject.transform.SetParent(this.gameObject.transform);
            cardController.transform.localScale = (new Vector3(1, 1, 1));
            cardController.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            cardController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            this._cards.Add(card);
        }

        public void RemoveCard(Transform card)
        {
            this._cards.Remove(card);
        }
    }
}