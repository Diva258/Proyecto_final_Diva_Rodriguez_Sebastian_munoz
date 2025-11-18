using Fusion;
using UnityEngine;

public class FusionBallController : NetworkBehaviour
{
    public float speed = 6f;

    [Header("Rebotes verticales")]
    public float minY = -3.5f;
    public float maxY = 3.5f;

    [Networked] private Vector2 Direction { get; set; }

    private Rigidbody rb;

    public override void Spawned()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority || rb == null)
            return;

        Vector3 pos = rb.position;
        pos += (Vector3)(Direction * speed * Runner.DeltaTime);

        // Rebote manual en Y
        if (pos.y >= maxY)
        {
            pos.y = maxY;
            Direction = new Vector2(Direction.x, -Mathf.Abs(Direction.y));
        }
        else if (pos.y <= minY)
        {
            pos.y = minY;
            Direction = new Vector2(Direction.x, Mathf.Abs(Direction.y));
        }

        rb.MovePosition(pos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Object.HasStateAuthority)
            return;

        // Rebote con paletas
        if (collision.gameObject.CompareTag("Paddle"))
        {
            Direction = new Vector2(-Direction.x, Direction.y);

            float extraY = Random.Range(-0.3f, 0.3f);
            Direction = new Vector2(Direction.x, Direction.y + extraY).normalized;
        }

        // Rebote con paredes (si las tagueas como Wall)
        if (collision.gameObject.CompareTag("Wall"))
        {
            Direction = new Vector2(Direction.x, -Direction.y);
        }
    }

    /// <summary>
    /// Lanza la pelota desde su posición actual hacia host o client.
    /// Solo la StateAuthority debe llamarlo (el server).
    /// </summary>
    public void Launch(bool haciaHost)
    {
        if (!Object.HasStateAuthority)
            return;

        float dirX = haciaHost ? -1f : 1f;
        float dirY = Random.Range(-0.6f, 0.6f);
        if (Mathf.Abs(dirY) < 0.2f)
            dirY = 0.3f * Mathf.Sign(dirY == 0 ? 1 : dirY);

        Direction = new Vector2(dirX, dirY).normalized;
    }
}
