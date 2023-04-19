using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    private Animator playerAnimator;


    // Start is called before the first frame update
    void Start() {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        float horizontalValue = Input.GetAxis("Horizontal");
        float verticalValue = Input.GetAxis("Vertical");

        if (playerAnimator != null) {
            if ((horizontalValue != 0 || verticalValue != 0)) {
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

                // aiming?
                playerAnimator.SetBool("aiming", Input.GetMouseButtonDown(1));
            }
        }
    }
}
