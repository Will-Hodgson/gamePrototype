using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EnemyCardMouseController : MonoBehaviour, IPointerDownHandler
    {
        private CardController _cardController;
        private Transform _selectedCardPanel;
        private Battlefield _battlefield;
        private GameStateController _gameState;

        void Awake()
        {
            this._cardController = this.GetComponent<CardController>();
            this._selectedCardPanel = GameObject.Find("SelectedCardPanel").transform;
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._gameState = GameObject.Find("Camera").GetComponent<GameStateController>();
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (this._cardController.boardLocation == Location.BATTLEFIELD)
            {
                // reset the selected card display
                foreach (Transform child in this._selectedCardPanel)
                {
                    Destroy(child.gameObject);
                }

                var duplicate = Instantiate(this.gameObject);
                Destroy(duplicate.GetComponent<EnemyCardMouseController>());
                duplicate.transform.SetParent(this._selectedCardPanel);
                duplicate.gameObject.GetComponent<CardController>().transform.localScale = (new Vector3(2.5f, 2.5f, 2.5f));
            }
        }
    }
}