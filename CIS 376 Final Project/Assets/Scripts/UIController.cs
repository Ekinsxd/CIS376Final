using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    private RawImage[] Lives;
    private RawImage HitIndicator;
    private TMP_Text text;
    private bool GameOver = false;
    private float FlashTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        Lives = GetComponentsInChildren<RawImage>();
        HitIndicator = Lives[Lives.Length - 1];
        text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        FlashTimer -= Time.deltaTime;

        Color color = HitIndicator.color;
        color.a = !GameOver ? (FlashTimer > 0 ? FlashTimer : 0) : 1;
        HitIndicator.color = color;
        
    }

    public void SetGameOver()
    {
        text.text = "You lose!!";
        GameOver = true;
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

        FlashTimer = 1f;

    }
}
