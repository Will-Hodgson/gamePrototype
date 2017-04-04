using UnityEngine;

namespace Assets.Scripts
{
	public class EnemyTurnState : State 
	{
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
				card.gameObject.AddComponent<EnemyTurnCardMouseController>();
			}
			for (int i = 0; i < this._playerHand.childCount; i++)
			{
				this._playerHand.GetChild(i).gameObject.AddComponent<EnemyTurnCardMouseController>();
			}
			for (int i = 0; i < this._playerDeck.childCount; i++)
			{
				this._playerDeck.GetChild(i).gameObject.AddComponent<EnemyTurnCardMouseController>();
			}
		}

		public override void Execute()
		{
		}

		public override void Exit()
		{
			foreach (var card in this._battlefield.cards)
			{
				Destroy(card.gameObject.GetComponent<EnemyTurnCardMouseController>());
			}
			for (int i = 0; i < this._playerHand.childCount; i++)
			{
				Destroy(this._playerHand.GetChild(i).gameObject.GetComponent<EnemyTurnCardMouseController>());
			}
			for (int i = 0; i < this._playerDeck.childCount; i++)
			{
				Destroy(this._playerDeck.GetChild(i).gameObject.GetComponent<EnemyTurnCardMouseController>());
			}
		}

		public override string Id()
		{
			return "EnemyTurnState";
		}
	}
}

