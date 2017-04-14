﻿using UnityEngine;

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
		}

		public override void Execute()
		{
		}

		public override void Exit()
		{
			foreach (Transform child in GameObject.Find("SelectedCardPanel").transform) {
				Destroy(child.gameObject);
			}
			GameObject.Find("Camera").GetComponent<GameStateController>().selectedCard = null;
		}

		public override string Id()
		{
			return "PlayerTurnState";
		}
	}
}