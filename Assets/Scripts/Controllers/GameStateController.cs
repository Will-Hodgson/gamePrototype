using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public enum GameState {
		MULLIGAN,
		PLAYERTURN,
		ENEMYTURN
	};

	public class GameStateController : MonoBehaviour {
		private GameState _currentState;
		private Battlefield _battlefield;
		private Transform _selectedCard;
		private Transform _selectedCardPanel;

		public Transform selectedCard {
			get { return this._selectedCard; }
			set { this._selectedCard = value; }
		}

		public GameState currentState {
			get { return this._currentState; }
			private set { this._currentState = value; }
		}

		public void Start() 
		{
			this._currentState = GameState.MULLIGAN;
			this._battlefield = GameObject.Find("Battlefield").GetComponent<Battlefield>();
			this._selectedCard = null;
			this._selectedCardPanel = GameObject.Find("SelectedCardPanel").transform;
		}

		public void ChangeState() {
			foreach (Transform child in this._selectedCardPanel) {
				Destroy(child.gameObject);
			}
			this._selectedCard = null;

			// reset all the squares to clear
			foreach (Transform square in this._battlefield.GetSquares())
			{
				square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
			}

			if (this._currentState == GameState.PLAYERTURN) {
				this._currentState = GameState.ENEMYTURN;
			} else {
				this._currentState = GameState.PLAYERTURN;
			}
		}

		public void UpdateText() 
		{
			GameObject.Find("Button/Text").GetComponent<Text>().text = this._currentState.ToString();
		}
	}
}
