using Fusion;
using UnityEngine;

public class PaddleNetworkController : NetworkBehaviour
{
    [Header("Movimiento")]
    public float speed = 8f;

    [Header("Límites en Y")]
    public float minY = -3.5f;
    public float maxY = 3.5f;

    public override void FixedUpdateNetwork()
    {
        // Solo el dueño de ESTA paleta lee input
        if (!HasInputAuthority)
            return;

        float input = Input.GetAxisRaw("Vertical"); // W/S o flechas

        Vector3 pos = transform.position;
        pos.y += input * speed * Runner.DeltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
