using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour {

    TextMeshProUGUI text;
    public static int score;

	// Use this for initialization
	void Start () {
        text = GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {
        text.SetText(score.ToString()); // show score as text

    }
}
