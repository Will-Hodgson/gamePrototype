using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameStateController : MonoBehaviour
    {
        private State _currentState;
        private Battlefield _battlefield;
        private Transform _selectedCard;
        private Transform _selectedCardPanel;

        public Transform selectedCard
        {
            get { return this._selectedCard; }
            set { this._selectedCard = value; }
        }

        public State currentState
        {
            get { return this._currentState; }
            private set { this._currentState = value; }
        }

        void Awake()
        {
            this._currentState = GameObject.Find("Camera").GetComponent<MulliganState>();
            this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
            this._selectedCard = null;
            this._selectedCardPanel = GameObject.Find("SelectedCardPanel").transform;
        }

        void Start()
        {
            this._currentState.Enter();
            this._currentState.Execute();
        }

        public void ChangeState()
        {
            foreach (Transform child in this._selectedCardPanel)
            {
                Destroy(child.gameObject);
            }
            this._selectedCard = null;

            // reset all the squares to clear
            foreach (Transform square in this._battlefield.GetSquares())
            {
                square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
            }

            this._currentState.Exit();
            this._currentState = this._currentState.NextState();
            this._currentState.Enter();
            this._currentState.Execute();
        }

        public void UpdateText()
        {
            GameObject.Find("Button/Text").GetComponent<Text>().text = this._currentState.Id();
        }
    }
}
