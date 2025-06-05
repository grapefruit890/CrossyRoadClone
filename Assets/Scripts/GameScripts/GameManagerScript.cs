using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    /// <summary>
    /// Ссылка на объект встроенного меню
    /// </summary>
    /// <example>
    /// Например, легко включить/отключить:
    /// <code>
    /// void Update() {
    /// BuiltInMenu.setActive(false);
    /// </code>
    /// </example>
    /// }
    public GameObject BuiltInMenu;
    private GameObject player;
    private Transform spawnPoint;
    // public GameObject[] characterPrefabs;
    private GameData data;
    private bool cursorVisible = false;
    public bool isPaused = false;

    void Start()
    {
        showBuiltInMenu(false);

        // data = BinarySaveSystem.Load();
        // int id = data.selectedCharacter;
        // player = characterPrefabs[id];

        // player = Instantiate(characterPrefabs[id], spawnPoint.position, spawnPoint.rotation);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
            cursorVisible = !cursorVisible;
            showBuiltInMenu(cursorVisible);
        } 
    }

    /// <summary>
    /// Функция отображения встроенного игрового меню (меню паузы) по нажатию на Escape
    /// </summary>
    /// <param name="visible">Флаг видимости игрового меню</param>
    /// <example>
    /// Пример использования фукнции для вызова встроенного меню
    /// <code>
    /// void Update()
    /// {
    ///     if (Input.GetKeyDown(KeyCode.Escape)) {
    ///     isPaused = !isPaused;
    ///     cursorVisible = !cursorVisible;
    ///     showBuiltInMenu(cursorVisible);
    /// } 
    /// }
    /// </code>
    /// </example>
    void showBuiltInMenu(bool visible) {
        if (isPaused) Time.timeScale = 0f; else Time.timeScale = 1f;

        BuiltInMenu.SetActive(visible);
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    /// <summary>
    /// Функция для скрытия встроенного меню и продолжения игры по нажатию на UI кнопку
    /// </summary>
    public void resumeButton() {
        isPaused = !isPaused;
        cursorVisible = !cursorVisible;
        showBuiltInMenu(cursorVisible);
    }

    /// <summary>
    /// Функция перезагрузки уровня по нажатию на UI кнопку
    /// </summary>
    public void restartButton() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Функция возвращения в главное меню игры по нажатию на UI кнопку
    /// </summary>
    public void mainMenuButton() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Функция полного выхода из игры по нажатию на UI кнопку
    /// </summary>
    public void exitButton() {
        Application.Quit();
    }
}
