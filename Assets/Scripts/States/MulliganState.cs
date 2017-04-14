using UnityEngine;

namespace Assets.Scripts
{
	public class MulliganState : State {

		private State _nextState;

		public void Start()
		{
			this._nextState = GameObject.Find("Camera").GetComponent<PlayerTurnState1>();
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
			return "MulliganState";
		}
	}
}
