using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activate : MonoBehaviour
{
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    public void DoActivate()
    {
        OnActivate.Invoke();
    }

    public void DoDeactivate()
    {
        OnDeactivate.Invoke();
    }
}
