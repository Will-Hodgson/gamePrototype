using UnityEngine;

namespace Assets.Scripts
{
	public class PlayerTurnState1 : State {

		private State _nextState;

		public void Start()
		{
			this._nextState = GameObject.Find("Camera").GetComponent<PlayerAttackState>();
		}

		public override void Enter()
		{
			// Display Playable/Movable cards
		}

		public override void Execute()
		{
			
		}

		public override void Exit()
		{

		}

		public override State NextState() {
			return this._nextState;
		}

		public override string Id()
		{
			return "PlayerTurnState1";
		}
	}
}
