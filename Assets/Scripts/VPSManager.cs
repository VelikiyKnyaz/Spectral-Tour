using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;
using System;
using TMPro; // Asegúrate de importar TextMeshPro

public class VPSManager : MonoBehaviour
{
    [SerializeField] private AREarthManager earthManager;

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

    [SerializeField] private ARAnchorManager aRAnchorManager;
    [SerializeField] private List<GeospatialObject> geospatialObjects = new List<GeospatialObject>();

    // Referencias a los textos de UI para latitud, longitud y altitud
    //[Header("UI Text References")]
    //[SerializeField] private TextMeshProUGUI latitudText;
    //[SerializeField] private TextMeshProUGUI longitudText;
    //[SerializeField] private TextMeshProUGUI altitudText;

    // Start is called before the first frame update
    void Start()
    {
        VerifyGeospatialSupport();

        // Opcional: Validar que las referencias de texto no sean nulas
        /*if (latitudText == null || longitudText == null || altitudText == null)
        {
            Debug.LogError("Asegúrate de asignar los textos de UI en el inspector.");
        }*/
    }

    private void VerifyGeospatialSupport()
    {
        var result = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
        switch (result)
        {
            case FeatureSupported.Supported:
                Debug.Log("Ready to use VPS");
                PlaceObjects();
                break;
            case FeatureSupported.Unknown:
                Debug.Log("Unknown...");
                Invoke("VerifyGeospatialSupport", 5.0f);
                break;
            case FeatureSupported.Unsupported:
                Debug.Log("VPS Unsupported");
                break;
        }
    }

    private void PlaceObjects()
    {
        if (earthManager.EarthTrackingState == TrackingState.Tracking)
        {
            var geospatialPose = earthManager.CameraGeospatialPose;

            foreach (var obj in geospatialObjects)
            {
                var earthPosition = obj.EarthPosition;
                var objAnchor = ARAnchorManagerExtensions.AddAnchor(aRAnchorManager, earthPosition.Latitude, earthPosition.Longitude, earthPosition.Altitude, Quaternion.identity);
                Instantiate(obj.ObjectPrefab, objAnchor.transform);
            }
        }
        else if (earthManager.EarthTrackingState == TrackingState.None)
        {
            Invoke("PlaceObjects", 5.0f);
        }
    }

    // Método Update para actualizar los textos en tiempo real
    /*void Update()
    {
        if (earthManager.EarthTrackingState == TrackingState.Tracking)
        {
            var geospatialPose = earthManager.CameraGeospatialPose;
            latitudText.text = $"Latitud: {geospatialPose.Latitude:F6}";
            longitudText.text = $"Longitud: {geospatialPose.Longitude:F6}";
            altitudText.text = $"Altitud: {geospatialPose.Altitude:F2} m";
        }
        else
        {
            latitudText.text = "Latitud: N/A";
            longitudText.text = "Longitud: N/A";
            altitudText.text = "Altitud: N/A";
        }
    }*/
}
