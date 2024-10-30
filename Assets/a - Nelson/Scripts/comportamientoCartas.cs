using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EncuestaManager : MonoBehaviour
{
    // Referencia a los paneles de preguntas
    public GameObject[] panelesDePreguntas;

    // Referencia a los paneles de resultados
    public GameObject panelResultadoEmpresario;
    public GameObject panelResultadoFilantropo;
    public GameObject panelResultadoArtista;
    public GameObject panelResultadoPolitico;

    // Puntajes para los diferentes roles
    private int puntajeEmpresario = 0;
    private int puntajeFilantropo = 0;
    private int puntajeArtista = 0;
    private int puntajePolitico = 0;

    private int preguntaActual = 0;

    // Método para seleccionar una respuesta
    public void SeleccionarRespuesta(int tipoRol)
    {
        // Sumar puntaje de acuerdo al rol asociado a la respuesta seleccionada
        switch (tipoRol)
        {
            case 0: // Empresario
                puntajeEmpresario++;
                break;
            case 1: // Filántropo
                puntajeFilantropo++;
                break;
            case 2: // Artista
                puntajeArtista++;
                break;
            case 3: // Político
                puntajePolitico++;
                break;
        }

        SiguientePregunta();
    }

    // Método para cambiar al siguiente panel de pregunta con efecto de fade
    private void SiguientePregunta()
    {
        StartCoroutine(FadeOutAndNext());
    }

    private System.Collections.IEnumerator FadeOutAndNext()
    {
        // Obtén el CanvasGroup del panel actual
        CanvasGroup currentPanelGroup = panelesDePreguntas[preguntaActual].GetComponent<CanvasGroup>();

        // Verificar si el CanvasGroup existe
        if (currentPanelGroup == null)
        {
            Debug.LogError("El panel actual no tiene un CanvasGroup adjunto.");
            yield break; // Salir si no hay CanvasGroup
        }

        // Realiza el fade out
        for (float t = 1; t > 0; t -= Time.deltaTime / 1.0f) // Cambiado a 1.0f para un fade out más largo
        {
            currentPanelGroup.alpha = t;
            yield return null;
        }

        currentPanelGroup.alpha = 0; // Asegúrate de que esté completamente desvanecido

        // Desactivar el panel actual
        panelesDePreguntas[preguntaActual].SetActive(false);

        // Aumentar la pregunta actual
        preguntaActual++;

        // Asegúrate de que no sobrepases el número de preguntas
        if (preguntaActual < panelesDePreguntas.Length)
        {
            // Activar el siguiente panel y realizar fade in
            CanvasGroup nextPanelGroup = panelesDePreguntas[preguntaActual].GetComponent<CanvasGroup>();

            if (nextPanelGroup == null)
            {
                Debug.LogError("El siguiente panel no tiene un CanvasGroup adjunto.");
                yield break; // Salir si no hay CanvasGroup
            }

            nextPanelGroup.alpha = 0; // Asegúrate de que esté invisible al principio
            nextPanelGroup.gameObject.SetActive(true); // Activar el siguiente panel

            // Realiza el fade in
            for (float t = 0; t <= 1; t += Time.deltaTime / 1.0f) // Cambiado a 1.0f para un fade in más largo
            {
                nextPanelGroup.alpha = t;
                yield return null;
            }
        }
        else
        {
            // Encuesta terminada, mostrar resultados o lo que desees hacer al final
            yield return MostrarResultadosConFade();
        }
    }

    private System.Collections.IEnumerator MostrarResultadosConFade()
    {
        // Determinar el puntaje máximo entre los roles
        int mayorPuntaje = Mathf.Max(puntajeEmpresario, puntajeFilantropo, puntajeArtista, puntajePolitico);

        // Crear una lista para almacenar los roles con el puntaje máximo
        List<GameObject> panelesConMayorPuntaje = new List<GameObject>();

        // Agregar los paneles que tienen el puntaje máximo
        if (puntajeEmpresario == mayorPuntaje)
            panelesConMayorPuntaje.Add(panelResultadoEmpresario);
        if (puntajeFilantropo == mayorPuntaje)
            panelesConMayorPuntaje.Add(panelResultadoFilantropo);
        if (puntajeArtista == mayorPuntaje)
            panelesConMayorPuntaje.Add(panelResultadoArtista);
        if (puntajePolitico == mayorPuntaje)
            panelesConMayorPuntaje.Add(panelResultadoPolitico);

        // Elegir un panel aleatorio entre los que tienen el puntaje máximo
        int indiceAleatorio = Random.Range(0, panelesConMayorPuntaje.Count);
        GameObject panelSeleccionado = panelesConMayorPuntaje[indiceAleatorio];

        // Desactivar todos los paneles de preguntas
        foreach (GameObject panel in panelesDePreguntas)
        {
            panel.SetActive(false);
        }

        // Asegúrate de que el panel seleccionado tenga un CanvasGroup
        CanvasGroup selectedPanelGroup = panelSeleccionado.GetComponent<CanvasGroup>();
        if (selectedPanelGroup == null)
        {
            selectedPanelGroup = panelSeleccionado.AddComponent<CanvasGroup>(); // Agrega un CanvasGroup si no existe
        }

        selectedPanelGroup.alpha = 0; // Asegúrate de que esté invisible al principio
        panelSeleccionado.SetActive(true); // Activar el panel seleccionado

        // Realiza el fade in para el resultado
        for (float t = 0; t <= 1; t += Time.deltaTime / 1.0f) // Cambiado a 1.0f para un fade in más largo
        {
            selectedPanelGroup.alpha = t;
            yield return null;
        }

        selectedPanelGroup.alpha = 1; // Asegúrate de que esté completamente visible
    }
}
