using System.Threading.Tasks;
using Fusion;
using UnityEngine;

public class GameFusionSpawner : MonoBehaviour
{
    public static GameFusionSpawner Instance;

    [Header("Runner (se puede dejar vacío)")]
    public NetworkRunner runner;

    [Header("Prefabs de red")]
    public NetworkObject hostPaddlePrefab;
    public NetworkObject clientPaddlePrefab;
    public NetworkObject ballPrefab;

    [Header("Puntos de aparición")]
    public Transform hostSpawn;
    public Transform clientSpawn;
    public Transform ballSpawn;

    // Pelota actual en juego (solo la conoce el server)
    private NetworkObject currentBall;

    private bool spawned = false;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        if (runner == null)
            runner = FindObjectOfType<NetworkRunner>();

        if (runner == null)
        {
            Debug.LogError("[GameFusionSpawner] No se encontró NetworkRunner en la escena.");
            return;
        }

        // Solo el HOST/Server spawnea las cosas
        if (!runner.IsServer)
            return;

        await Task.Yield();
        await SpawnAllAsync();
    }

    private async Task SpawnAllAsync()
    {
        if (spawned) return;
        spawned = true;

        bool firstPlayer = true;

        // Creamos las 2 paletas, una por jugador
        foreach (var player in runner.ActivePlayers)
        {
            bool isHostPaddle = firstPlayer;
            firstPlayer = false;

            NetworkObject prefab = isHostPaddle ? hostPaddlePrefab : clientPaddlePrefab;
            Transform spawn = isHostPaddle ? hostSpawn : clientSpawn;

            NetworkObject obj = await runner.SpawnAsync(prefab, spawn.position, spawn.rotation, player);
            Debug.Log($"[GameFusionSpawner] Spawn paddle {(isHostPaddle ? "HOST" : "CLIENT")} para {player} ({obj.name})");
        }

        // Pelota inicial
        await SpawnBallAsync(true); // por ejemplo, primero va hacia el client
    }

    /// <summary>
    /// Spawnea una nueva pelota en el punto ballSpawn y la lanza.
    /// Solo la llama el server.
    /// </summary>
    private async Task SpawnBallAsync(bool haciaHost)
    {
        if (ballPrefab == null || ballSpawn == null)
        {
            Debug.LogWarning("[GameFusionSpawner] Falta asignar ballPrefab o ballSpawn.");
            return;
        }

        NetworkObject ballObj = await runner.SpawnAsync(
            ballPrefab,
            ballSpawn.position,
            ballSpawn.rotation,
            null);

        currentBall = ballObj;

        var ballCtrl = ballObj.GetComponent<FusionBallController>();
        if (ballCtrl != null && ballObj.HasStateAuthority)
        {
            ballCtrl.Launch(haciaHost);
        }

        Debug.Log("[GameFusionSpawner] Spawn pelota " + ballObj.name);
    }

    /// <summary>
    /// Destruye la pelota actual y crea una nueva en el spawn.
    /// </summary>
    public async void RespawnBall(bool haciaHost)
    {
        if (!runner || !runner.IsServer)
            return;

        if (currentBall != null)
        {
            runner.Despawn(currentBall);
            currentBall = null;
        }

        await SpawnBallAsync(haciaHost);
    }
}
