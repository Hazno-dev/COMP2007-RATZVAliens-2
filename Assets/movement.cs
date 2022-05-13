using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class movement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyJump;

    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("SlopeHandle")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode sprintkey = KeyCode.LeftShift;
    public KeyCode crouchkey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    public static float hozInput;
    public static float vertInput;

    [Header("Visuals")]
    public Animator GunAnims;
    public Gun GunScript;

    public Tutorial tut;

    public AudioClip JumpSound;

    //Old Ani code
    //public Transform GunStatic;
    //public Transform GunHolder;
    //private float hoverHeight;
    //private float hoverRange;
    //private float hoverSpeed;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        shooting,
        air
    }

    // Start is called before the first frame update
    void Start()
    {
        //hoverRange = 0.002f;
        //hoverSpeed = 1f;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyJump = true;

        playerHeight = transform.localScale.y;
        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //hoverHeight = GunStatic.position.y;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        //print(grounded);
        //grounded = Physics.CheckCapsule(transform.position, transform.position - new Vector3(0, (playerHeight * 0.25f + 0.2f), 0), 2, whatIsGround);

        myInput();
        statehandle();

        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;

        //Gun bobbin'
        //GunHolder.position = new Vector3(transform.position.x, hoverHeight + Mathf.Cos(Time.time * hoverSpeed) * hoverRange, transform.position.z);

    }
    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    //Input handling
    private void myInput()
    {
        //Debug.Log(Input.GetAxis("Horizontal"));
        //Debug.Log(Input.GetAxis("Vertical"));
        if (PlayerStats.Interacting == true)
        {
            hozInput = 0f;
            vertInput = 0f;
            return;
        }
        hozInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");
        if (hozInput != 0 || vertInput != 0 && Tutorial.InTutorial) tut.TutorialDone("WASD");
        if (Input.GetKey(jumpkey) && grounded && readyJump)
        {
            if (Tutorial.InTutorial) tut.TutorialDone("JUMP");
            readyJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //if (Input.GetKeyDown(crouchkey))
        //{
        //    transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        //    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        //}

        //if (Input.GetKeyUp(crouchkey))
        //{
        //    transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        //    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        //}
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * vertInput + orientation.right * hozInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(getslopemovedirec() * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (grounded) rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded) rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    //Ensures the player consistently has a speed regardless of the terrains verticality
    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed) rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatvel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatvel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        AudioSource.PlayClipAtPoint(JumpSound, transform.position, AudioManager.ReturnVolume() / 20);
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyJump = true;
        exitingSlope = false;
    }

    //Handles inputs and determines what the state of the player currently is
    public void statehandle()
    {
        //if (Input.GetKey(crouchkey) && grounded)
        //{
        //    state = MovementState.crouching;
        //    moveSpeed = crouchSpeed;
        // thanks bug now i have rats
        if(grounded && Input.GetKey(sprintkey))
        {
            state = MovementState.sprinting;
            GunAnims.SetBool("IsRunning", true);
            //hoverSpeed = 3f;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            GunAnims.SetBool("IsRunning", false);
            //hoverSpeed = 1f;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
            //hoverSpeed = 1f;
        }

        if (Input.GetMouseButton(0) && Time.time >= Gun.nextTimeToFire && Gun.Ammo > 0)
        {
            if (PlayerStats.Interacting) return;
            if (Tutorial.InTutorial) tut.TutorialDone("SHOOT");
            Gun.Ammo--;
            Gun.nextTimeToFire = Time.time + 1f / Gun.fireRate;
            //GunAnims["GunFire"].wrapMode = WrapMode.Once;
            GunAnims.SetTrigger("Shoot");
            GunScript.Shoot();
        }

    }

    //Checks if the player is sloped, changes velocity direction/vectors accordingly
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //print(angle);
            return angle < maxSlopeAngle && angle > 10;
        }
        return false;
    }

    private Vector3 getslopemovedirec()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    
}
