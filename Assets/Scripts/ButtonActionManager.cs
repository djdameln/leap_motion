using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActionManager : MonoBehaviour {

    void QuitGame()
    {
        Application.Quit();
    }

    void LoadAboutScene()
    {
        SceneManager.LoadScene(sceneName: "about_scene");
    }
    // load dragon game scene
    void LoadDragonGame(){
        SceneManager.LoadScene(sceneName: "dragon_game");
    }

    // load instruction dragon game instruction scene
    void LoadDragonInstructions()
    {
        SceneManager.LoadScene(sceneName: "dragon_instructions");
    }

    // load bomb game scene
    void LoadBombGame()
    {
        SceneManager.LoadScene(sceneName: "bomb_game");
    }

    // load bomb dragon game instruction scene
    void LoadBombInstructions()
    {
        SceneManager.LoadScene(sceneName: "bomb_instructions");
    }

    // load main menu scene
    void LoadMainMenu()
    {
        SceneManager.LoadScene(sceneName: "main_menu");
    }

    // restart current scene
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // reset high scores of all games
    void ResetHighScores()
    {
        PlayerPrefs.DeleteAll();
        GameObject message = Resources.Load("ResetMessage") as GameObject;
        Instantiate(message); // instantiate confirm message
    }

    // continue dragon game from last checkpoint
    void ContinueDragonGame()
    {
        if (SceneManager.GetActiveScene().name == "dragon_game")
        {
            FindObjectOfType<DragonGameManager>().ContinueFromCheckpoint();
        }
        else if (SceneManager.GetActiveScene().name == "bomb_game")
        {
            FindObjectOfType<BombGameManager>().ContinueFromCheckpoint();
        }
    }

    public void StartSceneCountDown(){
        // start the countdown for the method associated with current button
        switch (this.gameObject.name){
            case "Quit Cube":
                Invoke("QuitGame", 2);
                break;
            case "About Cube":
                Invoke("LoadAboutScene", 2);
                break;
            case "Dragon Instr Button":
                Invoke("LoadDragonInstructions", 2);
                break;
            case "Dragon Cube":
                Invoke("LoadDragonGame", 2);
                break;
            case "Bomb Cube":
                Invoke("LoadBombGame", 2);
                break;
            case "Bomb Instr Button":
                Invoke("LoadBombInstructions", 2);
                break;
            case "Menu Cube":
                Invoke("LoadMainMenu", 2);
                break;
            case "Restart Cube":
                Invoke("RestartScene", 2);
                break;
            case "Reset Score Cube":
                Invoke("ResetHighScores", 2);
                break;
            case "Continue Cube":
                Invoke("ContinueDragonGame", 2);
                break;
        }
    }
}
