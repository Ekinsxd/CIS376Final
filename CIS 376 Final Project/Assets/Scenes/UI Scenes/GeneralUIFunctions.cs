using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralUIFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    float time = 0;

    public void MainMenu() { SceneManager.LoadScene(0); }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 6)
        {
            MainMenu();
        }

        time += Time.deltaTime;
    }
}
