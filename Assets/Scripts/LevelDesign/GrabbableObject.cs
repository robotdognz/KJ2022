using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Together.Actors;

public class GrabbableObject : MonoBehaviour
{
    public Trigger _Trigger;

    private void Awake()
    {
        _Trigger.TriggerStayCol += TriggerStay;
    }

    private void TriggerStay(Collider2D Collision)
    {
        if (Collision.GetComponentInParent<PlayerController>())
        {
            Character Char = PlayerController.GetCharacterByCollider(Collision);
            if (Input.GetButtonDown(Char == PlayerController.Instance.Player ? "Grab" : "JoyGrab"))
            {
                if (Collision.gameObject.layer == gameObject.layer || LayerMask.LayerToName(gameObject.layer).ToLower().Contains("shared"))
                    PlayerController.Instance.PickupObject(Char, transform);
            }
        }
    }
}
