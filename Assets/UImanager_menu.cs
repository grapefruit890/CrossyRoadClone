using TMPro;
using UnityEngine;

public class UImanager_menu : MonoBehaviour
{
    public TextMeshProUGUI bestScoreMessage;
    private int bestScore;
    void Start()
    {
        // bestScore = PlayerPrefs.GetInt("BestScore", 0);
        // bestScoreMessage.text = "Best Score: " + bestScore;

        GameData loadedData = BinarySaveSystem.Load();
        bestScoreMessage.text = "Your best score: " + loadedData.bestScore + "\nCoins: " + loadedData.coins;
    }

    public void UpdateCoinsUI()
    {
        GameData data = BinarySaveSystem.Load();
        bestScoreMessage.text = "Your best score: " + data.bestScore + "\nCoins: " + data.coins;
    }
}
