using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    private GameObject roadLine;
    private int grassCounter = 0;
    private int roadCounter = 0;
    private float maxZ;
    public Transform player;
    public GameObject[] chunkPrefabs; 
    public float chunkSize = 5; 
    public int initialChunks = 5; 
    public GameObject linePrefab;

    public GameObject newChunk;

    public Queue<GameObject> activeChunks = new Queue<GameObject>();
    private Queue<GameObject> activeRoadLines = new Queue<GameObject>();
    private Vector3 nextSpawnPosition = Vector3.zero;
    private GameData data;
    public GameObject[] characterPrefabs;
    public GameObject characterPrefab;
    

    void Start()
    {
        // if (player == null)
        // {
        //     player = GameObject.FindGameObjectWithTag("Player").transform;
        // }

        if (characterPrefab == null)
        {
            data = BinarySaveSystem.Load();
            int id = data.selectedCharacter;
            characterPrefab = characterPrefabs[id];

            player = Instantiate(characterPrefab).transform;
        }

        int backwardChunks = 8;
        nextSpawnPosition = new Vector3(0, 0, -chunkSize * backwardChunks);

        for (int i = 0; i < initialChunks; i++)
        {
            SpawnChunk();
        }

    }

    void Update()
    {
        if (Vector3.Distance(player.position, nextSpawnPosition) < chunkSize * 13f) 
        {
            SpawnChunk();
        }

        if (activeChunks.Count > 0) {
            GameObject oldestChunk = activeChunks.Peek();

            maxZ = player.GetComponent<PlayerController>().GetMaxZ();
            if (maxZ - oldestChunk.transform.position.z > 8f)
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
    }


    /// <summary> Функция генерации чанков </summary>
    void SpawnChunk()
    {
        GameObject chunkPrefab = chunkPrefabs[UnityEngine.Random.Range(0, chunkPrefabs.Length)];

        if (chunkPrefab.name == "grass_1" || chunkPrefab.name == "grass_2") {
            roadCounter = 0;
            grassCounter++;

            if (grassCounter % 2 == 0) {
                chunkPrefab = chunkPrefabs[0];
            }
            else {
                chunkPrefab = chunkPrefabs[1];
            }
        }

        if (chunkPrefab.name == "road") {
            grassCounter = 0;;
            roadCounter++;

            if (roadCounter >= 2) {
                Vector3 linePos = new Vector3(nextSpawnPosition.x, nextSpawnPosition.y + 0.11f, nextSpawnPosition.z - (chunkSize / 2));
                roadLine = Instantiate(linePrefab, linePos, Quaternion.identity);
                activeRoadLines.Enqueue(roadLine);
            }
        }

        if (chunkPrefab.name == "railway") {
            roadCounter = 0;
            grassCounter = 0;
        }

        if (chunkPrefab.name == "water") {
            roadCounter = 0;
            grassCounter = 0;
        }

        newChunk = Instantiate(chunkPrefab, nextSpawnPosition, Quaternion.identity);
        
        activeChunks.Enqueue(newChunk);
        nextSpawnPosition += new Vector3(0, 0, chunkSize);
    }
}
