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

    // M�todo para cambiar al siguiente panel de pregunta
    private void SiguientePregunta()
    {
        if (preguntaActual < panelesDePreguntas.Length - 1)
        {
            panelesDePreguntas[preguntaActual].SetActive(false); // Desactivar el panel actual
            preguntaActual++;
            panelesDePreguntas[preguntaActual].SetActive(true); // Activar el siguiente panel
        }
        else
        {
            // Encuesta terminada, mostrar resultados o lo que desees hacer al final
            MostrarResultados();
        }
    }

    private void MostrarResultados()
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

        // Activar el panel seleccionado
        panelSeleccionado.SetActive(true);

        // Actualizar la variable p�blica con el resultado del test
        if (panelSeleccionado == panelResultadoEmpresario)
            resultadoTest = "Empresario";
        else if (panelSeleccionado == panelResultadoFilantropo)
            resultadoTest = "Filantropo";
        else if (panelSeleccionado == panelResultadoArtista)
            resultadoTest = "Artista";
        else if (panelSeleccionado == panelResultadoPolitico)
            resultadoTest = "Politico";
    }
}
