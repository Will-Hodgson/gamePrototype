using UnityEngine;

namespace Assets.Scripts
{
	public class PlayerAttackState : State {

		private State _nextState;

		public void Start()
		{
			this._nextState = GameObject.Find("Camera").GetComponent<PlayerTurnState2>();
		}

		public override void Enter()
		{

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
			return "PlayerAttackState";
		}
	}
}
