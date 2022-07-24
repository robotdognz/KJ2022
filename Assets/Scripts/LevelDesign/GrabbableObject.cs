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
        _Trigger.TriggerExitCol += TriggerExit;
    }

    private void TriggerStay(Collider2D Collision)
    {
        if (Collision.GetComponentInParent<PlayerController>())
        {
            Character Char = PlayerController.GetCharacterByCollider(Collision);

            if (Char != null && Char.CharacterObject.gameObject.layer == gameObject.layer || LayerMask.LayerToName(gameObject.layer).ToLower().Contains("shared"))
                Char.LookingGrabbable = gameObject;
        }
    }

    private void TriggerExit(Collider2D Collision)
    {
        if (Collision.GetComponentInParent<PlayerController>())
        {
            Character Char = PlayerController.GetCharacterByCollider(Collision);

            if (Char != null && Char.LookingGrabbable == gameObject)
                Char.LookingGrabbable = null;
        }
    }

    private float SampleCurve(float Time, float Start, float Peak, float Distance = 1) 
    {
        return new AnimationCurve(new Keyframe(0, Start), new Keyframe(Distance, Peak)).Evaluate(Time);
    }

    public void DropCube(float Direction, Vector2 StartPos) 
    {
        StartCoroutine(DropFunction(Direction, StartPos));
    }

    private IEnumerator DropFunction(float Direction, Vector2 Start) 
    {
        Debug.Log($"StartX: {Start.x}, StartY: {Start.y}");
        float T = 0;

        GetComponent<Rigidbody2D>().simulated = false;

        while (T < 1f) 
        {
            T += Time.deltaTime * 4;

            transform.position = (new Vector2(Start.x + (T * 1.1f * Direction), SampleCurve(T, Start.y, Start.y + 0.5f, 1.1f)));

            yield return null;
        }
        
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().simulated = true;
    }
}
