using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class PowerupScript : MonoBehaviour {

    [Header("Type")]
    public BulletType type;

    private float lifetime = 10; // 10 seconds


    // Start is called before the first frame update
    void Start() {
            
    }

    // Update is called once per frame
    void Update() {
        // transform.up because the axis' are a little off
        // from when I made the 3D powerups in Blender :/
        transform.Rotate(transform.up * 50 * Time.deltaTime, Space.Self);
        lifetime -= Time.deltaTime;

        if (lifetime <= 0) {
            DestroyPowerup();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        //Debug.Log($"{other.name} hit {this.name}!");

        if (other.name == "Player") {
            PowerupSpawner.Spawned = false;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void DestroyPowerup() {
        Debug.Log($"lifetime exceeded for powerup {type}");
        PowerupSpawner.Spawned = false;
        Destroy(gameObject);
    }
}
