using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : MonoBehaviour {
    Rigidbody rb;
    float lifeTime = 0.0f;
    public BulletType bulletType;


    void Start() {
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update() {
        // shoot bullet on first update
        if (lifeTime <= 0.0f) {
            ShootBullet(bulletType);
        }

        lifeTime += Time.deltaTime;
        //if (lifeTime > 2.0f) {
        //    Destroy(gameObject);
        //}
    }


    /// <summary>
    /// Shoot bullet using <see cref="bulletType"/>
    /// </summary>
    /// <param name="type"> <see cref="BulletType"/> to shoot (assigned by Player) </param>
    private void ShootBullet(BulletType type) {
        switch (bulletType) {
            case BulletType.Normal:
            case BulletType.B:
            case BulletType.S:
            case BulletType.M:
            case BulletType.R:
            case BulletType.F:
                break;
            default:
                Debug.Log("unknown bullet type");
                break;
        }

        //Debug.Log(bulletType);
        rb.AddForce(transform.forward * 75.0f, ForceMode.Impulse);
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
