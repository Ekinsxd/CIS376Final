using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBulletScript : MonoBehaviour {
    public float bulletSpeed;
    Rigidbody rb;
    Vector3 direction;
    float lifeTime = 0.0f;
    float maxLife = 5.0f;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        direction = transform.forward;
    }

    // Update is called once per frame
    void Update() {

        if (lifeTime <= 0.0f) {
            rb.AddForce(transform.forward * 4.0f, ForceMode.Impulse);
        }

        lifeTime += Time.deltaTime;
        if (lifeTime > maxLife) {
            Destroy(gameObject);
        }
        transform.position += direction * bulletSpeed * Time.deltaTime;
        var rotation = transform.rotation * Quaternion.Euler(new Vector3(Random.Range(5, 20), 0, 0));
        transform.rotation = rotation;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Bullet" && other.tag != "Spawner" && other.tag != "Player")
            Destroy(gameObject);
    }
}
