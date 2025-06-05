using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactersMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject boxMenu;
    public GameObject bestScore;
    public GameObject boxButton;
    private GameData loadedCharacter;
    public static int selectedCharacterID = 0;
    public int totalCharacters = 6;

    public List<GameObject> CharacterButtons;

    void OnEnable()
    {
        GameData data = BinarySaveSystem.Load();
        List<int> unlocked = data.unlockedCharacters;

        for (int i = 0; i < CharacterButtons.Count; i++)
        {
            GameObject button = CharacterButtons[i];
            bool isUnlocked = unlocked.Contains(i);

            Image characterIcon = button.transform.Find("Image").GetComponentInChildren<Image>();
            if (characterIcon != null)
            {
                characterIcon.color = isUnlocked ? Color.white : new Color(0f, 0f, 0f, 1f);
            }

            // Button btn = button.GetComponent<Button>();
            // btn.interactable = isUnlocked;
        }

        updateBoxButton();

    }

    public void backButton()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);

        bestScore.SetActive(true);

    }

    public void selectCharacter(int id)
    {
        Debug.Log("Выбран персонаж: " + id);

        GameData data = BinarySaveSystem.Load();

        if (data.unlockedCharacters.Contains(id))
        {
            data.selectedCharacter = id;
            selectedCharacterID = id;
            // updateSelectUI();

            BinarySaveSystem.Save(data);
            Debug.Log("Персонаж сохранен");
        }

        else
        {
            Debug.Log("Персонаж " + id + " еще не разблокирован");
        }

        // updateSelectUI();
    }

    public void openButton()
    {
        gameObject.SetActive(false);
        boxMenu.SetActive(true);
    }

    public void updateBoxButton()
    {
        GameData data = BinarySaveSystem.Load();

        bool allUnlocked = data.unlockedCharacters.Count >= totalCharacters;
        bool hasEnoughCoins = data.coins >= 1;

        boxButton.GetComponent<Button>().interactable = !allUnlocked && hasEnoughCoins;
    }

    // private void updateSelectUI()
    // {
    //     for (int i = 0; i < CharacterButtons.Count; i++)
    //     {
    //         Image frame = CharacterButtons[i].GetComponentInChildren<Image>();
    //         if (frame != null)
    //         {
    //             frame.color = (i == selectedCharacterID) ? new Color(120f, 255f, 120f, 255f): new Color(150f, 150f, 150f, 255f);
    //         }
    //     }
    // }
}
