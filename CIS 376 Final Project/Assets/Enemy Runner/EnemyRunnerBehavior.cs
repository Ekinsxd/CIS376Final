using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRunnerBehavior : MonoBehaviour {
    public Transform target;
    Vector3 destination;
    NavMeshAgent agent;
    public int health = 2;
    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(destination, target.position) > 1.0f) { agent.destination = target.position; }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Bullet") {
            TakeDamage();
        }
    }

    private void TakeDamage() {
        Debug.Log(health);
        health--;
        if (health <= 0) { Destroy(gameObject); }
    }
}
