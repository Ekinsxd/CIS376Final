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

    public Powerup currentPowerup;
    public BulletController bulletPrefab;
    public GameObject barrelEnd;
    public GameObject gun;

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

    GameController gc;


    /// <summary>
    /// First call upon player creation
    /// </summary>
    private void Start() {
        currentPowerup = Powerup.None;
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
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
    /// Get player input for jumping and shooting
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
            ShootWeapon();
        }
    }


    #region PLAYER MOVEMENT


    /// <summary>
    /// Move player in move direction
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
    /// Player jump physics
    /// </summary>
    private void Jump() {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    /// <summary>
    /// Reset jump
    /// </summary>
    private void ResetJump() {
        readyToJump = true;
    }


    #endregion PLAYER MOVEMENT

    #region WEAPON

    /// <summary>
    /// Shoot weapon depending on current powerup
    /// </summary>
    private void ShootWeapon() {
        switch (currentPowerup) {
            case Powerup.S:
                ShootSBullet();
                break;

            case Powerup.M:
                ShootMBullet();
                break;

            case Powerup.R:
            case Powerup.F:
            case Powerup.None:
            default:
                BulletController bullet = Instantiate(bulletPrefab, barrelEnd.transform.position, barrelEnd.transform.rotation);
                elapsedTime = 0;
                break;
        }

    }


    /// <summary>
    /// 
    /// </summary>
    private void ShootMBullet() {

    }


    /// <summary>
    /// Shoot spread bullets
    /// Shoots multiple bullets at once
    /// </summary>
    private void ShootSBullet() {
        float spreadAngle = 2.0f;       // Angle between shots
        float timeBetweenShots = 0.5f;  // Minimum time between shots
        float nextShot = 0.0f;

        nextShot = Time.time + timeBetweenShots;
        var hAngle = Quaternion.AngleAxis(-2.5f * spreadAngle, transform.up) * transform.rotation;
        var hDelta = Quaternion.AngleAxis(spreadAngle, transform.up);

        var vAngle = Quaternion.AngleAxis(-2.5f * spreadAngle, transform.right) * transform.rotation;
        var vDelta = Quaternion.AngleAxis(spreadAngle, transform.right);

        for (var i = 0; i < 10; i++) {
            if (i < 5) {
                Instantiate(bulletPrefab, barrelEnd.transform.position, hAngle);
                hAngle = hDelta * hAngle;
            } else {
                Instantiate(bulletPrefab, barrelEnd.transform.position, vAngle);
                vAngle = vDelta * vAngle;
            }
        }
    }

    #endregion WEAPON

    #region EVENTS

    /// <summary>
    /// Handle collisions
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        // enemy bullet
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if (other.tag == "Bullet" && other.gameObject.layer == enemyLayer) {
            gc.LoseLife();
        }

        // powerup
        int powerupLayer = LayerMask.NameToLayer("Powerup");
        if (other.gameObject.layer == powerupLayer) {
            int powerUp = 0;
            switch (other.tag) {
                case "S":
                    powerUp = 1;
                    break;
                case "M":
                    powerUp = 2;
                    break;
                case "R":
                    powerUp = 3;
                    break;
                case "F":
                    powerUp = 4;
                    break;
                default:
                    break;
            }

            Debug.Log(((Powerup)powerUp));
            if (powerUp > 0 && ((Powerup)powerUp) != currentPowerup) {
                currentPowerup = ((Powerup)powerUp);
            }
        }
    }

    #endregion EVENTS
}
