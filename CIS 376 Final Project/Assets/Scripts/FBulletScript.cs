using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBulletScript : MonoBehaviour {
    public float bulletSpeed;
    Rigidbody rb;
    Vector3 direction;
    float lifeTime = 0.0f;
    float maxLife = 5.0f;

    /// <summary>
    /// The function adds an impulse force to the rigidbody component of the game object in the forward
    /// direction only once at the creation of the object.
    /// </summary>
    void Start() {
        rb = GetComponent<Rigidbody>();
        direction = transform.forward;
        rb.AddForce(direction * 4.0f, ForceMode.Impulse);
    }

    // Update is called once per frame
    /// <summary>
    /// The function updates the position and rotation of a game object representing a bullet.
    /// </summary>
    void Update() {
        lifeTime += Time.deltaTime;

        if (lifeTime > maxLife) {
            Destroy(gameObject);
        }

        transform.position += direction * bulletSpeed * Time.deltaTime;

        var rotation = transform.rotation * Quaternion.Euler(new Vector3(Random.Range(5, 20), 0, 0));
        transform.rotation = rotation;
    }


    /// <summary>
    /// The function destroys the game object if it collides with anything other than a bullet, spawner,
    /// or player.
    /// </summary>
    /// <param name="Collider">Collider is a component in Unity that represents the shape of an object
    /// for the purposes of physical collisions. It can be attached to any GameObject and defines a
    /// boundary around the object that can interact with other colliders in the scene. In this code
    /// snippet, the OnTriggerEnter method is called when another collider enters the</param>
    private void OnTriggerEnter(Collider other) {
        int noncollideableLayer = LayerMask.NameToLayer("Non-collideable");

        if (other.tag != "Bullet" && other.tag != "Spawner" && other.tag != "Player" && other.gameObject.layer != noncollideableLayer)
            Destroy(gameObject);
    }
}
