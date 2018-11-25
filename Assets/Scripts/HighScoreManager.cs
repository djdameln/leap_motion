using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    TextMeshProUGUI text; // text displaying highscore
    int value; // value of highscore
    public static bool newHigh = false; // flag indicating if new high score has been reached

    // Use this for initialization
    void Start()
    {
        newHigh = false;
        text = GetComponent<TextMeshProUGUI>();
    }
    
    // returns key of this scenes high score dictionary entry
    string GetKey()
    {
        string key;
        switch (SceneManager.GetActiveScene().name)
        {
            case "bomb_game":
                key = "BombHighScore";
                break;
            case "dragon_game":
                key = "DragonHighScore";
                break;
            default:
                key = "";
                break;
        }
        return key;
    }

    // each frame
    void Update()
    {
        string key = GetKey();
        value = PlayerPrefs.GetInt(key, 0); // get current high score
        if (Score.score > value) // compare current score
        {
            PlayerPrefs.SetInt(key, Score.score); // update high score
            newHigh = true;
        }
        text.SetText(PlayerPrefs.GetInt(key, 0).ToString()); // show high score
    }
}
