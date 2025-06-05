using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    public float startSpeed = 720f;
    public float endSpeed = 60f;
    public float decelerationTime = 2f;
    public int spinRounds = 5;

    private float currentSpeed;
    private float spinTime;
    private float targetSpinAngle;
    private float rotatedAngle;
    private bool finishedSpinning = false;

    void Start()
    {
        currentSpeed = startSpeed;
        spinTime = 0f;
        rotatedAngle = 0f;
        targetSpinAngle = 360f * spinRounds;
    }

    void Update()
    {
        float delta = currentSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, delta);

        if (!finishedSpinning)
        {
            rotatedAngle += delta;

            if (rotatedAngle >= targetSpinAngle)
            {
                finishedSpinning = true;
                currentSpeed = endSpeed;
            }
            else
            {
                float t = rotatedAngle / targetSpinAngle; // от 0 до 1
                currentSpeed = Mathf.Lerp(startSpeed, endSpeed, t);
            }
        }
    }
}
