using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palo : Armas
{
    [Header("Atributos del arma")]
    public string nombre = "Palo";
    public float cadenciaDeTiro = 0.25f;
    public float precision = 100f;
    public float retroceso = 0f;
    public float alcance = 0f;
    public int ammo = 1;
    public int distancia = 0;
    public bool silenciador = true;
    public Sprite imagen;

    [Header("Posicion del arma")]
    public Vector3 posicion;
    public Vector3 rotacionEnemigo;
    public Vector3 rotacionJugador;

    private GameObject target;

    public override string Nombre()
    {
        return nombre;
    }

    public override int MaxAmmo()
    {
        return 1;
    }

    public override int Ammo()
    {
        return ammo;
    }

    public override int DistanciaDeTiro()
    {
        return distancia;
    }

    public override float TiempoRecarga()
    {
        return 0;
    }

    public override float CadenciaDeTiro()
    {
        return cadenciaDeTiro;
    }

    public override float Precision()
    {
        return precision;
    }

    public override float Alcance()
    {
        return alcance;
    }

    public override bool Silenciador()
    {
        return silenciador;
    }

    public override Sprite Imagen()
    {
        return imagen;
    }

    public override Vector3 posFix()
    {
        return posicion;
    }

    public override Vector3 rotFix()
    {
        return rotacionEnemigo;
    }

    public override Vector3 rotFixPlayer()
    {
        return rotacionJugador;
    }

    public override void Disparo()
    {
        if (target != null)
        {
            target.GetComponent<HPScript>().RecibirDano(1);
        }
    }

    public override void VarAmmo(bool recarga, int x)
    {
    }

    public override void CargarHabilidades(bool player)
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemigo")
            target = other.gameObject;
    }
}
