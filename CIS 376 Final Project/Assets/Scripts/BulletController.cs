using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    Rigidbody rb;
    float lifeTime = 0.0f;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 75.0f, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update() {
        lifeTime += Time.deltaTime;
        if (lifeTime > 2.0f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("hit" + other.name + "!");
        Destroy(gameObject);
    }
}
