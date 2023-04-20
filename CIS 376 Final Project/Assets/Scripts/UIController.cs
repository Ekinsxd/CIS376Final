using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    RawImage[] Lives;
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        Lives = GetComponentsInChildren<RawImage>();
        text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameOver()
    {
        text.text = "You lose!!";
    }

    public void LoseLife(int count)
    {
        foreach (RawImage life in Lives)
        {
            if (count >= 0){
                life.enabled = false;
                count--;
            }
        }
    }
}
