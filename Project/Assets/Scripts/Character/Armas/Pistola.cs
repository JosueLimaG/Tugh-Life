using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistola : Armas
{
    //Variables publicas que reemplazan las variables de la clase Armas
    public string nombre = "Pistola";
    public int maxAmmo = 12;
    public float tiempoRecarga = 4f;
    public float cadenciaDeTiro = 1f;
    public float precision = 70f;
    public float retroceso = 50f;
    public float alcance = 150f;
    public int ammo = 12;
    public Sprite imagen;

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

    public override Sprite Imagen()
    {
        return imagen;
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

    //Metodo usado para descartar el arma actual, se lo quita de la jerarquia del jugador.
    public override void Descartar()
    {
        {
            gameObject.transform.parent = null;
            activo = false;
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
