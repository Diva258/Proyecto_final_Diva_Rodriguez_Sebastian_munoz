using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneChanger : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Escena de juego (Pong)")]
    [Tooltip("Índice de la escena del juego en Build Settings")]
    public int gameSceneBuildIndex = 1;   // Cambia este número según tu Build Settings

    private int connectedPlayers = 0;

    // --- INetworkRunnerCallbacks ---

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        connectedPlayers++;
        Debug.Log($"[Fusion] PlayerJoined {player}. Conectados: {connectedPlayers}");

        // Solo el host manda el cambio de escena
        if (runner.IsServer && connectedPlayers >= 2)
        {
            Debug.Log("[Fusion] Hay 2 jugadores, cargando escena de juego...");

            var sceneRef = SceneRef.FromIndex(gameSceneBuildIndex);

            // Carga la escena de juego como única escena
            runner.LoadScene(sceneRef, LoadSceneMode.Single);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        connectedPlayers = Mathf.Max(connectedPlayers - 1, 0);
        Debug.Log($"[Fusion] PlayerLeft {player}. Conectados: {connectedPlayers}");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log($"[Fusion] Escena activa: {SceneManager.GetActiveScene().name}");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("[Fusion] OnSceneLoadStart");
    }

    // ----- Los demás callbacks los dejamos vacíos -----

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"[Fusion] Shutdown: {shutdownReason}");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("[Fusion] Conectado al servidor");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log($"[Fusion] Desconectado: {reason}");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log($"[Fusion] Falló conexión: {reason}");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
}
