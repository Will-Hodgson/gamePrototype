﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EnemyDeckController : MonoBehaviour
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
                obj.GetComponent<CanvasGroup>().alpha = 0f;
                obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
                obj.GetComponent<CardController>().Init(Owner.ENEMY);
                card.SetParent(GameObject.Find("EnemyDeckPanel/EnemyDeck").transform);
                this._cards.Add(card);
            }
        }

        public void DrawCard()
        {
            if (this._cards.Count == 0)
                return;
            var cardController = this._cards[0].gameObject.GetComponent<CardController>();
            this._cards.RemoveAt(0);
            cardController.DrawCard();
            cardController.gameObject.AddComponent<EnemyCardMouseController>();
            cardController.gameObject.transform.SetParent(GameObject.Find("EnemyHand").transform);
            cardController.transform.localScale = (new Vector3(1, 1, 1));
            cardController.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            if (this._cards.Count == 0)
            {
                this.gameObject.GetComponentInChildren<Text>().text = "EMPTY";
            }
        }

        public void ReplaceCard(Transform card)
        {
            var cardController = card.gameObject.GetComponent<CardController>();
            cardController.ReplaceCard();
            this._cards.Add(card);
            Destroy(cardController.gameObject.GetComponent<EnemyCardMouseController>());
            cardController.gameObject.transform.SetParent(this.gameObject.transform);
            cardController.transform.localScale = (new Vector3(1, 1, 1));
            cardController.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
            cardController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
}