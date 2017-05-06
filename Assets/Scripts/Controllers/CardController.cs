using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Assets.Scripts
{
    public enum Location
    {
        DECK,
        HAND,
        BATTLEFIELD,
        GRAVEYARD
    };

    public enum Owner
    {
        PLAYER,
        ENEMY
    };

    public class CardController : MonoBehaviour
    {
        private Battlefield _battlefield;
        private HandController _handController;
        private GraveyardController _graveyardController;
        public Location boardLocation { get; set; }
        public Owner ownedBy { get; private set; }

        void Awake()
        {
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._handController = null;
            this._graveyardController = null;
            this.boardLocation = Location.DECK;
        }

        public void Init(Owner owner)
        {
            this.ownedBy = owner;
            if (owner == Owner.PLAYER)
            {
                this._handController = GameObject.Find("PlayerHand").GetComponent<HandController>();
                this._graveyardController = GameObject.Find("PlayerGraveyardPanel/PlayerGraveyard").GetComponent<GraveyardController>();
            }
            else
            {
                this._handController = GameObject.Find("EnemyHand").GetComponent<HandController>();
                this._graveyardController = GameObject.Find("EnemyGraveyardPanel/EnemyGraveyard").GetComponent<GraveyardController>();
            }
        }

        public void DiscardCard()
        {
            if (this.boardLocation == Location.GRAVEYARD)
            {
                Debug.LogWarning("Discarded a card that is already in the graveyard");
            }
            if (this.boardLocation == Location.BATTLEFIELD)
            {
                this._battlefield.DeleteCard(this.transform);
            }
            else if (this.boardLocation == Location.HAND)
            {
                this._handController.RemoveCard(this.transform);
            }
            this._graveyardController.AddCard(this.transform);
        }

        public virtual void PlayCard(Transform square = null)
        {
            UnitController unitController = this.GetComponent<UnitController>();
            if (unitController)
            {
                unitController.PlayCard(square);
            }
            else
            {
                return;
            }
        }

        public void ColorRed()
        {
            this.GetComponent<Image>().color = UnityEngine.Color.red;
        }

        public void ColorGreen()
        {
            this.GetComponent<Image>().color = UnityEngine.Color.green;
        }

        public void ColorBlue()
        {
            this.GetComponent<Image>().color = UnityEngine.Color.blue;
        }

        public void ResetColor()
        {
            this.GetComponent<Image>().color = UnityEngine.Color.white;
        }
    }
}
