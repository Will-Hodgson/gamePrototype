using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts {
	public class CardMouseController : MonoBehaviour, IPointerDownHandler {

		private CardController _cardController;
		private Transform _selectedCardPanel;
		private Battlefield _battlefield;
		private GameStateController _gameState;

		void Start () {
			this._cardController = this.GetComponent<CardController>();
			this._selectedCardPanel = GameObject.Find("SelectedCardPanel").transform;
			this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
			this._gameState = GameObject.Find ("Camera").GetComponent<GameStateController> ();
		}
		
		public void OnPointerDown(PointerEventData data) {
			if ((this._gameState.currentState.Id() == "PlayerTurnState1" || this._gameState.currentState.Id() == "PlayerTurnState2") && this._cardController.ownedBy == Owner.PLAYER) {
				// Card selected - show available moves and attacks

				// reset all the squares to clear
				foreach (Transform square in this._battlefield.GetSquares())
				{
					square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
				}

				if (this._cardController.boardLocation == Location.BATTLEFIELD) 
				{
					this._cardController.square.gameObject.GetComponent<Image>().color = UnityEngine.Color.gray;
				}

				var moveSquares = this.GetComponent<CardController>().SquaresInMoveDistance();
				var attackSquares = this.GetComponent<CardController>().SquaresInAttackDistance ();
				foreach (var square in moveSquares)
				{
					square.gameObject.GetComponent<Image>().color = UnityEngine.Color.green;
				}
				foreach (var square in attackSquares)
				{
					square.gameObject.GetComponent<Image>().color = UnityEngine.Color.red;
				}
				this._gameState.selectedCard = this.gameObject.transform;
			}

			// reset the selected card display
			foreach (Transform child in this._selectedCardPanel) {
				Destroy(child.gameObject);
			}
				
			var duplicate = Instantiate(this.gameObject);
			Destroy(duplicate.GetComponent<CardMouseController>());
			duplicate.transform.SetParent(this._selectedCardPanel);
			duplicate.gameObject.GetComponent<CardController>().transform.localScale = (new Vector3(2.5f, 2.5f, 2.5f));
		}
	}
}
