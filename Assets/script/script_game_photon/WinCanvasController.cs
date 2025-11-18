using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCanvasController : MonoBehaviour
{
    [Header("Nombre de la escena de menú")]
    public string menuSceneName = "principal"; // 👈 cámbialo por el nombre real

    private NetworkRunner runner;

    private void Awake()
    {
        runner = FindObjectOfType<NetworkRunner>();
    }

    // Botón: Reiniciar SOLO la escena de juego
    public void OnRestartGameButton()
    {
        if (runner != null && runner.IsServer)
        {
            // El host recarga la escena actual para todos
            string currentScene = SceneManager.GetActiveScene().name;
            runner.LoadScene(currentScene);
        }
        else
        {
            Debug.Log("[WinCanvasController] Solo el HOST puede reiniciar la partida.");
        }
    }

    // Botón: Volver al menú principal
    public void OnBackToMenuButton()
    {
        if (runner != null && runner.IsServer)
        {
            runner.LoadScene(menuSceneName);
        }
        else
        {
            // Por si pruebas sin red / en editor solo
            SceneManager.LoadScene(menuSceneName);
        }
    }
}
