using System.Collections; // Agrega esta línea
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using TMPro; // Asegúrate de incluir esto

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField]
    private AbstractMap _map;

    [SerializeField]
    private float _spawnScale = 100f;

    [SerializeField]
    private GameObject _markerPrefab;

    private GameObject _spawnedObject;

    // Referencias a TextMeshProUGUI para mostrar latitud y longitud
    [Header("UI Text References")]
    [SerializeField]
    private TextMeshProUGUI latitudeText;
    [SerializeField]
    private TextMeshProUGUI longitudeText;

    private bool isLocationServiceRunning = false;
    private double currentLatitude;
    private double currentLongitude;
    private double currentAltitude;

    void Start()
    {
        // Verificar referencias necesarias
        if (_map == null)
        {
            Debug.LogError("AbstractMap no asignado. Por favor, asígnalo en el inspector.");
        }

        if (_markerPrefab == null)
        {
            Debug.LogError("MarkerPrefab no asignado. Por favor, asígnalo en el inspector.");
        }

        if (latitudeText == null || longitudeText == null)
        {
            Debug.LogError("LatitudeText o LongitudeText no asignados. Por favor, asígnalos en el inspector.");
        }

        // Instanciar el marcador pero desactivarlo inicialmente
        _spawnedObject = Instantiate(_markerPrefab);
        _spawnedObject.SetActive(false);

        // Iniciar el servicio de ubicación
        StartCoroutine(StartLocationService());
    }

    IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("El usuario no ha habilitado el GPS");
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.Log("Tiempo de espera agotado");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("No se pudo determinar la ubicación del dispositivo");
            yield break;
        }
        else
        {
            // El servicio de ubicación está funcionando
            isLocationServiceRunning = true;

            currentLatitude = Input.location.lastData.latitude;
            currentLongitude = Input.location.lastData.longitude;
            currentAltitude = Input.location.lastData.altitude;

            // Activar el objeto instanciado
            _spawnedObject.SetActive(true);
        }
    }

    void Update()
    {
        if (isLocationServiceRunning)
        {
            // Obtener la latitud y longitud actuales
            double latitude = Input.location.lastData.latitude;
            double longitude = Input.location.lastData.longitude;

            // Actualizar los textos de UI con las coordenadas actuales
            latitudeText.text = $"Latitud: {latitude:F6}";
            longitudeText.text = $"Longitud: {longitude:F6}";

            // Crear un Vector2d a partir de la latitud y longitud
            Vector2d location = new Vector2d(latitude, longitude);

            // Convertir las coordenadas geográficas a posición en el mundo de Unity
            Vector3 mapPosition = _map.GeoToWorldPosition(location, true);

            // Actualizar la posición y escala del objeto instanciado
            _spawnedObject.transform.localPosition = mapPosition;
            _spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

            // Asegurarse de que el objeto esté activo
            if (!_spawnedObject.activeInHierarchy)
            {
                _spawnedObject.SetActive(true);
            }
        }
        else
        {
            // Desactivar el objeto si el servicio de ubicación no está funcionando
            if (_spawnedObject.activeInHierarchy)
            {
                _spawnedObject.SetActive(false);
            }

            // Actualizar la UI para mostrar que las coordenadas no están disponibles
            latitudeText.text = "Latitud: N/A";
            longitudeText.text = "Longitud: N/A";

            Debug.Log("El servicio de ubicación no está en funcionamiento.");
        }
    }
}
