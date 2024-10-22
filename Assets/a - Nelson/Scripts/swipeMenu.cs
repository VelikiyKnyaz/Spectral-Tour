using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject scrollbar;
    float scroll_pos = 0;
    float[] pos;
    public float waveFrequency = 2f; // Frecuencia de la ola
    public float waveAmplitude = 50f; // Amplitud de la ola
    public float waveSpeed = 2f; // Velocidad de la ola
    bool isScrolling = false; // Bandera para detectar el desplazamiento manual
    Vector2[] originalPositions; // Array para guardar las posiciones originales

    // Start is called before the first frame update
    void Start()
    {
        // Guardar las posiciones originales de las cartas
        originalPositions = new Vector2[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            originalPositions[i] = transform.GetChild(i).localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0)) // Detectar input del mouse
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            isScrolling = true; // El usuario está desplazando manualmente
        }
        else
        {
            isScrolling = false; // El usuario ha dejado de desplazar manualmente
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        // Detectar y escalar las cartas según la posición
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                // Carta seleccionada: Escalar a 1 y habilitar interacción
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                EnableInteraction(transform.GetChild(i), true);

                // Movimiento en ola para carta seleccionada
                float wave = Mathf.Sin(Time.time * waveSpeed + i * waveFrequency) * waveAmplitude;
                Vector2 newPosition = originalPositions[i] + new Vector2(0, wave);
                transform.GetChild(i).localPosition = Vector2.Lerp(transform.GetChild(i).localPosition, newPosition, 0.1f);
            }
            else
            {
                // Deshabilitar las otras cartas
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                EnableInteraction(transform.GetChild(i), false);

                // Regresar suavemente las cartas no seleccionadas a su posición original
                transform.GetChild(i).localPosition = Vector2.Lerp(transform.GetChild(i).localPosition, originalPositions[i], 0.1f);
            }
        }
    }

    // Método para habilitar o deshabilitar la interacción de una carta
    void EnableInteraction(Transform card, bool isEnabled)
    {
        // Si la carta tiene un componente interactivo (ej. Button), habilitarlo o deshabilitarlo
        Button button = card.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = isEnabled;
        }
    }

    // Método para manejar clic en cartas inactivas
    public void OnCardClick(Transform card)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (transform.GetChild(i) == card)
            {
                scrollbar.GetComponent<Scrollbar>().value = pos[i];
                break;
            }
        }
    }
}
