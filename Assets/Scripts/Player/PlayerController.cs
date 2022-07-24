using UnityEngine;
using UnityEngine.Events;

namespace Together.Actors
{
    [System.Serializable]
    public class Character
    {
        public Vector2 StartPosition;
        public Rigidbody2D CharacterObject;
        public int JumpCount;
        public bool InverseCharacter = false;
        public bool IsInLight = false;
        public Material Renderer;
        public float ShadowDeathTime = 3;
        private float m_ShadowDeathTimer;
        public float MovespeedMultiplier = 1;

        public bool IsWaitingForSplit;
        public float WaitTime;
        public float CurrentWait;
        public GameObject LookingGrabbable;

        public float ShadowDeathTimer
        {
            get
            {
                return m_ShadowDeathTimer;
            }
            set
            {
                Renderer.SetFloat("_DeathVignetteOpacity", Mathf.Lerp(1, 0, value));
                m_ShadowDeathTimer = value;
            }
        }

        public GameObject GrabbedObject
        {
            get
            {
                if (PickupPos.childCount > 0)
                    return PickupPos.GetChild(0).gameObject;

                return null;
            }
        }
        public Transform PickupPos;
        public bool GrabbedObjectThisFrame = false;
    }

    public class PlayerController : MonoBehaviour
    {
        #region Static Hooks
        public static PlayerController Instance;
        public static Vector2 ActivePlayerVelocity => Instance.m_ActivePlayerVelocity;
        public static Rigidbody2D ActivePlayer => Instance.m_ActivePlayer;
        public static Rigidbody2D InactivePlayer => Instance.m_InactivePlayer;

        public static Character GetCharacterByCollider(Collider2D Collider)
        {
            Rigidbody2D RB2D;

            if (RB2D = Collider.GetComponent<Rigidbody2D>())
            {
                if (RB2D == Instance.Player.CharacterObject)
                    return Instance.Player;
                else if (RB2D == Instance.Shadow.CharacterObject)
                    return Instance.Shadow;
            }

            return null;
        }
        #endregion

        #region Non-Static Hooks
        public Vector2 m_ActivePlayerVelocity => ActiveCharacter ? Shadow.CharacterObject.velocity : Player.CharacterObject.velocity;
        public Rigidbody2D m_ActivePlayer => ActiveCharacter ? Shadow.CharacterObject : Player.CharacterObject;
        public Rigidbody2D m_InactivePlayer => ActiveCharacter ? Player.CharacterObject : Shadow.CharacterObject;

        public Character m_ActiveCharacter => ActiveCharacter ? Shadow : Player;
        public Character m_InactiveCharacter => ActiveCharacter ? Player : Shadow;
        #endregion

        public bool ActiveCharacter { get; private set; } = false; // Decides which actor is currently being controlled
        public bool Multiplayer = false; // Use this when we implement multiplayer to disable the desaturation effect

        public Character Player, Shadow;
        private int Player1Jumps, Player2Jumps; // Scuffed? Yes but this will tell the game how many times a given player has jumped. Prevents a potential bug wher ea player could jump infinitely if they switch quick enough
        [SerializeField] private float PlayerSpeed = 5;
        [SerializeField] private float JumpForce = 250;
        [SerializeField] private int JumpCount = 2; // The maximum amount of times a player can jump. 2 > Double Jump, 3 > Triple Jump, etc
        [Space]
        [SerializeField] private float SwitchAnimationSpeed = 2; // How fast the switch animation is. 1 will make the switch take 1 second, 2 will make it take half a second. 3, a third, so on so forth
        public bool InSync = true;
        [SerializeField] private Vector2 DropObjectForce = new Vector2(10, 150);
        [Space]
        public GameObject SharedLayerRef;
        public GameObject LightLayerRef;
        public GameObject DarkLayerRef;
        [SerializeField] public Animator SplitSreenUI;

        // Check if player is grounded and return true
        private bool IsGrounded() => ActivePlayer.GetComponentInChildren<Trigger>().TriggerState;

        public bool IsGrounded(Character Char) => Char.CharacterObject.GetComponentInChildren<Trigger>().TriggerState;

        private void Awake()
        {
            Instance = this;

            Player.ShadowDeathTimer = 1;
            Shadow.ShadowDeathTimer = 1;

            Player.StartPosition = Player.CharacterObject.transform.position;
            Shadow.StartPosition = Shadow.CharacterObject.transform.position;

            Shadow.CharacterObject.gameObject.SetActive(false);
        }

        public void SplitPlayer(Collider2D Collider)
        {
            if (Shadow.CharacterObject.gameObject.activeSelf == false)
            {
                Shadow.CharacterObject.transform.position = Player.CharacterObject.transform.position;
                Shadow.CharacterObject.velocity = Player.CharacterObject.velocity;
                Shadow.StartPosition = Shadow.CharacterObject.transform.position;
            }

            Shadow.CharacterObject.gameObject.SetActive(true);
        }

        public void JoinPlayers()
        {
            Shadow.CharacterObject.gameObject.SetActive(false);
            ActiveCharacter = false;
        }

        public void PickupObject(Character Char, Transform Obj)
        {
            if (Char != null && Obj)
            {
                if (Char.GrabbedObject == null)
                {
                    if (Obj.GetComponent<Rigidbody2D>())
                        Obj.GetComponent<Rigidbody2D>().simulated = false;

                    Obj.transform.parent = Char.PickupPos;
                    Obj.transform.localPosition = Vector3.zero;

                    Char.GrabbedObjectThisFrame = true;
                }
            }
        }

        public void DropObject(Character Char)
        {
            if (Char.GrabbedObject && !Char.GrabbedObjectThisFrame)
            {
                if (Char.GrabbedObject.GetComponent<Rigidbody2D>())
                    Char.GrabbedObject.GetComponent<Rigidbody2D>().simulated = true;

                Transform GrabbedObject = Char.GrabbedObject.transform;
                GrabbedObject.parent = null;

                if (GrabbedObject.GetComponent<Rigidbody2D>())
                    GrabbedObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(DropObjectForce.x * (Char.CharacterObject.GetComponent<SpriteRenderer>().flipX ? -1 : 1), DropObjectForce.y));
            }
        }

        private void MoveCharacter(Character Character, string HorizontalInput, string VerticalInput, string JumpInput, string GrabInput)
        {
            float TargetSpeed = Input.GetAxisRaw(HorizontalInput) * PlayerSpeed;

            bool DidMovementInput = false;

            Character.CharacterObject.velocity = new Vector2(TargetSpeed * Character.MovespeedMultiplier, Character.CharacterObject.velocity.y);

            if (Character.CharacterObject.velocity.x > 0)
                Character.CharacterObject.GetComponent<SpriteRenderer>().flipX = false;
            else if (Character.CharacterObject.velocity.x < 0)
                Character.CharacterObject.GetComponent<SpriteRenderer>().flipX = true;

            if (IsGrounded(Character))
                Character.JumpCount = JumpCount;

            if (Input.GetButtonDown(GrabInput))
            {
                if (Character.GrabbedObject)
                {
                    DropObject(Character);
                }
                else
                {
                    PickupObject(Character, Character.LookingGrabbable.transform);
                }
                DidMovementInput = true;
            }

            bool JumpBtn = false;

            if (Input.GetButtonDown(JumpInput))
            {
                if (Character.JumpCount > 0)
                {
                    Character.CharacterObject.velocity = new Vector2(Character.CharacterObject.velocity.x, 0);
                    Character.CharacterObject.AddForce(new Vector2(0, Character.InverseCharacter ? -JumpForce : JumpForce));
                    // ActivePlayer.GetComponentInChildren<Trigger>().TriggerState = false;
                    Character.JumpCount--;
                    JumpBtn = true;
                }

                DidMovementInput = true;
            }

            if (Character.CharacterObject.velocity.x != 0)
                DidMovementInput = true;

            if (Character.IsWaitingForSplit && !DidMovementInput)
            {
                Character.CurrentWait -= Time.deltaTime;

                if (Character.CurrentWait <= 0)
                {
                    SplitPlayer(Character.CharacterObject.GetComponent<Collider2D>());
                    Character.IsWaitingForSplit = false;
                }
            }
            else if (DidMovementInput && Character.IsWaitingForSplit)
            {
                Character.CurrentWait = Character.WaitTime;
            }

            Animator Anim;
            if (Anim = Character.CharacterObject.GetComponent<Animator>())
            {
                Anim.SetBool("Moving", Character.CharacterObject.velocity.x != 0);
                Anim.SetBool("Grounded", IsGrounded(Character));
                if (JumpBtn)
                    Anim.SetTrigger("Jump");
            }

            Character.GrabbedObjectThisFrame = false;
        }

        public void SynchronizePlayerLocations()
        {
            InactivePlayer.transform.position = new Vector3(ActivePlayer.transform.position.x, ActivePlayer.transform.position.y * (m_InactiveCharacter.InverseCharacter ? -1 : 1), InactivePlayer.transform.position.z);
            InactivePlayer.velocity = new Vector2(ActivePlayer.velocity.x, ActivePlayer.velocity.y * (m_InactiveCharacter.InverseCharacter ? -1 : 1));
        }

        public void ResetPlayer(int Target)
        {
            Target = Mathf.Clamp(Target, 0, 2);

            switch (Target)
            {
                case 0:
                    Player.CharacterObject.transform.position = Player.StartPosition;
                    break;
                case 1:
                    Shadow.CharacterObject.transform.position = Shadow.StartPosition;
                    break;
                case 2:
                    Player.CharacterObject.transform.position = Player.StartPosition;
                    Shadow.CharacterObject.transform.position = Shadow.StartPosition;
                    break;
            }
        }

        private void Update()
        {
            int RemainingJumps = 0;

            if (SplitSreenUI)
                SplitSreenUI.SetBool("SplitScreen", Shadow.CharacterObject.gameObject.activeSelf);

            if (Input.GetKeyDown(KeyCode.JoystickButton7))
            {
                Multiplayer = !Multiplayer;
            }

            if (Shadow.CharacterObject.gameObject.activeSelf)
            {
                if (!InSync)
                {
                    if (!Multiplayer)
                    {
                        if (Input.GetKeyDown(KeyCode.Tab))
                        {
                            ActiveCharacter = !ActiveCharacter;
                        }

                        ActivePlayer.GetComponent<Animator>().speed = 1;
                        InactivePlayer.GetComponent<Animator>().speed = 0;

                        MoveCharacter(m_ActiveCharacter, "Horizontal", "Vertical", "Jump", "Grab");

                        if (ActiveCharacter)
                            Player2Jumps = RemainingJumps;
                        else
                            Player1Jumps = RemainingJumps;

                        ActivePlayer.constraints = RigidbodyConstraints2D.FreezeRotation;
                        InactivePlayer.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    }
                    else
                    {
                        ActivePlayer.GetComponent<Animator>().speed = 1;
                        InactivePlayer.GetComponent<Animator>().speed = 1;

                        MoveCharacter(Player, "Horizontal", "Vertical", "Jump", "Grab");
                        MoveCharacter(Shadow, "JoyHorizontal", "JoyVertical", "JoyJump", "JoyGrab");

                        Player.CharacterObject.constraints = RigidbodyConstraints2D.FreezeRotation;
                        Shadow.CharacterObject.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }

                    #region Desaturation. DON'T LOOK AT ME!!!
                    if (!Multiplayer)
                    {
                        if (!ActiveCharacter)
                            Shadow.CharacterObject.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Shadow.CharacterObject.GetComponent<Renderer>().material.GetFloat("_Saturation"), 0, Time.deltaTime * SwitchAnimationSpeed));
                        else
                            Shadow.CharacterObject.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Shadow.CharacterObject.GetComponent<Renderer>().material.GetFloat("_Saturation"), 1, Time.deltaTime * SwitchAnimationSpeed));
                    }
                    else
                        Shadow.CharacterObject.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Shadow.CharacterObject.GetComponent<Renderer>().material.GetFloat("_Saturation"), 1, Time.deltaTime * SwitchAnimationSpeed));

                    #endregion
                }
                else
                {
                    MoveCharacter(Player, "Horizontal", "Vertical", "Jump", "Grab");
                    MoveCharacter(Shadow, "Horizontal", "Vertical", "Jump", "Grab");

                    Shadow.CharacterObject.GetComponent<Renderer>().material.SetFloat("_Saturation", Mathf.MoveTowards(Shadow.CharacterObject.GetComponent<Renderer>().material.GetFloat("_Saturation"), 1, Time.deltaTime * SwitchAnimationSpeed));
                    
                    Shadow.CharacterObject.constraints = RigidbodyConstraints2D.FreezeRotation;
                    Player.CharacterObject.constraints = RigidbodyConstraints2D.FreezeRotation;
                }

                #region ShadowBehaviour
                if (!Shadow.IsInLight)
                {
                    Shadow.CharacterObject.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
                    Shadow.MovespeedMultiplier = 0.3f;
                    Shadow.ShadowDeathTimer -= Time.deltaTime / Shadow.ShadowDeathTime;
                }
                else
                {
                    Shadow.CharacterObject.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>().enabled = true;
                    Shadow.MovespeedMultiplier = 1;
                    Shadow.ShadowDeathTimer += Time.deltaTime / (Shadow.ShadowDeathTime / 2);
                }

                Shadow.ShadowDeathTimer = Mathf.Clamp01(Shadow.ShadowDeathTimer);

                if (Shadow.ShadowDeathTimer <= 0 || Shadow.ShadowDeathTimer <= 0)
                {
                    Shadow.ShadowDeathTimer = 1;
                    Shadow.CharacterObject.transform.position = Player.CharacterObject.transform.position;
                    Shadow.CharacterObject.velocity = Player.CharacterObject.velocity;
                }
                #endregion
            }
            else
            {
                Player.CharacterObject.GetComponent<Animator>().speed = 1;
                MoveCharacter(Player, "Horizontal", "Vertical", "Jump", "Grab");
            }
        }
    }
}