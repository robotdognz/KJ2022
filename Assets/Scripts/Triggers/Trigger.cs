using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    public UnityEvent TriggerEnter;
    public UnityAction<Collider2D> TriggerEnterCol = new UnityAction<Collider2D>((Collider2D C) => { });
    public UnityEvent TriggerStay;
    public UnityAction<Collider2D> TriggerStayCol = new UnityAction<Collider2D>((Collider2D C) => { });
    public UnityEvent TriggerExit;
    public UnityAction<Collider2D> TriggerExitCol = new UnityAction<Collider2D>((Collider2D C) => { });
    [HideInInspector] public bool TriggerState;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;
        if (collision.tag != "Trigger")
        {
            TriggerState = true;
            TriggerEnter.Invoke();
            TriggerEnterCol.Invoke(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;
        if (collision.tag != "Trigger")
        {
            TriggerStay.Invoke();
            TriggerStayCol.Invoke(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;
        if (collision.tag != "Trigger")
        {
            TriggerState = false;
            TriggerExit.Invoke();
            TriggerExitCol.Invoke(collision);
        }
    }
}
