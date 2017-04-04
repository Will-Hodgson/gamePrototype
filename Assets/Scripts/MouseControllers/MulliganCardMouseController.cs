using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Assets.Scripts
{
	public class MulliganCardMouseController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
	{
		private Transform _card;
		private Transform _selectedCardPanel;

		public void Start()
		{
			this._card = this.transform;
			this._selectedCardPanel = GameObject.Find("SelectedCardPanel").transform;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this._card.parent == this._selectedCardPanel) { return; }
		}

		public void OnPointerDown(PointerEventData eventData)
		{

		}

		public void OnPointerExit(PointerEventData eventData)
		{

		}
	}
}