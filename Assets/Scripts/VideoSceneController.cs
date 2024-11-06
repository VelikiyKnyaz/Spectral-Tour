using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSceneController : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        // Obtiene el componente VideoPlayer y reproduce el video al iniciar la escena
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoEnd; // Añade el evento para detectar el final del video
        }
        else
        {
            Debug.LogWarning("VideoPlayer no encontrado en el GameObject.");
        }
    }

    void Update()
    {
        // Verifica si el usuario toca la pantalla
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Si el toque es una fase de inicio
            if (touch.phase == TouchPhase.Began)
            {
                // Detecta si el toque fue en el GameObject
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                {
                    // Cambia de escena
                    SceneManager.LoadScene("Home");
                }
            }
        }
    }

    // Método que se llama al finalizar la reproducción del video
    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("Home");
    }
}
