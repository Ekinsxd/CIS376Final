using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using BulletTypes;

/// <summary>
/// Player controller
/// </summary>
public class PlayerController : MonoBehaviour {

    #region Movement
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    #endregion Movement

    # region Bullets/Gun
    [Header("Bullets/Gun")]
    
    public BulletType bulletType;
    public BulletController bulletPrefab;
    public GameObject barrelEnd;
    public GameObject gun;
    [HideInInspector] public GameObject hCrossHair;
    [HideInInspector] public GameObject vCrossHair;
    
    #endregion Bullets/Gun

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey;
    public KeyCode shootKey;

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
    /// </summary>ww
    private void Start() {
        // normal gun to start
        bulletType = BulletType.Normal;
        //hCrossHair = Resources.Load("horizon_crosshir", typeof(GameObject)) as GameObject;
        //vCrossHair = Resources.Load("vertical_crosshir", typeof(GameObject)) as GameObject;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }


    /// <summary>
    /// Update player game object
    /// </summary>
    private void Update() {
        elapsedTime += Time.deltaTime;

        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, whatIsGround);

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
            BulletController bullet = Instantiate(bulletPrefab, barrelEnd.transform.position, barrelEnd.transform.rotation);
            bullet.bulletType = this.bulletType;
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
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Acceleration);
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


    #region Events

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        Debug.Log($"{this.name} hit {other.name}!");

        int powerUp = 6;
        switch (other.name) {
            case "Normal":
                powerUp = 0;
                break;
            case "B":
                powerUp = 1;
                break;
            case "S":
                powerUp = 2;
                break;
            case "M":
                powerUp = 3;
                break;
            case "R":
                powerUp = 4;
                break;
            case "F":
                powerUp = 5;
                break;
            default:
                Debug.Log("unknown bullet type");
                break;
        }

        if (bulletType == ((BulletType)powerUp)) {
            Debug.Log($"BULLET TYPE: {bulletType} POWERUP: {powerUp}");
        }

    }

    #endregion Events
}
