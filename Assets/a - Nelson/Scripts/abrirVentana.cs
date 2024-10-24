using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel; // El panel de la ventana emergente
    public Button showButton; // El botón para mostrar la ventana emergente
    public Button closeButton; // El botón para cerrar la ventana emergente

    public void Start()
    {
        // Asegúrate de que la ventana esté oculta al inicio
        popupPanel.SetActive(false);

        // Asigna las funciones a los botones
        showButton.onClick.AddListener(ShowPopup);
        closeButton.onClick.AddListener(HidePopup);
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true); // Mostrar la ventana emergente
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false); // Ocultar la ventana emergente
    }
}