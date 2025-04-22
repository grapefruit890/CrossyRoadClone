using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class waterSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject waterLilyPrefab;
    private GameObject waterLily;
    public GameObject waterSplashPrefab;
    void Start()
    {
        if (player == null) {
            player = GameObject.FindWithTag("Player");
        }
        Vector3 spawnPos = new Vector3(transform.position.x -0.057f, transform.position.y, transform.position.z + 0.15f);
        if (waterLily == null) {
            waterLily = Instantiate(waterLilyPrefab, spawnPos, Quaternion.Euler(transform.rotation.x, transform.rotation.y - 225f, transform.rotation.z)); 
        }
    }

    void Update() {
        if (waterLily != null && player != null) {
            if (player.transform.position.z - waterLily.transform.position.z > 5f) {
                Destroy(waterLily);
                waterLily = null;
            }
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("Player")) {
            Vector3 pos = new Vector3 (player.transform.position.x, player.transform.position.y - 0.3f, player.transform.position.z);
            Instantiate(waterSplashPrefab, pos, Quaternion.identity);
        }
    }
}
