using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UImanager : MonoBehaviour
{
    public TextMeshProUGUI scoreMessage;
    public TextMeshProUGUI DebugInfoMessage;
    public TextMeshProUGUI deadMessage;
    public PlayerController player;
    private float DebugInfo;
    private Vector3 XYZ;
    private TextMeshProUGUI XYZMessage;
    private float previousScore;
    private int previousCoins;
    void Start()
    {
        StartCoroutine(WaitForPlayer());
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        InvokeRepeating("ShowDebugInfo", 0f, 1f);
        previousScore = 0;
        previousCoins = 0;
        // Application.targetFrameRate = 60;
        // QualitySettings.vSyncCount = 0;
        scoreMessage.text = "Score: 0\nCoins: 0";     

        // deadMessage.gameObject.SetActive(false);

        // ShowDebugInfo();
    }

    void Update()
    {
        if (player.scoreCounter != previousScore || player.coinsCounter != previousCoins) {
            scoreMessage.text = "Score: " + player.scoreCounter + "\nCoins: " + player.coinsCounter;

            deadMessage.text = "Your score: " + player.scoreCounter + "\nPress SPACE to restart";

            previousScore = player.scoreCounter;
            previousCoins = player.coinsCounter;
        }
        
        if (!(player.isAlive)) deadMessage.gameObject.SetActive(true);
    }

    /// <summary>
    /// Отобразить отладочную информацию (для разработчика)
    /// </summary>
    void ShowDebugInfo() {
        DebugInfo = 1f / Time.deltaTime;
        XYZ = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        DebugInfoMessage.text = "FPS: " + Mathf.RoundToInt(DebugInfo) + "\nXYZ: " + XYZ;


    }
    IEnumerator WaitForPlayer()
    {
        while (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
                player = obj.GetComponent<PlayerController>();

            yield return null;
        }
    }

}
