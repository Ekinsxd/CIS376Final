using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : MonoBehaviour {
    Rigidbody rb;
    float lifeTime = 0.0f;

    [HideInInspector] public BulletType bulletType;


    void Start() {
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update() {
        // shoot bullet on first update
        if (lifeTime <= 0.0f) {
            ShootBullet();
        }

        lifeTime += Time.deltaTime;
        if (lifeTime > 2.0f) {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// Shoot bullet using <see cref="bulletType"/>
    /// </summary>
    /// <param name="type"> <see cref="BulletType"/> to shoot (assigned by Player) </param>
    private void ShootBullet() {
        float bulletSpeed = 75.0f;

        switch (bulletType) {
            case BulletType.Normal:
            case BulletType.S:
            case BulletType.R:
            case BulletType.F:
                break;

            case BulletType.M:
                bulletSpeed = 200.0f;
                break;

            default:
                Debug.Log("unknown bullet type");
                break;
        }

        Debug.Log(bulletType);
        Debug.Log(transform.forward * bulletSpeed);
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        //Debug.Log("hit" + other.name + "!");
        Destroy(gameObject);
    }
}
