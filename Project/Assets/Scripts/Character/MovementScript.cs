using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class MovementScript : MonoBehaviour
{
    [HideInInspector] public float angulo;
    [HideInInspector] public Transform aim;

    private float velocidad = 12;
    private Rigidbody rb;
    private Inventario inventario;
    private GameObject armaCerca;
    private Vector3 joyInput;

    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        aim = GameObject.Find("Aim").transform;
        inventario = transform.GetChild(0).GetComponent<Inventario>(); //Se obtiene el componente Inventario del jugador para ejecutar ciertos movimientos basicos.
        Invoke("MostrarInvetario", 0.1f);
    }

    void FixedUpdate()
    {
        //Aplicando velocidad al personaje
        MovimientoPersonaje();
        //Calculando la rotacion para mirar siempre al cursor
        MirarAlCursor();

        if (Input.GetButtonDown("Triangle"))
        {
            inventario.CambiarArma();
            MostrarInvetario();
        }

        if (Input.GetButtonDown("X"))
        {
            inventario.NuevaArma();
            MostrarInvetario();
        }
    }

    void MostrarInvetario()
    {
        UIManager.instance.ActualizarInventario();
        UIManager.instance.ActualizarInformacion();
    }

    void ActualizarInventario(int id)
    {
        //Debug.Log(inventario.InfoInventario());
        //UIManager.instance.ActualizarInfomacion(inventario.InfoInventario());
    }

    void MovimientoPersonaje()
    {
        float inputX = Mathf.Round(Input.GetAxisRaw("Horizontal") * 10) / 10;
        float inputZ = Mathf.Round(Input.GetAxisRaw("Vertical") * 10) / 10;

        rb.velocity = new Vector3(inputX * velocidad, 0, inputZ * velocidad);
    }

    void MirarAlCursor()
    {
        /*if (joystick)
        {
            float aimX = Input.GetAxisRaw("AimX");
            float aimY = Input.GetAxisRaw("AimY");
            joyInput = new Vector2(aimX, aimY);

            if (aimY < 0)
                joyInput *= -1f;

            float angle = Vector2.Angle(joyInput, new Vector2(1, 0));
            transform.eulerAngles = new Vector3(90, 0, angle);
        }
        else
        {*/
            transform.LookAt(aim);
            transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z + 180);
        //}
    }
}