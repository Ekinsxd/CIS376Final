using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeScript : MonoBehaviour
{
    public GameObject explosion;
    private GameObject Player;
    private float timer = 1;
    private Transform[] Bridges;
    private bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Bridges =  GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //refresh the bridges array
        Bridges =  GetComponentsInChildren<Transform>();
        timer += Time.deltaTime;

        if (!started && Player.transform.position.z > Bridges[0].position.z - 1)
        {
            timer = -0.1f;
            Debug.Log("reached a bridge");
            started = true;
        }
        
        if (started)
        {
            foreach (Transform link in Bridges)
            {
                if (link != transform)
                {
                    if (timer > 0)
                    {
                        var explosions = Instantiate(explosion, link.position, link.rotation);
                        explosions.transform.rotation = Quaternion.Lerp(link.rotation, Quaternion.FromToRotation(Vector3.up, Vector3.up), 10);
                        Destroy(link.gameObject);
                        timer = -0.3f;
                        return;
                    }
                }
            }
        }
    }
}
