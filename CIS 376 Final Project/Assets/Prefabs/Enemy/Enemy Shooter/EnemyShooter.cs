using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : MonoBehaviour {
    public Transform target;
    Vector3 destination;
    NavMeshAgent agent;
    public int health = 1;
    public GameObject projectile;
    public float fireDelay = 1;
    private float elapsedTime = 0;
    //Location of the enemy bullet spawn.
    private Transform location;
    //location of enemy
    private Transform enemyLocation;
    public AudioClip hitSound, shootSound, dieSound;

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;
        target = GameObject.Find("Player").transform;
        location = transform.GetChild(0).gameObject.transform;
        enemyLocation = GetComponent<Transform>();
        //projectile = Resources.Load("Bullet", typeof(GameObject)) as GameObject;

    }

    void Update() {
        float distance = Vector3.Distance(enemyLocation.position, target.position);

        if (distance > 10.0f) { 
            agent.destination = target.position; 
            //Debug.Log("Move to player"); 
            //Debug.Log(distance); 
        } else {
            FireProjectile();
        }
        elapsedTime += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Bullet") {
            TakeDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
        {
        if (other.gameObject.tag == "Bullet")
        {
            TakeDamage();
        }

    }

    private void TakeDamage() {
        //Debug.Log(health);
        health--;
        if (health <= 0) { AudioSource.PlayClipAtPoint(dieSound, target.position); Destroy(gameObject);}
        else { AudioSource.PlayClipAtPoint(hitSound, target.position); }
    }

    private void FireProjectile() {
        agent.SetDestination(transform.position);
        //Debug.Log("fire projectile");
        transform.LookAt(target.position);
        if (elapsedTime >= fireDelay) {
            elapsedTime = 0;
            AudioSource.PlayClipAtPoint(shootSound, target.position);
            Instantiate(projectile, location.position, location.rotation);
        }
    }
}
