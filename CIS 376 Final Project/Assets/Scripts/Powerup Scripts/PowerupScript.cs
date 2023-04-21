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


    // Update is called once per frame
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
    /// Check for player collisions
    /// </summary>
    /// <param name="other"></param>
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
