using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip hitSound, explosion;
    public static bool okToHit = false;
    public int health = 10;
    Transform player;
    
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (okToHit)
        {
            if (other.gameObject.tag == "Bullet")
            {
                health--;
                AudioSource.PlayClipAtPoint(hitSound, player.position);

                if (health <= 0) { DestroyWall();}
            }
        
        }
    }
    private void DestroyWall()
    {
        AudioSource.PlayClipAtPoint(explosion, player.position);
        Destroy(gameObject);
    }
}
