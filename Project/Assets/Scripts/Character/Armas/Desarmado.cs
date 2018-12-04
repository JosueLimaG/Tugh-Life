using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desarmado : Armas
{
    [Header("Atributos del arma")]
    public string nombre = "Desarmado";
    public float cadenciaDeTiro = 0.25f;
    public float precision = 90f;
    public float retroceso = 0f;
    public float alcance = 1f;
    public int ammo = 1;
    public int distancia = 0;
    public bool silenciador = true;
    public Sprite imagen;

    [Header("Posicion del arma")]
    public Vector3 posicion;
    public Vector3 rotacion;

    private List<GameObject> enemigos = new List<GameObject>();

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
        return rotacion;
    }

    public override Vector3 rotFixPlayer()
    {
        return new Vector3(0, 0, 0);
    }

    public override void Disparo()
    {
        foreach(GameObject enemigo in enemigos)
        {
            enemigo.GetComponent<HPScript>().RecibirDano(0);
        }
        Debug.Log("Disparo de " + Nombre());
    }

    public override void VarAmmo(bool recarga, int x)
    {
        Debug.Log("Los punos no se recargan");
    }

    public override void CargarHabilidades(bool player)
    {
        Debug.Log("No cuenta con habilidades");
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        enemigos.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        enemigos.Remove(collision.gameObject);
    }
}
