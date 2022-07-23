using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Activate
{
    public enum LeverType
    {
        Constant,
        OnOff
    }

    [Tooltip("Constant will always trigger OnActivate, OnOff will will switch between triggering OnActivate and OnDeactivate")]
    public LeverType leverType;

    Trigger trigger;

    protected bool activate = true;

    void Start()
    {
        trigger = GetComponentInChildren<Trigger>();
        trigger.TriggerEnter.AddListener(Triggered);
    }

    void Triggered()
    {
        switch (leverType)
        {
            case LeverType.Constant:
                DoActivate();
                break;
            case LeverType.OnOff:
                if (activate)
                {
                    DoActivate();
                    activate = false;
                }
                else
                {
                    DoDeactivate();
                    activate = true;
                }
                break;
        }
    }

}
