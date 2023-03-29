using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour
{
    public Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("start-button");
        startButton.clicked += StartButtonPressed;
        
    }

    void StartButtonPressed()
    {
        SceneManager.LoadScene("Playground");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
