using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public Button muteButton;       // El botón de mute
    public Text buttonText;         // Texto del botón (si tienes uno)
    private bool isMuted = false;   // Estado de mute

    private void Start()
    {
        // Asegúrate de que el botón llame al método ToggleMute al hacer clic
        muteButton.onClick.AddListener(ToggleMute);
        UpdateButtonText(); // Establece el texto inicial del botón
    }

    // Método para alternar el mute
    public void ToggleMute()
    {
        isMuted = !isMuted; // Cambia el estado de mute
        AudioListener.volume = isMuted ? 0 : 1; // Silencia o activa el volumen general
        UpdateButtonText(); // Actualiza el texto del botón según el estado de mute
    }

    // Actualiza el texto del botón dependiendo del estado de mute
    private void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = isMuted ? "Unmute" : "Mute";
        }
    }
}