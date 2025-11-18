using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject canvasMenuPrincipal;   // CANVAS_MENU
    public GameObject canvasHost;            // CANVAS_HOST
    public GameObject canvasOtro;            // Opcional si usas otro canvas

    // 👉 BOTÓN: Ir a HOST (BOTON_ONLINE_JUGAR)
    public void IrAHost()
    {
        canvasMenuPrincipal.SetActive(false);
        canvasHost.SetActive(true);

        if (canvasOtro != null)
            canvasOtro.SetActive(false);
    }

    // 👉 BOTÓN: Volver al menú
    public void VolverAlMenu()
    {
        canvasMenuPrincipal.SetActive(true);
        canvasHost.SetActive(false);

        if (canvasOtro != null)
            canvasOtro.SetActive(false);
    }
}
