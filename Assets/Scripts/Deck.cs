using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Deck : MonoBehaviour
    {
        private List<Card> _deck;

        public List<Card> deck
        {
            get { return this._deck; }
        }

        public int Length()
        {
            return this._deck.Count;
        }

        public void AddCard(Card card)
        {
            this._deck.Add(card);
        }

        public void RemoveCard(Card card)
        {
            this._deck.Remove(card);
        }
    }
}