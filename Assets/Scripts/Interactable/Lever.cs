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

    private enum LeverState
    {
        Left,
        Right,
        None
    }

    [Tooltip("Constant will always trigger OnActivate, OnOff will will switch between triggering OnActivate and OnDeactivate")]
    public LeverType leverType;

    private LeverState currentLeverState = LeverState.None;
    private LeverState activateLeverState = LeverState.None;

    [SerializeField] Trigger leftTrigger;
    [SerializeField] Trigger rightTrigger;

    void Start()
    {
        switch (leverType)
        {
            case LeverType.Constant:
                leftTrigger.TriggerEnter.AddListener(Left);
                rightTrigger.TriggerEnter.AddListener(Right);
                break;
            case LeverType.OnOff:
                leftTrigger.TriggerEnter.AddListener(Left);
                rightTrigger.TriggerEnter.AddListener(Right);
                break;
        }

    }

    void Left()
    {
        switch (currentLeverState)
        {
            case LeverState.None:
                currentLeverState = LeverState.Left;
                activateLeverState = currentLeverState;
                DoActivate();
                break;
            case LeverState.Right:
                currentLeverState = LeverState.Left;
                switch (leverType)
                {
                    case LeverType.Constant:
                        DoActivate();
                        break;
                    case LeverType.OnOff:
                        if (currentLeverState == activateLeverState)
                        {
                            DoActivate();
                        }
                        else
                        {
                            DoDeactivate();
                        }
                        break;
                }
                break;
        }
    }

    void Right()
    {
        switch (currentLeverState)
        {
            case LeverState.None:
                currentLeverState = LeverState.Right;
                activateLeverState = currentLeverState;
                DoActivate();
                break;
            case LeverState.Left:
                currentLeverState = LeverState.Right;
                switch (leverType)
                {
                    case LeverType.Constant:
                        DoActivate();
                        break;
                    case LeverType.OnOff:
                        if (currentLeverState == activateLeverState)
                        {
                            DoActivate();
                        }
                        else
                        {
                            DoDeactivate();
                        }
                        break;
                }
                break;
        }


    }

}
