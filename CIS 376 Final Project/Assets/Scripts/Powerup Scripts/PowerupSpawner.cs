using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    /// <summary>
    /// Powerup Prefabs
    /// </summary>
    public PowerupScript[] powerups = new PowerupScript[4];


    /// <summary>
    /// True/false values for tracking which powerups have already
    /// spawned
    /// Index should correspond with <see cref="powerups"/> index
    /// </summary>
    private bool[] spawnedPowerups = new bool[4];


    /// <summary>
    /// Powerup currently spawned?
    /// </summary>
    public static bool Spawned;


    // Start is called before the first frame update
    void Start() {
        Spawned = false;
    }


    // Update is called once per frame
    void Update() { }


    /// <summary>
    /// Check for player collision
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other) {
        if (other.name == "Player") {
            if (!Spawned) {
                SpawnRandomPowerup(other.transform);
                Spawned = true;
            }
        }
    }


    /// <summary>
    /// Spawn a random powerup in game
    /// </summary>
    /// <param name="player"></param>
    void SpawnRandomPowerup(Transform player) {
        int powerupType = Random.Range(0, 4);
        PowerupScript powerup = powerups[powerupType];
        
        // TODO: reset spawnedPowerups once all are true
        // ensures that we don't spawn the same powerup twice
        while (spawnedPowerups[powerupType]) {
            powerupType = Random.Range(0, 4);
            powerup = powerups[powerupType];
        }
        
        Vector3 pos = player.position + (player.forward * 20);
        Instantiate(powerup, pos, Quaternion.identity);
        spawnedPowerups[powerupType] = true;
    }
}