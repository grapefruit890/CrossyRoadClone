using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawnerMeny : MonoBehaviour
{
    public GameObject player;
    public GameObject[] treePrefabs;
    public GameObject coinPrefab;
    public float coinSpawnChance;

    private GameObject treePrefab;
    private Vector3 treePos;
    private Vector3 coinPos;
    private Queue<GameObject> activeTrees = new Queue<GameObject>();
    
    private GameObject newTree;
    private GameObject newCoin;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
       StartCoroutine(treeSpawn()); 
    }

    void Update()
    {
        if (activeTrees.Count > 0) {
            GameObject oldestTree = activeTrees.Peek();

            if (player.transform.position.z - oldestTree.transform.position.z > 6f)
            {
                GameObject treeToRemove = activeTrees.Dequeue();
                Destroy(treeToRemove);
            }
        }

        if (player != null && newCoin != null) {
            if (player.transform.position.z - newCoin.transform.position.z > 5f) {
                Destroy(newCoin);
                newCoin = null;
            }
        }
        
    }

IEnumerator treeSpawn()
{
    bool spawnCoin = Random.value < coinSpawnChance;
    List<int> xCoordinates = new List<int>() {-14, -13, -12, -11, -10, -9, -8, -7, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
    int treesToSpawn = Random.Range(1, 10); 
    int coinsToSpawn = Random.Range(0, 2);

    int treesCounter = 0;
    int coinsCounter = 0;

    while (xCoordinates.Count > 0 && (treesCounter < treesToSpawn))
    {
        if (treesCounter < treesToSpawn && xCoordinates.Count > 0)
        {
            treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            int index = Random.Range(0, xCoordinates.Count);
            int x = xCoordinates[index];
            xCoordinates.RemoveAt(index);
            treePos = new Vector3(x, transform.position.y + 0.2f, transform.position.z);
            newTree = Instantiate(treePrefab, treePos, Quaternion.identity);
            activeTrees.Enqueue(newTree);
            treesCounter++;
            yield return null;
        }

        if (spawnCoin && coinsCounter == 0 && xCoordinates.Count > 0)
        {
            int index = Random.Range(0, xCoordinates.Count);
            int x = xCoordinates[index];
            xCoordinates.RemoveAt(index);
            coinPos = new Vector3(x, transform.position.y + 0.2f, transform.position.z);
            newCoin = Instantiate(coinPrefab, coinPos, Quaternion.identity);
            coinsCounter++;
            yield return null;
        }
    }
}

}
