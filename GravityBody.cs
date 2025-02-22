using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    private static float GRAVITY_STRENGTH = 3000.0f; // Reducido de 2000 a 500


    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 gravityDirection = GetGravityDirection();
        _rb.AddForce(gravityDirection * (GRAVITY_STRENGTH * Time.fixedDeltaTime), ForceMode.Acceleration);

        // Ajustar la rotación para que la bola apunte en la dirección de la gravedad, evitando cambios bruscos
        Vector3 targetUp = -gravityDirection;
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetUp);

        // Interpolación suave para evitar rotaciones bruscas
        _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, Time.fixedDeltaTime * 2f));

    }

    public Vector3 GetGravityDirection()
{
    int gravityLayer = LayerMask.GetMask("GravitySource");
    Collider[] objetos = Physics.OverlapSphere(transform.position, 50f, gravityLayer); // Solo detecta objetos en la capa "GravitySource"

    GameObject objetoMasCercano = null;
    float distanciaMinima = Mathf.Infinity;

    foreach (Collider col in objetos)
    {
        float distancia = Vector3.Distance(col.transform.position, transform.position);
        if (distancia < distanciaMinima)
        {
            distanciaMinima = distancia;
            objetoMasCercano = col.gameObject;
        }
    }

    if (objetoMasCercano != null)
    {
        return (objetoMasCercano.transform.position - transform.position).normalized;
    }
    return Vector3.zero;
}

}