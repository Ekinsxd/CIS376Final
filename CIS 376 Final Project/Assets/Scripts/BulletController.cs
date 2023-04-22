using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : MonoBehaviour {
    Rigidbody rb;
    float lifeTime = 0.0f;


    /// <summary>
    /// The function adds an impulse force to the Rigidbody component of the object in the forward
    /// direction.
    /// </summary>
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 25.0f, ForceMode.Impulse);
    }


    /// <summary>
    /// This function destroys a game object after a certain amount of time has passed.
    /// </summary>
    void Update() {
        lifeTime += Time.deltaTime;
        if (lifeTime > 2.0f) {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// The function destroys the game object if it collides with anything other than a bullet or a
    /// spawner.
    /// </summary>
    /// <param name="Collider">Collider is a component in Unity that represents the shape of an object
    /// for the purposes of physical collisions. It can be attached to any GameObject and defines a
    /// boundary around the object that can interact with other colliders in the scene. In this code
    /// snippet, OnTriggerEnter is a method that is called when the collider</param>
    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Bullet" && other.tag != "Spawner")
            Destroy(gameObject);
    }
}
