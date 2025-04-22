using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject BuiltInMenu;
    private bool cursorVisible = false;
    public bool isPaused = false;

    void Start() {
        showBuiltInMenu(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
            cursorVisible = !cursorVisible;
            showBuiltInMenu(cursorVisible);
        } 
    }

    void showBuiltInMenu(bool visible) {
        if (isPaused) Time.timeScale = 0f; else Time.timeScale = 1f;

        BuiltInMenu.SetActive(visible);
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void resumeButton() {
        isPaused = !isPaused;
        cursorVisible = !cursorVisible;
        showBuiltInMenu(cursorVisible);
    }

    public void restartButton() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void mainMenuButton() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void exitButton() {
        Application.Quit();
    }
}
