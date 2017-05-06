using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour 
{
    public string name { get; set; }
    public abstract string EventTrigger();
    public abstract void TriggerAbility();
}
