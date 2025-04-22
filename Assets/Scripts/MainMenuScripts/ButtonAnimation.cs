using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    public Animator animator;
    void Start() {
        animator = GetComponent<Animator>();
        animator.Play("StartButtonAnim", -1, 0f);  
    }

    void Update() {
        
    }
}
