
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject[] trainPrefabs;
    public GameObject trafficLightPrefab;
    public AudioSource audioSource;
    public AudioClip clip;
    

    private Queue<TrainData> activeTrains = new Queue<TrainData>();
    private float spawnInterval;
    private float firstSpawnTime;
    private int startX;
    private int endX;
    public float speed;
    private Vector3 trafficLightPos;
    private GameObject trafficLight;
    private Transform light1;
    private Transform light2;
    private Renderer renderLight1;
    private Renderer renderLight2;
    private float blinkDuration = 1f;
    private Coroutine blinkRoutine;
    private float blinkSpeed = 0.2f;


    void Start()
    {




        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        // сделать спавн светофоров здесь
        trafficLightPos = new Vector3(transform.position.x, transform.position.y + 0.63f ,transform.position.z + 0.1f);
        
        trafficLight = Instantiate(trafficLightPrefab, trafficLightPos, Quaternion.identity);
        light1 = trafficLight.transform.Find("light1");
        light2 = trafficLight.transform.Find("light2");
        renderLight1 = light1.GetComponent<Renderer>();
        renderLight2 = light2.GetComponent<Renderer>();
        renderLight1.material.color = Color.black;
        renderLight2.material.color = Color.black;

        startX = Random.Range(0, 2) == 0 ? 20 : -25;
        if (startX == 20) {
            endX = -25;
        }
        else {
            endX = 20;
        }

        firstSpawnTime = Random.Range(3f, 10f);
        spawnInterval  = Random.Range(5f, 10f);

        InvokeRepeating("SpawnTrain", firstSpawnTime, spawnInterval);

    }

    void Update()
    {
        List<TrainData> trainsToRemove = new List<TrainData>();
        if (activeTrains.Count == 0) return;


        foreach (TrainData trainData in activeTrains)
        {
            if (trainData.obj == null) continue;

            trainData.obj.transform.position = Vector3.MoveTowards(
                trainData.obj.transform.position,
                trainData.targetPos,
                trainData.speed * Time.deltaTime
            );

            if (Vector3.Distance(trainData.obj.transform.position, trainData.targetPos) < 0.1f)
            {
                // trainsToRemove.Add(trainData);
                Destroy(trainData.obj);
            }

            if (player.transform.position.z - trainData.obj.transform.position.z > 6f)
            {
                // trainsToRemove.Add(trainData);
                Destroy(trainData.obj);
            }
        }

        if (trafficLight != null && player.transform.position.z - trafficLight.transform.position.z > 5f) {
            Destroy(trafficLight);
            trafficLight = null; 
            Destroy(renderLight1);
            renderLight1 = null;
            Destroy(renderLight2);
            renderLight2 = null;
        }

    }

    void SpawnTrain()
    {
        StartBlinking();
    }
    // Вспомогательный класс для хранения информации о поезде
    private class TrainData
    {
        public GameObject obj;
        public Vector3 targetPos;
        public float speed;

        public TrainData(GameObject obj, Vector3 targetPos, float speed)
        {
            this.obj = obj;
            this.targetPos = targetPos;
            this.speed = speed;
        }
    }
    IEnumerator BlinkLights() {
        float timer = 0f;

        if (trafficLight != null && audioSource != null) {
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }

            audioSource.clip = clip;
            audioSource.Play();
        } 

        while (timer < blinkDuration)
        {
            if (trafficLight == null) yield break;

            renderLight1.material.color = Color.black;
            renderLight2.material.color = Color.red;
            yield return new WaitForSeconds(blinkSpeed);
            timer += blinkSpeed;

            renderLight1.material.color = Color.red;
            renderLight2.material.color = Color.black;
            yield return new WaitForSeconds(blinkSpeed);
            timer += blinkSpeed;
        }

        GameObject trainPrefab = trainPrefabs[Random.Range(0, trainPrefabs.Length)];

        Quaternion rotation = startX == 20 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

        Vector3 spawnPos = new Vector3(startX, -0.4f, transform.position.z + 0.04f);
        Vector3 targetPos = new Vector3(endX, -0.4f, transform.position.z + 0.04f);

        GameObject train = Instantiate(trainPrefab, spawnPos, rotation);

        TrainData trainData = new TrainData(train, targetPos, speed);
        activeTrains.Enqueue(trainData);

        if (renderLight1 != null) {
            renderLight1.material.color = Color.black;
        }

        if (renderLight2 != null) {
            renderLight2.material.color = Color.black;
        }
        blinkRoutine = null;
    }

    public void StartBlinking() {
        if (blinkRoutine == null) {
            blinkRoutine = StartCoroutine(BlinkLights());
        }
    }

}
