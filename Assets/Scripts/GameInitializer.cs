using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
	public class GameInitializer : MonoBehaviour
	{	
		[SerializeField] private Transform _cardPrefab;
	
		void Start()
		{
			// Spawn some enemies to beat up
			var card = Instantiate(this._cardPrefab);
			var obj = card.gameObject;
			obj.GetComponent<CardController>().Init(Owner.ENEMY);
			var cardController = card.gameObject.GetComponent<CardController>();
			cardController.boardLocation = Location.DECK;
			cardController.DrawCard();
			cardController.gameObject.transform.SetParent(GameObject.Find("PlayerHand").transform);
			cardController.transform.localScale = (new Vector3(1, 1, 1));
			cardController.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
			cardController.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
			cardController.MoveCard(GameObject.Find("Battlefield").GetComponent<Battlefield>().GetSquareAt(1, 1));
			card.SetParent(GameObject.Find("Battlefield").GetComponent<Battlefield>().GetSquareAt(1, 1));
		}
	}
}
