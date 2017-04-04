using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class PlayerHandController : MonoBehaviour, IPointerDownHandler
	{
		public void OnPointerDown(PointerEventData data)
		{
			foreach (Transform child in GameObject.Find("SelectedCardPanel").transform)
			{
				var cardController = child.GetComponent<CardController>();
				if (cardController.ownedBy == Owner.PLAYER)
				{
					foreach (var card in GameObject.Find("Battlefield").GetComponent<Battlefield>().cards)
					{
						card.GetComponent<CardController>().square.GetComponent<Image>().color = UnityEngine.Color.clear;
						card.GetComponent<CanvasGroup>().blocksRaycasts = true;
					}
					Destroy(GameObject.Find("SelectedCardPanel").transform.GetChild(0).gameObject);
				}
			}
		}
	}
}
