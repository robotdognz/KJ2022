using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Activate
{
    Trigger trigger;

    void Start()
    {
        trigger = GetComponentInChildren<Trigger>();
        trigger.TriggerEnter.AddListener(Enter);
        trigger.TriggerExit.AddListener(Exit);
    }

    void Enter()
    {
        DoActivate();
    }

    void Exit()
    {
        DoDeactivate();
    }

}
