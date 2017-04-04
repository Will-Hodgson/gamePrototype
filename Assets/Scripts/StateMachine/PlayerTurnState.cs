using UnityEngine;

namespace Assets.Scripts
{
	public class PlayerTurnState : State {
		private Battlefield _battlefield;
		private Transform _playerHand;
		private Transform _playerDeck;

		public void Start()
		{
			this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
			this._playerHand = GameObject.Find("PlayerHand").transform;
			this._playerDeck = GameObject.Find("PlayerDeckPanel/PlayerDeck").transform;
		}

		public override void Enter()
		{
			foreach (var card in this._battlefield.cards)
			{
				if (card.GetComponent<CardController>().ownedBy == Owner.PLAYER)
				{
					card.gameObject.AddComponent<PlayerTurnCardMouseController>();
				}
				else 
				{
					card.gameObject.AddComponent<EnemyTurnCardMouseController>();
				}
			}
			for (int i = 0; i < this._playerHand.childCount; i++)
			{
				this._playerHand.GetChild(i).gameObject.AddComponent<PlayerTurnCardMouseController>();
			}
			for (int i = 0; i < this._playerDeck.childCount; i++)
			{
				this._playerDeck.GetChild(i).gameObject.AddComponent<PlayerTurnCardMouseController>();
			}
		}

		public override void Execute()
		{
		}

		public override void Exit()
		{
			foreach (var card in this._battlefield.cards)
			{
				if (card.GetComponent<CardController>().ownedBy == Owner.PLAYER)
				{
					Destroy(card.gameObject.GetComponent<PlayerTurnCardMouseController>());
				}
				else 
				{
					Destroy(card.gameObject.GetComponent<EnemyTurnCardMouseController>());
				}
			}
			for (int i = 0; i < this._playerHand.childCount; i++)
			{
				Destroy(this._playerHand.GetChild(i).gameObject.GetComponent<PlayerTurnCardMouseController>());
			}
			for (int i = 0; i < this._playerDeck.childCount; i++)
			{
				Destroy(this._playerDeck.GetChild(i).gameObject.GetComponent<PlayerTurnCardMouseController>());
			}
		}

		public override string Id()
		{
			return "PlayerTurnState";
		}
	}
}
