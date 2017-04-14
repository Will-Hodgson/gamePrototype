using UnityEngine;

namespace Assets.Scripts
{
    abstract public class State : MonoBehaviour
    {
        abstract public void Enter();

        abstract public void Execute();

        abstract public void Exit();

        abstract public State NextState();

        abstract public string Id();
    }
}
