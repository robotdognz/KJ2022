using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Together.Actors
{
    public class CameraController : MonoBehaviour
    {
        public Transform Target;
        public float MoveSpeed = 7.5f;

        private void Update()
        {
            Target = PlayerController.ActivePlayer.transform;

            transform.position = Vector3.MoveTowards(transform.position, Target.position, MoveSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }
}