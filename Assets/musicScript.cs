using UnityEngine;

public class musicScript : MonoBehaviour
{
    private static musicScript instance;

    public AudioSource musicSource;
    private bool musicEnabled = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void toogleMusic()
    {
        musicEnabled = !musicEnabled;
        musicSource.volume = musicEnabled ? 1f : 0f;
    }
}
