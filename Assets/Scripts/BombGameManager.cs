using UnityEngine;
using UnityEngine.SceneManagement;

public class BombGameManager : MonoBehaviour {
    
    bool hasEnded = false;
    GameObject panel; // panel showing score and highscore when game over
    GameObject hud; // hud showing health and score
    GameObject buttons; // buttons for returning to menu and play again

    // initialize
    void Awake()
    {
        panel = GameObject.FindGameObjectWithTag("GameOverPanel");
        hud = GameObject.FindGameObjectWithTag("HUD");
        buttons = Resources.Load("GameOverButtons") as GameObject;
        Score.score = 0;
    }

    // each frame
    void Update()
    {
        if (HealthManager.lives == 0) // check if player is out of lives
        {
            EndGame();
        }
    }

    // game over method
    public void EndGame()
    {
        if (!hasEnded) // check if not already ended
        {
            hasEnded = true;
            GetComponent<BombGenerator>().enabled = false; // stop spawning bombs
            GetComponent<AudioSource>().Stop();
            DisableAllBombs();

            panel.SetActive(true); // show panel and buttons
            hud.SetActive(false);
            Invoke("ShowButtons", 2);
        }
    }

    // show buttons for returning to menu an starting over
    void ShowButtons()
    {
        Instantiate(buttons);
    }

    void DisableAllBombs()
    {
        BombBehaviour[] allBombs = FindObjectsOfType<BombBehaviour>(); // find all bombs
        for (int i = 0; i < allBombs.Length; i++)
        {
            allBombs[i].DisableBomb(); // disable bomb
        }
    }
}
