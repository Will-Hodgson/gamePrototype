using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class PlayerTurnSquareMouseController : MonoBehaviour, IPointerDownHandler
	{
		public void OnPointerDown(PointerEventData data)
		{
			var selectedCard = GameObject.Find("Camera").GetComponent<CardMouseController>().card;
			if (selectedCard != null)
			{
				var cardController = selectedCard.GetComponent<CardController>();
				if (cardController.ownedBy == Owner.PLAYER && GameObject.Find("Camera").GetComponent<GameStateController>().State().Equals("PlayerTurnState"))
				{
					var moveSquares = cardController.SquaresInMoveDistance();
					var attackSquares = cardController.SquaresInAttackDistance();
					if (this.GetComponent<SquareController>().card == null) 
					{
						if (moveSquares.Contains(this.transform))
						{
							cardController.MoveCard(this.transform);
							cardController.transform.SetParent(this.transform);
						}
					}
					else if (attackSquares.Contains(this.transform)) 
					{
						// attack card
					}
					else{
						return;
					}

					// reset all the squares to clear
					foreach (Transform square in GameObject.Find("Battlefield").GetComponent<Battlefield>().GetSquares())
					{
						square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
					}
					foreach (Transform child in GameObject.Find("SelectedCardPanel").transform) {
						Destroy(child.gameObject);
					}
					selectedCard = null;
				}
			}
		}
	}
}