using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpener : MonoBehaviour
{
    public Animator boxAnimator;
    public Animator flashPanelAnimator;
    public Animator rotateAnimation;
    public GameObject box;
    public GameObject mainMenu;
    public GameObject bestScore;
    public GameObject BackButton;
    public GameObject[] characterPrefabs;
    public GameObject characterPrefab;
    public UImanager_menu UImenu;
    GameObject go;

    public int totalCharacters = 6;

    void OnEnable()
    {
        boxOpen();
        StartCoroutine(PlayOpenBoxAndFlash());
    }

    private IEnumerator PlayOpenBoxAndFlash()
    {
        box.SetActive(true);

        boxAnimator.Play("boxOpening", 0, 0f);
        yield return new WaitForSeconds(2f);
        flashPanelAnimator.Play("flashPanel", 0, 0f);
        yield return new WaitForSeconds(1f);
        box.SetActive(false);
        go = Instantiate(characterPrefab);
        yield return new WaitForSeconds(1f);
        BackButton.SetActive(true);
    }

    public void backButton()
    {
        Destroy(go);
        gameObject.SetActive(false);
        BackButton.SetActive(false);
        mainMenu.SetActive(true);
        bestScore.SetActive(true);
    }


    private void boxOpen()
    {
        GameData data = BinarySaveSystem.Load();

        List<int> locked = new List<int>();

        for (int i = 0; i < totalCharacters; i++)
        {
            if (!data.unlockedCharacters.Contains(i))
            {
                locked.Add(i);
            }
        }

        if (locked.Count == 0)
        {
            Debug.Log("Все персонажи уже открыты");
            return;
        }

        int newCharacter = locked[Random.Range(0, locked.Count)];
        data.unlockedCharacters.Add(newCharacter);
        data.selectedCharacter = newCharacter;
        data.coins -= 1;
        BinarySaveSystem.Save(data);

        UImenu.UpdateCoinsUI();
        Debug.Log("Открыт новый персонаж: " + newCharacter);

        characterPrefab = characterPrefabs[newCharacter];
    }

}