using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateGroup : MonoBehaviour
{
    public Activate[] Activations;
    public UnityEvent OnActivationComplete;
    public UnityEvent OnActivationUncomplete;
    private bool RunEvent = false;

    private void Update()
    {
        bool AllOn = true;

        foreach (Activate A in Activations)
        {
            if (!A.IsOn)
            {
                AllOn = false;
                break;
            }
        }

        if (AllOn && !RunEvent)
        {
            OnActivationComplete.Invoke();
            RunEvent = true;
        }
        else if (!AllOn && RunEvent)
        {
            OnActivationUncomplete.Invoke();
            RunEvent = false;
        }
    }
}
