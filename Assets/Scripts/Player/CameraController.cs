using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Together.Actors
{
    public class CameraController : MonoBehaviour
    {
        public Transform Player, PlayerCamera;
        public Transform Shadow, ShadowCamera;
        public float MoveSpeed = 7.5f;

        private void Update()
        {
            Player = PlayerController.Instance.Player.CharacterObject.transform;
            Shadow = PlayerController.Instance.Shadow.CharacterObject.transform;

            PlayerCamera.position = Vector3.MoveTowards(PlayerCamera.position, Player.position, MoveSpeed * Time.deltaTime);
            ShadowCamera.position = Vector3.MoveTowards(ShadowCamera.position, Shadow.position, MoveSpeed * Time.deltaTime);

            PlayerCamera.position = new Vector3(PlayerCamera.position.x, PlayerCamera.position.y, -10);
            ShadowCamera.position = new Vector3(ShadowCamera.position.x, ShadowCamera.position.y, -10);
        }
    }
}