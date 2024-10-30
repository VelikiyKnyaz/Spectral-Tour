using UnityEngine;
using UnityEngine.Video;

public class VideoVolumeController : MonoBehaviour
{
    public VideoPlayer[] videoPlayers;  // Un array con los 3 VideoPlayers
    public Transform userTransform;     // El Transform del usuario (o la cámara AR)
    public float maxDistance = 10f;     // Distancia máxima donde el volumen será 0
    public float minDistance = 2f;      // Distancia mínima donde el volumen será 1

    void Update()
    {
        // Iteramos sobre cada VideoPlayer en el array
        foreach (VideoPlayer videoPlayer in videoPlayers)
        {
            // Calculamos la distancia entre el GameObject del VideoPlayer y el usuario
            float distance = Vector3.Distance(videoPlayer.transform.position, userTransform.position);

            // Ajustamos el volumen en función de la distancia
            float volume = Mathf.Clamp01(1 - (distance - minDistance) / (maxDistance - minDistance));

            // Asignamos el volumen al VideoPlayer
            videoPlayer.SetDirectAudioVolume(0, volume);
        }
    }
}
