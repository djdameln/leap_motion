using UnityEngine;
using UnityEngine.SceneManagement;

public class DragonGameManager : MonoBehaviour
{

    bool hasEnded = false;
    GameObject panel; // panel showing score and highscore after game over
    GameObject hud; // HUD showing health + score
    GameObject buttons; // buttons for back to menu and play again
    GameObject checkPointButtons; // buttons when dying after checkpoint
    GameObject checkPointMessage; // message confirming checkpoint reached
    int checkPoint = 0; // current checkpoint
    int[] checkPointScores = {0, 20, 40, 60, 100}; // last element indicates until what score difficulty will increase

    public AudioSource backgroundMusic;
    public AudioSource hitByEnemySound;

    void Awake()
    {
        panel = GameObject.FindGameObjectWithTag("GameOverPanel");
        hud = GameObject.FindGameObjectWithTag("HUD");
        buttons = Resources.Load("GameOverButtons") as GameObject;
        checkPointButtons = Resources.Load("CheckPointButtons") as GameObject;
        checkPointMessage = Resources.Load("CheckpointMessage") as GameObject;
        Score.score = 0;
    }

    // increase score, called when hitting enemy
    public void IncrementScore()
    {
        Score.score++;
        if (checkPoint < checkPointScores.Length - 2 && Score.score == checkPointScores[checkPoint + 1])
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
        if (!hasEnded) // check if game already ended
        {
            hasEnded = true;
            GetComponent<EnemyGenerator>().enabled = false; // stop generating enemies

            backgroundMusic.Stop(); // stop background music
            DisableAllEnemies(); // freeze and kill enemies
            
            panel.SetActive(true); // show panel
            hud.SetActive(false); // hide HUD
            
            Invoke("ShowButtons", 2); // instantiate buttons
        }
    }

    // continue from previous checkpoint, reset score to checkpoint score
    public void ContinueFromCheckpoint()
    {
        hasEnded = false;
        HealthManager.lives = 3;
        Score.score = checkPointScores[checkPoint];
        GetComponent<EnemyGenerator>().enabled = true;
        backgroundMusic.Play();
        panel.SetActive(false);
        hud.SetActive(true);
        Destroy(GameObject.FindGameObjectWithTag("Buttons"));
    }

    // instantiate buttons for returning to menu and restarting game
    void ShowButtons()
    {
        // instantiate buttons
        if (checkPoint>0)
        {
            Instantiate(checkPointButtons);
        }
        else
        {
            Instantiate(buttons);
        }
    }

    // kill all enemies when game over
    void DisableAllEnemies()
    {
        EnemyBehaviour[] allEnemies = FindObjectsOfType<EnemyBehaviour>();
        for (int i = 0; i < allEnemies.Length; i++)
        {
            allEnemies[i].Kill();
        }
    }

    // return current checkpoint, needed by enemy generator to set difficulte
    public int GetCheckpoint()
    {
        return checkPoint;
    }

    // return the scores that trigger checkpoints, needed by enemy generator to set difficulty
    public int[] GetCheckPointScores()
    {
        return checkPointScores;
    }
    
}
