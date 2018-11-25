using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreMessageBehaviour : MonoBehaviour {

    TextMeshProUGUI tm;

    // Use this for initialization
    void Start()
    {
        tm = gameObject.GetComponent<TextMeshProUGUI>();
        tm.enabled = false;
    }

    void Update()
    {
        if (HighScoreManager.newHigh) // show text if new high
        {
            tm.enabled = true;
        }
    }

}
