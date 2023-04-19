using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject enemyContainer;

    void Start()
    {
        enemyContainer = transform.GetChild(0).gameObject;
        enemyContainer.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
            enemyContainer.SetActive(true);
    }
}
