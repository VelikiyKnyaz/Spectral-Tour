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

    // Variable p�blica que almacenar� el resultado del test
    public string resultadoTest = null;  // Ser� nulo inicialmente

    // Puntajes para los diferentes roles
    private int puntajeEmpresario = 0;
    private int puntajeFilantropo = 0;
    private int puntajeArtista = 0;
    private int puntajePolitico = 0;

    private int preguntaActual = 0;

    // M�todo para seleccionar una respuesta
    public void SeleccionarRespuesta(int tipoRol)
    {
        // Sumar puntaje de acuerdo al rol asociado a la respuesta seleccionada
        switch (tipoRol)
        {
            case 0: // Empresario
                puntajeEmpresario++;
                break;
            case 1: // Fil�ntropo
                puntajeFilantropo++;
                break;
            case 2: // Artista
                puntajeArtista++;
                break;
            case 3: // Pol�tico
                puntajePolitico++;
                break;
        }

        SiguientePregunta();
    }

    // M�todo para cambiar al siguiente panel de pregunta con efecto de fade
    private void SiguientePregunta()
    {
        StartCoroutine(FadeOutAndNext());
    }

    private System.Collections.IEnumerator FadeOutAndNext()
    {
        // Obt�n el CanvasGroup del panel actual
        CanvasGroup currentPanelGroup = panelesDePreguntas[preguntaActual].GetComponent<CanvasGroup>();

        // Verificar si el CanvasGroup existe
        if (currentPanelGroup == null)
        {
            Debug.LogError("El panel actual no tiene un CanvasGroup adjunto.");
            yield break; // Salir si no hay CanvasGroup
        }

        // Realiza el fade out
        for (float t = 1; t > 0; t -= Time.deltaTime / 1.0f) // Cambiado a 1.0f para un fade out m�s largo
        {
            currentPanelGroup.alpha = t;
            yield return null;
        }

        currentPanelGroup.alpha = 0; // Aseg�rate de que est� completamente desvanecido

        // Desactivar el panel actual
        panelesDePreguntas[preguntaActual].SetActive(false);

        // Aumentar la pregunta actual
        preguntaActual++;

        // Aseg�rate de que no sobrepases el n�mero de preguntas
        if (preguntaActual < panelesDePreguntas.Length)
        {
            // Activar el siguiente panel y realizar fade in
            CanvasGroup nextPanelGroup = panelesDePreguntas[preguntaActual].GetComponent<CanvasGroup>();

            if (nextPanelGroup == null)
            {
                Debug.LogError("El siguiente panel no tiene un CanvasGroup adjunto.");
                yield break; // Salir si no hay CanvasGroup
            }

            nextPanelGroup.alpha = 0; // Aseg�rate de que est� invisible al principio
            nextPanelGroup.gameObject.SetActive(true); // Activar el siguiente panel

            // Realiza el fade in
            for (float t = 0; t <= 1; t += Time.deltaTime / 1.0f) // Cambiado a 1.0f para un fade in m�s largo
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
        // Determinar el puntaje m�ximo entre los roles
        int mayorPuntaje = Mathf.Max(puntajeEmpresario, puntajeFilantropo, puntajeArtista, puntajePolitico);

        // Crear una lista para almacenar los roles con el puntaje m�ximo
        List<GameObject> panelesConMayorPuntaje = new List<GameObject>();

        // Agregar los paneles que tienen el puntaje m�ximo
        if (puntajeEmpresario == mayorPuntaje)
            panelesConMayorPuntaje.Add(panelResultadoEmpresario);
        if (puntajeFilantropo == mayorPuntaje)
            panelesConMayorPuntaje.Add(panelResultadoFilantropo);
        if (puntajeArtista == mayorPuntaje)
            panelesConMayorPuntaje.Add(panelResultadoArtista);
        if (puntajePolitico == mayorPuntaje)
            panelesConMayorPuntaje.Add(panelResultadoPolitico);

        // Elegir un panel aleatorio entre los que tienen el puntaje m�ximo
        int indiceAleatorio = Random.Range(0, panelesConMayorPuntaje.Count);
        GameObject panelSeleccionado = panelesConMayorPuntaje[indiceAleatorio];

        // Desactivar todos los paneles de preguntas
        foreach (GameObject panel in panelesDePreguntas)
        {
            panel.SetActive(false);
        }

        // Aseg�rate de que el panel seleccionado tenga un CanvasGroup
        CanvasGroup selectedPanelGroup = panelSeleccionado.GetComponent<CanvasGroup>();
        if (selectedPanelGroup == null)
        {
            selectedPanelGroup = panelSeleccionado.AddComponent<CanvasGroup>(); // Agrega un CanvasGroup si no existe
        }

        selectedPanelGroup.alpha = 0; // Aseg�rate de que est� invisible al principio
        panelSeleccionado.SetActive(true); // Activar el panel seleccionado

        // Realiza el fade in para el resultado
        for (float t = 0; t <= 1; t += Time.deltaTime / 1.0f) // Cambiado a 1.0f para un fade in m�s largo
        {
            selectedPanelGroup.alpha = t;
            yield return null;
        }

        selectedPanelGroup.alpha = 1; // Aseg�rate de que est� completamente visible

        // Actualizar la variable pública con el resultado del test
        if (panelSeleccionado == panelResultadoEmpresario)
        {
            PlayerPrefs.SetString("ResultadoTest", "Empresario");
            PlayerPrefs.Save();
        }
        else if (panelSeleccionado == panelResultadoFilantropo)
        {
            PlayerPrefs.SetString("ResultadoTest", "Filantropo");
            PlayerPrefs.Save();
        }
        else if (panelSeleccionado == panelResultadoArtista)
        {
            PlayerPrefs.SetString("ResultadoTest", "Artista");
            PlayerPrefs.Save();
        }
        else if (panelSeleccionado == panelResultadoPolitico)
        {
            PlayerPrefs.SetString("ResultadoTest", "Politico");
            PlayerPrefs.Save();
        }
        Debug.Log(PlayerPrefs.GetString("ResultadoTest", ""));
    }
}
