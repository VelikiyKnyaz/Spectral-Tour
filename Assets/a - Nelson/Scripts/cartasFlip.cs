using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    public float rotationSpeed = 360f; // Grados por segundo
    public GameObject cardBack;
    public bool cardBackIsActive;
    private bool isFlipping = false;
    private Quaternion targetRotation;
    private bool hasFlipped = false; // Para evitar que gire más de una vez

    // Start is called before the first frame update
    void Start()
    {
        cardBackIsActive = false;
        targetRotation = transform.rotation; // Inicializa la rotación objetivo
    }

    // Update is called once per frame
    void Update()
    {
        // Solo permite el giro si no se ha hecho ya
        if (Input.GetMouseButtonDown(0) && !isFlipping && !hasFlipped)
        {
            StartFlip();
        }

        // Continuar rotando hacia la rotación objetivo
        if (isFlipping)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Verificar si llegamos a la rotación objetivo
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isFlipping = false;
                Flip(); // Cambiar la visibilidad de la carta cuando termina el giro
                hasFlipped = true; // Marcar que ya se hizo un giro
            }
        }
    }

    public void StartFlip()
    {
        isFlipping = true;

        // Establecer la nueva rotación objetivo (180 grados en el eje Y)
        targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180f, transform.eulerAngles.z);
    }

    public void Flip()
    {
        // Alternar la visibilidad de la parte trasera de la carta
        if (cardBackIsActive)
        {
            cardBack.SetActive(false);
            cardBackIsActive = false;
        }
        else
        {
            cardBack.SetActive(true);
            cardBackIsActive = true;
        }
    }
}
