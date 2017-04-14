using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class GameStateController : MonoBehaviour {
		private State[] _states;
		private State _currentState;
		private Transform _selectedCard;

		public Transform selectedCard {
			get { return this._selectedCard; }
			set { this._selectedCard = value; }
		}

		public void Start() 
		{
			var camera = GameObject.Find("Camera").transform;
			this._states = new State[3];
			this._states[0] = GameObject.Find("Camera").GetComponent<MulliganState>();
			this._states[1] = GameObject.Find("Camera").GetComponent<PlayerTurnState>();
			this._states[2] = GameObject.Find("Camera").GetComponent<EnemyTurnState>();

			this._currentState = this._states[0];
			this._selectedCard = null;
		}

		public void ChangeState() {
			this._currentState.Exit();
			if (this._currentState.Id().Equals("MulliganState"))
			{
				this._currentState = this._states[1];
			}
			else if (this._currentState.Id().Equals("PlayerTurnState"))
			{
				this._currentState = this._states[2];

			}
			else
			{
				this._currentState = this._states[1];

			}
			this._currentState.Enter();
		}

		public string State()
		{
			return this._currentState.Id();
		}

		public void UpdateText() 
		{
			GameObject.Find("Button/Text").GetComponent<Text>().text = this.State();
		}
	}
}
