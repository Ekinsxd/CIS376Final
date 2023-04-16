using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour {

    [Header("Type")]
    public BulletType type;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per framewddw
    void Update() {
        // transform.up because the axis' are a little off
        // from when I made the 3D powerups in Blender :/
        transform.Rotate(transform.up * 50 * Time.deltaTime, Space.Self);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        //Debug.Log($"{other.name} hit {this.name}!");

        if (other.name == "Player") {
            Destroy(gameObject);
        }
    }
}
