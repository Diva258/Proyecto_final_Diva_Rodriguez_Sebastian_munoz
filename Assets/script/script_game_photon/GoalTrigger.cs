using Fusion;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [Header("¿Quién GANA punto cuando entra la pelota aquí?")]
    public bool scoreForHost = true;

    private NetworkRunner runner;

    private void Awake()
    {
        runner = FindObjectOfType<NetworkRunner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var ball = other.GetComponent<FusionBallController>();
        if (ball == null || runner == null)
            return;

        if (!runner.IsServer)
            return;

        // Sumar punto
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(scoreForHost);

        // Aquí tú ya estás haciendo el respawn de la pelota con GameFusionSpawner
        bool ballTowardsHost = !scoreForHost;
        if (GameFusionSpawner.Instance != null)
            GameFusionSpawner.Instance.RespawnBall(ballTowardsHost);
    }
}
