using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    //Script utilizado para el movimiento de la mira, esta no sigue al mouse, se mueve junto a ella permitiendo la compatibilidad con Joystick cambiando unos pocos parametros.

    public float sensibilidad = 15f;
    public bool invertX = false;
    public bool invertY = false;

    private float mouseX;
    private float mouseY;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Antes de asignar el valor a cada eje se pregunta si este es invertido o no, pudiendo cambiar este valor en las opciones del juego

        if (invertX)
            mouseX = -Input.GetAxis("AimX");
        else
            mouseX = Input.GetAxis("AimX");

        if (invertY)
            mouseY = -Input.GetAxis("AimY");
        else
            mouseY = Input.GetAxis("AimY");

        rb.velocity = new Vector3(mouseX * sensibilidad, 0, mouseY * sensibilidad);

        rb.AddForce(new Vector3(Mathf.Sin(Time.time * 4) * 30, 0, Mathf.Cos(Time.time * 4) * 30));

    }

    private void OnBecameInvisible()
    {
        Vector3 addPos = new Vector3(1, 0, 1);
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = playerPos + addPos;
    }
}