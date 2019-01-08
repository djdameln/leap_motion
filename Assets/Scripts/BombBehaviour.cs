using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    public float explosionTime = 10; // time from spawn to explode, public allows changing from editor

    bool disabled = false;
    Animator animator; // for starting the color change
    Rigidbody rb; // for changing velocity
    int region; // spawn region this instance is assigned to
    BombGenerator bg; // reference to bombgenerator
    BombGameManager gm; // reference to game manager

    GameObject scoreUpText; // live+ and +1 messages
    GameObject oneUpIcon;

    float startTime; // for storing time at which bomb was created

    // Use this for initialization
    void Start()
    {
        scoreUpText = Resources.Load("1upText") as GameObject;
        oneUpIcon = Resources.Load("1upIcon") as GameObject;

        rb = GetComponent<Rigidbody>(); // get rb object
        animator = GetComponent<Animator>(); // get animator
        float animationDuration = GetAnimationDuration("ColorChange"); // get duration of bomb clip

        Invoke("Explode", explosionTime); // initialize explosion
        Invoke("StartAnimation", explosionTime-animationDuration); // initialize animation

        startTime = Time.time; // store time at which bomb was spawned

        gm = FindObjectOfType<BombGameManager>();
    }
    
    // method that returns the duration of the animation associated with the bomb object class
    float GetAnimationDuration(string clipName)
    {
        float time=0f;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController; //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == clipName)        //if it has the same name as your clip
            {
                time = ac.animationClips[i].length;
            }
        }
        return time;
    }

    // instantiates graphic explosion, destroys bomb, decrement health
    void Explode()
    {
        GameObject explosion = Resources.Load("GraphicExplosion") as GameObject;
        Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(this.gameObject);
        bg.FreeRegion(region); // tell bomb generator that region is now free
        gm.DecreaseHealth();
    }

    void StartAnimation()
    {
        animator.Play("ColorChange");
    }

    void Update()
    {
        if (disabled)
        {
            GetComponent<Elastic>().enabled = false; // once bomb is disabled, stop elastic
        }
    }

    // called when fuse removed by hands
    public void Dismantle()
    {
        if (!disabled) // only needed if bomb not already disabled
        {
            DisableBomb();
            if (Time.time < startTime + 3 && HealthManager.lives < 3) // bonus live when fast enough
            {
                Instantiate(oneUpIcon, transform.position, Quaternion.Euler(0, 0, 0));
                gm.IncreaseHealth();
            }
            else // otherwise show +1 message
            {
                GetComponent<AudioSource>().Play();
                Instantiate(scoreUpText, transform.position, Quaternion.Euler(0, 0, 0));
            }
            //Score.score += 1;
            gm.IncrementScore();
        }
    }

    // called when dismantled and when game over
    public void DisableBomb()
    {
        if (!disabled)
        {
            disabled = true;
            CancelInvoke(); // cancel explosion
            GetComponent<Elastic>().enabled = false;
            StartDestroyCountdown();
            rb.useGravity = true; // fall 
            bg.FreeRegion(region);
            animator.Play("Default"); // show default colors
        }
    }

    // destroy instance
    public void DestroyWrapper()
    {
        Destroy(this.gameObject);
    }

    // delayed destroy
    public void StartDestroyCountdown()
    {
        // start the countdown for destroying the object
        Invoke("DestroyWrapper", 3);
    }

    // attach bomb to region, called from bombgenerator
    public void setRegion(int region)
    {
        this.region = region;
    }

    // attach bomb generator, call from bomb generator.
    public void setBombGenerator(BombGenerator bg)
    {
        this.bg = bg;
    }
}
