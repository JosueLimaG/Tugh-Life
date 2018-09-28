using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [HideInInspector]
    public float velocidad = 7f;
    private Transform aim;
    private Rigidbody2D rb;

    void Start ()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody2D>();
        aim = GameObject.Find("Aim").transform;
    }

    void FixedUpdate()
    {
        //Aplicando velocidad al personaje
        MovimientoPersonaje();
        //Calculando la rotacion para mirar siempre al cursor
        MirarAlCursor();
    }

    void MovimientoPersonaje()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(inputX, inputY, 0f) * velocidad;
    }

    void MirarAlCursor()
    {
        
        Vector3 dis = aim.transform.position - transform.position;
        dis = aim.transform.InverseTransformDirection(dis);
        float angulo = Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angulo);
    }
}