using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class BallMovement : MonoBehaviour
{
    
    public float velocidad;

    public float fuerzaSalto = 15.0f;

    public Camera camara;

    private Vector3 offset;

    public  Rigidbody rb;

    private bool enSuelo;

    //private int Premio = 0;

    private GravityBody gravityBody;

    void Start()
    {
        gravityBody = GetComponent<GravityBody>();

        velocidad  = 0.1f;
        rb = GetComponent<Rigidbody>();

        // Calcular el desplazamiento de la cámara

        offset = camara.transform.position - transform.position;


    }


    void Update()
    {
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");

    // Obtener la dirección de la gravedad
    Vector3 gravityDirection = gravityBody.GetGravityDirection();

    // Eje "arriba" relativo a la bola (opuesto a la gravedad)
    Vector3 up = -gravityDirection;

    // Ejes de movimiento relativos a la gravedad
    Vector3 right = Vector3.Cross(transform.forward, up).normalized;  // Derecha relativa a la orientación de la bola
    Vector3 forward = Vector3.Cross(up, right).normalized;            // Adelante relativo a la gravedad

    // Movimiento basado en la orientación actual
    Vector3 movimiento = (right * moveHorizontal + forward * moveVertical) * velocidad;

    rb.AddForce(movimiento, ForceMode.VelocityChange);


    // Saltar (ahora saltará en la dirección opuesta a la gravedad actual)
    if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
    {
        rb.AddForce(-gravityDirection * fuerzaSalto, ForceMode.Impulse);
        enSuelo = false;
    }
    }


    private void OnCollisionEnter(Collision collision)
    {
    if (collision.contacts.Length > 0)
    {
        Vector3 normal = collision.contacts[0].normal;
        if (Vector3.Dot(normal, -gravityBody.GetGravityDirection()) > 0.5f) 
        {
            enSuelo = true;
        }
    }
    }

    private void OnCollisionExit(Collision collision)
    {
        StartCoroutine(VerificarSiSigueEnElSuelo());
    }

    private IEnumerator VerificarSiSigueEnElSuelo()
    {
        yield return new WaitForFixedUpdate();
        enSuelo = Physics.Raycast(transform.position, gravityBody.GetGravityDirection(), 1.5f);
    }



}