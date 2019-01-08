using UnityEngine;
using UnityEngine.SceneManagement;

public class BombGameManager : MonoBehaviour {
    
    bool hasEnded = false;
    GameObject panel; // panel showing score and highscore when game over
    GameObject hud; // hud showing health and score
    GameObject buttons; // buttons for returning to menu and play again
    GameObject checkPointButtons; // buttons when dying after checkpoint
    GameObject checkPointMessage; // message confirming checkpoint reached
    int checkPoint = 0; // current checkpoint
    int[] checkPointScores = {0, 5, 10, 20};

    public AudioSource backgroundMusic;

    // initialize
    void Awake()
    {
        panel = GameObject.FindGameObjectWithTag("GameOverPanel");
        hud = GameObject.FindGameObjectWithTag("HUD");
        buttons = Resources.Load("GameOverButtons") as GameObject;
        checkPointButtons = Resources.Load("CheckPointButtons") as GameObject;
        checkPointMessage = Resources.Load("CheckpointMessage") as GameObject;
        Score.score = 0;
    }

    // increase score, called when disabled bomb
    public void IncrementScore()
    {
        Score.score++;
        if (checkPoint < checkPointScores.Length - 1 && Score.score == checkPointScores[checkPoint + 1])
        {
            checkPoint++;
            Instantiate(checkPointMessage);
        }
    }

    // 1up
    public void IncreaseHealth()
    {
        HealthManager.lives++;
    }

    // 1down
    public void DecreaseHealth()
    {
        if (--HealthManager.lives == 0) // end game if out of lives
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
            //GetComponent<AudioSource>().Stop();
            backgroundMusic.Stop();
            DisableAllBombs();

            panel.SetActive(true); // show panel and buttons
            hud.SetActive(false);
            Invoke("ShowButtons", 2);
        }
    }

    // continue from previous checkpoint, reset score to checkpoint score
    public void ContinueFromCheckpoint()
    {
        hasEnded = false;
        HealthManager.lives = 3;
        Score.score = checkPointScores[checkPoint];
        GetComponent<BombGenerator>().enabled = true;
        backgroundMusic.Play();
        panel.SetActive(false);
        hud.SetActive(true);
        Destroy(GameObject.FindGameObjectWithTag("Buttons"));
    }

    // instantiate buttons for returning to menu and restarting game
    void ShowButtons()
    {
        // instantiate buttons
        if (checkPoint > 0)
        {
            Instantiate(checkPointButtons);
        }
        else
        {
            Instantiate(buttons);
        }
    }

    void DisableAllBombs()
    {
        BombBehaviour[] allBombs = FindObjectsOfType<BombBehaviour>(); // find all bombs
        for (int i = 0; i < allBombs.Length; i++)
        {
            allBombs[i].DisableBomb(); // disable bomb
        }
    }

    // return current checkpoint, needed by bomb generator to set difficulte
    public int GetCheckpoint()
    {
        return checkPoint;
    }

    // return the scores that trigger checkpoints, needed by bomb generator to set difficulty
    public int[] GetCheckPointScores()
    {
        return checkPointScores;
    }
}
