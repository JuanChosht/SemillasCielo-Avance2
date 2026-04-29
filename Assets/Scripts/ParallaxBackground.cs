using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxSpeed = 0.3f; // entre 0 y 1, más bajo = más lento
    
    private float startPosX;
    private float spriteWidth;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        startPosX = transform.position.x;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        // Movimiento parallax
        float distX = cameraTransform.position.x * parallaxSpeed;
        transform.position = new Vector3(startPosX + distX, 
                                         transform.position.y, 
                                         transform.position.z);

        // Loop infinito del fondo
        float relativeX = cameraTransform.position.x * (1 - parallaxSpeed);
        if (relativeX > startPosX + spriteWidth)
            startPosX += spriteWidth;
        else if (relativeX < startPosX - spriteWidth)
            startPosX -= spriteWidth;
    }
}