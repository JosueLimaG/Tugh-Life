using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript : MonoBehaviour
{
    private Animator pies;
    private Animator anim;
    private Rigidbody rb;
    private Inventario inventario;

    private bool resetDisparo = false;

    public bool recargando;
    public bool disparo;

	void Start ()
    {
        pies = transform.GetChild(1).GetComponent<Animator>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        inventario = transform.GetChild(0).GetComponent<Inventario>();
    }

    void Update ()
    {
        //Pies();                           //Deshabilitado por bugs visuales y problemas al aplicar el angulo correcto

        Disparo();

        anim.SetInteger("Arma", Armas(inventario.activa.GetComponent<Armas>().Nombre()));
	}

    void Pies()
    {
        if (Mathf.Abs(rb.velocity.x) > 0.1f || Mathf.Abs(rb.velocity.z) > 0.1f)
        {
            pies.SetBool("Movimiento", true);
            float x = transform.position.x - rb.velocity.x; 
            float z = transform.position.z - rb.velocity.z; 

            float angulo = Mathf.Atan(z / x) * Mathf.Rad2Deg;      

            if (x < 0 && z < 0)
                angulo = ((90 - angulo) * -1) + -90; 
            else if (x < 0 && z > 0)
                angulo = 180 + angulo;

            pies.transform.eulerAngles = new Vector3(0, 0, 30);
        }
        else
        {
            pies.SetBool("Movimiento", false);
        }
    }

    void Disparo()
    {
        if (resetDisparo)
        {
            disparo = false;
            resetDisparo = false;
        }

        anim.SetBool("Disparo", disparo);

        if (disparo)
        {
            resetDisparo = true;
        }
    }

    int Armas(string nombre)
    {
        switch (nombre)
        {
            case "Desarmado":
                return 0;
            case "Palo":
                return 1;
            case "Cuchillo":
                return 2;
            case "Pistola":
                return 3;
            case "Escopeta":
                return 4;
            case "Metralleta":
                return 5;
            default:
                return 0;
        }
    }

    public void Cargando(bool x)
    {
        try
        {
            anim.SetBool("Recarga", x);
        }
        catch
        {
            print("Error al setear el bool de: " + name);
        }
    }
}
