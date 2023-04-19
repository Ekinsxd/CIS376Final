using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    /// <summary>
    /// Powerup Prefabs
    /// </summary>
    public GameObject[] powerups = new GameObject[4];

    public static bool Spawned;

    // Start is called before the first frame update
    void Start() {
        Spawned = false;
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.name == "Player") {
            if (!Spawned) {
                Debug.Log($"Player position: {other.transform.position}");
                SpawnRandomPowerup(other.transform);
                Spawned = true;
            }
        }
    }


    void SpawnRandomPowerup(Transform player) {
        GameObject powerup = powerups[Random.Range(0,4)];

        Vector3 pos = player.position.normalized * 15;
        Debug.Log($"POSITION: {pos}");
        Instantiate(powerup, pos, Quaternion.identity);

        //spawned = false;
        //while ()

    }
}