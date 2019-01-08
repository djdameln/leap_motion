using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGenerator : MonoBehaviour {

    public GameObject bomb; // bomb prefab
    float maxSpawnTime = 15f;
    float minSpawnTime = 5f;
    float timeSpan = 180f;
    float minDist = 0.3f;

    /* there are 6 regions in the workspace where bombs can spawn,
      this means there can be at most 6 bombs at the same time */
    Vector3[] spawnRegions = new Vector3[6]; // array of region centroids
    List<int> freeRegions = new List<int>(); // list of regions not holding a bomb

    // initialize
    void Awake(){
        InitializeSpawnRegions(); // set region centroids
        bomb = Resources.Load("Bomb") as GameObject;
    }

    // hard code spawn region centroids, initialize all regions as 'free'
    void InitializeSpawnRegions()
    {
        spawnRegions[0] = new Vector3(-0.3f, 0.55f, 0.2f);
        spawnRegions[1] = new Vector3(0f, 0.55f, 0.2f);
        spawnRegions[2] = new Vector3(0.3f, 0.55f, 0.2f);
        spawnRegions[3] = new Vector3(-0.3f, 0.25f, 0.2f);
        spawnRegions[4] = new Vector3(0f, 0.25f, 0.2f);
        spawnRegions[5] = new Vector3(0.3f, 0.25f, 0.2f);

        for (int region=0; region<6; region++)
        {
            freeRegions.Add(region);
        }
    }

    // generate smaller spawn time as play time increases
    float GenerateSpawnTime()
    {
        float spawnTime;
        if (Time.timeSinceLevelLoad > timeSpan) // highest difficulty reached
        {
            spawnTime = minSpawnTime;
        }
        else
        {
            spawnTime = maxSpawnTime - ((maxSpawnTime - minSpawnTime) / (timeSpan)) * Time.timeSinceLevelLoad;
        }
        return spawnTime;
    }

    public void CheckFieldEmpty()
    {
        if (freeRegions.Count == 6)
        {
            StopAllCoroutines();
            StartCoroutine(InitializeSpawn(0.0f));
        }
    }

    // stop generating bombs
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // start spawning bombs
    private void OnEnable()
    {
        StartCoroutine(InitializeSpawn(3.0f));
    }

    // this method delays the spawning of the first bomb
    IEnumerator InitializeSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(SpawnBombs());
    }

    // method that repeatedly generated a bomb in a random unoccupied region
    IEnumerator SpawnBombs(){
        while (true){
            
            if (freeRegions.Count>0) // only spawn bomb if there is a free region
            {
                int region = freeRegions[Random.Range(0, freeRegions.Count)]; // pick random region
                // pick spawn position with random offset around region centroid
                Vector3 spawnPosition = spawnRegions[region] + new Vector3((float)Random.Range(-0.05f, 0.05f), (float)Random.Range(-0.05f, 0.05f), 0f);
                freeRegions.Remove(region); // remove region from list of free regions
                GameObject newBomb = Instantiate(bomb, spawnRegions[region], Quaternion.Euler(0, 180, 0)) as GameObject; // instantiate bomb
                newBomb.GetComponent<BombBehaviour>().setRegion(region); // tell bomb which region it is in
                newBomb.GetComponent<BombBehaviour>().setBombGenerator(this); // create reference to this bomb generator
            }
            yield return new WaitForSeconds(GenerateSpawnTime());
        }
    }

    // add a region to the list of free regions, called from BombBehaviour
    public void FreeRegion(int region)
    {
        freeRegions.Add(region);
        if (freeRegions.Count == 6) // if there are no bombs left, do not wait for next spawn time, spawn new bomb immediately
        {
            StopAllCoroutines();
            StartCoroutine(InitializeSpawn(0.5f));
        }
    }

}
