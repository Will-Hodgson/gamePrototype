using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Assets.Scripts
{
	public class CardMouseController : MonoBehaviour
	{
		public Transform card { get; set; }
		private Transform _selectedCardPanel;

		public void Start()
		{
			this.card = null;
		}
	}
}
