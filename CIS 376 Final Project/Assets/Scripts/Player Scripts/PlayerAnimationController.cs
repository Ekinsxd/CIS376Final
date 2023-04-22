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
        bool walking = false;
        bool running = false;
        if (playerAnimator != null) {
            foreach (KeyCode key in moveKeys) {
                if (Input.GetKey(key)) {
                    running = Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);
                    walking = !running;
                    break;
                }
            }

            // set animation states
            playerAnimator.SetBool("walking", walking);
            playerAnimator.SetBool("running", running);
        }
    }
}
