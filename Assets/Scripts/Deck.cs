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

        public void Add(Card card)
        {
            this._deck.Add(card);
        }

        public void Remove(Card card)
        {
            this._deck.Remove(card);
        }
    }
}