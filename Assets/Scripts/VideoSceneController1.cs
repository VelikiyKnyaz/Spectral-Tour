using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSceneController1 : MonoBehaviour
{
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
}
