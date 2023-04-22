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
    /// <summary>
    /// The Start function initializes a boolean variable called Spawned to false.
    /// </summary>
    void Start() {
        Spawned = false;
    }


    // Update is called once per frame
    void Update() { }


    /// <summary>
    /// This function spawns a random powerup when the player collides with a trigger collider, but only
    /// if a powerup has not already been spawned.
    /// </summary>
    /// <param name="Collider">Collider is a component in Unity that defines the shape of an object's
    /// collision boundary. It is used to detect collisions with other objects in the game world. In
    /// this code snippet, OnTriggerEnter is a method that is called when the collider of the current
    /// object collides with another collider. The parameter "other</param>
    void OnTriggerEnter(Collider other) {
        if (other.name == "Player") {
            if (!Spawned) {
                SpawnRandomPowerup(other.gameObject);
                Spawned = true;
            }
        }
    }


    /// <summary>
    /// This function spawns a random powerup near the player, ensuring that the same powerup is not
    /// spawned twice.
    /// </summary>
    /// <param name="GameObject">The parameter "GameObject player" is a reference to the player object
    /// in the game.</param>
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