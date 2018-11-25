using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartDestroyCountdown();
	}

    // destroy with delay
    void StartDestroyCountdown()
    {
        Invoke("DestroyWrapper", 2);
    }

    // destroy current game object
    void DestroyWrapper()
    {
        Destroy(this.gameObject);
    }
}
