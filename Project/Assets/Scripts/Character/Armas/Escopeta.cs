using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escopeta : Armas
{
    //Variables publicas que reemplazan las variables de la clase Armas
    [Header("Atributos del arma")]
    public string nombre = "Escopeta";
    public int maxAmmo = 2;
    public int numeroDeBalas = 4;
    public float tiempoRecarga = 8f;
    public float cadenciaDeTiro = 1f;
    public float precision = 60f;
    public float alcance = 100f;
    public int ammo = 2;
    public int distancia = 1;
    public bool silenciador = false;
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

    public override int DistanciaDeTiro()
    {
        return distancia;
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
    //Cada arma tiene un tipo de disparo diferente.
    public override void Disparo()
    {
        VarAmmo(false, -1);
        disparoSystem.Emit(numeroDeBalas);
        //Debug.Log("Disparo de " + Nombre());
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
        if (recarga)                                    //Si la instuccion es recargar, se llena el cargador
        {
            int a = maxAmmo - ammo;
            if (savedAmmo >= a)
            {
                savedAmmo -= a;
                ammo += a;
            }
            else
            {
                ammo = savedAmmo;
                savedAmmo = 0;
            }

            inventario.balasEscopeta = savedAmmo;
        }
        else
            ammo += x;                                  //Si la instuccion no es recargar, se suma la municion al monto asignado en x
    }

    public override void CargarHabilidades(bool player)
    {
        float[] info = GameManager.instance.ps.ObtenerDatos(3, player);
        if (info != null)
        {
            maxAmmo = (int)info[0];
            tiempoRecarga = info[1];
            cadenciaDeTiro = info[2];
            numeroDeBalas = (int)info[3];

            if (info[4] == 1)
                silenciador = true;
            else
                silenciador = false;
        }
    }
}
