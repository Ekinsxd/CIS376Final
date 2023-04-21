using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseWallScript : MonoBehaviour
{
    // Start is called before the first frame update
    int turretDown = 0;
    public GameObject turret1, turret2, winTrigger;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (turret1.transform.GetChild(0).gameObject.activeSelf && !turret1.transform.GetChild(1).gameObject.activeSelf){ turretDown++;}
        if (turret2.transform.GetChild(0).gameObject.activeSelf && !turret2.transform.GetChild(1).gameObject.activeSelf) { turretDown++;}
        if (turretDown > 1)
        {
            winTrigger.SetActive(true);
            SealController.okToHit = true;
        }
    }
}
