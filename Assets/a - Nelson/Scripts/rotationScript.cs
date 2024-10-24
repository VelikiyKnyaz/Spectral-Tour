using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    // Velocidad de rotación (en grados por segundo)
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    // Update se llama una vez por frame
    void Update()
    {
        // Rotar el objeto automáticamente
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}