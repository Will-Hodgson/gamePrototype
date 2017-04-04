using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Assets.Scripts
{
	public class EnemyTurnCardMouseController : MonoBehaviour, IPointerDownHandler
	{
		private Transform _selectedCardPanel;
		private CardMouseController _cardMouseController;

		public void Start()
		{
			this._selectedCardPanel = GameObject.Find("SelectedCardPanel").transform;
			this._cardMouseController = GameObject.Find("Camera").GetComponent<CardMouseController>();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			foreach (Transform child in this._selectedCardPanel) {
				Destroy(child.gameObject);
			}
			this._cardMouseController.card = this.transform;
			var duplicate = Instantiate(this.gameObject);
			duplicate.transform.SetParent(this._selectedCardPanel);
			duplicate.gameObject.GetComponent<CardController>().transform.localScale = (new Vector3(2.5f, 2.5f, 2.5f));
		}
	}
}