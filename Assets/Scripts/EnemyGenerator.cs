using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    GameObject chickenEnemy; // enemy prefabs
    GameObject condorEnemy;
    GameObject dragonEnemy;

    DragonGameManager gm; // reference to game manager

    float minSpawnTime = 0.5f; // time between enemies at highest difficulty
    float maxSpawnTime = 5f; // time between enemies at lowest difficulty
    int scoreRange; // score at which difficulty is maxed out

    int[] checkPointScores; // scores that trigger checkpoints

    Vector3[] enemyWeights = new Vector3[5]; // probability of spawning different enemies for each stage of game

    // Use this for initialization
    void Awake () {
        chickenEnemy = Resources.Load("FlyingChicken") as GameObject;
        condorEnemy = Resources.Load("FlyingCondor") as GameObject;
        dragonEnemy = Resources.Load("FlyingDragon") as GameObject;
        // get game manager for checkpoint value
        gm = FindObjectOfType<DragonGameManager>();
        // initialize enemy weights
        enemyWeights[0] = new Vector3(1.0f, 0.0f, 0.0f); // at higher checkpoints and scores, P(condor) and P(dragon) should increase
        enemyWeights[1] = new Vector3(1.0f, 0.0f, 0.0f);
        enemyWeights[2] = new Vector3(0.3f, 0.7f, 0.0f);
        enemyWeights[3] = new Vector3(0.1f, 0.2f, 0.7f);
        enemyWeights[4] = new Vector3(0.1f, 0.2f, 0.7f);

        checkPointScores = gm.GetCheckPointScores(); // get trigger points from game manager

        scoreRange = checkPointScores[checkPointScores.Length - 1]; // score range is last item in trigger points
    }

    // stop spawning enemies
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
    // start spawning enemies
    private void OnEnable()
    {
        StartCoroutine(SpawnEnemies());
    }

    // generate spawn probability P(enemy|score)
    float[] GenerateWeights()
    {
        float[] w = new float[3];
        Vector3 weights; // represent as vector to allow vectorized weight calculation
        int checkPoint = gm.GetCheckpoint(); // current checkpoint
        if (Score.score >= scoreRange) // game already at highest difficulty
        {
            weights = enemyWeights[enemyWeights.Length-1];
        }
        else
        {
            // progress from current to next checkpoint [0,1]
            float progress = (float)(Score.score - checkPointScores[checkPoint]) / (checkPointScores[checkPoint + 1] - checkPointScores[checkPoint]);
            // calculate P
            weights = enemyWeights[checkPoint] - ((enemyWeights[checkPoint] - enemyWeights[checkPoint + 1]) * progress);
        }
        w[0] = weights.x; // convert vector to array
        w[1] = weights.y;
        w[2] = weights.z;
        return w;
    }

    // randomly pick enemy type from probability distribution
    int GenerateEnemyType()
    {
        float[] weights = GenerateWeights(); // generate Pdist
        float total = weights[0] + weights[1] + weights[2]; // should always be 1, but just to be sure
        float val = Random.Range(0f, 1f); // generate random value [0,1]
        for (int enemyType = 0; enemyType < 3; enemyType++) // pick randomly weighted
        {
            if (val < weights[enemyType])
            {
                return enemyType;
            }
            val -= weights[enemyType];
        }
        return -1; // should not reach this line
    }

    // Generate spawn time, smaller spawn time at higher scores
    float GenerateSpawnTime()
    {
        float spawnTime = maxSpawnTime - ((maxSpawnTime - minSpawnTime) / (scoreRange)) * System.Math.Min(Score.score, scoreRange);
        spawnTime = spawnTime + (float)Random.Range(-0.2f * spawnTime, 0.2f * spawnTime);
        return spawnTime;
    }

    // spawn the enemies
    IEnumerator SpawnEnemies()
    {
        while( true )
        {
            // generate enemy type and random spawn position
            int enemyType = GenerateEnemyType();
            float xPos = (float)Random.Range(EnemyBehaviour.spawnRangeX[0], EnemyBehaviour.spawnRangeX[1]);
            float yPos = (float)Random.Range(EnemyBehaviour.spawnRangeY[0], EnemyBehaviour.spawnRangeY[1]);
            Vector3 spawn_position = new Vector3(xPos, yPos, 10.0f);
            // spawn enemy
            switch (enemyType){
                case 0:
                    Instantiate(chickenEnemy, spawn_position, Quaternion.Euler(0, 180, 0));
                    break;
                case 1:
                    Instantiate(condorEnemy, spawn_position, Quaternion.Euler(0, 180, 0));
                    break;
                case 2:
                    Instantiate(dragonEnemy, spawn_position, Quaternion.Euler(0, 180, 0));
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(GenerateSpawnTime());
        }
    }
}
