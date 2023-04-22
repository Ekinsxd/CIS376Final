using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {
    private float xRotation;
    private float yRotation;

    public Transform orientation;

    public float sensX;
    public float sensY;

    // Start is called before the first frame update
    /// <summary>
    /// The Start function locks the cursor and makes it invisible in a C# program.
    /// </summary>
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    /// <summary>
    /// This function updates the rotation of a game object based on the user's mouse input.
    /// </summary>
    void Update() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
