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
            if (Input.GetButtonDown("Grab"))
            {
                if (Collision.gameObject.layer == gameObject.layer)
                    PlayerController.Instance.PickupObject(PlayerController.Instance.m_ActiveCharacter, transform);
            }
        }
    }
}
