using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    private Animator playerAnimator;
    private KeyCode[] moveKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    // Start is called before the first frame update
    void Start() {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        float horizontalValue = Input.GetAxis("Horizontal");
        float verticalValue = Input.GetAxis("Vertical");

        if (playerAnimator != null) {
            foreach (KeyCode key in moveKeys) {
                if (Input.GetKey(key)) {
                    bool running = Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);
                    if (running) {
                        // running
                        playerAnimator.SetBool("walking", false);
                        playerAnimator.SetBool("running", true);
                    } else {
                        // walking
                        playerAnimator.SetBool("running", false);
                        playerAnimator.SetBool("walking", true);
                    }
                    break;
                }
            }
            // aiming?
            playerAnimator.SetBool("aiming", Input.GetKey(KeyCode.Mouse1));
            Debug.Log($"WALKING: {playerAnimator.GetBool("walking")}");
            Debug.Log($"RUNNING: {playerAnimator.GetBool("running")}");
            Debug.Log($"AIMING: {playerAnimator.GetBool("aiming")}");

        }
    }
}
