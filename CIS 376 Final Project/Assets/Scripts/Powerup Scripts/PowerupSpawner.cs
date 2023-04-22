using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    /// <summary>
    /// Powerup Prefabs
    /// </summary>
    public GameObject[] powerups = new GameObject[4];


    /// <summary>
    /// True/false values for tracking which powerups have already
    /// spawned
    /// Index should correspond with <see cref="powerups"/> index
    /// </summary>
    private bool[] spawnedPowerups = { false, false, false, false };


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
                SpawnRandomPowerup(other.gameObject);
                Spawned = true;
            }
        }
    }


    /// <summary>
    /// Spawn a random powerup in game
    /// </summary>
    /// <param name="player"></param>
    void SpawnRandomPowerup(GameObject player) {
        int powerupType = Random.Range(0, 4);
        GameObject powerup = powerups[powerupType];

        // ensures that we don't spawn the same powerup twice
        while (spawnedPowerups[powerupType]) {
            powerupType = Random.Range(0, 4);
            powerup = powerups[powerupType];
        }

        Vector3 powerupPos = player.transform.position + (player.transform.forward * 20);

        if (powerupPos.y > 2) { // player in air (spawn powerup on ground)
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, Vector3.down, out hit)) {
                powerupPos.y -= hit.distance;
                powerupPos.y += 1;
            }
        }

        Instantiate(powerup, powerupPos, Quaternion.identity);
        spawnedPowerups[powerupType] = true;
    }
}