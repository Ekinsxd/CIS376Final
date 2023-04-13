﻿using UnityEngine;
using System.Collections;
//using UnityStandardAssets.ImageEffects;

public enum GunStyles {
    nonautomatic,
    automatic
}

public class GunScript : MonoBehaviour {
    [Tooltip("Selects type of waepon to shoot rapidly or one bullet per click.")]
    public GunStyles currentStyle;
    [HideInInspector]
    public MouseLookScript mls;

    [Header("Player movement properties")]
    [Tooltip("Speed is determined via gun because not every gun has same properties or weights so you MUST set up your speeds here")]
    public int walkingSpeed = 3;
    [Tooltip("Speed is determined via gun because not every gun has same properties or weights so you MUST set up your speeds here")]
    public int runningSpeed = 5;

    private Transform player;
    private Camera cameraComponent;
    private Transform gunPlaceHolder;

    private PlayerMovementScript pmS;

    /*
	 * Collection the variables upon awake that we need.
	 */
    void Awake() {
        mls = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLookScript>();
        player = mls.transform;
        mainCamera = mls.myCamera;
        secondCamera = GameObject.FindGameObjectWithTag("SecondCamera").GetComponent<Camera>();
        cameraComponent = mainCamera.GetComponent<Camera>();
        pmS = player.GetComponent<PlayerMovementScript>();

        bulletSpawnPlace = GameObject.FindGameObjectWithTag("BulletSpawn");
        hitMarker = transform.Find("hitMarkerSound").GetComponent<AudioSource>();

        startLook = mouseSensitvity_notAiming;
        startAim = mouseSensitvity_aiming;
        startRun = mouseSensitvity_running;

        rotationLastY = mls.currentYRotation;
        rotationLastX = mls.currentCameraXRotation;
    }


    [HideInInspector]
    public Vector3 currentGunPosition;
    [Header("Gun Positioning")]
    [Tooltip("Vector 3 position from player SETUP for NON AIMING values")]
    public Vector3 restPlacePosition;
    [Tooltip("Vector 3 position from player SETUP for AIMING values")]
    public Vector3 aimPlacePosition;
    [Tooltip("Time that takes for gun to get into aiming stance.")]
    public float gunAimTime = 0.1f;

    private Vector3 gunPosVelocity;
    private float cameraZoomVelocity;
    private float secondCameraZoomVelocity;

    private Vector2 gunFollowTimeVelocity;


    /*
	Update loop calling for methods that are descriped below where they are initiated.
	*/
    void Update() {

        Animations();

        GiveCameraScriptMySensitvity();

        PositionGun();

        Shooting();

        Sprint(); //iff we have the gun you sprint from here, if we are gunless then its called from movement script

        CrossHairExpansionWhenWalking();
    }


    /*
	*Update loop calling for methods that are descriped below where they are initiated.
	*+
	*Calculation of weapon position when aiming or not aiming.
	*/
    void FixedUpdate() {
        RotationGun();

        /*
		 * Changing some values if we are aiming, like sensitity, zoom racion and position of the waepon.
		 */
        //if aiming
        if (Input.GetAxis("Fire2") != 0) {
            gunPrecision = gunPrecision_aiming;
            recoilAmount_x = recoilAmount_x_;
            recoilAmount_y = recoilAmount_y_;
            recoilAmount_z = recoilAmount_z_;
            currentGunPosition = Vector3.SmoothDamp(currentGunPosition, aimPlacePosition, ref gunPosVelocity, gunAimTime);
            cameraComponent.fieldOfView = Mathf.SmoothDamp(cameraComponent.fieldOfView, cameraZoomRatio_aiming, ref cameraZoomVelocity, gunAimTime);
            secondCamera.fieldOfView = Mathf.SmoothDamp(secondCamera.fieldOfView, secondCameraZoomRatio_aiming, ref secondCameraZoomVelocity, gunAimTime);
        }
        //if not aiming
        else {
            gunPrecision = gunPrecision_notAiming;
            recoilAmount_x = recoilAmount_x_non;
            recoilAmount_y = recoilAmount_y_non;
            recoilAmount_z = recoilAmount_z_non;
            currentGunPosition = Vector3.SmoothDamp(currentGunPosition, restPlacePosition, ref gunPosVelocity, gunAimTime);
            cameraComponent.fieldOfView = Mathf.SmoothDamp(cameraComponent.fieldOfView, cameraZoomRatio_notAiming, ref cameraZoomVelocity, gunAimTime);
            secondCamera.fieldOfView = Mathf.SmoothDamp(secondCamera.fieldOfView, secondCameraZoomRatio_notAiming, ref secondCameraZoomVelocity, gunAimTime);
        }

    }

    [Header("Sensitvity of the gun")]
    [Tooltip("Sensitvity of this gun while not aiming.")]
    public float mouseSensitvity_notAiming = 10;
    //[HideInInspector]
    [Tooltip("Sensitvity of this gun while aiming.")]
    public float mouseSensitvity_aiming = 5;
    //[HideInInspector]
    [Tooltip("Sensitvity of this gun while running.")]
    public float mouseSensitvity_running = 4;
    /*
	 * Used to give our main camera different sensivity options for each gun.
	 */
    void GiveCameraScriptMySensitvity() {
        mls.mouseSensitvity_notAiming = mouseSensitvity_notAiming;
        mls.mouseSensitvity_aiming = mouseSensitvity_aiming;
    }

    /*
	 * Used to expand position of the crosshair or make it dissapear when running
	 */
    void CrossHairExpansionWhenWalking() {

        if (player.GetComponent<Rigidbody>().velocity.magnitude > 1 && Input.GetAxis("Fire1") == 0) {//ifnot shooting

            expandValues_crosshair += new Vector2(20, 40) * Time.deltaTime;
            if (player.GetComponent<PlayerMovementScript>().maxSpeed < runningSpeed) { //not running
                expandValues_crosshair = new Vector2(Mathf.Clamp(expandValues_crosshair.x, 0, 10), Mathf.Clamp(expandValues_crosshair.y, 0, 20));
                fadeout_value = Mathf.Lerp(fadeout_value, 1, Time.deltaTime * 2);
            } else {//running
                fadeout_value = Mathf.Lerp(fadeout_value, 0, Time.deltaTime * 10);
                expandValues_crosshair = new Vector2(Mathf.Clamp(expandValues_crosshair.x, 0, 20), Mathf.Clamp(expandValues_crosshair.y, 0, 40));
            }
        } else {//if shooting
            expandValues_crosshair = Vector2.Lerp(expandValues_crosshair, Vector2.zero, Time.deltaTime * 5);
            expandValues_crosshair = new Vector2(Mathf.Clamp(expandValues_crosshair.x, 0, 10), Mathf.Clamp(expandValues_crosshair.y, 0, 20));
            fadeout_value = Mathf.Lerp(fadeout_value, 1, Time.deltaTime * 2);

        }

    }

    /* 
	 * Changes the max speed that player is allowed to go.
	 * Also max speed is connected to the animator which will trigger the run animation.
	 */
    void Sprint() {// Running();  so i can find it with CTRL + F
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxisRaw("Fire2") == 0 && Input.GetAxisRaw("Fire1") == 0) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                if (pmS.maxSpeed == walkingSpeed) {
                    pmS.maxSpeed = runningSpeed;//sets player movement peed to max

                } else {
                    pmS.maxSpeed = walkingSpeed;
                }
            }
        } else {
            pmS.maxSpeed = walkingSpeed;
        }

    }

    private float startLook, startAim, startRun;

    private Vector3 velV;
    [HideInInspector]
    public Transform mainCamera;
    private Camera secondCamera;
    /*
	 * Calculatin the weapon position accordingly to the player position and rotation.
	 * After calculation the recoil amount are decreased to 0.
	 */
    void PositionGun() {
        transform.position = Vector3.SmoothDamp(transform.position,
            mainCamera.transform.position -
            (mainCamera.transform.right * (currentGunPosition.x + currentRecoilXPos)) +
            (mainCamera.transform.up * (currentGunPosition.y + currentRecoilYPos)) +
            (mainCamera.transform.forward * (currentGunPosition.z + currentRecoilZPos)), ref velV, 0);



        pmS.cameraPosition = new Vector3(currentRecoilXPos, currentRecoilYPos, 0);

        currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref velocity_z_recoil, recoilOverTime_z);
        currentRecoilXPos = Mathf.SmoothDamp(currentRecoilXPos, 0, ref velocity_x_recoil, recoilOverTime_x);
        currentRecoilYPos = Mathf.SmoothDamp(currentRecoilYPos, 0, ref velocity_y_recoil, recoilOverTime_y);

    }


    [Header("Rotation")]
    private Vector2 velocityGunRotate;
    private float gunWeightX, gunWeightY;
    [Tooltip("The time waepon will lag behind the camera view best set to '0'.")]
    public float rotationLagTime = 0f;
    private float rotationLastY;
    private float rotationDeltaY;
    private float angularVelocityY;
    private float rotationLastX;
    private float rotationDeltaX;
    private float angularVelocityX;
    [Tooltip("Value of forward rotation multiplier.")]
    public Vector2 forwardRotationAmount = Vector2.one;
    /*
	* Rotatin the weapon according to mouse look rotation.
	* Calculating the forawrd rotation like in Call Of Duty weapon weight
	*/
    void RotationGun() {

        rotationDeltaY = mls.currentYRotation - rotationLastY;
        rotationDeltaX = mls.currentCameraXRotation - rotationLastX;

        rotationLastY = mls.currentYRotation;
        rotationLastX = mls.currentCameraXRotation;

        angularVelocityY = Mathf.Lerp(angularVelocityY, rotationDeltaY, Time.deltaTime * 5);
        angularVelocityX = Mathf.Lerp(angularVelocityX, rotationDeltaX, Time.deltaTime * 5);

        gunWeightX = Mathf.SmoothDamp(gunWeightX, mls.currentCameraXRotation, ref velocityGunRotate.x, rotationLagTime);
        gunWeightY = Mathf.SmoothDamp(gunWeightY, mls.currentYRotation, ref velocityGunRotate.y, rotationLagTime);

        transform.rotation = Quaternion.Euler(gunWeightX + (angularVelocityX * forwardRotationAmount.x), gunWeightY + (angularVelocityY * forwardRotationAmount.y), 0);
    }

    private float currentRecoilZPos;
    private float currentRecoilXPos;
    private float currentRecoilYPos;
    /*
	 * Called from ShootMethod();, upon shooting the recoil amount will increase.
	 */
    public void RecoilMath() {
        currentRecoilZPos -= recoilAmount_z;
        currentRecoilXPos -= (Random.value - 0.5f) * recoilAmount_x;
        currentRecoilYPos -= (Random.value - 0.5f) * recoilAmount_y;
        mls.wantedCameraXRotation -= Mathf.Abs(currentRecoilYPos * gunPrecision);
        mls.wantedYRotation -= (currentRecoilXPos * gunPrecision);

        expandValues_crosshair += new Vector2(6, 12);

    }

    [Header("Shooting setup - MUSTDO")]
    [HideInInspector] public GameObject bulletSpawnPlace;
    [Tooltip("Bullet prefab that this waepon will shoot.")]
    public GameObject bullet;
    [Tooltip("Rounds per second if weapon is set to automatic rafal.")]
    public float roundsPerSecond;
    private float waitTillNextFire;
    /*
	 * Checking if the gun is automatic or nonautomatic and accordingly runs the ShootMethod();.
	 */
    void Shooting() {
        if (Input.GetButtonDown("Fire1")) {
            ShootMethod();
        }

        waitTillNextFire -= roundsPerSecond * Time.deltaTime;
    }


    [HideInInspector] public float recoilAmount_z = 0.5f;
    [HideInInspector] public float recoilAmount_x = 0.5f;
    [HideInInspector] public float recoilAmount_y = 0.5f;
    [Header("Recoil Not Aiming")]
    [Tooltip("Recoil amount on that AXIS while NOT aiming")]
    public float recoilAmount_z_non = 0.5f;
    [Tooltip("Recoil amount on that AXIS while NOT aiming")]
    public float recoilAmount_x_non = 0.5f;
    [Tooltip("Recoil amount on that AXIS while NOT aiming")]
    public float recoilAmount_y_non = 0.5f;
    [Header("Recoil Aiming")]
    [Tooltip("Recoil amount on that AXIS while aiming")]
    public float recoilAmount_z_ = 0.5f;
    [Tooltip("Recoil amount on that AXIS while aiming")]
    public float recoilAmount_x_ = 0.5f;
    [Tooltip("Recoil amount on that AXIS while aiming")]
    public float recoilAmount_y_ = 0.5f;
    [HideInInspector] public float velocity_z_recoil, velocity_x_recoil, velocity_y_recoil;
    [Header("")]
    [Tooltip("The time that takes weapon to get back on its original axis after recoil.(The smaller number the faster it gets back to original position)")]
    public float recoilOverTime_z = 0.5f;
    [Tooltip("The time that takes weapon to get back on its original axis after recoil.(The smaller number the faster it gets back to original position)")]
    public float recoilOverTime_x = 0.5f;
    [Tooltip("The time that takes weapon to get back on its original axis after recoil.(The smaller number the faster it gets back to original position)")]
    public float recoilOverTime_y = 0.5f;

    [Header("Gun Precision")]
    [Tooltip("Gun rate precision when player is not aiming. THis is calculated with recoil.")]
    public float gunPrecision_notAiming = 200.0f;
    [Tooltip("Gun rate precision when player is aiming. THis is calculated with recoil.")]
    public float gunPrecision_aiming = 100.0f;
    [Tooltip("FOV of first camera when NOT aiming(ONLY SECOND CAMERA RENDERS WEAPONS")]
    public float cameraZoomRatio_notAiming = 60;
    [Tooltip("FOV of first camera when aiming(ONLY SECOND CAMERA RENDERS WEAPONS")]
    public float cameraZoomRatio_aiming = 40;
    [Tooltip("FOV of second camera when NOT aiming(ONLY SECOND CAMERA RENDERS WEAPONS")]
    public float secondCameraZoomRatio_notAiming = 60;
    [Tooltip("FOV of second camera when aiming(ONLY SECOND CAMERA RENDERS WEAPONS")]
    public float secondCameraZoomRatio_aiming = 40;
    [HideInInspector]
    public float gunPrecision;

    [Tooltip("Audios for shootingSound, and reloading.")]
    public AudioSource shoot_sound_source, reloadSound_source;
    [Tooltip("Sound that plays after successful attack bullet hit.")]
    public static AudioSource hitMarker;

    /*
	* Sounds that is called upon hitting the target.
	*/
    public static void HitMarkerSound() {
        hitMarker.Play();
    }

    /*
	 * Called from Shooting();
	 */
    private void ShootMethod() {
        if (waitTillNextFire <= 0 && pmS.maxSpeed < 5) {
            if (bullet)
                Instantiate(bullet, bulletSpawnPlace.transform.position, bulletSpawnPlace.transform.rotation);
            else
                print("Missing the bullet prefab");

            if (shoot_sound_source)
                shoot_sound_source.Play();

            RecoilMath();

            waitTillNextFire = 1;
        }
    }


    /*
	 * Draw crosshair.
	 */
    void OnGUI() {
        DrawCrosshair();
    }

    [Header("Crosshair properties")]
    public Texture horizontal_crosshair, vertical_crosshair;
    public Vector2 top_pos_crosshair, bottom_pos_crosshair, left_pos_crosshair, right_pos_crosshair;
    public Vector2 size_crosshair_vertical = new Vector2(1, 1), size_crosshair_horizontal = new Vector2(1, 1);
    [HideInInspector]
    public Vector2 expandValues_crosshair;
    private float fadeout_value = 1;
    /*
	 * Drawing the crossHair.
	 */
    void DrawCrosshair() {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeout_value);
        if (Input.GetAxis("Fire2") == 0) {//if not aiming draw
            GUI.DrawTexture(new Rect(vec2(left_pos_crosshair).x + position_x(-expandValues_crosshair.x) + Screen.width / 2, Screen.height / 2 + vec2(left_pos_crosshair).y, vec2(size_crosshair_horizontal).x, vec2(size_crosshair_horizontal).y), vertical_crosshair);//left
            GUI.DrawTexture(new Rect(vec2(right_pos_crosshair).x + position_x(expandValues_crosshair.x) + Screen.width / 2, Screen.height / 2 + vec2(right_pos_crosshair).y, vec2(size_crosshair_horizontal).x, vec2(size_crosshair_horizontal).y), vertical_crosshair);//right

            GUI.DrawTexture(new Rect(vec2(top_pos_crosshair).x + Screen.width / 2, Screen.height / 2 + vec2(top_pos_crosshair).y + position_y(-expandValues_crosshair.y), vec2(size_crosshair_vertical).x, vec2(size_crosshair_vertical).y), horizontal_crosshair);//top
            GUI.DrawTexture(new Rect(vec2(bottom_pos_crosshair).x + Screen.width / 2, Screen.height / 2 + vec2(bottom_pos_crosshair).y + position_y(expandValues_crosshair.y), vec2(size_crosshair_vertical).x, vec2(size_crosshair_vertical).y), horizontal_crosshair);//bottom
        }

    }

    //#####		RETURN THE SIZE AND POSITION for GUI images ##################
    private float position_x(float var) {
        return Screen.width * var / 100;
    }
    private float position_y(float var) {
        return Screen.height * var / 100;
    }
    private float size_x(float var) {
        return Screen.width * var / 100;
    }
    private float size_y(float var) {
        return Screen.height * var / 100;
    }
    private Vector2 vec2(Vector2 _vec2) {
        return new Vector2(Screen.width * _vec2.x / 100, Screen.height * _vec2.y / 100);
    }
    //#

    public Animator handsAnimator;
    /*
	* Fetching if any current animation is running.
	* Setting the reload animation upon pressing R.
	*/
    void Animations() {

        if (handsAnimator) {
            handsAnimator.SetFloat("walkSpeed", pmS.currentSpeed);
            //handsAnimator.SetBool("aiming", Input.GetButton("Fire2"));
            handsAnimator.SetInteger("maxSpeed", pmS.maxSpeed);
        }

    }

    [Header("Animation names")]
    //public string reloadAnimationName = "Player_Reload";
    public string aimingAnimationName = "Player_AImpose";
    //public string meeleAnimationName = "Character_Malee";
}
