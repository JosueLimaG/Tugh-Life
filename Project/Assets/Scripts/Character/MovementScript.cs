using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
//using UnityEngine.UI;

public class MovementScript : MonoBehaviour
{
    [HideInInspector] public Transform aim;

    private float velocidad = 12;
    private Rigidbody rb;
    private Inventario inventario;
    private GameObject armaCerca;
    private Vector3 Axis;
    private float anguloTemp;

    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        aim = GameObject.Find("Aim").transform;
        if (!GameManager.instance.windows)
        {
            aim.parent = this.transform;
            aim.localPosition = new Vector3(0, -5, 10);
            aim.eulerAngles = new Vector3(0, 0, 0);
        }
        inventario = transform.GetChild(0).GetComponent<Inventario>(); //Se obtiene el componente Inventario del jugador para ejecutar ciertos movimientos basicos.
        Invoke("MostrarInvetario", 0.1f);
    }

    void FixedUpdate()
    {
        Axis = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));

        //Aplicando velocidad al personaje
        MovimientoPersonaje();
        //Calculando la rotacion para mirar siempre al cursor
        MirarAlCursor();

        if (CrossPlatformInputManager.GetButtonDown("Triangle"))
        {
            inventario.CambiarArma();
            MostrarInvetario();
        }

        if (CrossPlatformInputManager.GetButtonDown("X"))
        {
            inventario.NuevaArma();
            MostrarInvetario();
        }
    }

    public void CambiarArma(int x)
    {
        inventario.CambiarArma(x);
        MostrarInvetario();
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
        float inputX = Mathf.Round(Axis.x * 10) / 10;
        float inputZ = Mathf.Round(Axis.y * 10) / 10;

        rb.velocity = new Vector3(inputX * velocidad, 0, inputZ * velocidad);
    }

    void MirarAlCursor()
    {
        if (GameManager.instance.windows)
        {
            transform.LookAt(aim);
            transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z + 180);
        }
        else
        {
            transform.eulerAngles = new Vector3(90, 0, Angulo());
        }
    }

    float Angulo()
    {
        float angulo = Mathf.Atan(Axis.y / Axis.x) * Mathf.Rad2Deg;           //Se obtiene el angulo a base de la tangente en Radianes y se lo convierte en Grados

        if (Axis.x < 0 && Axis.y < 0)                                         //Se corrige el angulo a base del valor del vector, al sacar el angulo
            angulo = ((90 - angulo) * -1) + -90;                    //a base de la tangente, solo los valores de -90 a 90 son correctos
        else if (Axis.x < 0 && Axis.y > 0)
            angulo = 180 + angulo;
        else if (Axis.x < 0 && Axis.y == 0)
            angulo = 180;
        angulo += 90;

        if (Axis.x == 0 && Axis.y == 0)
            return anguloTemp;

        anguloTemp = angulo;
        return angulo;
    }
}