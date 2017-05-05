using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Card : MonoBehaviour
    {
        public int manaCost { get; set; }
        public string name { get; set; }
    }
}