using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class PowerupScript : MonoBehaviour {

    /// <summary>
    /// Type of powerup
    /// </summary>
    [Header("Type")]
    public Powerup type;


    /// <summary>
    /// Total lifetime of powerup
    /// </summary>
    private float lifetime = 10; // 10 seconds


    // Start is called before the first frame update
    void Start() { }



    /// <summary>
    /// This function rotates a powerup object and destroys it if its lifetime has exceeded.
    /// </summary>
    void Update() {
        // transform.up because the axis' are a little off
        // from when I made the 3D powerups in Blender :/
        transform.Rotate(transform.up * 50 * Time.deltaTime, Space.Self);
        lifetime -= Time.deltaTime;

        if (lifetime <= 0) {
            Debug.Log($"lifetime exceeded for powerup {type}");
            DestroyPowerup();
        }
    }


    /// <summary>
    /// This function destroys a powerup when the player collides with it.
    /// </summary>
    /// <param name="Collider">Collider is a component in Unity that defines the shape of an object's
    /// collision boundary. It is used to detect collisions with other objects in the game world. In
    /// this code snippet, OnTriggerEnter is a method that is called when the collider of the object
    /// this script is attached to collides with another collider.</param>
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            DestroyPowerup();
        }
    }


    /// <summary>
    /// Destroy powerup after lifetime is exceeded
    /// </summary>
    public void DestroyPowerup() {
        PowerupSpawner.Spawned = false;
        Destroy(gameObject);
    }
}
