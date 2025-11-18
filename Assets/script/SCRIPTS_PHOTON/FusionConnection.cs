using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FusionConnection : MonoBehaviour
{
    [Header("Runner (opcional)")]
    [SerializeField] private NetworkRunner runner;

    [Header("Nombre de la sala")]
    public string sessionName = "SalaPong";

    // --- BOTONES ---

    // Llama esto desde BtnHost (OnClick)
    public void StartHost()
    {
        StartGame(GameMode.Host);
    }

    // Llama esto desde BtnClient (OnClick)
    public void StartClient()
    {
        StartGame(GameMode.Client);
    }

    // --- LÓGICA INTERNA ---

    private async void StartGame(GameMode mode)
    {
        // Si no tenemos runner asignado, lo buscamos en el mismo objeto
        if (runner == null)
            runner = GetComponent<NetworkRunner>();

        // Si todavía no hay runner, lo creamos
        if (runner == null)
            runner = gameObject.AddComponent<NetworkRunner>();

        runner.ProvideInput = true;

        // Escena actual (el menú) como escena de inicio de Fusion
        var sceneRef = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

        // Info de escena para Fusion 2
        var sceneInfo = new NetworkSceneInfo();
        if (sceneRef.IsValid)
        {
            // Single = solo esta escena, las demás se descargan
            sceneInfo.AddSceneRef(sceneRef, LoadSceneMode.Single);
        }

        Debug.Log($"[FusionConnection] Iniciando runner como {mode}...");

        await runner.StartGame(new StartGameArgs
        {
            GameMode    = mode,
            SessionName = sessionName,
            Scene       = sceneInfo,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        Debug.Log($"[FusionConnection] Runner iniciado como {mode}");
    }
}
