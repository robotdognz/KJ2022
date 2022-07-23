using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Activate
{
    Trigger trigger;
    private bool isEntering;
    private bool isExiting;

    void Start()
    {
        trigger = GetComponentInChildren<Trigger>();
        trigger.TriggerEnter.AddListener(Enter);
        trigger.TriggerExit.AddListener(Exit);
    }

    void Enter()
    {
        if (!isEntering) StartCoroutine(EnterEnum());
    }

    private IEnumerator EnterEnum()
    {
        isEntering = true;
        DoActivate();
        yield return new WaitForSeconds(0.2f);
        isEntering = false;
    }

    void Exit()
    {
        if (!isExiting) StartCoroutine(ExitEnum());
    }
    
    private IEnumerator ExitEnum()
    {
        isExiting = true;
        DoDeactivate();
        yield return new WaitForSeconds(0.2f);
        isExiting = false;
    }

}
