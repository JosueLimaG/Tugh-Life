using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metralleta : Armas
{
    //Variables publicas que reemplazan las variables de la clase Armas
    [Header("Atributos del arma")]
    public string nombre = "Metralleta";
    public int maxAmmo = 32;
    public float tiempoRecarga = 5f;
    public float cadenciaDeTiro = 0.1f;
    public float precision = 90f;
    public float retroceso = 75f;
    public float alcance = 120f;
    public int ammo = 32;
    public bool sileniador = false;
    public Sprite imagen;

    [Header("Posicion del arma")]
    public Vector3 posicion;
    public Vector3 rotacionEnemigo;
    public Vector3 rotacionJugador;

    public override string Nombre()
    {
        return nombre;
    }

    public override int MaxAmmo()
    {
        return maxAmmo;
    }

    public override int Ammo()
    {
        return ammo;
    }

    public override float TiempoRecarga()
    {
        return tiempoRecarga;
    }

    public override float CadenciaDeTiro()
    {
        return cadenciaDeTiro;
    }

    public override float Precision()
    {
        return precision;
    }

    public override float Retroceso()
    {
        return retroceso;
    }

    public override float Alcance()
    {
        return alcance;
    }

    public override bool Silenciador()
    {
        return sileniador;
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

    //Cada arma tiene un tipo de disparo diferente.
    public override void Disparo()
    {
        VarAmmo(false, -1);
        disparoSystem.Emit(1);
        Debug.Log("Disparo de " + Nombre());
    }

    //Se calcula un raycast para obtener informacion del objetivo al que se apunta.
    private void Update()
    {
        Debug.DrawRay(transform.position, (aim.position - transform.position).normalized);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (aim.position - transform.position).normalized, Alcance());
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.name);
        }
    }

    //Metodo publico usado para cambiar el valor de la municion actual, si el bool recarga es true se llena el cargador restando la municion del total almacenado.
    public override void VarAmmo(bool recarga, int x)
    {
        if (recarga)
        {
            int a = maxAmmo - ammo;
            savedAmmo -= a;

            if (savedAmmo >= maxAmmo)
                ammo = maxAmmo;
            else
                ammo = savedAmmo;
        }
        else
            ammo += x;
    }

}
