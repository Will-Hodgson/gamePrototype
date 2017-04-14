using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DeckController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Transform _cardPrefab;
        private List<Transform> _cards;

        public void Awake()
        {
			this._cards = new List<Transform>();
            for (var i = 0; i < 30; i++)
            {
                var card = Instantiate(this._cardPrefab);
				var obj = card.gameObject;
                obj.GetComponent<CanvasGroup>().alpha = 0f;
                obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
				obj.GetComponent<CardController>().Init(Owner.PLAYER);
                card.SetParent(GameObject.Find("PlayerDeckPanel/PlayerDeck").transform);
                this._cards.Add(card);
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (this._cards.Count == 0) return;
            var cardController = this._cards[0].gameObject.GetComponent<CardController>();
            this._cards.RemoveAt(0);
            cardController.DrawCard();
			Debug.Log("Draw card" + cardController.GetInstanceID());
			cardController.gameObject.AddComponent<CardMouseController>();
            cardController.gameObject.transform.SetParent(GameObject.Find("PlayerHand").transform);
            cardController.transform.localScale = (new Vector3(1, 1, 1));
            cardController.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            cardController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (this._cards.Count == 0)
            {
                this.gameObject.GetComponentInChildren<Text>().text = "EMPTY";
            }
        }

		public  void DrawCard() {
			if (this._cards.Count == 0)
				return;
			var cardController = this._cards[0].gameObject.GetComponent<CardController>();
			this._cards.RemoveAt(0);
			cardController.DrawCard();
			Debug.Log("Draw card" + cardController.GetInstanceID());
			cardController.gameObject.AddComponent<CardMouseController>();
			cardController.gameObject.transform.SetParent(GameObject.Find("PlayerHand").transform);
			cardController.transform.localScale = (new Vector3(1, 1, 1));
			cardController.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			cardController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
			if (this._cards.Count == 0) {
				this.gameObject.GetComponentInChildren<Text>().text = "EMPTY";
			}
		}

		public void ReplaceCard(Transform card) {
			var cardController = card.gameObject.GetComponent<CardController>();
			cardController.ReplaceCard();
			this._cards.Add(card);
			Debug.Log("Replaced card" + cardController.GetInstanceID());
			Destroy(cardController.gameObject.GetComponent<CardMouseController>());
			cardController.gameObject.transform.SetParent(this.gameObject.transform);
			cardController.transform.localScale = (new Vector3(1, 1, 1));
			cardController.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
			cardController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
    }
}
