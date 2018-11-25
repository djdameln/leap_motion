using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class HandTracker : MonoBehaviour {

    Frame frame; // Leap motion frame
    Controller controller; // Leap motion controller

    Hand leftHand, rightHand;
    List<Finger> leftFingers, rightFingers; // lists of fingers

    // Use this for initialization
    void Start () {
        controller = new Controller(); // create instance of controller
    }
	
	// Update is called once per frame
	void Update () {
        frame = controller.Frame(); // get current frame
        if (frame.Hands.Count > 0) // check if hands visible in frame
        {
            List<Hand> hands = frame.Hands;
            foreach (var hand in hands) // find left and right hands
            {
                leftHand = (hand.IsLeft ? hand : leftHand);
                rightHand = (hand.IsRight ? hand : rightHand);
            }
        }

        if (leftHand != null) // check if left hand has been found
        {
            Vector leftPosition = leftHand.PalmPosition; // cartesian coordinates of left hand
            Vector leftVelocity = leftHand.PalmVelocity; // velocity vector of left hand
            leftFingers = leftHand.Fingers;
        }

        if (rightHand != null) // check if right hand has been found
        {
            Vector rightPosition = rightHand.PalmPosition; // cartesian coordinates of left hand
            Vector rightVelocity = rightHand.PalmVelocity; // velocity vector of left hand
            rightFingers = rightHand.Fingers;
        }

        // Implement your own code here for storing the desired hand and finger tracking info.
        // ...
        // ...
        // ...
    }
}
