using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Together.Levels
{
    public class SidedObjectBehaviour : MonoBehaviour
    {
        public bool TargetSide; // False > Top, True > Bottom
        [SerializeField] private float LightTransitionSpeed = 1;

        protected virtual void ObjectLit()
        {
            Light2D AttachedLight = GetComponent<Light2D>();

            if (AttachedLight)
            {
                AttachedLight.intensity = Mathf.MoveTowards(AttachedLight.intensity, 1, Time.deltaTime * LightTransitionSpeed);
            }
        }

        protected virtual void ObjectUnlit()
        {
            Light2D AttachedLight = GetComponent<Light2D>();

            if (AttachedLight)
            {
                AttachedLight.intensity = Mathf.MoveTowards(AttachedLight.intensity, 0, Time.deltaTime * LightTransitionSpeed);
            }
        }

        private void Update()
        {
            if (Actors.PlayerController.Instance.ActiveCharacter == TargetSide || Actors.PlayerController.Instance.InSync)
                ObjectLit();
            else
                ObjectUnlit();
        }
    }
}