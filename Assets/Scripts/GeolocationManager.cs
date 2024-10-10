using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GeolocationManager : MonoBehaviour
{
    [Serializable]
    public struct GeospatialObject
    {
        public GameObject ObjectPrefab;
        public EarthPosition EarthPosition;
    }

    [Serializable]
    public struct EarthPosition
    {
        public double Latitude;
        public double Longitude;
        public double Altitude;
    }

    [SerializeField] private List<GeospatialObject> geospatialObjects = new List<GeospatialObject>();

    private bool isLocationServiceRunning = false;
    private double currentLatitude;
    private double currentLongitude;
    private double currentAltitude;

    void Start()
    {
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

            PlaceObjects();
        }
    }

    void PlaceObjects()
    {
        foreach (var obj in geospatialObjects)
        {
            var earthPosition = obj.EarthPosition;

            // Calcular la posición relativa a la ubicación actual
            Vector3 position = GPSToUnityWorldPosition(currentLatitude, currentLongitude, earthPosition.Latitude, earthPosition.Longitude);

            Instantiate(obj.ObjectPrefab, position, Quaternion.identity);
        }
    }

    Vector3 GPSToUnityWorldPosition(double currentLat, double currentLon, double targetLat, double targetLon)
    {
        // Aproximación de metros por grado
        float meterPerLat = 111132.954f; // metros por grado de latitud
        float meterPerLon = 111132.954f * Mathf.Cos((float)(currentLat * Mathf.Deg2Rad)); // metros por grado de longitud en la latitud actual

        float deltaLat = (float)(targetLat - currentLat);
        float deltaLon = (float)(targetLon - currentLon);

        float deltaNorthing = deltaLat * meterPerLat;
        float deltaEasting = deltaLon * meterPerLon;

        // En Unity, el eje x es horizontal (este-oeste), el eje z es profundidad (norte-sur)
        Vector3 position = new Vector3(deltaEasting, 0, deltaNorthing);

        return position;
    }

    void Update()
    {
        if (isLocationServiceRunning)
        {
            // Opcionalmente, actualiza la ubicación actual si es necesario
            currentLatitude = Input.location.lastData.latitude;
            currentLongitude = Input.location.lastData.longitude;
            currentAltitude = Input.location.lastData.altitude;
        }
    }
}
