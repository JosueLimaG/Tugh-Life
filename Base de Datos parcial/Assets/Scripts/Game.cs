using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public InputField correoTXT;
    public InputField celularTXT;
    public InputField diamantesTXT;
    public InputField nivelTXT;
    public GameObject panelGO;
    public GameObject rankingGO;
    int puntosDB;

    public void GuardarPuntosDB()
    {
        puntosDB = Random.Range(0, 1500);
        rankingGO.GetComponent<RankingManager>().InsertarPuntos(correoTXT.text, celularTXT.text, puntosDB, diamantesTXT.text, nivelTXT.text);
    }
}
