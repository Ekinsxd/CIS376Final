using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : MonoBehaviour {
    Rigidbody rb;
    float lifeTime = 0.0f;


    void Start() {
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update() {
        // shoot bullet on first update
        if (lifeTime <= 0.0f) {
            rb.AddForce(transform.forward * 25.0f, ForceMode.Impulse);
        }

        lifeTime += Time.deltaTime;
        if (lifeTime > 2.0f) {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Bullet" && other.tag != "Spawner")
            Destroy(gameObject);
    }
}
