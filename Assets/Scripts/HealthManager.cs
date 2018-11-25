using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LaireonFramework;

public class HealthManager : MonoBehaviour {

    public static int lives;
    BeatingHealthBar healthBar; // on-screen hearts

	// Use this for initialization
	void Start () {
        lives = 3; // start with 3 lives
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<BeatingHealthBar>();
	}

    // Update is called once per frame
    void Update () {
        if (lives > 1) {
            healthBar.currentValue = lives;
        } else if (lives == 1)
        {
            healthBar.currentValue = lives - 0.5f; // lower value to trigger beating heart animation
        }
        else
        {
            healthBar.currentValue = 0;
        }
    }
}
