using UnityEngine;

namespace Assets.Scripts
{
	public class MulliganState : State {
		public override void Enter()
		{
		}

		public override void Execute()
		{
		}

		public override void Exit()
		{
			foreach (Transform child in GameObject.Find("SelectedCardPanel").transform) {
				Destroy(child.gameObject);
			}
		}

		public override string Id()
		{
			return "MulliganState";
		}
	}
}