using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 8f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Solo plano XZ
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotation;
    }

    // Llamado desde PongGameManager
    public void Lanzar(Vector3 dir)
    {
        dir.y = 0f;
        rb.linearVelocity = dir.normalized * speed;
    }

    void FixedUpdate()
    {
        // Mantenerla siempre en el plano XZ y con velocidad constante
        Vector3 v = rb.linearVelocity;
        v.y = 0f;

        if (v.sqrMagnitude < 0.01f)
            return; // está casi parada, el manager ya la relanzará si hace falta

        rb.linearVelocity = v.normalized * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Rebote "natural" usando la normal de la superficie
        if (rb.linearVelocity.sqrMagnitude <= 0.01f) return;

        Vector3 v = rb.linearVelocity;
        v.y = 0f;

        ContactPoint contact = collision.GetContact(0);
        Vector3 normal = contact.normal;
        normal.y = 0f;
        normal.Normalize();

        // Reflejar la dirección respecto a la normal (como una pelota en una pared)
        Vector3 newDir = Vector3.Reflect(v.normalized, normal);

        // Evitar direcciones casi paralelas que la hagan "pegarse"
        if (Mathf.Abs(newDir.x) < 0.2f)
            newDir.x = Mathf.Sign(newDir.x) * 0.2f;
        if (Mathf.Abs(newDir.z) < 0.2f)
            newDir.z = Mathf.Sign(newDir.z) * 0.2f;

        rb.linearVelocity = newDir.normalized * speed;
    }
}
