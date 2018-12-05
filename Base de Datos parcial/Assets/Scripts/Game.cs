using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public Text puntosTXT;
    public Text nombreTXT;
    public GameObject panelGO;
    public GameObject rankingGO;
    int puntosDB;

    public void GenerarPuntos()
    {
        float puntos = Random.Range(0,1500);
        puntosDB = (int)puntos;
        puntosTXT.text = puntos.ToString();
    }

    public void ActivarPanel()
    {
        panelGO.SetActive(true);
    }

    public void GuardarPuntosDB()
    {
        rankingGO.GetComponent<RankingManager>().InsertarPuntos(nombreTXT.text, puntosDB);
        panelGO.SetActive(false);
    }
}
