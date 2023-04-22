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

    /// <summary>
    /// The Start function finds the player GameObject and gets the child transforms of the current
    /// GameObject (bridge links).
    /// </summary>
    void Start()
    {
        Player = GameObject.Find("Player");
        Bridges =  GetComponentsInChildren<Transform>();
    }

    /// <summary>
    /// This function updates the state of bridges in the game and triggers an explosion on each bridge
    /// after a certain amount of time has passed.
    /// </summary>
    /// <returns>
    /// If the condition `if (!started)` is true, then the function returns without executing the rest
    /// of the code. If the condition `if (timer <= 0)` is true for any link in the `foreach` loop,
    /// then the code inside the loop is skipped and the loop moves on to the next link. If none of
    /// these conditions are met, then nothing is returned and the function continues
    /// </returns>
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
        
        if (!started)
        {
            return;
        }
        
        foreach (Transform link in Bridges)
        {
            if (link == transform)
            {
                continue;
            }
            if (timer <= 0)
                {
                continue;
            }
            BlowUp(link);
            return;
        }
    }

    /// <summary>
    /// The function creates an explosion at the position and rotation of a given transform, destroys
    /// the transform, and sets a timer.
    /// </summary>
    /// <param name="Transform">Transform is a data type in Unity that represents the position,
    /// rotation, and scale of a game object. It is used to manipulate the position, rotation, and scale
    /// of a game object in the scene. In this code, the Transform parameter is used to specify the
    /// position and rotation of the explosion object</param>
    void BlowUp(Transform link){
        var explosions = Instantiate(explosion, link.position, link.rotation);
        explosions.transform.rotation = Quaternion.Lerp(link.rotation, Quaternion.FromToRotation(Vector3.up, Vector3.up), 10);
        Destroy(link.gameObject);
        timer = -0.3f;
    }
}
