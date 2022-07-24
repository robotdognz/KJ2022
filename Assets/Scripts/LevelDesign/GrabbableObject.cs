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

            if (Char != null && Char.CharacterObject.gameObject.layer == Collision.gameObject.layer || LayerMask.LayerToName(Collision.gameObject.layer).ToLower().Contains("shared"))
                Char.LookingGrabbable = gameObject;
        }
    }

    private void TriggerExit(Collider2D Collision)
    {
        if (Collision.GetComponentInParent<PlayerController>())
        {
            Character Char = PlayerController.GetCharacterByCollider(Collision);

            if (Char != null && Char.CharacterObject.gameObject.layer == Collision.gameObject.layer || LayerMask.LayerToName(Collision.gameObject.layer).ToLower().Contains("shared"))
                Char.LookingGrabbable = null;
        }
    }
}
