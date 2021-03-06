﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    //La intencion es que todos los NPC tambien tengan un elemento Inventario y puedan realizar las mismas acciones que el jugador, por ello se llaman a los metodos desde el script jugador, que detecta los input

    public GameObject desarmadoPrefab;
    public GameObject paloPrefab;
    public GameObject cuchilloPrefab;
    public GameObject pistolaPrefab;
    public GameObject metralletaPrefab;
    public GameObject escopetaPrefab;

    GameObject[] contenido = new GameObject[2];
    private GameObject armaTemp;
    private GameObject armaTemp2;
    private bool antibalas = false;
    private Armas arma1;
    private Armas arma2;

    [HideInInspector] public int balasPistola;
    [HideInInspector] public int balasEscopeta;
    [HideInInspector] public int balasMetralleta;
    [HideInInspector] public GameObject armaPiso;
    [HideInInspector] public GameObject activa;

    private void Start()
    {
        float[] temp = GameManager.instance.ps.ObtenerDatos(4, transform.parent.tag == "Player");
        balasEscopeta = (int)temp[1];
        balasMetralleta = (int)temp[2];
        balasPistola = (int)temp[3];

        ObtenerArmas();
        OrdenarInventario();
    }

    void ObtenerArmas()
    {
        if (transform.parent.tag == "Player")
        {
            switch (GameManager.instance.ps.arma2)
            {
                case "Pistola":
                    armaTemp = Instantiate(pistolaPrefab);
                    break;
                case "Metralleta":
                    armaTemp = Instantiate(metralletaPrefab);
                    break;
                case "Escopeta":
                    armaTemp = Instantiate(escopetaPrefab);
                    break;
                case "Palo":
                    armaTemp = Instantiate(paloPrefab);
                    break;
                case "Cuchillo":
                    armaTemp = Instantiate(cuchilloPrefab);
                    break;
            }

            armaTemp.transform.parent = transform;
            armaTemp.GetComponent<Armas>().Iniciar();

            switch (GameManager.instance.ps.arma1)
            {
                case "Pistola":
                    armaTemp2 = Instantiate(pistolaPrefab);
                    break;
                case "Metralleta":
                    armaTemp2 = Instantiate(metralletaPrefab);
                    break;
                case "Escopeta":
                    armaTemp2 = Instantiate(escopetaPrefab);
                    break;
                case "Palo":
                    armaTemp2 = Instantiate(paloPrefab);
                    break;
                case "Cuchillo":
                    armaTemp2 = Instantiate(cuchilloPrefab);
                    break;
            }
            armaTemp2.transform.parent = transform;
            armaTemp2.GetComponent<Armas>().Iniciar();
        }
    }

    void OrdenarInventario()
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

        arma1 = contenido[0].GetComponent<Armas>();
        arma2 = contenido[1].GetComponent<Armas>();
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

    public void CambiarArma(int x)
    {
        if (x == 0 && activa == contenido[1])
        {
            activa = contenido[0];
            contenido[1].SetActive(false);
        }

        if (x == 1 && activa == contenido[0])
        {
            activa = contenido[1];
            contenido[0].SetActive(false);
        }

        activa.SetActive(true);
    }


    //Metodo para intercambiar un arma del inventario con otra. Cuando no haya otra arma disponible cerca se deshechara la que se tiene como activa.
    public void NuevaArma()
    {
        if (armaPiso == null)                                                       //Se comprueba si hay un arma disponible para ser recogida
        {
            if (activa.GetComponent<Armas>().Nombre() != "Desarmado")                   //Comprueba si el jugador tiene un arma
            {
                armaPiso = Instantiate(desarmadoPrefab);                            //Se instancia un "Desarmado" para ser asignado en el inventario ya que no puede estar vacio
                armaPiso.transform.SetParent(transform);                            //El arma se vuelve hija del inventario
                armaPiso.GetComponent<Armas>().Iniciar();
                activa.GetComponent<Armas>().Descartar();                           //Se descarta el arma activa
            }
        }
        else
        {

            if (activa.GetComponent<Armas>().name != "Desarmado")                   //Comprueba si el jugador tiene un arma
            {
                activa.GetComponent<Armas>().Descartar();                           //Se descarta el arma actual

                armaPiso.transform.SetParent(transform);                            //El arma se vuelve hija del inventario
            }

            armaPiso.GetComponent<Armas>().Iniciar();                               //Se activa el arma que se recogio
        }

        armaPiso = null;
        OrdenarInventario();                                                        //Se ordena el inventario con la nueva informacion
        CambiarArma();
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Arma")
            armaPiso = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Arma")
            armaPiso = other.gameObject;
    }
}
