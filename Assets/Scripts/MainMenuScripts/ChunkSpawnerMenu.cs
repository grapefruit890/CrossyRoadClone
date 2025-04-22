using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawnerMenu : MonoBehaviour
{
    private GameObject roadLine;
    // private GameObject trafficLight;
    private int grassCounter = 0;
    private int roadCounter = 0;
    public Transform player;
    public GameObject[] chunkPrefabs; 
    public float chunkSize = 5; 
    public int initialChunks = 5; 
    public GameObject linePrefab;
    // public GameObject trafficLightPrefab;

    public GameObject newChunk;

    public Queue<GameObject> activeChunks = new Queue<GameObject>();
    private Queue<GameObject> activeRoadLines = new Queue<GameObject>();
    // public Queue<GameObject> activeTrafficLights = new Queue<GameObject>();
    private Vector3 nextSpawnPosition = Vector3.zero;

    void Start()
    {
        for (int i = 0; i < initialChunks; i++)
        {
            SpawnChunk();
        }

    }

    void Update()
    {
        if (Vector3.Distance(player.position, nextSpawnPosition) < chunkSize * 9f) 
        {
            SpawnChunk();
        }

        if (activeChunks.Count > 0) {
            GameObject oldestChunk = activeChunks.Peek();

            if (player.transform.position.z - oldestChunk.transform.position.z > 7f)
            {
                GameObject chunkToRemove = activeChunks.Dequeue();
                Destroy(chunkToRemove);
            }
        }

        if (activeRoadLines.Count != 0) {
            if (player.transform.position.z - activeRoadLines.Peek().transform.position.z > 6f) {
                GameObject oldRoadLine = activeRoadLines.Dequeue();
                Destroy(oldRoadLine);
            }
        }

        // if (activeTrafficLights.Count != 0) {
        //     if (player.transform.position.z - activeTrafficLights.Peek().transform.position.z > 6f) {
        //         GameObject oldTrafficLight = activeTrafficLights.Dequeue();
        //         Destroy(oldTrafficLight);
        //     }
        // }
    }


    void SpawnChunk()
    {
        GameObject chunkPrefab = chunkPrefabs[UnityEngine.Random.Range(0, chunkPrefabs.Length)];

        if (chunkPrefab.name == "grass_1_menu" || chunkPrefab.name == "grass_2_menu") {
            roadCounter = 0;
            grassCounter++;

            if (grassCounter % 2 == 0) {
                chunkPrefab = chunkPrefabs[0];
            }
            else {
                chunkPrefab = chunkPrefabs[1];
            }
        }

        if (chunkPrefab.name == "road_menu") {
            grassCounter = 0;;
            roadCounter++;

            if (roadCounter >= 2) {
                Vector3 linePos = new Vector3(nextSpawnPosition.x, nextSpawnPosition.y + 0.11f, nextSpawnPosition.z - (chunkSize / 2));
                roadLine = Instantiate(linePrefab, linePos, Quaternion.identity);
                activeRoadLines.Enqueue(roadLine);
            }
        }

        if (chunkPrefab.name == "railway_menu") {
            roadCounter = 0;
            grassCounter = 0;

            // Vector3 trafficLightPos = new Vector3(nextSpawnPosition.x, nextSpawnPosition.y + 0.62f, nextSpawnPosition.z + 0.1f);
            // trafficLight = Instantiate(trafficLightPrefab, trafficLightPos, Quaternion.identity);
            // activeTrafficLights.Enqueue(trafficLight);
        }

        if (chunkPrefab.name == "water_menu") {
            roadCounter = 0;
            grassCounter = 0;
        }

        newChunk = Instantiate(chunkPrefab, nextSpawnPosition, Quaternion.identity);
        
        activeChunks.Enqueue(newChunk);
        nextSpawnPosition += new Vector3(0, 0, chunkSize);

    }

}
