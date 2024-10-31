using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpawnOnMapFantasmas : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    [Header("Ubicaciones por categoría")]
    [SerializeField]
    [Geocode]
    string[] _locationStringsPolitico;

    [SerializeField]
    [Geocode]
    string[] _locationStringsEmpresario;

    [SerializeField]
    [Geocode]
    string[] _locationStringsFilantropo;

    [SerializeField]
    [Geocode]
    string[] _locationStringsArtista;

    Vector2d[] _locations;

    [SerializeField]
    float _spawnScale = 100f;

    [SerializeField]
    GameObject _markerPrefab;

    List<GameObject> _spawnedObjects;

    // Variable pública booleana para verificar si el jugador está cerca de algún punto
    public bool jugadorCerca;

    // Referencia al panel de UI
    public GameObject PanelAR;

    void Start()
    {
        // Recuperar el resultado del test guardado
        string resultadoTest = PlayerPrefs.GetString("ResultadoTest", "");

        if (string.IsNullOrEmpty(resultadoTest))
        {
            Debug.LogWarning("NO SE HA REALIZADO EL TEST");
            return;
        }

        // Seleccionamos las ubicaciones según el resultado del test
        string[] _selectedLocationStrings;

        switch (resultadoTest)
        {
            case "Politico":
                _selectedLocationStrings = _locationStringsPolitico;
                break;
            case "Empresario":
                _selectedLocationStrings = _locationStringsEmpresario;
                break;
            case "Filantropo":
                _selectedLocationStrings = _locationStringsFilantropo;
                break;
            case "Artista":
                _selectedLocationStrings = _locationStringsArtista;
                break;
            default:
                _selectedLocationStrings = new string[0];
                Debug.LogWarning("Resultado del test no reconocido");
                break;
        }

        _locations = new Vector2d[_selectedLocationStrings.Length];
        _spawnedObjects = new List<GameObject>();

        for (int i = 0; i < _selectedLocationStrings.Length; i++)
        {
            var locationString = _selectedLocationStrings[i];
            _locations[i] = Conversions.StringToLatLon(locationString);
            var instance = Instantiate(_markerPrefab);
            instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedObjects.Add(instance);
        }

        // Iniciar la comprobación periódica
        StartCoroutine(VerificarJugadorCerca());
    }

    void Update()
    {
        int count = _spawnedObjects.Count;
        jugadorCerca = false;

        // Obtener la latitud y longitud actuales del jugador
        double jugadorLatitud = Input.location.lastData.latitude;
        double jugadorLongitud = Input.location.lastData.longitude;
        Vector2d jugadorPosicion = new Vector2d(jugadorLatitud, jugadorLongitud);

        for (int i = 0; i < count; i++)
        {
            var spawnedObject = _spawnedObjects[i];
            var location = _locations[i];
            spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
            spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

            // Verificar si el jugador está cerca de algún punto spawneado (dentro de un radio de 250 m)
            double distancia = Vector2d.Distance(jugadorPosicion, location);
            if (distancia <= 0.25) // 250 metros en kilómetros
            {
                jugadorCerca = true;
                break;
            }
        }
    }

    IEnumerator VerificarJugadorCerca()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            if (jugadorCerca && PanelAR != null && !PanelAR.activeInHierarchy)
            {
                PanelAR.SetActive(true);
            }
        }
    }
}

