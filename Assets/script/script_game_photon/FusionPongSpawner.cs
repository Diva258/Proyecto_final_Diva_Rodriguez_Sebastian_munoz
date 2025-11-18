using Fusion;
using UnityEngine;

public class FusionPongSpawner : MonoBehaviour
{
    [Header("Prefabs de red")]
    public NetworkObject hostPaddlePrefab;
    public NetworkObject clientPaddlePrefab;
    public NetworkObject ballPrefab;

    [Header("Puntos de spawn")]
    public Transform hostSpawn;
    public Transform clientSpawn;
    public Transform ballSpawn;

    /// <summary>
    /// Llamar esto desde FusionConnection.OnSceneLoadDone
    /// cuando se cargue la escena del juego.
    /// </summary>
    public void SpawnPongObjects(NetworkRunner runner)
    {
        if (!runner.IsServer)
            return; // solo el server spawnea

        // 🟡 Spawn de paletas para cada jugador conectado
        foreach (var player in runner.ActivePlayers)
        {
            bool isHost = player == runner.LocalPlayer;

            NetworkObject prefab = isHost ? hostPaddlePrefab : clientPaddlePrefab;
            Transform spawn = isHost ? hostSpawn : clientSpawn;

            if (prefab == null || spawn == null)
            {
                Debug.LogError("[FusionPongSpawner] Falta asignar prefab o spawn en el inspector.");
                continue;
            }

            runner.Spawn(prefab, spawn.position, spawn.rotation, player);
        }

        // ⚪ Spawn de la pelota (sin dueño específico)
        if (ballPrefab != null && ballSpawn != null)
        {
            runner.Spawn(ballPrefab, ballSpawn.position, ballSpawn.rotation);
        }
        else
        {
            Debug.LogError("[FusionPongSpawner] Falta asignar ballPrefab o ballSpawn.");
        }
    }
}
