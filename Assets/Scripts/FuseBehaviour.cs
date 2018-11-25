using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBehaviour : MonoBehaviour {

    Rigidbody rb; // rigid body object of fuse

	// Use this for initialization
	void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // called when grabbed
    public void DetachFromBomb() {
        transform.SetParent(null, true); // detach from parent object
        rb.detectCollisions = true; // activate rigid body
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.WakeUp(); // maybe not needed
        StartDestroyCountdown();
    }

    // destroys object
    public void DestroyWrapper()
    {
        Destroy(this.gameObject);
    }

    // destroy with delay
    public void StartDestroyCountdown()
    {
        // start the countdown for destroying the object
        Invoke("DestroyWrapper", 3);
    }
}
