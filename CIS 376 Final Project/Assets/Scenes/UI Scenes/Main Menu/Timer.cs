using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public float currentTime;
    public bool mainActive = true;
    public UIDocument mainMenu;
    public UIDocument credits;

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKey && mainActive) { SceneManager.LoadScene(0); }

        currentTime += Time.deltaTime;
        if (currentTime >= 5)
        {
            mainActive = !mainActive;
            if (mainActive)
            {
                ActivateMainMenu();
            }
            else
            {
                ActivateCredits();
            }
        }
        if (Input.anyKey) { ActivateMainMenu(); }

    }

    void ActivateMainMenu()
    {
        currentTime = 0;
        Debug.Log("Main Menu should be active.");
        mainMenu.gameObject.SetActive(true);
        credits.gameObject.SetActive(false);
        mainActive = true;
    }

    void ActivateCredits()
    {
        currentTime = 0;
        Debug.Log("Credits should be active.");
        credits.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        mainActive = false;
        
    }
}
