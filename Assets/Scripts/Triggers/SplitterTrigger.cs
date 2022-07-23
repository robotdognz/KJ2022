using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterTrigger : MonoBehaviour
{
    private bool DoneSplit = false;
    public float WaitLength = 3;
    public Trigger TargetTrigger;

    private void Awake()
    {
        TargetTrigger.TriggerEnterCol += (Collider2D Collider) =>
        {
            Together.Actors.PlayerController PC;

            if (PC = Collider.GetComponentInParent<Together.Actors.PlayerController>())
            {
                if (Collider == PC.Player.CharacterObject.GetComponent<Collider2D>())
                {
                    PC.Player.IsWaitingForSplit = true;
                    PC.Player.WaitTime = WaitLength;
                }
                else if (Collider == PC.Shadow.CharacterObject.GetComponent<Collider2D>())
                {
                    PC.Shadow.IsWaitingForSplit = true;
                    PC.Shadow.WaitTime = WaitLength;
                }
            }
            else
            {
                if (Collider.GetComponent<GrabbableObject>())
                {
                    if (Collider.gameObject.layer == Together.Actors.PlayerController.Instance.SharedLayerRef.layer)
                    {
                        GameObject LightBox = Instantiate(Collider.gameObject, Collider.transform.position, Collider.transform.rotation);
                        GameObject DarkBox = Instantiate(Collider.gameObject, Collider.transform.position, Collider.transform.rotation);

                        LightBox.layer = Together.Actors.PlayerController.Instance.LightLayerRef.layer;
                        DarkBox.layer = Together.Actors.PlayerController.Instance.DarkLayerRef.layer;

                        Destroy(Collider.gameObject);
                    }
                }
            }
        };

        TargetTrigger.TriggerExitCol += (Collider2D Collider) =>
        {
            Together.Actors.PlayerController PC;

            if (PC = Collider.GetComponentInParent<Together.Actors.PlayerController>())
            {
                if (Collider == PC.Player.CharacterObject.GetComponent<Collider2D>())
                {
                    PC.Player.IsWaitingForSplit = false;
                }
                else if (Collider == PC.Shadow.CharacterObject.GetComponent<Collider2D>())
                {
                    PC.Shadow.IsWaitingForSplit = false;
                }
            }
        };
    }
}
