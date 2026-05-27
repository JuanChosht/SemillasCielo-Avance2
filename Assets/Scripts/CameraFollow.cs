using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public Transform target;
    public float smoothSpeed = 5f;
    public float offsetX = 0f;
    public float offsetY = 2f;
    public float minY = -3f; // límite mínimo de la cámara

    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.2f;
    private float shakeTimer;

    void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float targetY = Mathf.Max(target.position.y + offsetY, minY);

        Vector3 targetPos = new Vector3(
            target.position.x + offsetX,
            targetY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        if (shakeTimer > 0)
        {
            transform.position += (Vector3)Random.insideUnitCircle * shakeMagnitude;
            shakeTimer -= Time.deltaTime;
        }
    }

    public void Shake()
    {
        shakeTimer = shakeDuration;
    }
}