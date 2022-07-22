using UnityEngine;
using UnityEngine.Events;

namespace Together.Actors
{
    [System.Serializable]
    public class Character
    {
        public Rigidbody2D CharacterObject;
        public int JumpCount;
        public bool InverseCharacter = false;
    }

    public class PlayerController : MonoBehaviour
    {
        #region Static Hooks
        public static PlayerController Instance;
        public static Vector2 ActivePlayerVelocity => Instance.m_ActivePlayerVelocity;
        public static Rigidbody2D ActivePlayer => Instance.m_ActivePlayer;
        public static Rigidbody2D InactivePlayer => Instance.m_InactivePlayer;
        #endregion

        #region Non-Static Hooks
        public Vector2 m_ActivePlayerVelocity => ActiveCharacter ? Shadow.CharacterObject.velocity : Player.CharacterObject.velocity;
        public Rigidbody2D m_ActivePlayer => ActiveCharacter ? Shadow.CharacterObject : Player.CharacterObject;
        public Rigidbody2D m_InactivePlayer => ActiveCharacter ? Player.CharacterObject : Shadow.CharacterObject;

        public Character m_ActiveCharacter => ActiveCharacter ? Shadow : Player;
        public Character m_InactiveCharacter => ActiveCharacter ? Player : Shadow;
        #endregion

        private bool ActiveCharacter = false; // Decides which actor is currently being controlled
        private bool Multiplayer = false; // Use this when we implement multiplayer to disable the desaturation effect

        [SerializeField] private Character Player, Shadow;
        private int Player1Jumps, Player2Jumps; // Scuffed? Yes but this will tell the game how many times a given player has jumped. Prevents a potential bug wher ea player could jump infinitely if they switch quick enough
        [SerializeField] private float PlayerSpeed = 5;
        [SerializeField] private float JumpForce = 250;
        [SerializeField] private int JumpCount = 2; // The maximum amount of times a player can jump. 2 > Double Jump, 3 > Triple Jump, etc
        [Space]
        [SerializeField] private float SwitchAnimationSpeed = 2; // How fast the switch animation is. 1 will make the switch take 1 second, 2 will make it take half a second. 3, a third, so on so forth
        public bool InSync = true;

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

        private void OnGUI()
        {
            GUILayout.Label(new GUIContent($"\"Player\".JumpCount > {Player.JumpCount}\n\"Shadow\".JumpCount > {Shadow.JumpCount}"));
        }

        private void Awake()
        {
            Instance = this;
            Player.CharacterObject.gravityScale = 1;
            Shadow.CharacterObject.gravityScale = -1;
        }

        private void MoveCharacter(Character Character, string HorizontalInput, string VerticalInput, string JumpInput)
        {
            float TargetSpeed = Input.GetAxisRaw(HorizontalInput) * PlayerSpeed;

            Character.CharacterObject.velocity = new Vector2(TargetSpeed, Character.CharacterObject.velocity.y);

            if (IsGrounded)
                Character.JumpCount = JumpCount;

            if (Input.GetButtonDown(JumpInput))
            {
                if (Character.JumpCount > 0)
                {
                    Character.CharacterObject.velocity = new Vector2(Character.CharacterObject.velocity.x, 0);
                    Character.CharacterObject.AddForce(new Vector2(0, Character.InverseCharacter ? -JumpForce : JumpForce));
                    ActivePlayer.GetComponentInChildren<Trigger>().TriggerState = false;
                    Character.JumpCount--;

                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y)) // Placeholder, Y obviously shouldn't be used as it's miles out of the road
            {
                ActiveCharacter = !ActiveCharacter;
            }

            int RemainingJumps = 0;

            if (!InSync)
            { 
                MoveCharacter(m_ActiveCharacter, "Horizontal", "Vertical", "Jump");

                if (ActiveCharacter)
                    Player2Jumps = RemainingJumps;
                else
                    Player1Jumps = RemainingJumps;

                ActivePlayer.constraints = RigidbodyConstraints2D.FreezeRotation;
                InactivePlayer.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

                #region Desaturation. DON'T LOOK AT ME!!!
                if (!Multiplayer)
                {
                    if (!ActiveCharacter)
                        Shadow.CharacterObject.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Shadow.CharacterObject.GetComponent<Renderer>().material.GetFloat("_Saturation"), 0, Time.deltaTime * SwitchAnimationSpeed));
                    else
                        Shadow.CharacterObject.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Shadow.CharacterObject.GetComponent<Renderer>().material.GetFloat("_Saturation"), 1, Time.deltaTime * SwitchAnimationSpeed));

                    if (!ActiveCharacter)
                        Player.CharacterObject.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Player.CharacterObject.GetComponent<Renderer>().material.GetFloat("_Saturation"), 1, Time.deltaTime * SwitchAnimationSpeed));
                    else
                        Player.CharacterObject.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Player.CharacterObject.GetComponent<Renderer>().material.GetFloat("_Saturation"), 0, Time.deltaTime * SwitchAnimationSpeed));
                }
                #endregion
            }
            else
            {
                MoveCharacter(Player, "Horizontal", "Vertical", "Jump");
                MoveCharacter(Shadow, "Horizontal", "Vertical", "Jump");

                Shadow.CharacterObject.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Shadow.CharacterObject.GetComponent<Renderer>().material.GetFloat("_Saturation"), 1, Time.deltaTime * SwitchAnimationSpeed));
                Player.CharacterObject.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Player.CharacterObject.GetComponent<Renderer>().material.GetFloat("_Saturation"), 1, Time.deltaTime * SwitchAnimationSpeed));

                Shadow.CharacterObject.constraints = RigidbodyConstraints2D.FreezeRotation;
                Player.CharacterObject.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
}