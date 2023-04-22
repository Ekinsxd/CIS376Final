using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPosition : MonoBehaviour {
    public Transform cameraPos;
    // Start is called before the first frame update
    void Start() {

    }

    /// <summary>
    /// The Update function sets the position of the object to match the position of the camera.
    /// </summary>
    void Update() {
        transform.position = cameraPos.position;
    }
}
