using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    // Velocidad de rotaci�n (en grados por segundo)
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    // Update se llama una vez por frame
    void Update()
    {
        // Rotar el objeto autom�ticamente
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}