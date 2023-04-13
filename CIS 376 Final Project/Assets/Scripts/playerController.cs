using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Player controller
/// </summary>
public class playerController : MonoBehaviour {

    #region Movement
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    
    #endregion Movement


    /// <summary> Gun bullets </summary>
    public GameObject bullet;
    public GameObject barrelEnd;
    public GameObject gun;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode shootKey = KeyCode.F;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    protected float elapsedTime = 0;


    /// <summary>
    /// 
    /// </summary>
    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        bullet = Resources.Load("Bullet", typeof(GameObject)) as GameObject;
        readyToJump = true;
    }


    /// <summary>
    /// Update player game object
    /// </summary>
    private void Update() {
        elapsedTime += Time.deltaTime;

        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        //Debug.Log("Grounded: " + grounded + "");
        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }


    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate() {
        MovePlayer();
    }


    /// <summary>
    /// 
    /// </summary>
    private void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKey(shootKey) && elapsedTime > 0.25f) {
            Instantiate(bullet, barrelEnd.transform.position, barrelEnd.transform.rotation);
            elapsedTime = 0;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    private void MovePlayer() {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // in air
        else if (!grounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    private void Jump() {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    /// <summary>
    /// 
    /// </summary>
    private void ResetJump() {
        readyToJump = true;
    }
}
