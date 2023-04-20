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
    // Start is called before the first frame update
    void Start()
    {
        DeathTimer = 1.5f;
        UIC = GameObject.FindWithTag("UI").GetComponent<UIController>();
    }

    // Update is called once per frame
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

    public void AddScore(int score)
    {
        Score += score;
    }

    public bool isGameOver()
    {
        return GameOver;
    }

    public void LoseLife()
    {
        UIC.LoseLife(3 - Lives);
        Lives--;
    }

}
