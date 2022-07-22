using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Together.Actors
{
    public class PlayerController : MonoBehaviour
    {
        private bool ActiveCharacter = false; // Decides which actor is currently being controlled

        [SerializeField] private Rigidbody2D Player, Shadow;
        [SerializeField] private float PlayerSpeed;
        [SerializeField] private float JumpForce;

        private void Awake()
        {
            Player.gravityScale = 1;
            Shadow.gravityScale = -1;
        }

        private void Update()
        {
            float TargetSpeed = Input.GetAxisRaw("Horizontal") * PlayerSpeed;
            
            if (Input.GetKeyDown(KeyCode.Y)) // Placeholder, Y obviously shouldn't be used as it's miles out of the road
            {
                ActiveCharacter = !ActiveCharacter;
            }

            Rigidbody2D ActiveRB = ActiveCharacter ? Shadow : Player;

            ActiveRB.velocity = new Vector2(TargetSpeed, ActiveRB.velocity.y);

            if (Input.GetButtonDown("Jump") || (ActiveCharacter ? Input.GetAxisRaw("Vertical") < 0 : Input.GetAxisRaw("Vertical") > 0)) 
            {
                ActiveRB.AddForce(new Vector2(0, ActiveCharacter ? -JumpForce : JumpForce));
            }
        }
    }
}