using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
	public class MulliganState : State {

		private State _nextState;
		private GameStateController _gameState;
		private DeckController _deckController;
		private Transform _playerHand;
		private Transform _mulliganButton;
		private Transform _keepCardsButton;

		public void Start()
		{
			this._nextState = GameObject.Find("Camera").GetComponent<PlayerTurnState1>();
			this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
			this._deckController = GameObject.Find("PlayerDeckPanel/PlayerDeck").GetComponent<DeckController>();
			this._playerHand = GameObject.Find("PlayerHand").transform;
			this._mulliganButton = GameObject.Find("MulliganButton").transform;
			this._keepCardsButton = GameObject.Find("KeepCardsButton").transform;
		}

		public override void Enter()
		{

		}

		public override void Execute()
		{
			// Draw 7 cards
			for (int i = 0; i < 7; i++) {
				this._deckController.DrawCard();
			}

			// Place Mulligan Button
		}

		public override void Exit()
		{
			this._mulliganButton.gameObject.SetActive(false);
			this._keepCardsButton.gameObject.SetActive(false);
		}

		public override State NextState() {
			return this._nextState;
		}

		public void mulligan() {
			List<Transform> temp = new List<Transform>();
			foreach (Transform card in this._playerHand) {
				temp.Add(card);
			}

			foreach (Transform card in temp) {
				this._deckController.ReplaceCard(card);
			}

			// Draw 7 cards
			for (int i = 0; i < 7; i++) {
				this._deckController.DrawCard();
			}
			this._gameState.ChangeState();
		}

		public void keepCards() {
			this._gameState.ChangeState();
		}

		public override string Id()
		{
			return "MulliganState";
		}
	}
}
