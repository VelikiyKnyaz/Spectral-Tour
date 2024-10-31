using UnityEngine;

public class ARSceneGhostActivation : MonoBehaviour
{
    // Referencias a los GameObjects de los fantasmas
    [SerializeField]
    private GameObject fantasmaMariaC;
    [SerializeField]
    private GameObject fantasmaPedroNel;
    [SerializeField]
    private GameObject fantasmaCoriolano;

    void Start()
    {
        // Recuperar el resultado del test guardado
        string resultadoTest = PlayerPrefs.GetString("ResultadoTest", "");

        // Desactivar todos los GameObjects al inicio
        fantasmaMariaC.SetActive(false);
        fantasmaPedroNel.SetActive(false);
        fantasmaCoriolano.SetActive(false);

        // Habilitar el GameObject correspondiente según el resultado del test
        switch (resultadoTest)
        {
            case "Politico":
                fantasmaMariaC.SetActive(true);
                break;
            case "Artista":
                fantasmaPedroNel.SetActive(true);
                break;
            case "Empresario":
                fantasmaCoriolano.SetActive(true);
                break;
            default:
                Debug.LogWarning("Resultado del test no reconocido");
                break;
        }
    }
}
