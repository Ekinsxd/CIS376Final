using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour
{
    int Lives = 3;
    int Score = 0;
    Transform UI;
    Transform UILife1;
    float DeathTimer;
    bool GameOver = false;
    bool Win = false;
    UIController UIC;

    /// <summary>
    /// The Start function sets the DeathTimer to 1.5f and gets the UIController component from a
    /// GameObject with the "UI" tag.
    /// </summary>
    void Start()
    {
        DeathTimer = 1.5f;
        UIC = GameObject.FindWithTag("UI").GetComponent<UIController>();
    }

    // Update is called once per frame
    /// <summary>
    /// The function updates the game state and loads the appropriate scene based on whether the player
    /// has lost or won.
    /// </summary>
    void Update()
    {
        if (Lives <= 0)
        {
            //set UI Text to gameover
            UIC.SetGameOver();
            GameOver = true;
        }

        if (GameOver)
        {
            // Game Over call game over screen after 3 seconds
            DeathTimer -= Time.deltaTime;
            if (DeathTimer <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
            }
        }

        if (Win)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Win Screen");
        }
    }

    /// <summary>
    /// The function adds a given score to the current score.
    /// </summary>
    /// <param name="score">The parameter "score" is an integer value that represents the amount of
    /// points to be added to the current score.</param>
    public void AddScore(int score)
    {
        Score += score;
    }

    /// <summary>
    /// This function returns a boolean value indicating whether the game is over or not.
    /// </summary>
    /// <returns>
    /// The method isGameOver() is returning a boolean value, specifically the value of the variable
    /// GameOver.
    /// </returns>
    public bool isGameOver()
    {
        return GameOver;
    }

    /// <summary>
    /// The function decreases the number of lives a player has and triggers a game over if the player
    /// has no more lives.
    /// </summary>
    public void LoseLife()
    {
        UIC.LoseLife(3 - Lives);
        Lives--;
        if (Lives == 0)
        {
            UIC.SetGameOver();
        }
    }

}
