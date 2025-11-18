using UnityEngine;

public class GolTrigger : MonoBehaviour
{
    public PongGameManager manager;
    public bool esGolJugador1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pelota"))
        {
            if (esGolJugador1)
                manager.GolJugador1();
            else
                manager.GolJugador2();
        }
    }
}
