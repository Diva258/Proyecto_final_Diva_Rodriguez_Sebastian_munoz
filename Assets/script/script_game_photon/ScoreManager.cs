using Fusion;
using UnityEngine;
using TMPro;

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager Instance;

    [Header("UI del marcador (TextMeshPro)")]
    public TextMeshProUGUI hostScoreText;
    public TextMeshProUGUI clientScoreText;

    [Header("UI de victoria")]
    public GameObject winCanvasRoot;      // 👈 AQUÍ vas a arrastrar tu WinCanvas
    public TextMeshProUGUI winText;       // Texto que dice quién ganó (dentro del canvas)

    [Header("UI del menú durante la partida (opcional)")]
    public GameObject menuRoot;           // Tu objeto "menu" normal

    [Header("Audio")]
    public AudioSource pointAudioSource;  // Sonido al hacer punto

    [Networked] private int HostScore { get; set; }
    [Networked] private int ClientScore { get; set; }

    [Networked] private NetworkBool GameEnded { get; set; }
    [Networked] private NetworkBool HostWon  { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Render()
    {
        // 🔢 Actualizar marcador
        if (hostScoreText != null)
            hostScoreText.text = HostScore.ToString();

        if (clientScoreText != null)
            clientScoreText.text = ClientScore.ToString();

        // 🏆 Mostrar / ocultar TODO el canvas de victoria
        if (winCanvasRoot != null)
        {
            if (GameEnded)
            {
                if (!winCanvasRoot.activeSelf)
                    Debug.Log("[ScoreManager] Activando WinCanvas");

                winCanvasRoot.SetActive(true);

                if (winText != null)
                {
                    winText.text = HostWon
                        ? "¡El jugador HOST ha ganado!"
                        : "¡El jugador CLIENT ha ganado!";
                }
            }
            else
            {
                if (winCanvasRoot.activeSelf)
                    Debug.Log("[ScoreManager] Ocultando WinCanvas");

                winCanvasRoot.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("[ScoreManager] winCanvasRoot NO está asignado en el inspector.");
        }

        // 🎛 Apagar menú durante pantalla de victoria
        if (menuRoot != null)
        {
            menuRoot.SetActive(!GameEnded);
        }
    }

    public void AddScore(bool scoredByHost)
    {
        // Solo la StateAuthority (host) modifica el marcador
        if (!Object.HasStateAuthority)
            return;

        if (GameEnded)
            return;

        if (scoredByHost)
            HostScore++;
        else
            ClientScore++;

        Debug.Log($"[ScoreManager] Puntaje -> HOST: {HostScore} | CLIENT: {ClientScore}");

        // 🔊 sonido de punto para todos
        PlayPointSoundRpc();

        // ¿Alguien llegó a 3?
        if (HostScore >= 3 || ClientScore >= 3)
        {
            GameEnded = true;
            HostWon   = HostScore >= 3;

            Debug.Log("[ScoreManager] Juego terminado. Ganador: " + (HostWon ? "HOST" : "CLIENT"));
        }
    }

    // RPC: el host lo llama, se ejecuta en TODOS
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void PlayPointSoundRpc()
    {
        if (pointAudioSource != null)
            pointAudioSource.Play();
        else
            Debug.LogWarning("[ScoreManager] pointAudioSource no está asignado.");
    }
}
