using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject[] carPrefabs;
    public float minCarSpeed = 1f;
    public float maxCarSpeed = 7f;
    public float spawnInterval = 7f;

    private Queue<CarData> activeCars = new Queue<CarData>();
    private float previousSpeed;
    private int startX;
    private int endX;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        startX = Random.Range(0, 2) == 0 ? 12 : -12;
        endX = -startX; 

        previousSpeed = maxCarSpeed;
        InvokeRepeating("SpawnCar", 0f, spawnInterval);

    }

    void Update()
    {
        List<CarData> carsToRemove = new List<CarData>();
        if (activeCars.Count == 0) return;


        foreach (CarData carData in activeCars)
        {
            if (carData.obj == null) continue;

            carData.obj.transform.position = Vector3.MoveTowards(
                carData.obj.transform.position,
                carData.targetPos,
                carData.speed * Time.deltaTime
            );

            if (Vector3.Distance(carData.obj.transform.position, carData.targetPos) < 0.1f)
            {
                // carsToRemove.Add(carData);
                Destroy(carData.obj);
            }

            if (player.transform.position.z - carData.obj.transform.position.z > 6f)
            {
                // carsToRemove.Add(carData);
                Destroy(carData.obj);
            }
        }


        // foreach (CarData carData in carsToRemove)
        // {
        //     activeCars = new Queue<CarData>(activeCars); 
        //     activeCars.Dequeue(); 
        //     Destroy(carData.obj);
        // }
    }

    void SpawnCar()
    {
        GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        Quaternion rotation = startX == 12 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

        Vector3 spawnPos = new Vector3(startX, 0.1f, transform.position.z);
        Vector3 targetPos = new Vector3(endX, 0.1f, transform.position.z);
        float speed;

        if (activeCars.Count == 0) {
            speed = Random.Range(minCarSpeed, maxCarSpeed);
            previousSpeed = speed;
        }
        else {
            speed = Random.Range(minCarSpeed, previousSpeed);
            previousSpeed = speed;
        }


        GameObject car = Instantiate(carPrefab, spawnPos, rotation);

        CarData carData = new CarData(car, targetPos, speed);
        activeCars.Enqueue(carData);

    }
    // Вспомогательный класс для хранения информации о машине
    private class CarData
    {
        public GameObject obj;
        public Vector3 targetPos;
        public float speed;
        private Ray ray;
        private RaycastHit hit;

        public CarData(GameObject obj, Vector3 targetPos, float speed)
        {
            this.obj = obj;
            this.targetPos = targetPos;
            this.speed = speed;
        }

        public void Beep() {
            Debug.Log("Beeep");
        }
    }
}
