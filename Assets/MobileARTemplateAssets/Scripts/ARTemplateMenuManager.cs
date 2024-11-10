using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;

/// <summary>
/// Gestiona la visualización del debug del suelo, asegurando que esté desactivado por defecto y permitiendo
/// activarlo o desactivarlo desde el editor mediante una casilla de verificación.
/// </summary>
public class ARTemplateMenuManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Activar o desactivar el debug del suelo.")]
    private bool m_DebugGroundEnabled = false;

    /// <summary>
    /// Activar o desactivar el debug del suelo.
    /// </summary>
    public bool debugGroundEnabled
    {
        get => m_DebugGroundEnabled;
        set
        {
            m_DebugGroundEnabled = value;
            ChangePlaneVisibility(m_DebugGroundEnabled);
        }
    }

    [SerializeField]
    [Tooltip("El prefab del plano con sombras y visuales de debug.")]
    GameObject m_DebugPlane;

    /// <summary>
    /// El prefab del plano con sombras y visuales de debug.
    /// </summary>
    public GameObject debugPlane
    {
        get => m_DebugPlane;
        set => m_DebugPlane = value;
    }

    [SerializeField]
    [Tooltip("El gestor de planos en la escena AR.")]
    ARPlaneManager m_PlaneManager;

    /// <summary>
    /// El gestor de planos en la escena AR.
    /// </summary>
    public ARPlaneManager planeManager
    {
        get => m_PlaneManager;
        set => m_PlaneManager = value;
    }

    readonly List<ARFeatheredPlaneMeshVisualizerCompanion> featheredPlaneMeshVisualizerCompanions = new List<ARFeatheredPlaneMeshVisualizerCompanion>();

    void OnEnable()
    {
        m_PlaneManager.planesChanged += OnPlaneChanged;
    }

    void OnDisable()
    {
        m_PlaneManager.planesChanged -= OnPlaneChanged;
    }

    void Start()
    {
        // Asegurar que el debug del suelo esté desactivado por defecto
        ChangePlaneVisibility(m_DebugGroundEnabled);
        m_PlaneManager.planePrefab = m_DebugPlane;
    }

    void ChangePlaneVisibility(bool setVisible)
    {
        foreach (var visualizer in featheredPlaneMeshVisualizerCompanions)
        {
            visualizer.visualizeSurfaces = setVisible;
        }
    }

    void OnPlaneChanged(ARPlanesChangedEventArgs eventArgs)
    {
        if (eventArgs.added.Count > 0)
        {
            foreach (var plane in eventArgs.added)
            {
                if (plane.TryGetComponent<ARFeatheredPlaneMeshVisualizerCompanion>(out var visualizer))
                {
                    featheredPlaneMeshVisualizerCompanions.Add(visualizer);
                    visualizer.visualizeSurfaces = m_DebugGroundEnabled;
                }
            }
        }

        if (eventArgs.removed.Count > 0)
        {
            foreach (var plane in eventArgs.removed)
            {
                if (plane.TryGetComponent<ARFeatheredPlaneMeshVisualizerCompanion>(out var visualizer))
                    featheredPlaneMeshVisualizerCompanions.Remove(visualizer);
            }
        }

        // Fallback si las cuentas no coinciden después de una actualización
        if (m_PlaneManager.trackables.count != featheredPlaneMeshVisualizerCompanions.Count)
        {
            featheredPlaneMeshVisualizerCompanions.Clear();
            foreach (var trackable in m_PlaneManager.trackables)
            {
                if (trackable.TryGetComponent<ARFeatheredPlaneMeshVisualizerCompanion>(out var visualizer))
                {
                    featheredPlaneMeshVisualizerCompanions.Add(visualizer);
                    visualizer.visualizeSurfaces = m_DebugGroundEnabled;
                }
            }
        }
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            ChangePlaneVisibility(m_DebugGroundEnabled);
        }
    }
}
