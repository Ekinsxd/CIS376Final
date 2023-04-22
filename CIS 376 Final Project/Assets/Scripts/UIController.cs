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

   /// <summary>
   /// The function initializes variables for the number of lives and hit indicator in a game, as well
   /// as a text component.
   /// </summary>
    void Start()
    {
        Lives = GetComponentsInChildren<RawImage>();
        HitIndicator = Lives[Lives.Length - 1];
        text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    /// <summary>
    /// This function updates the alpha value of a color based on a timer and game over status.
    /// </summary>
    void Update()
    {
        FlashTimer -= Time.deltaTime;

        Color color = HitIndicator.color;
        color.a = !GameOver ? (FlashTimer > 0 ? FlashTimer : 0) : 1;
        HitIndicator.color = color;
        
    }

    /// <summary>
    /// The SetGameOver function sets the GameOver boolean to true and displays "You lose!!" in a text
    /// field.
    /// </summary>
    public void SetGameOver()
    {
        text.text = "You lose!!";
        GameOver = true;
    }

    /// <summary>
    /// The function disables a certain number of lives represented by RawImage components and sets a
    /// flash timer for the hit indicator.
    /// </summary>
    /// <param name="count">The number of lives lost.</param>
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
