using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Header("Puntos de cámara")]
    public Transform menuPosition;
    public Transform comoJugarPosition;
    public Transform hostPosition;
    public float speed = 3f;

    private Transform target;

    [Header("Canvas")]
    public GameObject menuPrincipal;      
    public GameObject canvaComoJugar;     
    public GameObject canvaHost;          
    public GameObject canvaJuego;         
    public GameObject canvaGanador;       

    void Start()
    {
        // Cámara empieza en el menú
        transform.position = menuPosition.position;
        transform.rotation = menuPosition.rotation;

        menuPrincipal.SetActive(true);
        canvaComoJugar.SetActive(false);
        canvaHost.SetActive(false);
        if (canvaJuego) canvaJuego.SetActive(false);
        if (canvaGanador) canvaGanador.SetActive(false);

        target = menuPosition;
    }

    void Update()
    {
        if (target == null) return;

        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * speed);
    }

    // ----------------------
    // BOTÓN: COMO JUGAR
    // ----------------------
    public void IrAComoJugar()
    {
        target = comoJugarPosition;

        menuPrincipal.SetActive(false);
        canvaComoJugar.SetActive(true);
        canvaHost.SetActive(false);
        if (canvaJuego) canvaJuego.SetActive(false);
        if (canvaGanador) canvaGanador.SetActive(false);
    }

    // ----------------------
    // BOTÓN: HOST
    // ----------------------
    public void IrAHost()
    {
        target = hostPosition;

        menuPrincipal.SetActive(false);
        canvaComoJugar.SetActive(false);
        canvaHost.SetActive(true);
        if (canvaJuego) canvaJuego.SetActive(false);
        if (canvaGanador) canvaGanador.SetActive(false);
    }

    // ----------------------
    // BOTÓN: VOLVER AL MENÚ
    // ----------------------
    public void VolverAlMenu()
    {
        target = menuPosition;

        menuPrincipal.SetActive(true);
        canvaComoJugar.SetActive(false);
        canvaHost.SetActive(false);
        if (canvaJuego) canvaJuego.SetActive(false);
        if (canvaGanador) canvaGanador.SetActive(false);
    }

    // ----------------------
    // BOTÓN: INICIAR PARTIDA
    // ----------------------
    public void IrAJuego()
    {
        if (canvaJuego == null)
        {
            Debug.LogError("Asigna el Canvas del juego (marcadores) en CameraMover");
            return;
        }

        canvaHost.SetActive(false);
        canvaJuego.SetActive(true);
        canvaGanador.SetActive(false);
        menuPrincipal.SetActive(false);
        canvaComoJugar.SetActive(false);
    }

    // ----------------------
    // MOSTRAR GANADOR
    // ----------------------
    public void MostrarGanador()
    {
        canvaJuego.SetActive(false);
        canvaGanador.SetActive(true);
        canvaHost.SetActive(false);
        canvaComoJugar.SetActive(false);
        menuPrincipal.SetActive(false);
    }
}
