using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    Vector3 targetPos; // position towards which enemy will move
    public float speed; // speed at which enemy moves, public to allow changing from editor
    bool hit = false; // flag indicating if enemy has been hit
    GameObject ScoreUpText; // +1 message
    GameObject OneUpIcon;  // lives+ message
    Vector3 hitPos; // position at which enemy was hit

    public static float[] spawnRangeX = {-4f, 4f}; // range in x,y at which enemies can spawn
    public static float[] spawnRangeY = {0f, 5f};

    float[] targetRangeX = {-0.3f, 0.3f}; // range in x,y towards which enemies will move
    float[] targetRangeY = { 0.1f, 0.4f };

    DragonGameManager gm; // reference to game manager

    // initialize
    void Start (){
        ScoreUpText = Resources.Load("1upText") as GameObject;
        OneUpIcon = Resources.Load("1upIcon") as GameObject;

        targetPos = GenerateTargetPosition();
        GetComponent<Rigidbody>().isKinematic = true; // set to kinematic to avoid collisions between enemies

        gm = FindObjectOfType<DragonGameManager>(); // find reference
    }

    // generate target position by mapping spawn position from spawn range to target range
    Vector3 GenerateTargetPosition()
    {
        float xPos = Map(transform.position.x, spawnRangeX[0], spawnRangeX[1], targetRangeX[0], targetRangeX[1]);
        float yPos = Map(transform.position.y, spawnRangeY[0], spawnRangeY[1], targetRangeY[0], targetRangeY[1]);
        float zPos = 0f;
        return new Vector3(xPos, yPos, zPos);
    }

    // map value s from range [a1,a2] to range [b1,b2]
    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    // Update is called once per frame
    void Update () {
        if (!hit)
        {
            if (transform.position != targetPos) // keep moving towards target
            {
                Vector3 pos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime); // calculate new position
                transform.position = pos; // move there
            }
            else
            {
                gm.DecreaseHealth(); // enemy reached target, player has been hit
                gm.hitByEnemySound.Play(); // play sound
                Destroy(this.gameObject);
            }
        }
	}

    // called when enemy is hit by leap hands
    public void OnHitByHands()
    {
        if (!hit) // check if not hit before
        {
            Hit();
            Instantiate(ScoreUpText, transform.position, Quaternion.Euler(0, 0, 0)); // show message
        }
    }

    // check for collisions, kill when hit by other enemy.
    void OnCollisionEnter(Collision col){
        if (col.gameObject.CompareTag("Enemy") && !this.hit && col.gameObject.GetComponent<EnemyBehaviour>().hit) // only count collision once
        {
            Hit();
            // activate gravity for both enemies to avoid collision chains
            GravityWrapper();
            col.gameObject.GetComponent<EnemyBehaviour>().GravityWrapper();
            // bonus live, only when enemy travelled far enough before collision
            if (HealthManager.lives < 3 && col.gameObject.GetComponent<EnemyBehaviour>().GetDistanceFromHit() > 0.3)
            {
                gm.IncreaseHealth();
                Instantiate(OneUpIcon, transform.position, Quaternion.Euler(0, 0, 0)); // show 1up icon
            } else
            {
                Instantiate(ScoreUpText, transform.position, Quaternion.Euler(0, 0, 0)); // show +1 icon
            }
        }
    }

    // kills enemy, plays sound, and saves location where enemy was killed
    void Hit()
    {
        Kill();
        gm.IncrementScore();
        GetComponent<AudioSource>().Play();
        hitPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // disables enemy
    public void Kill()
    {
        hit = true;
        StartDestroyCountdown();
        Invoke("GravityWrapper", 2);
        // disable kinematic when enemy killed
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().WakeUp(); // maybe not needed
    }

    // return position at which enemy was hit. needed by 
    public float GetDistanceFromHit()
    {
        return Vector3.Distance(transform.position, hitPos);
    }

    // method that activated gravity for this enemy
    public void GravityWrapper()
    {
        GetComponent<Rigidbody>().useGravity = true;
    }

    // method that destroys this enemy
    void DestroyWrapper(){
        Destroy(this.gameObject);
    }

    // destroy with delay
    public void StartDestroyCountdown(){
        // start the countdown for destroying the object
        Invoke("DestroyWrapper", 4);
    }
    
}
