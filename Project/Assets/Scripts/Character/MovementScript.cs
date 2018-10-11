using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class MovementScript : MonoBehaviour
{
    [HideInInspector] public float velocidad = 7f; //Se oculta la velocidad para evitar su manipulado libre, sin evitar su acceso a otros scripts
    [HideInInspector] public float angulo;
    [HideInInspector] public  Transform aim;

    private Rigidbody2D rb;
    private Inventario inventario;
    private GameObject armaCerca;

    void Start ()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody2D>();
        aim = GameObject.Find("Aim").transform;
        inventario = transform.GetChild(0).GetComponent<Inventario>(); //Se obtiene el componente Inventario del jugador para ejecutar ciertos movimientos basicos.
        MostrarInvetario();
    }

    void FixedUpdate()
    {
        //Aplicando velocidad al personaje
        MovimientoPersonaje();
        //Calculando la rotacion para mirar siempre al cursor
        MirarAlCursor();

        if (Input.GetButtonDown("Cambiar"))
        {
            inventario.CambiarArma();
            MostrarInvetario();
        }

        if(Input.GetButtonDown("Descartar"))
        {
            inventario.NuevaArma();
            MostrarInvetario();
        }
    }

    void MostrarInvetario()
    {
        UIManager.instance.ActualizarInventario(inventario.ConsultarInventario());
    }

    void ActualizarInventario(int id)
    {
        Debug.Log(inventario.InfoInventario());
        //UIManager.instance.ActualizarInfomacion(inventario.InfoInventario());
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
        angulo = Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angulo);
    }
}