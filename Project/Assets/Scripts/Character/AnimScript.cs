using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript : MonoBehaviour
{
    private Animator pies;
    private Animator anim;
    private Rigidbody rb;
    private Inventario inventario;

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
        if (rb.velocity.x != 0 || rb.velocity.z != 0)
        {
            pies.SetBool("Movimiento", true);
        }
        else
        {
            pies.SetBool("Movimiento", false);
        }

        switch(inventario.activa.GetComponent<Armas>().name)
        {
            case "Desarmado":
                anim.SetInteger("Arma", 0);
                break;
            case "Palo":
                anim.SetInteger("Arma", 1);
                break;
            case "Cuchillo":
                anim.SetInteger("Arma", 2);
                break;
            case "Pistola":
                anim.SetInteger("Arma", 3);
                break;
            case "Escopeta":
                anim.SetInteger("Arma", 4);
                break;
            case "Metralleta":
                anim.SetInteger("Arma", 5);
                break;
        }

        //anim.SetBool("Recarga", recargando);
        //anim.SetBool("Disparo", disparo);

        Vector2 position = transform.position;
        Vector2 velocidad = rb.velocity;
        pies.transform.eulerAngles = new Vector3(0, 0, Vector2.Angle(position, velocidad));
	}

    public void Cargando(bool x)
    {
        anim.SetBool("Recarga", x);
    }
}
