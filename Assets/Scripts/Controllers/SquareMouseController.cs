using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class SquareMouseController : MonoBehaviour, IPointerDownHandler {

		private Battlefield _battlefield;
		private GameStateController _gameStateController;
		private Transform _selectedCardPanel;

		public void Start() 
		{
			this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
			this._gameStateController = GameObject.Find("Camera").GetComponent<GameStateController>();
			this._selectedCardPanel = GameObject.Find("SelectedCardPanel").transform;
		}

		public void OnPointerDown(PointerEventData data) {
			var selectedCard = this._gameStateController.selectedCard;
			if (selectedCard != null) {
				var cardController = selectedCard.GetComponent<CardController>();
				if (cardController.ownedBy == Owner.PLAYER && this._gameStateController.currentState == GameState.PLAYERTURN) {
					var moveSquares = cardController.SquaresInMoveDistance();
					var attackSquares = cardController.SquaresInAttackDistance();

					if (this.GetComponent<SquareController>().card == null) {
						// Move action
						if (moveSquares.Contains(this.transform)) {
							cardController.MoveCard(this.transform);
							cardController.transform.SetParent(this.transform);
						}
					}
					else if (attackSquares.Contains(this.transform)) {
						// Attack action
					}
					else {
						return;
					}

					// reset all the squares to clear
					foreach (Transform square in this._battlefield.GetSquares())
					{
						square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
					}
					foreach (Transform child in this._selectedCardPanel) {
						Destroy(child.gameObject);
					}
					this._gameStateController.selectedCard = null;
				}
			}
		}
	}
}

