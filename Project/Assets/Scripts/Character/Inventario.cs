using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    //La intencion es que todos los NPC tambien tengan un elemento Inventario y puedan realizar las mismas acciones que el jugador, por ello se llaman a los metodos desde el script jugador, que detecta los input

    public GameObject desarmadoPrefab;

    GameObject[] contenido = new GameObject[2];
    private GameObject armaPiso;
    private bool antibalas = false;
    private Armas arma1;
    private Armas arma2;

    public int balasPistola;
    public int balasEscopeta;
    public int balasMetralleta;

    [HideInInspector] public GameObject activa;

    void Awake()
    {
        int i = 0;

        //Se cuentan los elementos dentro del inventario del personaje y se eliminan los que estan de sobra, un personaje puede cargar solo dos armas.
        foreach (Transform child in transform)
        {
            if (child.tag == "Arma")
            {
                if (i < 2)
                {
                    contenido[i] = child.gameObject;
                    i++;
                }
                else
                {
                    Destroy(child);
                    Debug.Log("El inventario solo permite 2 armas. " + child.name + " eliminado.");
                }
            }
        }

        //En caso de que cuente con solo un arma, o ninguna, se asigna al segundo slot de armas un "desarmado".

        if (contenido[0] == null)
        {
            GameObject instance = Instantiate(desarmadoPrefab);
            instance.transform.parent = transform;
            contenido[0] = instance;
        }

        if (contenido[1] == null)
        {
            GameObject instance = Instantiate(desarmadoPrefab);
            instance.transform.parent = transform;
            contenido[1] = instance;
        }

        //Se establece como arma activa al primer elemento del inventario.
        activa = contenido[1];
        CambiarArma();

        Armas arma1 = contenido[0].GetComponent<Armas>();
        Armas arma2 = contenido[1].GetComponent<Armas>();
    }


    //Metodo para cambiar entre las dos armas almacenadas.
    public void CambiarArma()
    {
        if (activa == contenido[0])
        {
            activa = contenido[1];
            contenido[0].SetActive(false);
        }
        else
        {
            activa = contenido[0];
            contenido[1].SetActive(false);
        }

        activa.SetActive(true);
    }


    //Metodo para intercambiar un arma del inventario con otra. Cuando no haya otra arma disponible cerca se deshechara la que se tiene como activa.
    public void NuevaArma()
    {
        if (armaPiso == null)
            armaPiso = desarmadoPrefab;

        if (activa.GetComponent<Armas>().name == "Desarmado" && armaPiso == null)
            Debug.Log("No se encontraron armas para recoger cerca");
        else
            activa.GetComponent<Armas>().Descartar();

        if (activa == contenido[0])
        {
            contenido[0] = armaPiso;
            activa = contenido[0];
        }
        else
        {
            contenido[1] = armaPiso;
            activa = contenido[1];
        }

        Armas arma1 = contenido[0].GetComponent<Armas>();
        Armas arma2 = contenido[1].GetComponent<Armas>();
    }

    public string[] ConsultarInventario()
    {
        string[] inventario = new string[3];
        inventario[0] = contenido[0].GetComponent<Armas>().Nombre();
        inventario[1] = contenido[1].GetComponent<Armas>().Nombre();

        if (antibalas)
            inventario[2] = "Antibalas";
        return inventario;
    }

    public int[] InfoInventario()
    {
        arma1 = contenido[0].GetComponent<Armas>();
        arma2 = contenido[1].GetComponent<Armas>();

        int[] info = new int[7];
        info[0] = arma1.Ammo();
        info[1] = arma1.MaxAmmo();

        if (arma1.Nombre() == "Pistola")
            info[2] = balasPistola;
        else if (arma1.Nombre() == "Metralleta")
            info[2] = balasMetralleta;
        else if (arma1.Nombre() == "Escopeta")
            info[2] = balasEscopeta;
        else
            info[2] = 0;

        info[3] = arma2.Ammo();
        info[4] = arma2.MaxAmmo();

        if (arma2.Nombre() == "Pistola")
            info[5] = balasPistola;
        else if (arma2.Nombre() == "Metralleta")
            info[5] = balasMetralleta;
        else if (arma2.Nombre() == "Escopeta")
            info[5] = balasEscopeta;
        else
            info[5] = 0;

        if (activa == contenido[0])
            info[6] = 0;
        else
            info[6] = 1;

        return info;
    }

    //Metodos para comprobar armas disponibles cerca del personaje, en caso de que ya tenga el arma se toma la municion.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Arma")
        {
            armaPiso = collision.gameObject;
            Armas temp = armaPiso.GetComponent<Armas>();
            if (!temp.activo && (contenido[0].GetComponent<Armas>().Nombre() == temp.Nombre() || contenido[1].GetComponent<Armas>().Nombre() == temp.Nombre()))
            {
                switch (temp.Nombre())
                {
                    case "Pistola":
                        balasPistola += temp.Ammo();
                        temp.VarAmmo(false, -temp.Ammo());
                        break;
                    case "Escopeta":
                        balasEscopeta += temp.Ammo();
                        temp.VarAmmo(false, -temp.Ammo());
                        break;
                    case "Metralleta":
                        balasMetralleta += temp.Ammo();
                        temp.VarAmmo(false, -temp.Ammo());
                        break;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Arma")
        {
            armaPiso = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Arma")
        {
            armaPiso = collision.gameObject;
        }
    }
}
