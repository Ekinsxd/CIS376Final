using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Player controller
/// </summary>
public class PlayerController : MonoBehaviour {

    // SHOOT SPORADICALLY (use f bullet for inspo)
    // FIX JUMPING

    #region Movement
    private const float playerHeight = 2.5f;
    private const float groundDrag = 4;
    private const float jumpForce = 175;
    private const float jumpCooldown = 1;
    //private const float airMultiplier = 0.4f;

    private float horizontalInput;
    private float verticalInput;
    private float moveSpeed = 8;

    private bool readyToJump;

    public Transform orientation;
    private Vector3 moveDirection;
    private Rigidbody rb;
    #endregion Movement

    #region Powerups/Bullets

    [Header("Powerups/Bullets")]
    public GameObject bulletPrefab;
    public GameObject bulletFPrefab;
    public GameObject barrelEnd;

    private Powerup currentPowerup;
    private bool readyToShoot;

    #endregion Powerups/Bullets

    [Header("Ground/Water Check")]
    public LayerMask groundLayer;
    private bool grounded;
    private bool inWater;
    private GameController gc;
    private float iFrames;
    private const KeyCode jumpKey = KeyCode.Space;

    /// <summary>
    /// First call upon player creation
    /// </summary>
    private void Start() {
        currentPowerup = Powerup.None;
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        readyToShoot = true;
        iFrames = 0;
    }


    /// <summary>
    /// Update player game object
    /// </summary>
    private void Update() {
        iFrames -= Time.deltaTime;

        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundLayer);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded) {
            rb.drag = groundDrag;
            rb.mass = 10;
        } else
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

        bool running = grounded && (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift));
        if (running) {
            moveSpeed = 10;
        } else {
            moveSpeed = 8;
        }

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // shoot weapon
        if (Input.GetMouseButton(0) && readyToShoot && !running) {
            readyToShoot = false;
            ShootWeapon();
            Invoke(nameof(ResetShoot), GetShotCooldown());
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
        if (grounded) {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        if (rb.velocity.y >= 0) { // in air
            rb.mass = 10;
        } else if (rb.velocity.y < 0) { // falling
            rb.mass = 80;
        }
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

            case Powerup.F:
                ShootFBullet();
                break;

            case Powerup.M:
            case Powerup.R:
            case Powerup.None:
            default:
                Instantiate(bulletPrefab, barrelEnd.transform.position, barrelEnd.transform.rotation);
                break;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    private void ShootFBullet() {
        Instantiate(bulletFPrefab, barrelEnd.transform.position, barrelEnd.transform.rotation);
    }



    /// <summary>
    /// Shoot spread bullets
    /// Shoots multiple bullets at once
    /// </summary>
    private void ShootSBullet() {
        int numBullets = 10;
        float spreadAngle = 1.0f; // angle between bullets
        float totalSpread = numBullets * spreadAngle;

        float startAngle = -(totalSpread) / 2.0f; // start at half of total spread
        float endAngle = startAngle + totalSpread;

        for (var i = 0; i < numBullets; i++) {
            Quaternion rotation = barrelEnd.transform.rotation * Quaternion.Euler(new Vector3(Random.Range(startAngle, endAngle), Random.Range(startAngle, endAngle), 0));
            Instantiate(bulletPrefab, barrelEnd.transform.position, rotation);
        }
    }


    /// <summary>
    /// Set ready to shoot
    /// </summary>
    private void ResetShoot() {
        readyToShoot = true;
    }


    /// <summary>
    /// Time between shots
    /// Shortened for machine gun powerup
    /// </summary>
    /// <returns></returns>
    private float GetShotCooldown() {

        switch (currentPowerup) {
            case Powerup.S:
                return 1f;
            case Powerup.F:
                return 0.35f;
            case Powerup.M:
                return 0.15f;
            case Powerup.R:
                return 0.15f / 2;
            case Powerup.None:
            default:
                return 0.5f;
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
        if (other.tag == "EnemyBullet") {
            if (iFrames > 0) {
                return;
            }
            gc.LoseLife();
            // 1 second of invincibility
            iFrames = 1f;
            Debug.Log($"PLAYER SHOT BY ENEMY");
            //Runner is also an enemy 
            Destroy(other.gameObject);

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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other) {
        if (other.collider.tag == "EnemyBullet") {
            if (iFrames > 0) {
                return;
            }
            gc.LoseLife();
            // 1 second of invincibility
            iFrames = 1f;
            Debug.Log($"PLAYER TACKLED BY ENEMY");
            //Runner is also an enemy 
            Destroy(other.gameObject);
        }
    }

    #endregion EVENTS
}
