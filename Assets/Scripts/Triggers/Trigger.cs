using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    public UnityEvent TriggerEnter;
    public UnityEvent TriggerStay;
    public UnityEvent TriggerExit;
    [HideInInspector] public bool TriggerState;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerState = true;
        TriggerEnter.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TriggerStay.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TriggerState = false;
        TriggerExit.Invoke();
    }
}
