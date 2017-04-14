using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class SquareController : MonoBehaviour
    {
		public int[] battlefieldLocation { get; set; }
		public CardController card { get; set; }
	
        public void Init(int[] location)
        {
            this.battlefieldLocation = location;
            this.card = null;
        }
    }
}