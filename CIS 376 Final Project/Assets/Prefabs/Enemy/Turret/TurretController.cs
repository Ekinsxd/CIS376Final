using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    Transform target;
    public int health = 3;
    public GameObject projectile;
    public float fireDelay = 1f;
    private float elapsedTime = 0;
    private Transform myLocation;
    private Transform barrel1, barrel2, turretBody;
    public AudioClip hitSound, shootSound, dieSound;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        turretBody = transform.GetChild(1).gameObject.transform;
        barrel1 = turretBody.GetChild(0).gameObject.transform;
        barrel2 = turretBody.GetChild(1).gameObject.transform;
        myLocation = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(myLocation.position, target.position);
        if (health <= 0) { return; }
        if (distance <= 20f) { FireProjectile(); }
        elapsedTime += Time.deltaTime;
    }

    void FireProjectile()
    {
        turretBody.LookAt(target.position);
        barrel1.LookAt(target.position);
        barrel2.LookAt(target.position);
        if (elapsedTime >= fireDelay)
        {
            elapsedTime = 0;
            AudioSource.PlayClipAtPoint(shootSound, target.position);
            Instantiate(projectile, barrel1.position, turretBody.rotation);
            Instantiate(projectile, barrel2.position, turretBody.rotation);
        }
    }
    void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(dieSound, target.position);
            turretBody.gameObject.SetActive(false);
            return;
        }
        AudioSource.PlayClipAtPoint(hitSound, target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (health <= 0) { return; }
        if (other.gameObject.tag == "Bullet")
        { TakeDamage(); }
    }
}
