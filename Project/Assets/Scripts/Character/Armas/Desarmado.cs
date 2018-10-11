using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desarmado : Armas
{
    public string nombre = "Desarmado";
    public float cadenciaDeTiro = 0.25f;
    public float precision = 90f;
    public float retroceso = 0f;
    public float alcance = 1f;
    public int ammo = 1;
    public Sprite imagen;

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

    public override void Disparo()
    {
        Debug.Log("Disparo de " + Nombre());
    }

    public override void Descartar()
    {
        Destroy(gameObject);
    }

    public override void VarAmmo(bool recarga, int x)
    {
        Debug.Log("Los punos no se recargan");
    }
}
