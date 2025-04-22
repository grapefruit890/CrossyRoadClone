using Unity.VisualScripting;
using UnityEngine;

public class water_lily_animation : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.Play("water_lily_splash", 0, 0f);
            // Debug.Log("water_lily_splash");
        }
    }
}
