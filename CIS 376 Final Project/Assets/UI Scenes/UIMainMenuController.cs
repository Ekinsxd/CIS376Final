using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour {
    
    /// <summary> Start button on main menu </summary>
    public Button startButton;

    // Start is called before the first frame update
    void Start() {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("start-button");
        startButton.clicked += StartButtonPressed;

    }


    /// <summary>
    /// Switches scenes to MainScene
    /// </summary>
    void StartButtonPressed() {
        SceneManager.LoadScene("MainScene");
    }
}
