using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class PlayerTurnCardMouseController : MonoBehaviour, IPointerDownHandler
	{
		private CardController _cardController;
		private Transform _selectedCardPanel;
		private CardMouseController _cardMouseController;
		private Battlefield _battlefield;

		public void Start()
		{
			this._cardController = this.GetComponent<CardController>();
			this._selectedCardPanel = GameObject.Find("SelectedCardPanel").transform;
			this._cardMouseController = GameObject.Find("Camera").GetComponent<CardMouseController>();
			this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
		}
			
		public void OnPointerDown(PointerEventData data)
		{
			// reset all the squares to clear
			foreach (Transform square in this._battlefield.GetSquares())
			{
				square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
			}

			// reset the selected card display
			foreach (Transform child in this._selectedCardPanel) {
				Destroy(child.gameObject);
			}

			this._cardMouseController.card = this.transform;
			var duplicate = Instantiate(this.gameObject);
			Destroy(duplicate.GetComponent<PlayerTurnCardMouseController>());
			duplicate.transform.SetParent(this._selectedCardPanel);
			duplicate.gameObject.GetComponent<CardController>().transform.localScale = (new Vector3(2.5f, 2.5f, 2.5f));
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
		}
	}
}