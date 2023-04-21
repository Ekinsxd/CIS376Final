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
    public GameObject bulletPrefab;
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

    private float shotCooldown = 0.5f;
    private bool readyToShoot = true;

    /// <summary>
    /// First call upon player creation
    /// </summary>
    private void Start() {
        currentPowerup = Powerup.F;
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

        if (Input.GetMouseButton(0) && elapsedTime > 0.25f) {
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
                if (readyToShoot) {
                    ShootSBullet();
                }
                break;

            case Powerup.M:
                if (readyToShoot) {
                    ShootMBullet();
                }
                break;

            case Powerup.F:
                if (readyToShoot) {
                    ShootFBullet();
                }
                break;

            case Powerup.R: // ???
            case Powerup.None:
            default:
                if (readyToShoot) {
                    GameObject bullet = Instantiate(bulletPrefab, barrelEnd.transform.position, barrelEnd.transform.rotation);
                    elapsedTime = 0;
                }
                break;
        }

        readyToShoot = false;
        Invoke(nameof(ResetShoot), shotCooldown);
    }


    /// <summary>
    /// 
    /// </summary>
    private void ShootFBullet() {
        //GameObject bullet = Instantiate(bulletPrefab, barrelEnd.transform.position, barrelEnd.transform.rotation);
        //Transform center = bullet.transform.parent;

        ////// offset from center
        //Vector3 fPos = bullet.transform.localPosition;
        //fPos.x += bullet.transform.localScale.x;
        //bullet.transform.localPosition = fPos;

        //Debug.Log(bullet.transform.localPosition);
        //// rotate around center
        //bullet.transform.RotateAround(center.position, Vector3.right, 20 * Time.deltaTime);

    }


    /// <summary>
    /// Shoot machine gun
    /// Shoots 5 bullets at once
    /// </summary>
    private void ShootMBullet() {
        float fireDelay = 1.0f;
        float autoTimer = 0.0f;
        for (int i = 0; i < 5; i++) {
            autoTimer -= Time.deltaTime;
            if (autoTimer <= 0) {
                Instantiate(bulletPrefab, barrelEnd.transform.position, barrelEnd.transform.rotation);
                autoTimer += fireDelay;
            }
        }
    }


    /// <summary>
    /// Shoot spread bullets
    /// Shoots multiple bullets at once
    /// </summary>
    private void ShootSBullet() {
        float numBullets = 5.0f; // per axis (5 horizontal, 5 vertical)
        float spreadAngle = 2.0f; // angle between bullets
        float startAngle = -(numBullets * spreadAngle) / 2.0f; // start at half of total spread (negative)

        // Creates a rotation which rotates startAngle degrees around y-axis.
        Quaternion hAngle = Quaternion.AngleAxis(startAngle, orientation.up) * barrelEnd.transform.rotation;
        Quaternion hDelta = Quaternion.AngleAxis(spreadAngle, orientation.up);

        // Creates a rotation which rotates startAngle degrees around x-axis.
        Quaternion vAngle = Quaternion.AngleAxis(startAngle, orientation.right) * barrelEnd.transform.rotation;
        Quaternion vDelta = Quaternion.AngleAxis(spreadAngle, orientation.right);

        for (var i = 0; i < (numBullets * 2); i++) {
            if (i < 5) { // horizontal spread
                Instantiate(bulletPrefab, barrelEnd.transform.position, hAngle);
                hAngle = hDelta * hAngle; // rotate

            } else {
                // vertical spread
                if (i != 8) { // share middle bullet
                    Instantiate(bulletPrefab, barrelEnd.transform.position, vAngle);
                }
                vAngle = vDelta * vAngle; // rotate
            }
        }
    }


    /// <summary>
    /// Set ready to shoot
    /// </summary>
    private void ResetShoot() {
        readyToShoot = true;
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
            Debug.Log($"PLAYER SHOT BY ENEMY");

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

            if (powerUp > 0 && ((Powerup)powerUp) != currentPowerup) {
                currentPowerup = ((Powerup)powerUp);
                Debug.Log($"REACHED POWERUP: {currentPowerup}");
            }
        }
    }

    #endregion EVENTS
}
