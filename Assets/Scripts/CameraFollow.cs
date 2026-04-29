using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // NOVA
    public float smoothSpeed = 5f;
    public float offsetX = 0f;
    public float offsetY = 2f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = new Vector3(
            target.position.x + offsetX,
            target.position.y + offsetY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}