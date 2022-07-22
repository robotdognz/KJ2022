using UnityEngine;
using UnityEngine.Events;

namespace Together.Actors
{
    public class PlayerController : MonoBehaviour
    {
        #region Static Hooks
        public static PlayerController Instance;
        public static Vector2 ActivePlayerVelocity => Instance.m_ActivePlayerVelocity;
        public static Rigidbody2D ActivePlayer => Instance.m_ActivePlayer;
        public static Rigidbody2D InactivePlayer => Instance.m_InactivePlayer;
        #endregion

        #region Non-Static Hooks
        public Vector2 m_ActivePlayerVelocity => ActiveCharacter ? Shadow.velocity : Player.velocity;
        public Rigidbody2D m_ActivePlayer => ActiveCharacter ? Shadow : Player;
        public Rigidbody2D m_InactivePlayer => ActiveCharacter ? Player : Shadow;
        #endregion

        private bool ActiveCharacter = false; // Decides which actor is currently being controlled
        private bool Multiplayer = false; // Use this when we implement multiplayer to disable the desaturation effect

        [SerializeField] private Rigidbody2D Player, Shadow;
        [SerializeField] private float PlayerSpeed;
        [SerializeField] private float JumpForce;
        [Space]
        [SerializeField] private float SwitchAnimationSpeed = 2;

        // Check if player is grounded and return true
        private bool IsGrounded
        {
            get
            {
                if (ActivePlayerVelocity.y == 0 && ActivePlayer.GetComponentInChildren<Trigger>().TriggerState) 
                {
                    return true;
                }

                return false;
            }
        }

        private void Awake()
        {
            Instance = this;
            Player.gravityScale = 1;
            Shadow.gravityScale = -1;
        }

        private void MoveCharacter(Rigidbody2D Character, string HorizontalInput, string VerticalInput, string JumpInput)
        {
            float TargetSpeed = Input.GetAxisRaw(HorizontalInput) * PlayerSpeed;

            Character.velocity = new Vector2(TargetSpeed, Character.velocity.y);

            if (IsGrounded)
                if (Input.GetButtonDown(JumpInput) || (ActiveCharacter ? Input.GetAxisRaw(VerticalInput) < 0 : Input.GetAxisRaw(VerticalInput) > 0))
                {
                    Character.AddForce(new Vector2(0, ActiveCharacter ? -JumpForce : JumpForce));
                    ActivePlayer.GetComponentInChildren<Trigger>().TriggerState = false;
                }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y)) // Placeholder, Y obviously shouldn't be used as it's miles out of the road
            {
                ActiveCharacter = !ActiveCharacter;
            }

            MoveCharacter(ActivePlayer, "Horizontal", "Vertical", "Jump");

            ActivePlayer.constraints = RigidbodyConstraints2D.FreezeRotation;
            InactivePlayer.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            #region Desaturation. DON'T LOOK AT ME!!!
            if (!Multiplayer)
            {
                if (!ActiveCharacter)
                    Shadow.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Shadow.GetComponent<Renderer>().material.GetFloat("_Saturation"), 0, Time.deltaTime * SwitchAnimationSpeed));
                else
                    Shadow.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Shadow.GetComponent<Renderer>().material.GetFloat("_Saturation"), 1, Time.deltaTime * SwitchAnimationSpeed));

                if (!ActiveCharacter)
                    Player.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Player.GetComponent<Renderer>().material.GetFloat("_Saturation"), 1, Time.deltaTime * SwitchAnimationSpeed));
                else
                    Player.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Player.GetComponent<Renderer>().material.GetFloat("_Saturation"), 0, Time.deltaTime * SwitchAnimationSpeed));
            }
            #endregion
        }
    }
}