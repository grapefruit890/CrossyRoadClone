using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    
    public Vector3 offset = new Vector3(0, 15, -10);
    public Vector3 offsetBackward = new Vector3(0, 15, -15);
    
    public float smoothSpeed = 0.125f;
    public float offsetSmoothSpeed = 5f;
    
    private bool isBackward = false;
    private Vector3 currentOffset;

    void Start()
    {
        // if (target == null)
        // {
        //     target = GameObject.FindGameObjectWithTag("Player").transform;
        // }
        StartCoroutine(WaitForPlayer());
        currentOffset = offset;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            isBackward = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            isBackward = false;
        }

        Vector3 desiredOffset = isBackward ? offsetBackward : offset;
        currentOffset = Vector3.Lerp(currentOffset, desiredOffset, offsetSmoothSpeed * Time.deltaTime);

        Vector3 desiredPosition = new Vector3(target.position.x + currentOffset.x,
                                               currentOffset.y,
                                               target.position.z + currentOffset.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
    IEnumerator WaitForPlayer()
    {
        while (target == null)
        {
            GameObject ply = GameObject.FindGameObjectWithTag("Player");
            if (ply != null)
                target = ply.transform;

            yield return null;
        }
    }

}
