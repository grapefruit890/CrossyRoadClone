using UnityEngine;

public class CameraLookHover : MonoBehaviour
{
    public float sensitivity = 0.7f;
    public float maxAngle = 10f;
    public float smoothSpeed = 7f;

    private Quaternion initialRotation;

    void Start()
    {
        // Сохраняем базовый поворот камеры
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Позиция мыши относительно центра экрана (-0.5 до +0.5)
        Vector2 mouseOffset = new Vector2(
            (Input.mousePosition.x / Screen.width - 0.5f),
            (Input.mousePosition.y / Screen.height - 0.5f)
        );

        // Вычисляем добавочные углы (не абсолютные!)
        float rotX = -mouseOffset.y * maxAngle * sensitivity;
        float rotY = mouseOffset.x * maxAngle * sensitivity;

        // Создаём "добавочный" поворот
        Quaternion hoverRotation = Quaternion.Euler(rotX, rotY, 0);

        // Итоговый поворот: базовый * добавка
        Quaternion targetRotation = initialRotation * hoverRotation;

        // Плавное движение к цели
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}
