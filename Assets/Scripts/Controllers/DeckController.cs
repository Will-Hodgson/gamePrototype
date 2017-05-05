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

        public void Init(List<string> cardList)
        {
            this._cards = new List<Transform>();
            CardFactory factory = new CardFactory();
            if (this.gameObject.name == "PlayerDeck")
            {
                foreach (string str in cardList)
                {
                    CardController card = factory.CreateCard(str).GetComponent<CardController>();
                    Unit data = card.GetComponent<Unit>();
                    CanvasGroup canvasGroup = card.GetComponent<CanvasGroup>();
                    card.GetComponentInChildren<Text>().text = data.name + "\nMana: " + data.manaCost.ToString()+ "\nAttack: " + data.attack.ToString() + "\nHealth: " + data.health.ToString();
                    card.Init(Owner.PLAYER);
                    card.transform.SetParent(GameObject.Find("PlayerDeckPanel/PlayerDeck").transform);
                    canvasGroup.alpha = 0f;
                    canvasGroup.blocksRaycasts = false;
                    this._cards.Add(card.transform);
                }
            }
            else
            {
                foreach (string str in cardList)
                {
                    CardController card = factory.CreateCard(str).GetComponent<CardController>();
                    Unit data = card.GetComponent<Unit>();
                    CanvasGroup canvasGroup = card.GetComponent<CanvasGroup>();
                    card.GetComponentInChildren<Text>().text = data.name + "\nMana: " + data.manaCost.ToString()+ "\nAttack: " + data.attack.ToString() + "\nHealth: " + data.health.ToString();
                    card.Init(Owner.ENEMY);
                    card.transform.SetParent(GameObject.Find("EnemyDeckPanel/EnemyDeck").transform);
                    canvasGroup.alpha = 0f;
                    canvasGroup.blocksRaycasts = false;
                    this._cards.Add(card.transform);
                }
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
