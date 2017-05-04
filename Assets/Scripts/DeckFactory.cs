using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class DeckFactory : MonoBehaviour
    {
        public static Deck CreateDeck(List<string> deckList)
        {
            Deck deck = new Deck();
            foreach (string str in deckList)
            {
                deck.AddCard(CardFactory.CreateCard(str));
            }
            return deck;
        }
    }
}