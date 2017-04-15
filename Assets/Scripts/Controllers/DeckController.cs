using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DeckController : MonoBehaviour
    {
        [SerializeField] private Transform _cardPrefab;
        private List<Transform> _cards;

        void Awake()
        {
            this._cards = new List<Transform>();
            for (var i = 0; i < 30; i++)
            {
                var card = Instantiate(this._cardPrefab);
                var obj = card.gameObject;
                if (this.gameObject.name == "PlayerDeck")
                {
                    obj.GetComponentInChildren<Text>().text = "P" + i.ToString();
                    obj.GetComponent<CardController>().Init(Owner.PLAYER);
                    card.SetParent(GameObject.Find("PlayerDeckPanel/PlayerDeck").transform);
                }
                else
                {
                    obj.GetComponentInChildren<Text>().text = "E" + i.ToString();
                    obj.GetComponent<CardController>().Init(Owner.ENEMY);
                    card.SetParent(GameObject.Find("EnemyDeckPanel/EnemyDeck").transform);
                }
                obj.GetComponent<CanvasGroup>().alpha = 0f;
                obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
                this._cards.Add(card);
            }
        }

        public Transform DrawCard()
        {
            if (this._cards.Count == 0)
            {
                this.gameObject.GetComponentInChildren<Text>().text = "EMPTY";
                return null;
            }
            Transform temp = this._cards[0];
            this._cards.RemoveAt(0);
            return temp;
        }

        public void ReplaceCard(Transform card)
        {
            var cardController = card.gameObject.GetComponent<CardController>();
            cardController.boardLocation = Location.DECK;
            Destroy(cardController.gameObject.GetComponent<CardMouseController>());
            cardController.gameObject.transform.SetParent(this.gameObject.transform);
            cardController.transform.localScale = (new Vector3(1, 1, 1));
            cardController.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            cardController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            this._cards.Add(card);
        }
    }
}
