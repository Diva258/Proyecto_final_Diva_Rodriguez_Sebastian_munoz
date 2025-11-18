using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [Header("Teclas")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;

    [Header("Movimiento")]
    public float speed = 6f;

    [Header("Límites automáticos")]
    public bool usarLimitesAutomaticos = true;
    public float rango = 0.3f;   // cuánto puede moverse hacia arriba/abajo

    [Header("Límites en Z (si no son automáticos)")]
    public float minZ;
    public float maxZ;

    void Start()
    {
        // Calcula límites alrededor de donde la DEJAS en el editor
        if (usarLimitesAutomaticos)
        {
            float z = transform.position.z;
            minZ = z - rango;
            maxZ = z + rango;
        }
    }

    void Update()
    {
        float move = 0f;

        if (Input.GetKey(upKey))
            move += speed * Time.deltaTime;

        if (Input.GetKey(downKey))
            move -= speed * Time.deltaTime;

        if (Mathf.Abs(move) > 0f)
        {
            // 👇 mover en EJE Z DEL MUNDO (no local)
            transform.Translate(0f, 0f, move, Space.World);

            // limitar en Z del mundo
            Vector3 pos = transform.position;
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
            transform.position = pos;
        }
    }
}
