using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public float sensibilidad = 15f;
    public bool invertX = false;
    public bool invertY = false;
    private float mouseX;
    private float mouseY;
    private Vector2 velocidad;
    private Rigidbody2D rb;
    private Vector2 playerPos;

    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GetMousePos();
    }

    void GetMousePos()
    {
        if (invertX)
            mouseX = -Input.GetAxis("Mouse X");
        else
            mouseX = Input.GetAxis("Mouse X");

        if (invertY)
            mouseY = -Input.GetAxis("Mouse Y");
        else
            mouseY = Input.GetAxis("Mouse Y");

        velocidad = new Vector2(mouseX * sensibilidad, mouseY * sensibilidad);
        rb.velocity = velocidad;
    }
}
