using UnityEngine;

public class CharactersMenu : MonoBehaviour
{
    public GameObject mainMenu;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void backButton() {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
