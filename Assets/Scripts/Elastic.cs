using UnityEngine;
using System.Collections;

public class Elastic : MonoBehaviour
{
    public float speed = 0.5f; // speed with which object will move back

    Vector3 originalPos; // vector holding original position of object
    Quaternion originalRot; // quaternion holding original orientation of object
    Rigidbody rb; // for changing velocity

    // Use this for initialization
    void Awake()
    {
        originalPos = transform.position; // store spawn position + orientation
        originalRot = transform.rotation;

        rb = GetComponent<Rigidbody>(); // get rb object
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != originalPos || transform.rotation != originalRot) // check if target pos+rot has been reached
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, originalPos, speed * Time.deltaTime); // calculate new position
            Quaternion rot = Quaternion.RotateTowards(transform.rotation, originalRot, speed * 200 * Time.deltaTime); // calculate new rotation
            transform.position = pos;
            transform.rotation = rot;
        }
    }

    // return original position of bomb
    public Vector3 GetOriginalPos()
    {
        return originalPos;
    }
    
    // stop any movement when elastic is switched on, makes sure that movement is smooth
    private void OnEnable()
    {
        rb.velocity = new Vector3(0f, 0f, 0f); // set velocity to 0
        rb.angularVelocity = new Vector3(0f, 0f, 0f); // set ang. vel. to 0
    }
}
