using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class shieldScript : MonoBehaviour
{
    public bool immune = false;
    public float duration;

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
           StartCoroutine("setImmune"); 
           Destroy(gameObject);
        }
    }

    IEnumerator setImmune() {
        immune = true;

        yield return new WaitForSeconds(duration);

        immune = false;
    }
}

