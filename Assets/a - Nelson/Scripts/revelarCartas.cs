using UnityEngine;
using System.Collections;

public class CartaRevelador : MonoBehaviour
{
    // Referencia a las cartas en el GridLayout
    public GameObject[] cartas; // Asigna tus cartas en el inspector
    public float tiempoEntreRevelaciones = 1.0f; // Tiempo en segundos entre revelaciones
    public float duracionFadeIn = 0.5f; // Duración del efecto fade-in

    private void Start()
    {
        // Comienza el proceso de revelación
        StartCoroutine(RevelarCartas());
    }

    private IEnumerator RevelarCartas()
    {
        // Recorre todas las cartas
        foreach (GameObject carta in cartas)
        {
            // Asegúrate de que la carta tenga un CanvasGroup
            CanvasGroup canvasGroup = carta.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = carta.AddComponent<CanvasGroup>(); // Agrega un CanvasGroup si no existe
            }

            canvasGroup.alpha = 0; // Asegúrate de que esté invisible al principio
            carta.SetActive(true); // Activa la carta

            // Realiza el fade-in
            for (float t = 0; t <= 1; t += Time.deltaTime / duracionFadeIn)
            {
                canvasGroup.alpha = t; // Cambia el alpha de 0 a 1
                yield return null; // Espera hasta el siguiente frame
            }

            canvasGroup.alpha = 1; // Asegúrate de que esté completamente visible

            // Espera el tiempo especificado antes de revelar la siguiente carta
            yield return new WaitForSeconds(tiempoEntreRevelaciones);
        }
    }
}
