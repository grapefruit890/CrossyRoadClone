using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject CharacterMenu;
    public void startButton() {
        SceneManager.LoadScene("Game");
    }

    public void exitButton() {
        Application.Quit();
    }

    public void selectCharacterButton() {
        CharacterMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
