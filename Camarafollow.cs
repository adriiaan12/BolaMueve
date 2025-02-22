using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // La bola
    public Vector3 offset = new Vector3(0, 4, -8); // Posición relativa fija detrás de la bola
    public float smoothSpeed = 5f; // Suavidad del seguimiento

    void LateUpdate()
    {
        if (target == null) return;

        // Obtener la dirección de la gravedad
        Vector3 gravityDirection = target.GetComponent<GravityBody>().GetGravityDirection();
        Vector3 up = -gravityDirection; // "Arriba" relativa a la gravedad

        // Obtener la orientación "hacia adelante" basada en la bola sin depender de su rotación
        Vector3 forward = Vector3.Cross(up, Vector3.right).normalized; // Mantiene un eje "adelante" estable

        // Nueva posición deseada para la cámara
        Vector3 desiredPosition = target.position + up * offset.y + forward * offset.z;

        // Movimiento suave de la cámara
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Hacer que la cámara mire a la bola con la nueva "arriba"
        transform.LookAt(target.position, up);
    }
}
