using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject menuComoJugar;

    public void AbrirComoJugar()
    {
        menuPrincipal.SetActive(false);
        menuComoJugar.SetActive(true);
    }

    public void VolverAlMenu()
    {
        menuComoJugar.SetActive(false);
        menuPrincipal.SetActive(true);
    }
}
