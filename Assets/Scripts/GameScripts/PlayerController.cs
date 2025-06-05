using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameManagerScript GameManager;
    public AudioSource audioSource;
    public Transform audioListenerTransform;
    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip deadSound;
    private bool isGrounded = false;
    private Rigidbody rb;
    private Animator anim;
    private Vector3 nextDir;
    private Vector3 currentPos;
    private float maxZ;
    private Vector3 bufferedInput;
    public float deathTimer = 0f;
    private int backJumps = 0;
    public float scoreCounter = 0;
    public int coinsCounter;
    public float jumpForce = 100f;
    public float speed = 5f;
    public float speedRot = 1000f;
    public float rotationOffset;
    public bool isFirstTurn = false;
    public bool isAlive = true;
    public int bestScore;
    public UImanager uimanager;

    private float holdTimer = 0f;
    private bool isHoldingKey = false;
    private KeyCode heldKey = KeyCode.None;
    public float squishThreshold = 0.2f;

    public shieldScript shield;
    private AudioClip currentAudioClip;
    private GameData loadedData;


    void Start()
    {
        // if (GameManager == null)
        // {
        //     GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        // }

        StartCoroutine(WaitForGameManager());
        StartCoroutine(WaitForAudioListener());

        // if (audioListenerTransform = null)
        // {
        //     audioListenerTransform = GameObject.FindGameObjectWithTag("AudioListener").transform;
        // }
        loadedData = BinarySaveSystem.Load();
        coinsCounter = loadedData.coins;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentPos = transform.position;
        nextDir = Vector3.zero;
        bufferedInput = Vector3.zero;
        scoreCounter = 0;
        backJumps = 0;
        maxZ = transform.position.z;
    }

    void Update()
    {
        if (!isAlive) return;

        deathTimer += Time.deltaTime;
        if ((maxZ - transform.position.z > 2.7f) || transform.position.y < -1 || (isFirstTurn && deathTimer >= 7))
        {
            StartCoroutine(Dead());
        }

        // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
        //     if (!isHoldingKey) {
        //         isHoldingKey = true;
        //         holdTimer = 0f;

        //         if (Input.GetKey(KeyCode.W)) heldKey = KeyCode.W;
        //         else if (Input.GetKey(KeyCode.A)) heldKey = KeyCode.A;
        //         else if (Input.GetKey(KeyCode.S)) heldKey = KeyCode.S;
        //         else if (Input.GetKey(KeyCode.D)) heldKey = KeyCode.D;
        //     }

        //     holdTimer += Time.deltaTime;

        //     if (holdTimer >= squishThreshold) {
        //         anim.SetInteger("isJumping", 1);
        //     }
        // }

        // else {
        //     isHoldingKey = false;
        //     holdTimer = 0f;
        //     heldKey = KeyCode.None;
        // }

        if (Input.GetKeyDown(KeyCode.A) && isAlive && !GameManager.isPaused)
        {
            if (transform.position.x >= -3.1)
            {
                anim.SetInteger("isJumping", 0);
                anim.SetInteger("noGround", 1);
                bufferedInput = new Vector3(-1, 0, 0);
                // backJumps = 0; ;
                deathTimer = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && isAlive && !GameManager.isPaused)
        {
            if (transform.position.x <= 3.1)
            {
                anim.SetInteger("isJumping", 0);
                anim.SetInteger("noGround", 1);
                bufferedInput = new Vector3(1, 0, 0);
                // backJumps = 0; ;
                deathTimer = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) && isAlive && !GameManager.isPaused)
        {
            anim.SetInteger("isJumping", 0);
            anim.SetInteger("noGround", 1);
            bufferedInput = new Vector3(0, 0, 1);
            // backJumps = 0; ;
            deathTimer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.S) && isAlive && !GameManager.isPaused)
        {
            anim.SetInteger("isJumping", 0);
            anim.SetInteger("noGround", 1);
            bufferedInput = new Vector3(0, 0, -1);
            backJumps++;
            deathTimer = 0;
        }

        Vector3 targetPos = new Vector3(currentPos.x, transform.position.y, currentPos.z) + nextDir;

        if (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (nextDir != Vector3.zero)
            {
                Vector3 lookDir = Quaternion.Euler(0, rotationOffset, 0) * nextDir;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDir), speedRot * Time.deltaTime);
            }
        }
        else
        {
            nextDir = Vector3.zero;
            currentPos = transform.position;

            if (currentPos.z > maxZ) maxZ = currentPos.z;

            currentPos.x = Mathf.Round(currentPos.x);
            currentPos.z = Mathf.Round(currentPos.z);

            if (bufferedInput != Vector3.zero && isGrounded)
            {
                if (CanJump(bufferedInput))
                {
                    nextDir = bufferedInput;
                    bufferedInput = Vector3.zero;
                    Move();
                }

                else
                {
                    bufferedInput = Vector3.zero;
                }
            }
        }
    }

    void LateUpdate()
    {
        if (audioListenerTransform != null)
        {
            audioListenerTransform.position = transform.position;
        }
    }

    void Move()
    {
        rb.AddForce(0, jumpForce, 0);
        PlayJumpSound();
        isGrounded = false;
        isFirstTurn = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            // anim.SetInteger("noGround", 0);
            if (transform.position.z > scoreCounter)
            {
                scoreCounter++;
                scoreCounter = Mathf.Round(scoreCounter);
                if (scoreCounter > bestScore) bestScore = (int)scoreCounter;
                // PlayerPrefs.SetInt("BestScore", bestScore);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!isAlive) return;
        if (collider.gameObject.CompareTag("Car") || collider.gameObject.CompareTag("Truck") || collider.gameObject.CompareTag("Train"))
        {
            Debug.Log("Dead!");
            StartCoroutine(Dead());
        }

        if (collider.gameObject.CompareTag("Coin"))
        {
            coinsCounter++;
            Debug.Log("Опа, монетка!");
            Destroy(collider.gameObject);

            if (audioSource != null)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                currentAudioClip = coinSound;
                audioSource.PlayOneShot(coinSound);
            }
        }

    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    IEnumerator Dead()
    {
        if (!isAlive) yield break;
        isAlive = false;
        anim.SetInteger("isDead", 1);

        bool saveFlag = false;
        if (scoreCounter > loadedData.bestScore)
        {
            loadedData.bestScore = (int)scoreCounter;
            saveFlag = true;
        }
        if (coinsCounter > loadedData.coins)
        {
            loadedData.coins = coinsCounter;
            saveFlag = true;
        }

        if (saveFlag) BinarySaveSystem.Save(loadedData);


        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // transform.localScale = new Vector3(0.4f, 0.1f, 0.4f);
    }

    void PlayJumpSound()
    {
        if (audioSource != null)
        {
            // if (audioSource.isPlaying) {
            //     audioSource.Stop();
            // }

            // if (currentAudioClip == coinSound) {
            //     currentAudioClip = null;
            //     return;
            // }
            audioSource.PlayOneShot(jumpSound);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, transform.forward);
    }

    bool CanJump(Vector3 direction)
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, direction);
        RaycastHit hit;

        float distance = 1f;

        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.CompareTag("Tree"))
            {
                Debug.Log("Дерево впереди, прыжок отменён");
                return false;
            }
        }

        return true;
    }

    // public void OnSwipe(Vector3 direction) {
    //     if (transform.position.x <= 3.1 && transform.position.x >= -3.1) {
    //         bufferedInput = direction;
    //         backJumps = direction == Vector3.back ? backJumps + 1 : 0;
    //         deathTimer = 0f;
    //     }
    // }


    IEnumerator WaitForAudioListener()
    {
        while (audioListenerTransform == null)
        {
            audioListenerTransform = GameObject.FindGameObjectWithTag("AudioListener").transform;
            yield return null;
        }
    }

    IEnumerator WaitForGameManager()
    {
        while (GameManager == null)
        {
            GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
            yield return null;
        }
    }

    public float GetMaxZ() // для chunkspawner
    {
        return maxZ;
    }
}
