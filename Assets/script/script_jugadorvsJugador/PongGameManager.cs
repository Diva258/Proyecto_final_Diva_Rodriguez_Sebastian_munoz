using UnityEngine;
using TMPro;

public class PongGameManager : MonoBehaviour
{
    [Header("Pelota")]
    public GameObject ballPrefab;        // Prefab de la pelota
    public Transform ballSpawnPoint;     // Punto donde aparece la pelota

    private BallController ballController;

    [Header("Marcadores")]
    public TMP_Text score1Text;
    public TMP_Text score2Text;
    public TMP_Text ganadorText;
    public int puntosParaGanar = 3;

    private int score1;
    private int score2;

    [Header("Cámara / UI")]
    public CameraMover cameraMover;      // Script de la cámara que maneja los canvas

    void Start()
    {
        ResetMarcadores();
    }

    // 🔵 Llamado por el botón PLAY (además de CameraMover.IrAJuego)
    public void IniciarJuego()
    {
        ResetMarcadores();
        CrearPelotaYLanzar();
    }

    // ------------------- PELOTA -------------------

    void CrearPelotaYLanzar()
    {
        // destruir pelota anterior si existe
        if (ballController != null)
        {
            Destroy(ballController.gameObject);
            ballController = null;
        }

        GameObject ball = Instantiate(ballPrefab, ballSpawnPoint.position, ballSpawnPoint.rotation);
        ballController = ball.GetComponent<BallController>();

        if (ballController == null)
        {
            Debug.LogError("El prefab de la pelota no tiene BallController.");
            return;
        }

        LanzarPelotaAleatoria();
    }

    void LanzarPelotaAleatoria()
    {
        if (ballController == null) return;

        // dirección aleatoria izquierda/derecha con un poco de Z
        Vector3 dir = new Vector3(
            Random.value < 0.5f ? -1f : 1f,    // X
            0f,
            Random.Range(-0.5f, 0.5f)          // Z
        );

        ballController.Lanzar(dir);
    }

    // ------------------- GOLES -------------------

    // Llama esto desde el trigger del lado del jugador 2 (gol a favor del jugador 1)
    public void GolJugador1()
    {
        score1++;
        ProcesarGol();
    }

    // Llama esto desde el trigger del lado del jugador 1 (gol a favor del jugador 2)
    public void GolJugador2()
    {
        score2++;
        ProcesarGol();
    }

    void ProcesarGol()
    {
        ActualizarMarcadores();

        // ¿Alguien ganó?
        if (score1 >= puntosParaGanar || score2 >= puntosParaGanar)
        {
            string mensaje = score1 >= puntosParaGanar ? "Jugador 1 ganó" : "Jugador 2 ganó";

            if (ganadorText != null)
                ganadorText.text = mensaje;

            // apaga pelota
            if (ballController != null)
            {
                Destroy(ballController.gameObject);
                ballController = null;
            }

            // deja que la cámara/canvas muestren el CANVAS_GANADOR
            if (cameraMover != null)
                cameraMover.MostrarGanador();
        }
        else
        {
            // sigue el partido: nueva pelota desde el centro
            CrearPelotaYLanzar();
        }
    }

    // ------------------- MARCADORES -------------------

    void ResetMarcadores()
    {
        score1 = 0;
        score2 = 0;
        ActualizarMarcadores();

        if (ganadorText != null)
            ganadorText.text = "";
    }

    void ActualizarMarcadores()
    {
        if (score1Text != null)
            score1Text.text = score1.ToString();

        if (score2Text != null)
            score2Text.text = score2.ToString();
    }
}
